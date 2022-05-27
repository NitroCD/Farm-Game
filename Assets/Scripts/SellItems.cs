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
        else if (!checkForInput)
        { CloseUI(); }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    { checkForInput = true; }

    public void OnTriggerExit2D(Collider2D collision)
    { checkForInput = false; }

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
        PlayerController.playerMoney += PlayerController.playerOreCount[0] * otherSellPrice[1];
        PlayerController.playerOreCount[0] = 0;
    }

    public void SellOres()
    {
        //starts at 1 to ignore selling stone
        for (int i = 1; i < PlayerController.playerOreCount.Length; i++)
        {
            PlayerController.playerMoney += PlayerController.playerOreCount[i] * otherSellPrice[i + 1];
            PlayerController.playerOreCount[i] = 0;
        }
    }
}
