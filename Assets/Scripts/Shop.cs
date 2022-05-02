using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public float[] seedPrice;
    public float landPrice = 50f;
    public float pathPrice = 10f;
    bool checkForInput = false;
    public GameObject shopUI;
    public GameObject seedsUI;
    bool openedUI= false;
    //public BuyLand buyLandScript;

    private void Start()
    {
        OpenUI();
    }

    // Checks for player input while they stand on the tile
    void Update()
    {
        if (checkForInput)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                if (!openedUI)
                {
                    OpenUI();
                    openedUI = true;
                }
                else if (openedUI)
                {
                    CloseUI();
                }
            }
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                CloseUI();
                CloseSeedUI();
            }
        }
        else
        {
            CloseUI();
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        checkForInput = true;
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        checkForInput = false;
        shopUI.SetActive(false);
        seedsUI.SetActive(false);
    }

    public void BuySeed(int seedType)
    {
        if (PlayerController.playerMoney >= seedPrice[seedType] * 10 && Input.GetKey(KeyCode.LeftShift))
        {
            PlayerController.playerMoney -= seedPrice[seedType] * 10;
            PlayerController.seeds[seedType] += 10;
        }
        else if (PlayerController.playerMoney >= seedPrice[seedType])
        {
            PlayerController.playerMoney -= seedPrice[seedType];
            PlayerController.seeds[seedType]++;
        }
    }

    public void BuyPath()
    {
        if (PlayerController.playerMoney >= pathPrice * 10 && Input.GetKey(KeyCode.LeftShift))
        {
            PlayerController.playerMoney -= pathPrice * 10;
            PlayerController.playerPathCount += 10;
        }
        else if (PlayerController.playerMoney >= landPrice)
        {
            PlayerController.playerMoney -= landPrice;
            PlayerController.playerLandCount++;
        }
    }

    public void BuyLand()
    {
        if(PlayerController.playerMoney >= landPrice*10 && Input.GetKey(KeyCode.LeftShift))
        {
            PlayerController.playerMoney -= landPrice*10;
            PlayerController.playerLandCount += 10;
        }
        else if (PlayerController.playerMoney >= landPrice)
        {
            PlayerController.playerMoney -= landPrice;
            PlayerController.playerLandCount++;
        }
    }

    public void OpenUI()
    {
        shopUI.SetActive(true);
    }
    public void CloseUI()
    {
        shopUI.SetActive(false);
        openedUI = false;
    }

    public void OpenSeedUI()
    {
        seedsUI.SetActive(true);
    }

    public void CloseSeedUI()
    {
        seedsUI.SetActive(false);
    }
}
