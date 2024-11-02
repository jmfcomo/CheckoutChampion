using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using static UnityEditor.Progress;

public class GameManager : MonoBehaviour
{
    public Customer John;


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

    //public List<Customer> customers = new List<Customer>
    //{
    //    new Customer 
    //    {
    //        name = "Alice",
    //        dialogue = "I'd like to buy some apples and oranges.",
    //        items = new List<CheckoutItem>
    //        {
    //            new CheckoutItem { prefab = applePrefab, quantity = 3 },
    //            new CheckoutItem { prefab = orangePrefab, quantity = 2 }
    //        }
    //    },
    //};
}
