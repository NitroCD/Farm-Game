using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //player
    public Rigidbody2D playerRB;
    public static float playerMoney;
    public static int[] seeds = new int[3];
    public static int seedSelection = 0;
    public static int[] crops = new int[3];
    public static int playerWaterCount = 0;
    int waterCountComparable = 0;
    public static bool wateringCanUnlocked = false;
    public static int playerWoodCount;
    public static int[] playerOreCount = new int[6];
    public static bool axeUnlocked = false;
    public static bool pickaxeUnlocked = false;
    Animator playerAnimator;

    //Movement
    float playerSpeed = 5f;
    float dirtPathBoost = 1.4f;
    public static int touchingDirtPath = 0;

    //tiles
    public static int playerLandCount = 0;
    public static int playerPathCount = 0;

    //Item GameObjects
    public GameObject playerWateringCan;
    public GameObject playerAxe;
    public GameObject playerPickaxe;
    public Image waterCanImage;
    public Sprite[] wateringCanSprite;

    //Item prices
    float wateringCanPrice = 50f;
    float axePrice = 400f;
    float pickaxePrice = 5000f;

    ///UI
    public GameObject inventoryUI;
    public Text moneyText;
    public Text[] cropText;
    public Text[] seedsText;
    public GameObject[] cropUIs;
    public GameObject[] seedUIs;
    public Text waterText;
    public Text woodText;
    public Text[] stonesText;
    public Text landText;
    public Text pathText;
    public GameObject woodUI;
    public GameObject stoneUI;
    //UI - hotbar - general
    public GameObject[] hotbarSelectionArray;
    float hotbarFlashCooldown = 1f;
    float timeSinceFlash;
    bool hotbarFlasher = false;
    public static int currentHBSlot;
    public static Hotbar currentHotbar;
    //UI - hotbar - tool
    bool startedTool;
    //UI - hotbar - building
    GameObject currentPreview;
    public GameObject[] previewArray;
    public GameObject previewParent;
    public GameObject previewHB;
    Quaternion previewRotation;
    int previewType = 0;
    public GameObject[] toolHBPrefabs;
    public GameObject[] buildHBPrefabs;
    public GameObject[] hotbarGOs;
    public GameObject dirtPathHotbar;
    //UI - item text
    public GameObject itemTextGO;
    float textCooldown = 1.0f;
    float lastItemSelection = 0f;
    float fadeCooldown = 0.04f;
    float lastFade = 0f;
    bool fadeRequired = true;
    Color currentFade = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    Color fadeReduction = new Color(0.0f, 0.0f, 0.0f, 0.05f);

    //Camera
    public static bool playerCanMove = true;
    public Transform plotCameraPosition;
    public GameObject mainCameraGO;
    float defaultCameraZoom = 5f;
    float maxCameraZoom = 10f;
    float timeLerped = 0f;
    float lerpDuration = 1f;

    //Build Mode
    public GameObject tileButtons;
    public GameObject tileUI;
    public static bool buildModeActive = false;
    bool zoomedOut = false;
    public static int dirtPathSelection = 0;
    public static int currentRotation = 0;
    bool readyToZoomIn = false;
    bool canOpenBuildMenu = true;

    HelpBoxController thisHelpBoxController;

    void Start()
    {
        //Setting variables for the start of the game
        SetItemText("Harvesting Scythe");
        playerMoney = 250f;        
        seeds[0] = 0;
        seeds[1] = 0;
        seeds[2] = 0;
        crops[0] = 0;
        crops[1] = 0;
        crops[2] = 0;
        cropUIs[1].SetActive(false);
        cropUIs[2].SetActive(false);
        seedUIs[1].SetActive(false);
        seedUIs[2].SetActive(false);
        playerWoodCount = 0;
        for (int i = 0; i < playerOreCount.Length; i++)
        { playerOreCount[i] = 0; }
        waterCountComparable = playerWaterCount;

        thisHelpBoxController = GameObject.FindGameObjectWithTag("HelpBoxController").GetComponent<HelpBoxController>();

        //reset the tools
        playerWateringCan.SetActive(false);
        playerAxe.SetActive(false);
        playerPickaxe.SetActive(false);

        //get the player animator and turn it off for now
        playerAnimator = gameObject.GetComponentInChildren<Animator>();
        playerAnimator.enabled = false;

        //spawn all the hotbar items, reset the hotbar items
        hotbarSelectionArray[currentHBSlot].SetActive(true);
        previewHB.SetActive(false);
        dirtPathHotbar.SetActive(false);
        currentHotbar = Hotbar.Tool;
        ResetHotbarSelection();
        ResetHotbar();

        //Turn off the locked UIs
        woodUI.SetActive(false);
        stoneUI.SetActive(false);
    }

    //move the player and update the ui every frame
    void Update()
    {
        if (waterCountComparable != playerWaterCount)
        {
            UpdateWateringCan();
            waterCountComparable = playerWaterCount;
        }

        if (playerCanMove)
        { Movement(); }
        else
        { StopMoving(); }

        if(buildModeActive)
        { BuildMode(false); }
        else if (readyToZoomIn)
        { MoveCameraIn(); }

        UpdateUI();

        // secret money doubler
        if (Input.GetKeyDown(KeyCode.Home))
        { playerMoney *= 2; }
    }

    //Called when the player enters build mode
    public void MoveCameraOut()
    {
        if (timeLerped <= lerpDuration)
        {
            Vector3 smoothPosition = Vector3.Lerp(transform.position + new Vector3(0f, 0f, -10f), plotCameraPosition.position, timeLerped / lerpDuration);
            mainCameraGO.transform.position = smoothPosition;

            float smoothFloat = Mathf.Lerp(defaultCameraZoom, maxCameraZoom, timeLerped / lerpDuration);
            mainCameraGO.GetComponent<Camera>().orthographicSize = smoothFloat;

            timeLerped += Time.deltaTime;
        }
        else if (timeLerped > lerpDuration)
        {
            mainCameraGO.transform.position = plotCameraPosition.position;

            mainCameraGO.GetComponent<Camera>().orthographicSize = maxCameraZoom;

            timeLerped = 0f;

            zoomedOut = true;
        }
    }

    //Called when the player exits build mode
    public void MoveCameraIn()
    {
        if (timeLerped <= lerpDuration)
        {            
            Vector3 smoothPosition = Vector3.Lerp(plotCameraPosition.position, GetComponentInParent<Transform>().transform.position + new Vector3(0f, 0f, -10f), timeLerped / lerpDuration);
            mainCameraGO.transform.position = smoothPosition;

            float smoothFloat = Mathf.Lerp(maxCameraZoom, defaultCameraZoom, timeLerped / lerpDuration);
            mainCameraGO.GetComponent<Camera>().orthographicSize = smoothFloat;

            timeLerped += Time.deltaTime;

            canOpenBuildMenu = false;
        }
        else if (timeLerped > lerpDuration)
        {
            mainCameraGO.transform.position = GetComponentInParent<Transform>().transform.position + new Vector3(0f, 0f, -10f);

            mainCameraGO.GetComponent<Camera>().orthographicSize = defaultCameraZoom;

            timeLerped = 0f;

            zoomedOut = false;
            canOpenBuildMenu = true;
            readyToZoomIn = false;
            playerCanMove = true;
        }
    }

    void Movement()
    {
        // player movement
        float currentXSpeed = 0;
        float currentYSpeed = 0;
        if (Input.GetKey(KeyCode.W))
        { currentYSpeed += playerSpeed; }
        if (Input.GetKey(KeyCode.S))
        { currentYSpeed -= playerSpeed; }
        if (Input.GetKey(KeyCode.A))
        { currentXSpeed -= playerSpeed; }
        if (Input.GetKey(KeyCode.D))
        { currentXSpeed += playerSpeed; }
        if (touchingDirtPath > 0)
        { currentXSpeed *= dirtPathBoost;
          currentYSpeed *= dirtPathBoost; }
        playerRB.velocity = new Vector2(currentXSpeed, currentYSpeed);

        // player animation
        playerAnimator.enabled = true;
        if (currentYSpeed > 0)
        { playerAnimator.Play("Player_Up"); }
        else if (currentYSpeed < 0)
        { playerAnimator.Play("Player_Down"); }
        else if (currentXSpeed < 0)
        { playerAnimator.Play("Player_Left"); }
        else if (currentXSpeed > 0)
        { playerAnimator.Play("Player_Right"); }
        else
        { StopMoving(); }
    }

    void StopMoving()
    {
        playerAnimator.enabled = false;
    }

    void UpdateUI()
    {
        //Update the money, seeds, and wheat count
        moneyText.text = playerMoney.ToString();
        for (int i = 0; i < cropText.Length; i++)
        { cropText[i].text = crops[i].ToString(); }
        for (int i = 0; i < seedsText.Length; i++)
        { seedsText[i].text = seeds[i].ToString(); }
        woodText.text = playerWoodCount.ToString();
        for (int i = 0; i < stonesText.Length; i++)
        { stonesText[i].text = playerOreCount[i].ToString(); }
        waterText.text = playerWaterCount.ToString();
        landText.text = playerLandCount.ToString();
        pathText.text = playerPathCount.ToString();

        // Update the hotbar
        HotBarController();
    }

    //Called when the player loads the game or buys the item
    public void ActivateAxe(bool axeLoading)
    {
        if (playerMoney >= axePrice && !axeUnlocked)
        {
            playerMoney -= axePrice;
            axeUnlocked = true;
            playerAxe.SetActive(true);
            woodUI.SetActive(true);
        }
        if (axeLoading)
        {
            axeUnlocked = true;
            playerAxe.SetActive(true);
            woodUI.SetActive(true);
        }
    }

    //Called when the player loads the game or buys the item
    public void ActivatePickaxe(bool pickaxeLoading)
    {
        if (playerMoney >= pickaxePrice && !pickaxeUnlocked)
        {
            playerMoney -= pickaxePrice;
            pickaxeUnlocked = true;
            playerPickaxe.SetActive(true);
            stoneUI.SetActive(true);
            thisHelpBoxController.ShowHelpBox(BoxNumber.stoneUnlocked);
        }
        if (pickaxeLoading)
        {
            pickaxeUnlocked = true;
            playerPickaxe.SetActive(true);
            stoneUI.SetActive(true);
        }
    }
    //Called when the player loads the game or buys the item
    public void ActivateWateringCan(bool canLoading)
    {
        if (playerMoney >= wateringCanPrice && !wateringCanUnlocked)
        {
            playerMoney -= wateringCanPrice;
            wateringCanUnlocked = true;
            playerWateringCan.SetActive(true);
        }
        if (canLoading)
        {
            wateringCanUnlocked = true;
            playerWateringCan.SetActive(true);
        }
    }
    //Used to control the activation and deactivation of build mode
    public void BuildMode(bool buildButtonPressed)
    {
        //disables opening menu while its zooming in
        if (canOpenBuildMenu)
        {
            //hide stuff and zoom out
            if (currentHotbar == Hotbar.Tool)
            {
                currentHotbar = Hotbar.Build;
                ResetHotbar();
                previewHB.SetActive(true);
                ChangeBuildPreview();
            }
            buildModeActive = true;
            tileButtons.SetActive(true);
            playerCanMove = false;
            if (inventoryUI.activeInHierarchy)
            { inventoryUI.SetActive(false); }
            if (!zoomedOut)
            { MoveCameraOut(); }
            previewArray[0].SetActive(true);
            tileUI.SetActive(true);
            thisHelpBoxController.ShowHelpBox(BoxNumber.buildOpened1);
            
            //zoom in when ready
            if (zoomedOut & (Input.GetKeyDown(KeyCode.Escape) || buildButtonPressed))
            {
                buildModeActive = false;
                inventoryUI.SetActive(true);
                readyToZoomIn = true;
                previewArray[0].SetActive(false);
                currentHotbar = Hotbar.Tool;
                ResetHotbar();
                previewHB.SetActive(false);
                tileUI.SetActive(false);
            }
        }
    }
    //Controls the visual and functional aspects of the hotbar
    void HotBarController()
    {
        //only the first time, reset the hotbar
        if (!startedTool)
        {
            ResetHotbar();
            startedTool = true;
        }

        //flash the hotbar selection overlay
        if (Time.time > hotbarFlashCooldown + timeSinceFlash)
        { HBFlash(currentHBSlot); }

        // Fade the item text above the hotbar
        if (fadeRequired)
        { FadeItemText(); }

        //if a number is pressed: turn the other hotbar selections off and turn that pressed one on, set the item text, reset the flash timer
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetHBSlot(0);
            if (currentHotbar == Hotbar.Tool)
            { SetItemText("Harvesting Scythe"); }
            else
            { SetItemText(""); }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetHBSlot(1);
            if (currentHotbar == Hotbar.Tool)
            { SetItemText("Planting Shovel"); }
            else
            { SetItemText(""); }
            if (currentHotbar == Hotbar.Build)
            { dirtPathHotbar.SetActive(true); }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SetHBSlot(2);
            if (wateringCanUnlocked && currentHotbar == Hotbar.Tool)
            { SetItemText("Watering Can"); }
            else
            { SetItemText(""); }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SetHBSlot(3);
            if (axeUnlocked && currentHotbar == Hotbar.Tool)
            { SetItemText("Woodcutting Axe"); }
            else
            { SetItemText(""); }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SetHBSlot(4);
            if (pickaxeUnlocked && currentHotbar == Hotbar.Tool)
            { SetItemText("Mining Pickaxe"); }
            else
            { SetItemText(""); }
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (currentRotation >= 3)
            { currentRotation = 0; }
            else
            { currentRotation++; }
            ChangeBuildPreview();
        }
        if (currentHotbar == Hotbar.Tool || currentHBSlot != 1)
        { dirtPathHotbar.SetActive(false); }
    }
    //Called whenever the player changes their hotbar selection
    void SetHBSlot(int hbslot)
    {
        currentHBSlot = hbslot;
        ResetHotbarSelection();
        hotbarSelectionArray[currentHBSlot].SetActive(true);
    }
    //Called whenever the player changes their hotbar selection
    void ResetHotbar()
    {
        //spawn in the current hotbar's objects
        if (currentHotbar == Hotbar.Tool)
        {
            for (int i = 0; i < buildHBPrefabs.Length; i++)
            { buildHBPrefabs[i].SetActive(false); }
            for (int i = 0; i < toolHBPrefabs.Length; i++)
            { toolHBPrefabs[i].SetActive(true); }
            if (!wateringCanUnlocked && toolHBPrefabs[2].activeInHierarchy)
            { toolHBPrefabs[2].SetActive(false); }
            if (!axeUnlocked && toolHBPrefabs[3].activeInHierarchy)
            { toolHBPrefabs[3].SetActive(false); }
            if (!pickaxeUnlocked && toolHBPrefabs[4].activeInHierarchy)
            { toolHBPrefabs[4].SetActive(false); }
        }
        else if (currentHotbar == Hotbar.Build)
        {
            for (int i = 0; i < toolHBPrefabs.Length; i++)
            { toolHBPrefabs[i].SetActive(false); }
            for (int i = 0; i < buildHBPrefabs.Length; i++)
            { buildHBPrefabs[i].SetActive(true); }
        }
    }
    //Used to change the little preview icon to the left of the screen when in build mode to show the player what they are going to place
    void ChangeBuildPreview()
    {
        //get rid of the old preview
        Destroy(currentPreview);

        //set the rotation
        previewRotation = Quaternion.Euler(0, 0, currentRotation * -90);
        previewParent.transform.rotation = previewRotation;

        //slot 1 = farmland
        if (currentHBSlot == 0)
        {
            previewType = 1;
            previewRotation = Quaternion.Euler(0, 0, 0);
        }
        //second slot = dirt paths
        else if(currentHBSlot == 1)
        { previewType = dirtPathSelection + 2; }
        //default = nothing
        else
        { previewType = 0; }

        // put a new preview
        currentPreview = Instantiate(previewArray[previewType], previewParent.transform);
    }
    //Used to select the type of path the player wants to build
    public void PathSelection(int selection)
    {
        dirtPathSelection = selection;
        ChangeBuildPreview();
    }
    //Used to select the type of seed the player wants to plant
    public void SeedSelection(int seedType)
    {
        seedSelection = seedType;
    }
    //Used to update the sprite and number on the watering can to reflect how full it is
    void UpdateWateringCan()
    {
        if (wateringCanUnlocked)
        {
            if (playerWaterCount < 1)
            {
                waterCanImage.sprite = wateringCanSprite[0];
            }
            else if (playerWaterCount < 16)
            {
                waterCanImage.sprite = wateringCanSprite[1];
            }
            else
            {
                waterCanImage.sprite = wateringCanSprite[2];
            }
        }
    }

    //flashes the green indicator to show the player what they are currently holding
    void HBFlash(int Slot)
    {        
        if (hotbarFlasher)
        {
            hotbarSelectionArray[Slot].SetActive(false);
            hotbarFlasher = false;
            timeSinceFlash = Time.time;
        }
        else
        {  
            hotbarSelectionArray[Slot].SetActive(true);
            hotbarFlasher = true;
            timeSinceFlash = Time.time;
        }
    }

    //changes the current hotbar selection in order to change it to another
    void ResetHotbarSelection()
    {
        for (int i = 0; i < hotbarSelectionArray.Length; i++)
        { hotbarSelectionArray[i].SetActive(false); }
        if(buildModeActive)
        { ChangeBuildPreview(); }
    }

    //changes the text above the hotbar based on the current selection
    void SetItemText(string text)
    {
        if (text == "")
        { itemTextGO.SetActive(false); }
        else
        { itemTextGO.SetActive(true); }
        //set the text to the recieved string
        itemTextGO.GetComponent<Text>().text = text;
        //reset the opacity of the text
        currentFade = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        itemTextGO.GetComponent<Text>().color = currentFade;
        //opacity fading is now required
        fadeRequired = true;
        //reset the item selection, fade, and flash timers
        timeSinceFlash = Time.time;
        lastItemSelection = Time.time;
        lastFade = Time.time;
        if (!hotbarFlasher)
        { hotbarFlasher = true; }
    }

    //fades the hotbar text after a timer
    void FadeItemText()
    {
        //wait a bit before fading
        if (Time.time > textCooldown + lastItemSelection)
        {
            if (Time.time > fadeCooldown + lastFade)
            {
                //reduce the opacity of the object and set it
                currentFade -= fadeReduction;
                itemTextGO.GetComponent<Text>().color = currentFade;
                //reset the fading timer
                lastFade = Time.time;
                //if its already invisible, fade is not required
                if (currentFade == new Color(1.0f, 1.0f, 1.0f, 0.0f))
                { fadeRequired = false; }
            }
        }
    }
}

public enum Hotbar
{
    Tool = 0,
    Build,
}
