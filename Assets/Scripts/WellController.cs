using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WellController : MonoBehaviour
{
    public GameObject PriceUI;

    public static bool isPurchased = false;
    bool checkForInput = false;

    public float wellCost = 200f;

    Animator wellAnimator;
    //SpriteRenderer wellSR;
    //public Sprite fullWell;

    //unlocks or locks the well if it was purchased and saved in PlayerPrefs
    public void LoadGame(int hasWell)
    {
        if(hasWell == 1)
        {
            isPurchased = true;
            PriceUI.SetActive(false);
            wellAnimator = GetComponentInChildren<Animator>();
            wellAnimator.enabled = true;
        }
        else
        {
            isPurchased = false;
            PriceUI.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //chechs if the player is standing on the tile and pressing E
        if (checkForInput)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (PlayerController.playerMoney >= wellCost && !isPurchased)
                {
                    PurchaseWell();
                }
                else if (PlayerController.currentHBSlot == 2 && PlayerController.wateringCanUnlocked && isPurchased)
                {
                    FillWaterCan();
                }
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        checkForInput = true;
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        checkForInput = false;
    }

    //purchases the well
    void PurchaseWell()
    {
        isPurchased = true;
        PlayerController.playerMoney -= wellCost;
        PriceUI.SetActive(false);

        //turn on the well animator
        wellAnimator = GetComponentInChildren<Animator>();
        wellAnimator.enabled = true;
    }

    //fills the water can
    void FillWaterCan()
    {
        PlayerController.playerWaterCount = 64;
    }
}
