using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class GameManager : MonoBehaviour
{
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
    public Build buildScript;

    //move these two buttons below to the slotButtonArray (and make it just a buttonArray
    public GameObject saveButton;
    public GameObject loadButton;
    public GameObject[] slotButtonArray;
    //keep these even if it says they are not used (they are used)
    Color saveButtonColour;
    Color loadButtonColour;
    //Color slotButtonColour;
    //public static bool saveMenuOpen;
    float saveCooldown = 30f;
    float timeSinceSave = -30f;
    float loadCooldown = 30f;
    float timeSinceLoad = -30f;

    //Building variables
    public GameObject tileButtonPrefab;
    public GameObject tileButtonParent;
    GameObject[] tileButtonArray = new GameObject[231];

    //Saving variables
    public int[] localTileArray = new int[231];
    public float playerCash;
    public int[] playerSeeds;
    public int[] playerCrops;
    public int playerWood;
    public int[] playerOres;
    public int playerPaths;
    public int playerLand;
    public bool wellStatus;
    public bool bucketStatus;
    public bool axeStatus;
    public bool pickaxeStatus;

    void Start()
    {
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
        for(int x= -10; x < 11; x++)
        {
            for(int y=1; y < 12; y++)
            {
                Vector3 position = new Vector3(x, y, 0);
                GameObject newTile = Instantiate(tileButtonPrefab, position, Quaternion.identity);
                newTile.GetComponent<Build>().gameManager = gameObject.GetComponent<GameManager>();

                newTile.transform.parent = tileButtonParent.transform;

                tileButtonArray[x+10 + (y-1)*21] = newTile;
            }
        }
        tileButtonParent.SetActive(false);
    }

    void LoadTiles(int index, int type, int rotationInt)
    {
        Quaternion rotation = Quaternion.Euler(0,0,rotationInt*-90);

        tileButtonArray[index].GetComponent<Build>().BuildTile(type, rotation);
    }

    void UnpackTileInt(int value)
    {
        int xPos = Mathf.FloorToInt(value / 100000);
        int yPos = Mathf.FloorToInt(value / 1000 - (xPos * 100));
        int index = xPos + (yPos - 1) * 21;
        int type = Mathf.FloorToInt((value / 10) - (xPos * 10000) - (yPos * 100));
        int rotation = value - (xPos * 100000) - (yPos * 1000) - (type*10);
        LoadTiles(index - 1, type, rotation);
    }

    // 1234067

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

    public void SaveButtonPress()
    {
        Debug.Log("save pressed");
        if (Time.time > timeSinceSave + saveCooldown)
        {
            timeSinceSave = Time.time;
            SaveGame();
        }
    }

    public void SaveGame()
    {
        string saveDestination = Application.persistentDataPath + "/save.dat";
        FileStream saveFile;

        if (File.Exists(saveDestination))
        {
            saveFile = File.OpenWrite(saveDestination);
            Debug.Log("Found an existing file at: " + saveDestination);
        }
        else
        {
            saveFile = File.Create(saveDestination);
            Debug.Log("Created a new file at: " + saveDestination);
        }

        GetData();
        GameData saveData = new GameData(localTileArray, playerCash, playerSeeds, playerCrops, playerWood, playerOres, playerPaths, playerLand, wellStatus, bucketStatus, axeStatus, pickaxeStatus);
        BinaryFormatter formatter = new BinaryFormatter();
        formatter.Serialize(saveFile, saveDestination);
        saveFile.Close();
    }

    public void LoadButtonPress()
    {
        Debug.Log("load pressed");
        if (Time.time > timeSinceLoad + loadCooldown)
        {
            PlayerController.buildModeActive = true;
            timeSinceLoad = Time.time;
            LoadGame();
            PlayerController.buildModeActive = false;
            Debug.Log("Loaded the game");
        }
    }
    
    public void LoadGame()
    {
        string saveDestination = Application.persistentDataPath + "/save.dat";
        FileStream saveFile;

        if (File.Exists(saveDestination))
        {
            saveFile = File.OpenRead(saveDestination);
        }
        else
        {
            Debug.Log("Loaded but there is no save");
            return;
        }

        BinaryFormatter formatter = new BinaryFormatter();
        GameData saveData = formatter.Deserialize(saveFile) as GameData;
        SetData(saveData);
        saveFile.Close();
    }

    void SetData(GameData saveData)
    {
        localTileArray = saveData.tileArray;
        PlayerController.playerMoney = saveData.money;
        PlayerController.seeds = saveData.seeds;
        PlayerController.crops = saveData.crops;
        PlayerController.playerWoodCount = saveData.wood;
        PlayerController.playerOreCount = saveData.ores;
        PlayerController.playerPathCount = saveData.paths;
        PlayerController.playerLandCount = saveData.land;
        WellController.isPurchased = saveData.well;
        PlayerController.wateringCanUnlocked = saveData.bucket;
        PlayerController.axeUnlocked = saveData.axe;
        PlayerController.pickaxeUnlocked = saveData.pickaxe;
    }

    void SetTiles()
    {
        for(int i = 0; i < localTileArray.Length; i++)
        {
            UnpackTileInt(localTileArray[i]);
        }
    }

    //stores important save game information into local variables such as the player's cash
    void GetData()
    {
        playerCash = PlayerController.playerMoney;
        playerSeeds = PlayerController.seeds;
        playerCrops = PlayerController.crops;
        playerWood = PlayerController.playerWoodCount;
        playerOres = PlayerController.playerOreCount;
        playerPaths = PlayerController.playerPathCount;
        playerLand = PlayerController.playerLandCount;
        wellStatus = WellController.isPurchased;
        bucketStatus = PlayerController.wateringCanUnlocked;
        axeStatus = PlayerController.axeUnlocked;
        pickaxeStatus = PlayerController.pickaxeUnlocked;
}

    public void updateTileArray(int index, int value)
    {
        localTileArray[index] = value;
    }
}
