using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandController : MonoBehaviour
{
    int plantedSeedType = 0;

    bool isPlanted = false;
    bool isHarvestable = false;
    bool isWatered = false;
    bool checkForInput = false;
    float growthTime;
    public float[] defaultGrowthTime;
    float growthCount = 0f;
    int growthStage = 0;
    int spriteCount = 0;
    public static int[] currentCropsPlanted = new int[3];
    public GameObject particleGreen1;
    bool particleInstantiated = false;
    SpriteRenderer thisSR;
    public Sprite[] landSprites;
    GameObject greenParticle;

    //Build mode variables
    public GameObject deleteButton;
    Build parentScript;
    GameObject thisGO;


    private void Start()
    {
        thisSR = gameObject.GetComponentInChildren<SpriteRenderer>();
        thisSR.sprite = landSprites[0];
        thisGO = gameObject;
        parentScript = transform.parent.gameObject.GetComponent<Build>();
    }

    void Update()
    {
        //updates the growth stage while a crop is planted
        if (isPlanted)
        {
            UpdateGrowth();
            if (Time.time >= growthCount + growthTime)
            { isHarvestable = true; }
        }

        //is true while the player stands on the tile
        if (checkForInput)
        {
            if (Input.GetKey(KeyCode.E))
            {
                //checks if the player can water the tile
                if(PlayerController.playerWaterCount > 0 && PlayerController.currentHBSlot == 2 && PlayerController.currentHotbar == Hotbar.Tool && !isWatered)
                {
                    WaterCrops();
                    PlayerController.playerWaterCount--;
                }
                //checks if the player can plant a seed
                if (!isPlanted && PlayerController.seeds[PlayerController.seedSelection] > 0 && PlayerController.currentHBSlot == 1 && PlayerController.currentHotbar == Hotbar.Tool)
                { PlantSeed(); }
                //checks if the player can harvest a crop
                if (isPlanted && isHarvestable && PlayerController.currentHBSlot == 0 && PlayerController.currentHotbar == Hotbar.Tool)
                {
                    Harvest();                    
                }
            }
        }

        if (PlayerController.buildModeActive)
        {
            deleteButton.SetActive(true);
        }
        else
        {
            deleteButton.SetActive(false);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    { checkForInput = true; }
    public void OnTriggerExit2D(Collider2D collision)
    { checkForInput = false; }

    //called when the player is holding the shovel tool while stadning on the tile and pressing E
    void PlantSeed()
    {
        plantedSeedType = PlayerController.seedSelection;
        isPlanted = true;
        currentCropsPlanted[plantedSeedType]++;
        PlayerController.seeds[PlayerController.seedSelection]--;
        growthCount = Time.time;
        growthTime = defaultGrowthTime[plantedSeedType];
        if (isWatered)
        { growthTime = growthTime / 2; }

    }

    //called when the player is holding the harvesting tool while stadning on the tile and pressing E
    void Harvest()
    {
        growthStage = 0;
        currentCropsPlanted[plantedSeedType] --;
        isHarvestable = false;
        isPlanted = false;
        isWatered = false;
        PlayerController.crops[plantedSeedType]++;
        thisSR.sprite = landSprites[0];
        Destroy(greenParticle);
        particleInstantiated = false;
        plantedSeedType = 0;
    }

    //called when the player holds the watering can while standing on the tile and pressing E
    void WaterCrops()
    {
        isWatered = true;
        UpdateSprite();
        growthTime = growthTime / 2;
    }

    //updates the growth stage based on how long it was planted
    void UpdateGrowth()
    {
        if (Time.time < growthCount + growthTime / 2)
        {
            growthStage = 1;
            UpdateSprite();
        }
        else if(Time.time < growthCount + growthTime)
        {
            growthStage = 2;
            UpdateSprite();
        }
        else if(Time.time >= growthCount + growthTime)
        {
            growthStage = 3;
            UpdateSprite();
            isHarvestable = true;
            if (!particleInstantiated)
            {
                greenParticle = Instantiate(particleGreen1, gameObject.GetComponentInParent<Transform>());
                particleInstantiated = true;
            }
        }
    }

    //changes the sprite when the tile is growing, or is watered
    void UpdateSprite()
    {
        spriteCount = growthStage + plantedSeedType*8;
        if(isWatered)
        { spriteCount += 4; }
        thisSR.sprite = landSprites[spriteCount];
    }

    public void DeleteTile(GameObject parent)
    {
        Debug.Log("boo");
        parentScript.DeletedTile(parent, 0);

        Destroy(parent);
    }
}
