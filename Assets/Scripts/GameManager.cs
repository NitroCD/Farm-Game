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

        // GameData saveData = new GameData("VARIABLE1", "VARIABLE2");
        //GameData saveData = new GameData();
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
        // Gamedata saveData = (GameData)formatter.Deserialize(saveFile);
        saveFile.Close();
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

        int[] tileArray = buildScript.tileArray();
        float playerCash = PlayerController.playerMoney;
        int[] playerSeeds = PlayerController.seeds;
        int[] playerCrops = PlayerController.crops;
        int playerWood = PlayerController.playerWoodCount;
        int[] playerOres = PlayerController.playerOreCount;
        int playerPaths = PlayerController.playerPathCount;
        int playerLand = PlayerController.playerLandCount;
        bool wellStatus = WellController.isPurchased;
        bool bucketStatus = PlayerController.wateringCanUnlocked;
        bool axeStatus = PlayerController.axeUnlocked;
        bool picaxeStatus = PlayerController.pickaxeUnlocked;
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
