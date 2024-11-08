using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public List<Customer> customers;
    public GameObject customerPrefab;
    public static GameManager S;
    public GameObject separatorPrefab;
    public GameObject cratePrefab;
    public GameObject cartPrefab;
    public float itemSpawnWait;

    public bool[] decorationsEnabled = new bool[8];

    public int score;
    public int day;
    public int money=0;

    public Customer currentCustomer { get; private set; }


    [Serializable]
    public class Customer
    {
        public List<CheckoutItem> items;
        public int dayBorn;
        public string name;
        public string dialogue;
    }

    [Serializable]
    public class CheckoutItem
    {
        public GameObject prefab;
        public int quantity;
    }

    private Transform spawnPoint;
    private Transform crateSpawnPoint;
    private bool isSpawningItems =false;
    public GameObject crateInstance;
    private GameObject cartInstance;
    private TMP_Text scoreText;
    private List<Customer> levelCustomers;
    private float speed;
    private bool allCustomersSpawned = false;
    private GameObject levelEndScreen;
    private GameManagerText[] levelEndText = new GameManagerText[6];

    private List<CustomerControl> customerModels = new List<CustomerControl>();
    private Decoration[] decorations = new Decoration[8];

    private void Awake()
    {

        if(GameManager.S != null)
        {
            Destroy(this);
        }
        else
        {
        DontDestroyOnLoad(this); 
        }
    }

    private void Start()
    {
        S = this;
        SceneManager.activeSceneChanged += OnActiveSceneChange;
        this.day = 0;
        speed = 0.01f;
    }
    // hand = GameObject.Find("Monster/Arm/Hand");





    //// called by MainMenu when we enter the game and need to set up the store
    //static public void LOAD_STORE()
    //{
    //    S.spawnPoint = GameObject.Find("Conveyor/SpawnPoint").transform;
    //    S.SpawnItem(S.applePrefab);
    //}

    private IEnumerator SpawnItems(List<CheckoutItem> checkoutItems)
    {
        isSpawningItems = true;
        List<int> tempQuantities = new List<int>();
        int totalNumberOfItemsToSpawn=0;
        foreach(CheckoutItem item in checkoutItems)
        {
            tempQuantities.Add(item.quantity);
            totalNumberOfItemsToSpawn += item.quantity;
        }

        for(int i = 0; i < totalNumberOfItemsToSpawn; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, checkoutItems.Count);
            //if rand does find one that we have any left of, just loop through until we find one that has some quantity left
            //print("random ind = " + randomIndex);
            //print("tempQuantities[randomIndex] = " + tempQuantities[randomIndex]);
            if(tempQuantities[randomIndex] <= 0)
            {
                for(int j = 0; j < tempQuantities.Count; j++)   
                {
                    if(tempQuantities[j] > 0)
                    {
                        randomIndex = j; 
                    }
                }
            }
            //print("random ind after = " + randomIndex);

            var randomItem = checkoutItems[randomIndex].prefab;
            tempQuantities[randomIndex]--;
            SpawnItem(randomItem);
            yield return new WaitForSeconds(itemSpawnWait);
        }
        isSpawningItems=false;
    }

    private void SpawnItem(GameObject itemPrefab)
    {
        GameObject item = Instantiate(itemPrefab);
        item.transform.position = spawnPoint.transform.position;
        ItemControl projControl = item.GetComponent<ItemControl>();
        projControl.SetSpeed(speed);
    }


    private void OnActiveSceneChange(Scene current, Scene next)
    {
        S.spawnPoint = GameObject.Find("Conveyor/SpawnPoint")?.transform;
        S.crateSpawnPoint = GameObject.Find("Conveyor/CrateSpawnPoint")?.transform;
        S.levelEndScreen = GameObject.Find("Canvas/LevelComplete");
        print(S.levelEndScreen);
        if(levelEndScreen != null)
        {
            S.levelEndText = S.levelEndScreen.GetComponentsInChildren<GameManagerText>();
        S.levelEndScreen.SetActive(false);
        }

        S.scoreText = GameObject.Find("Canvas/Score")?.GetComponent<TMP_Text>();
        S.decorations = GameObject.Find("Decorations").GetComponentsInChildren<Decoration>();

        StartCoroutine(StartLevel());
    }


    private IEnumerator StartLevel()
    {
        score = 0;
        speed *= 1.15f;
        speed = Mathf.Clamp(speed, 0, .03f);
        day++;
        allCustomersSpawned = false;
        if (S.decorations != null)
        {
            SetDecorations();
        }
        if(spawnPoint)//is this a playable level?
        {
            crateInstance = Instantiate(cratePrefab, crateSpawnPoint.position, Quaternion.identity);
            cartInstance = Instantiate(cartPrefab, new Vector3(-6, 0.5199f, 1.31f), Quaternion.identity);
            cartInstance.GetComponent<MoveToCart>().StartAnimation();

        //iterate through customers



        foreach(var customer in GetLevelCustomers())
        {
            currentCustomer = customer;
            NewCustomer();

                // Dynamic soundtrack
                foreach (var tracks in SoundtrackManager.s.tracks)
                {
                    tracks.state = SoundtrackManager.PlayState.PendingMute;
                }

                foreach (var checkout in customer.items)
                {
                    try
                    {
                        SoundtrackManager.s.tracks[checkout.prefab.GetComponent<Grabbable>().instrument].state = SoundtrackManager.PlayState.Pending;
                    } catch
                    {

                    }
                
                }

                //print(customer.name);
                StartCoroutine(SpawnItems(customer.items));
                yield return new WaitUntil(() => !isSpawningItems);
                Instantiate(separatorPrefab, spawnPoint.position, Quaternion.identity).GetComponent<ItemControl>().SetSpeed(speed);
                yield return new WaitForSeconds(7.5f);
            }
            allCustomersSpawned = true;
            yield return new WaitForSeconds(10f);
            LevelComplete();
            //SceneManager.LoadScene(2);
        }
    }

    private void NewCustomer()
    {
        GameObject customer = Instantiate(customerPrefab);
        customer.GetComponentInChildren<CustomerControl>().dialogueString = currentCustomer.dialogue;
        customer.GetComponentInChildren<CustomerControl>().nameString = currentCustomer.name;
        customerModels.Add(customer.GetComponent<CustomerControl>());
    }

    public void EndCustomer()
    {
        crateInstance.GetComponent<MoveToCart>().StartAnimation();

        Invoke("Score", 4f);
    }

    public void Score()
    {
        score += Scoring.s.GetScore();
        scoreText.text = score.ToString();
        

        // Animate old cart out
        cartInstance.GetComponent<MoveToCart>().steps[0].pos = cartInstance.transform.position;
        cartInstance.GetComponent<MoveToCart>().steps[1].pos = cartInstance.transform.position + new Vector3(6, 0, 0);

        crateInstance.transform.parent = cartInstance.transform;

        cartInstance.GetComponent<MoveToCart>().StartAnimation((MoveToCart a) => { Destroy(a.gameObject); });

        // Animate old customer out
        customerModels.First().RetriggerAnimation();
        customerModels.RemoveAt(0);

        if(allCustomersSpawned == false)
        {
            // Spawn new cart and crate
            crateInstance = Instantiate(cratePrefab, crateSpawnPoint.position, Quaternion.identity);
            cartInstance = Instantiate(cartPrefab, new Vector3(-6, 0.5199f, 1.31f), Quaternion.identity);

            // Animate new crate (not really working)
            List<MoveToCart.AnimationStep> tempSteps = new List<MoveToCart.AnimationStep>(crateInstance.GetComponent<MoveToCart>().steps);
            crateInstance.GetComponent<MoveToCart>().steps.Clear();

            //crateInstance.GetComponent<MoveToCart>().steps = new List<MoveToCart.AnimationStep>();
            MoveToCart.AnimationStep step = new MoveToCart.AnimationStep();
            step.pos = crateInstance.transform.position + new Vector3(0, 0, 6);
            crateInstance.GetComponent<MoveToCart>().steps.Add(step);
            step.pos = crateSpawnPoint.position;
            step.duration = 3;
            step.curve = AnimationCurve.EaseInOut(0, 0, 1, 1);
            crateInstance.GetComponent<MoveToCart>().steps.Add(step);
            crateInstance.GetComponent<MoveToCart>().StartAnimation((MoveToCart a) => { a.steps = tempSteps; });

            // Animate new cart
            cartInstance.GetComponent<MoveToCart>().StartAnimation();
        }

    }

    private void SetDecorations()
    {
        for(int i = 0;i< decorations.Length; i++)
        {
            //print("deco at index "+i+"="+decorations[i].name);
            decorations[i].gameObject.SetActive(decorationsEnabled[i]);
        }
    }

    private List<Customer> GetLevelCustomers()
    {
        List<Customer> selectedCustomers = new List<Customer>();
        System.Random random = new System.Random();

        while (selectedCustomers.Count < 3)
        {
            int randomIndex = random.Next(customers.Count);
            if (!selectedCustomers.Contains(customers[randomIndex]) && customers[randomIndex].dayBorn <= day)
            {
                selectedCustomers.Add(customers[randomIndex]);
            }
        }
        return selectedCustomers;
    }
    private void LevelComplete()
    {
        money += score / 200;

        levelEndScreen.SetActive(true); 
        levelEndText[0].GetComponent<TMP_Text>().text = "Day " + (day) + " Complete!";
        levelEndText[1].GetComponent<TMP_Text>().text = customers[0].name;
        levelEndText[2].GetComponent<TMP_Text>().text = customers[1].name;
        levelEndText[3].GetComponent<TMP_Text>().text = customers[2].name;
        levelEndText[4].GetComponent<TMP_Text>().text = score.ToString();
        levelEndText[5].GetComponent<TMP_Text>().text = (score/200).ToString();
    }
}

