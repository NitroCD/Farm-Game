using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //save data variables
    public static float playerSaveCash;
    public static int playerSaveLandCount;
    public static int[] playerSaveSeedsCount = new int[3];
    public static int[] playerSaveCropCount = new int[3];
    public static int playerSaveWoodCount;
    public static int wellStatusSave;
    public static int waterCanStatusSave;
    public static int axeStatusSave;
    public static int pickaxeStatusSave;
    public static int playerPlotSave;

    //Tree variables
    public GameObject treePrefab;
    public Transform[] treeSpawnAreas;
    public GameObject treeSpawnParent;

    //Stone variables
    public GameObject stonePrefab;
    public GameObject stoneSpawnParent;
    public Transform[] stoneSpawnAreas;

    //load data variables
    public PlayerController playerScript;
    public WellController wellScript;

    //variables for saving
    public GameObject saveUI;
    //move these two buttons below to the slotButtonArray (and make it just a buttonArray
    public GameObject saveButton;
    public GameObject loadButton;
    public GameObject[] slotButtonArray;
    //keep these even if it says they are not used (they are used)
    Color saveButtonColour;
    Color loadButtonColour;
    //Color slotButtonColour;
    //public static bool saveMenuOpen;

    //Building variables
    public GameObject tileButtonPrefab;
    public GameObject tileButtonParent;

    void Start()
    {
        //turn off the UI
        SaveMenuCloser();
        SpawnTileButtons();
        SpawnTrees();
        SpawnTreeArray();
        SpawnStoneArray();
    }

    //runs when the player presses the "Settings" button in-game
    public void SaveMenuOpener()
    {
        saveUI.SetActive(true);
    }

    //runs when the player presses the "X" button in the settings menu
    public void SaveMenuCloser()
    {
        saveUI.SetActive(false);
    }

    //spawns trees in every transform in the spawn array
    public void SpawnTrees()
    {
        for (int i = 0; i < 28; i++)
        {
            Instantiate(treePrefab, treeSpawnAreas[i]);
        }
    }

    void SpawnTileButtons()
    {
        for(int i= -10; i < 11; i++)
        {
            for(int j=1; j < 12; j++)
            {
                Vector3 position = new Vector3(i, j, 0);
                GameObject newTile = Instantiate(tileButtonPrefab, position, Quaternion.identity);

                newTile.transform.parent = tileButtonParent.transform;
            }
        }
        tileButtonParent.SetActive(false);
    }

    void SpawnStoneArray()
    {
        bool canSpawn = true;
        float randomRange = 0.45f;
        for(int x = -30; x < 31; x++)
        {
            for(int y = 10; y > -11; y--)
            { 
                if(canSpawn)
                {
                    float randomFloatX = Random.Range(-randomRange, randomRange);
                    float randomFloatY = Random.Range(-randomRange, randomRange);
                    GameObject newStone = Instantiate(stonePrefab, stoneSpawnParent.transform);
                    Vector3 position = new Vector3(x+randomFloatX, y+randomFloatY - 25, 0);
                    newStone.transform.position = position;
                    canSpawn = false;
                }
                else
                {
                    canSpawn = true;
                }
            }
        }
    }
    void SpawnTreeArray()
    {
        bool canSpawn = true;
        float randomRange = 0.5f;
        for (int x = -30; x < 31; x += 3)
        {
            for (int y = 10; y > -11; y -= 3)
            {
                if (canSpawn)
                {
                    float randomFloatX = Random.Range(-randomRange, randomRange);
                    float randomFloatY = Random.Range(-randomRange, randomRange);
                    GameObject newTree = Instantiate(treePrefab, treeSpawnParent.transform);
                    Vector3 position = new Vector3(x + randomFloatX, y + randomFloatY + 35, 0);
                    newTree.transform.position = position;
                    //canSpawn = false;
                }
                else
                {
                    canSpawn = true;
                }
            }
        }
    }

    //saves the game (triggered by "save" button)
    public void SaveGame()
    {
        
    }
    
    //loads the game using stored PlayerPrefs data, (triggered by "load" button)
    public void LoadGame()
    {
        
    }

    //stores important save game information into local variables such as the player's cash
    void GetData()
    {
        
    }
}
