using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeController : MonoBehaviour
{
    //private variables
    bool checkForInput = false;
    float harvestCooldown;
    float timeSinceHarvest;
    bool isActive = true;
    int timesHit = 0;
    int requiredHits;
    int woodIncrement;
    TreeType thisTreeType;

    //public variables
    public GameObject treeEffectsPrefab;
    public GameObject[] treeTypes;
    public Collider2D[] treeColliderArray;

    private void Start()
    {
        // Turn off the trees,
        Activate(false);
        // Randomize the type of tree,
        randomizeTreeType();
        // Turn back on the trees.
        Activate(true);
    }

    private void Update()
    {
        //activate the tree if its time
        if(!isActive)
        {
            if (Time.time - timeSinceHarvest >= harvestCooldown)
            {
                Activate(true);
            }
        }
        //if its active, see if the player can harvest
        else
        {
            if (checkForInput)
            {
                if (PlayerController.currentHBSlot == 3 && PlayerController.currentHotbar == Hotbar.Tool && PlayerController.axeUnlocked && Input.GetKeyDown(KeyCode.E))
                {
                    Harvest();
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    { checkForInput = true; }
    private void OnTriggerExit2D(Collider2D collision)
    { checkForInput = false; }

    void Harvest()
    {
        switch (thisTreeType)
        {
            case TreeType.treeMedium:
            ///change these ints for different tree types
                requiredHits = 3; //input 1 for the tree to be broken in 1 hit, 2 for 2 hits, etc.
                woodIncrement = 1; //amount of wood given when the tree is broken

                // increments the hit count and spawns particles
                if (timesHit < requiredHits - 1)
                {
                    timesHit++;
                    Instantiate(treeEffectsPrefab, gameObject.GetComponentInParent<Transform>());
                }
                // On the third hit (0,1,2), destroy the object
                else if (timesHit == requiredHits - 1)
                {
                    timesHit = 0;
                    Instantiate(treeEffectsPrefab, gameObject.GetComponentInParent<Transform>());
                    timeSinceHarvest = Time.time;
                    harvestCooldown = Random.Range(40, 61);
                    PlayerController.playerWoodCount = PlayerController.playerWoodCount + woodIncrement;
                    Activate(false);
                    randomizeTreeType();
                }
                break;
            case TreeType.treeLarge:
            ///change these ints for different tree types
                requiredHits = 5; //input 1 for the tree to be broken in 1 hit, 2 for 2 hits, etc.
                woodIncrement = 2; //amount of wood given when the tree is broken

                // increments the hit count and spawns particles
                if (timesHit < requiredHits - 1)
                {
                    timesHit++;
                    Instantiate(treeEffectsPrefab, gameObject.GetComponentInParent<Transform>());
                }
                // On the fifth hit (0,1,2,3,4), destroy the object
                else if (timesHit == requiredHits - 1)
                {
                    timesHit = 0;
                    Instantiate(treeEffectsPrefab, gameObject.GetComponentInParent<Transform>());
                    timeSinceHarvest = Time.time;
                    harvestCooldown = Random.Range(40, 61);
                    PlayerController.playerWoodCount = PlayerController.playerWoodCount + woodIncrement;
                    Activate(false);
                    randomizeTreeType();
                }
                break;
        }
    }

    void Activate(bool recievedBool)
    {
        isActive = recievedBool;
        switch (thisTreeType)
        {
            case TreeType.treeMedium:
                if (isActive)
                {
                    treeTypes[0].SetActive(isActive);
                    treeTypes[1].SetActive(!isActive);
                    treeColliderArray[0].enabled = true;
                    treeColliderArray[1].enabled = false;
                }
                else if (!isActive)
                {
                    treeTypes[0].SetActive(isActive);
                    treeTypes[1].SetActive(isActive);
                    treeColliderArray[0].enabled = false;
                    treeColliderArray[1].enabled = false;
                }
                break;
            case TreeType.treeLarge:
                if (isActive)
                {
                    treeTypes[0].SetActive(!isActive);
                    treeTypes[1].SetActive(isActive);
                    treeColliderArray[0].enabled = false;
                    treeColliderArray[1].enabled = true;
                }
                else if (!isActive)
                {
                    treeTypes[0].SetActive(isActive);
                    treeTypes[1].SetActive(isActive);
                    treeColliderArray[0].enabled = false;
                    treeColliderArray[1].enabled = false;
                }
                break;
        }
    }
    private void randomizeTreeType()
    {
        int randomTree = Random.Range(0, 100);
        switch (randomTree)
        {
            // 60% of the time it will be a medium tree
            case var n when n < 60:
                thisTreeType = TreeType.treeMedium;
                break;
            // 40% of the time it will be a large tree
            case var n when n < 100:
                thisTreeType = TreeType.treeLarge;
                break;
        }
    }
}

public enum TreeType
{
    treeMedium = 0,
    treeLarge,
}