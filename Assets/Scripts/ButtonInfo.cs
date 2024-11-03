using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;
using TMPro;

public class ButtonInfo : MonoBehaviour
{
    public int itemID;
    public TMP_Text PriceText;
    public GameObject ShopManager;

    void Start()
    {
        if (PriceText == null)
            PriceText = transform.Find("PriceText").GetComponent<TMP_Text>();
    }

    void Update()
    {
        PriceText.text = "Price: $" + ShopManager.GetComponent<ShopManagerScript>().shopItems[1, itemID].ToString();
    }
}
