using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.SceneManagement;


public class ShopManagerScript : MonoBehaviour
{
    public int[,] shopItems = new int[3, 8];
    public int money;
    public TMP_Text MoneyText;

    //in order for this to work properly with GameManager, these lists must be maintained in a certain order
    public List<GameObject> decorations = new List<GameObject>();
    public List<GameObject> buttons = new List<GameObject>();

    void Start()
    {
        money = GameManager.S.money;
        MoneyText.text = "$" + money.ToString();

        // Initatialize array
        for(int i = 0; i < 8; i++)
        {
            shopItems[0, i] = i;   // Assign IDs
            decorations[i].SetActive(GameManager.S.decorationsEnabled[i]);
            // Initialize quantities
            if (GameManager.S.decorationsEnabled[i]) //is this decoration already enabled
            {
                shopItems[2, i] = 1;
                buttons[i].SetActive(false);

            }
            else
            {
                shopItems[2, i] = 0;    
            }
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

        // music
        foreach (var track in SoundtrackManager.s.tracks)
        {
            track.state = SoundtrackManager.PlayState.PendingMute;
        }

        SoundtrackManager.s.tracks[7].state = SoundtrackManager.PlayState.Pending;
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

    private bool[] DecorationsStatus()
    {
        var decorationsEnabled = new bool[8];
        for(int i = 0;i< decorationsEnabled.Length;i++)
        {
            if(shopItems[2,i] == 1)
            {
                decorationsEnabled[i] = true;
            }
            else
            {
                decorationsEnabled[i] = false; 
            }
        }
            return decorationsEnabled;
;    }

    public void NextLevel()
    {
        GameManager.S.money = money;
        GameManager.S.decorationsEnabled = DecorationsStatus();
        SceneManager.LoadScene(1);
    }
}
