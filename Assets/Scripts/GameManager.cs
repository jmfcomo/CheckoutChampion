using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using static UnityEditor.Progress;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public List<Customer> customers;
    static private GameManager S;
    public GameObject customerPrefab;
    public float itemSpawnWait;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        //GameObjectEvents
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
    private bool isSpawningItems =false;


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
            print("random ind = " + randomIndex);
            print("tempQuantities[randomIndex] = " + tempQuantities[randomIndex]);
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
            print("random ind after = " + randomIndex);

            var randomItem = checkoutItems[randomIndex].prefab;
            tempQuantities[randomIndex]--;
            SpawnItem(randomItem);
            yield return new WaitForSeconds(1f);
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
        
        print(spawnPoint);

        StartCoroutine(StartLevel());
    }


    private IEnumerator StartLevel()
    {
        //iterate through customers
        foreach(var customer in customers)
        {
            NewCustomer();
            print(customer.name);
            StartCoroutine(SpawnItems(customer.items));
            yield return new WaitUntil(() => !isSpawningItems);

        }
    }

    private void NewCustomer()
    {
        GameObject customer = Instantiate(customerPrefab);
    }

    private void EndCustomer()
    {

    }
}