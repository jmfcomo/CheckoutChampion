using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;


public class ShopManagerScript : MonoBehaviour
{
    public int[,] shopItems = new int[3, 8];
    public float money;
    public TMP_Text MoneyText;

    public List<GameObject> decorations = new List<GameObject>();

    void Start()
    {
        MoneyText.text = "$" + money.ToString();

        // Initatialize array
        for(int i = 0; i < 8; i++)
        {
            shopItems[0, i] = i;   // Assign IDs
            shopItems[2, i] = 0;    // Initialize quantities
        }

        // Prices
        shopItems[1, 0] = 300;
        shopItems[1, 1] = 200;
        shopItems[1, 2] = 100;
        shopItems[1, 3] = 20;
        shopItems[1, 4] = 50;
        shopItems[1, 5] = 40;
        shopItems[1, 6] = 30;
        shopItems[1, 7] = 500;
    }

    public void Buy ()
    {
        GameObject ButtonRef = EventSystem.current.currentSelectedGameObject;
        int itemID = ButtonRef.GetComponent<ButtonInfo>().itemID;

        if (money >= shopItems[1, itemID])
        {
            money -= shopItems[1, itemID];
            MoneyText.text = "$" + money.ToString();
            shopItems[2, itemID] = 1;

            decorations[itemID].SetActive(true);
            ButtonRef.SetActive(false);
        }
    }
}
