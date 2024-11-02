using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using static UnityEditor.Progress;

public class GameManager : MonoBehaviour
{
    public List<Customer> customers;

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



    






    //public void SpawnItem(GameObject itemPrefab)
    //{
    //    GameObject item = Instantiate(itemPrefab);
    //    item.transform.position = spawnPoint.transform.position;
    //    ItemControl projControl = item.GetComponent<ItemControl>();
    //    projControl.SetSpeed(speed);
    //}
}
