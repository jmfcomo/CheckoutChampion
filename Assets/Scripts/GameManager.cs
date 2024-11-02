using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using static UnityEditor.Progress;

public class GameManager : MonoBehaviour
{
    public List<Customer> customers;


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
}
