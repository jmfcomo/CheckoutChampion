using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using static UnityEditor.Progress;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public List<Customer> customers;
    public GameObject customerPrefab;
    public static GameManager S;
    public GameObject separatorPrefab;
    public GameObject cratePrefab;
    public GameObject cartPrefab;
    public float itemSpawnWait;

    public int score;

    public Customer currentCustomer { get; private set; }

    private void Awake()
    {
        DontDestroyOnLoad(this); 
    }

    [Serializable]
    public class Customer
    {
        public List<CheckoutItem> items;
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
    private GameObject crateInstance;
    private GameObject cartInstance;
    private TMP_Text scoreText;


    private void Start()
    {
        S = this;
        SceneManager.activeSceneChanged += OnActiveSceneChange;
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
        //projControl.SetSpeed(speed);
    }


    private void OnActiveSceneChange(Scene current, Scene next)
    {
        S.spawnPoint = GameObject.Find("Conveyor/SpawnPoint").transform;
        S.crateSpawnPoint = GameObject.Find("Conveyor/CrateSpawnPoint").transform;
        S.scoreText = GameObject.Find("Canvas/Score").GetComponent<TMP_Text>();

        StartCoroutine(StartLevel());
    }


    private IEnumerator StartLevel()
    {
        score = 0;

        crateInstance = Instantiate(cratePrefab, crateSpawnPoint.position, Quaternion.identity);
        cartInstance = Instantiate(cartPrefab, new Vector3(-6, 0.5199f, 1.31f), Quaternion.identity);
        cartInstance.GetComponent<MoveToCart>().StartAnimation();

        //iterate through customers
        foreach(var customer in customers)
        {
            NewCustomer();
            currentCustomer = customer;

            // Dynamic soundtrack
            foreach (var tracks in SoundtrackManager.s.tracks)
            {
                tracks.state = SoundtrackManager.PlayState.PendingMute;
            }

            foreach (var checkout in customer.items)
            {
                SoundtrackManager.s.tracks[checkout.prefab.GetComponent<Grabbable>().instrument].state = SoundtrackManager.PlayState.Pending;
            }

            //print(customer.name);
            StartCoroutine(SpawnItems(customer.items));
            yield return new WaitUntil(() => !isSpawningItems);
            Instantiate(separatorPrefab, spawnPoint.position, Quaternion.identity);
            yield return new WaitForSeconds(7.5f);
        }
    }

    private void NewCustomer()
    {
        GameObject customer = Instantiate(customerPrefab);
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

        // Animate cart
        cartInstance.GetComponent<MoveToCart>().StartAnimation();

    }
}
