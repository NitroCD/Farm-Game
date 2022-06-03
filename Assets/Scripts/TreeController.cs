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
    //public GameObject[] effectTypes 

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
                    HarvestController();
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    { checkForInput = true; }
    private void OnTriggerExit2D(Collider2D collision)
    { checkForInput = false; }

    void HarvestController()
    {
        switch (thisTreeType)
        {
            case TreeType.treeMedium:
                requiredHits = 3; //input 1 for the tree to be broken in 1 hit, 2 for 2 hits, etc.
                woodIncrement = 1; //amount of wood given when the tree is broken
                Harvest(requiredHits, woodIncrement);
                break;
            case TreeType.treeLarge:
                requiredHits = 5; //input 1 for the tree to be broken in 1 hit, 2 for 2 hits, etc.
                woodIncrement = 2; //amount of wood given when the tree is broken
                Harvest(requiredHits, woodIncrement);
                break;
                ///when adding new trees:
                // 1. change the required hits, input 1 for the tree to be broken in 1 hit, 2 for 2 hits, etc.
                // 2. change the increments int to the amount of wood given when the tree is broken
        }
    }

    void Harvest(int hits, int wood)
    {
        // increments the hit count and spawns particles
        if (timesHit < hits - 1)
        {
            timesHit++;
            Instantiate(treeEffectsPrefab, gameObject.GetComponentInParent<Transform>());
        }
        // On the nth hit (0,1,2... n), destroy the object
        else if (timesHit == hits - 1)
        {
            timesHit = 0;
            Instantiate(treeEffectsPrefab, gameObject.GetComponentInParent<Transform>());
            timeSinceHarvest = Time.time;
            harvestCooldown = Random.Range(40, 61);
            PlayerController.playerWoodCount += wood;
            Activate(false);
            randomizeTreeType();
        }
    }

    void Activate(bool recievedBool)
    {
        isActive = recievedBool;
        int tree = (int)thisTreeType;

        // these arrays must be the same length
        for (int i = 0; i < treeTypes.Length; i++)
        {
            treeTypes[i].SetActive(false);
            treeColliderArray[i].enabled = false;
        }
        if (isActive)
        {
            treeTypes[tree].SetActive(true);
            treeColliderArray[tree].enabled = true;
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