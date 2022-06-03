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
    float timeSinceSave = 0f;
    float loadCooldown = 30f;
    float timeSinceLoad = 0f;

    //Building variables
    public GameObject tileButtonPrefab;
    public GameObject tileButtonParent;
    GameObject[] tileButtonArray = new GameObject[231];

    //Saving variables
    public int[] tileArray = new int[231];
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

        UnpackTileInt(1209023);
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

                newTile.transform.parent = tileButtonParent.transform;

                Debug.Log(x + 10 + (y - 1) * 21);

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
        }
        else
        {
            saveFile = File.Create(saveDestination);
        }

        GetData();
        GameData saveData = new GameData(tileArray, playerCash, playerSeeds, playerCrops, playerWood, playerOres, playerPaths, playerLand, wellStatus, bucketStatus, axeStatus, pickaxeStatus);
        BinaryFormatter formatter = new BinaryFormatter();
        formatter.Serialize(saveFile, saveDestination);
        saveFile.Close();
    }

    public void LoadButtonPress()
    {
        Debug.Log("load pressed");
        if (Time.time > timeSinceLoad + loadCooldown)
        {
            timeSinceLoad = Time.time;
            LoadGame();
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
        GameData saveData = (GameData)formatter.Deserialize(saveFile);
        SetData(saveData);
        saveFile.Close();
    }

    void SetData(GameData saveData)
    {
        tileArray = saveData.tileArray;
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
        for(int i = 0; i < tileArray.Length; i++)
        {
            int num = tileArray[i];
            if(num != 0)
            {

            }
        }
    }

    //stores important save game information into local variables such as the player's cash
    void GetData()
    {
        //money
        //seeds
        //crops
        //wood
        //stones
        //path tiles
        //farmland
        //well status
        //bucket status
        //axe status
        //picaxe status


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
        tileArray[index] = value;
    }



    /*void OldSavingAndLoading()
    {
        //save

        GetData();
        //stores all of the data in PlayerPrefs
        PlayerPrefs.SetFloat("Player Cash", playerSaveCash);

        PlayerPrefs.SetInt("Player Crops 0", playerSaveCropCount[0]);
        PlayerPrefs.SetInt("Player Crops 1", playerSaveCropCount[1]);
        PlayerPrefs.SetInt("Player Crops 2", playerSaveCropCount[2]);

        PlayerPrefs.SetInt("Player Seeds 0", playerSaveSeedsCount[0]);
        PlayerPrefs.SetInt("Player Seeds 1", playerSaveSeedsCount[1]);
        PlayerPrefs.SetInt("Player Seeds 2", playerSaveSeedsCount[2]);

        PlayerPrefs.SetInt("Player Land", playerSaveLandCount);
        PlayerPrefs.SetInt("Player Wood", playerSaveWoodCount);
        PlayerPrefs.SetInt("Player Well Status", wellStatusSave);
        PlayerPrefs.SetInt("Player Water Can Status", waterCanStatusSave);
        PlayerPrefs.SetInt("Player Axe Status", axeStatusSave);
        PlayerPrefs.SetInt("Player Pickaxe Status", pickaxeStatusSave);
        PlayerPrefs.SetInt("Player Plots", playerPlotSave);



        //load
        int[] crops = new int[3];
        int[] seeds = new int[3];

        //takes the player's cash, wheat, and seeds from the PlayerPrefs file
        float cash = PlayerPrefs.GetFloat("Player Cash");

        crops[0] = PlayerPrefs.GetInt("Player Crops 0");
        crops[1] = PlayerPrefs.GetInt("Player Crops 1");
        crops[2] = PlayerPrefs.GetInt("Player Crops 2");

        seeds[0] = PlayerPrefs.GetInt("Player Seeds 0");
        seeds[1] = PlayerPrefs.GetInt("Player Seeds 1");
        seeds[2] = PlayerPrefs.GetInt("Player Seeds 2");

        int wood = PlayerPrefs.GetInt("Player Wood");

        int stone = PlayerPrefs.GetInt("Player Stone");

        //Runs a function on the PlayerController script and passes it the PlayerPrefs data so it can be loaded
        playerScript.LoadGame(cash, seeds, crops, wood, stone);

        //takes the land count from PlayerPrefs and passes it to the LandController so it can be spawned
        int landCount = PlayerPrefs.GetInt("Player Land");
        int plotCount = PlayerPrefs.GetInt("Player Plots");
        //landPurchaseScript.LoadGame(landCount, plotCount);

        //Takes the purchased or unpurchased status from the PlayerPrefs and sends it to the WellController
        int wellStatus = PlayerPrefs.GetInt("Player Well Status");
        wellScript.LoadGame(wellStatus);

        //Takes the status of the tools from PlayerPrefs and sends it to the PlayerController
        int canStatus = PlayerPrefs.GetInt("Player Water Can Status");
        if (canStatus > 0)
        { playerScript.ActivateWateringCan(true); }
        int axeStatus = PlayerPrefs.GetInt("Player Axe Status");
        if (axeStatus > 0)
        { playerScript.ActivateAxe(true); }
        int pickaxeStatus = PlayerPrefs.GetInt("Player Pickaxe Status");
        if (pickaxeStatus > 0)
        { playerScript.ActivatePickaxe(true); }
        Debug.Log("this is running");

        //Prints the loaded data to the console
        Debug.Log("Loaded Cash:" + cash);
        Debug.Log("Loaded Land:" + landCount);
        Debug.Log("Loaded Well:" + wellStatus);
    }*/
}
