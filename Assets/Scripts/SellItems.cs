using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellItems : MonoBehaviour
{
    public GameObject sellUI;
    bool checkForInput = false;
    public int[] cropSellPrice;
    public int[] otherSellPrice;
    bool openedUI = false;
    private void Start()
    {
        OpenUI();
    }

    // Checks for player input while they stand on the tile
    void Update()
    {
        if (checkForInput)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (!openedUI)
                { OpenUI(); }
                else if (openedUI)
                { CloseUI(); }
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            { CloseUI(); }
        }
        else
        { CloseUI(); }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        checkForInput = true;
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        checkForInput = false;
        sellUI.SetActive(false);
    }

    public void OpenUI()
    {
        sellUI.SetActive(true);
        openedUI = true;
    }
    public void CloseUI()
    {
        sellUI.SetActive(false);
        openedUI = false;
    }

    public void SellCrops(int cropType)
    {
        PlayerController.playerMoney += PlayerController.crops[cropType] * cropSellPrice[cropType];
        PlayerController.crops[cropType] = 0;
    }
    public void SellWood()
    {
        PlayerController.playerMoney += PlayerController.playerWoodCount * otherSellPrice[0];
        PlayerController.playerWoodCount = 0;
    }
    public void SellStone()
    {
        PlayerController.playerMoney += PlayerController.playerStoneCount * otherSellPrice[1];
        PlayerController.playerStoneCount = 0;
    }

    public void SellOres()
    {
        //iron
        PlayerController.playerMoney += PlayerController.playerIronCount * otherSellPrice[2];
        PlayerController.playerIronCount = 0;
        //coal
        PlayerController.playerMoney += PlayerController.playerCoalCount * otherSellPrice[3];
        PlayerController.playerCoalCount = 0;
        //gold
        PlayerController.playerMoney += PlayerController.playerGoldCount * otherSellPrice[4];
        PlayerController.playerGoldCount = 0;
        //copper
        PlayerController.playerMoney += PlayerController.playerCopperCount * otherSellPrice[5];
        PlayerController.playerCopperCount = 0;
        //sapphire
        PlayerController.playerMoney += PlayerController.playerSapphireCount * otherSellPrice[6];
        PlayerController.playerSapphireCount = 0;
    }
}
