using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneController : MonoBehaviour
{
    //private variables
    bool checkForInput = false;
    float mineCooldown;
    float timeSinceMined;
    bool isActive = true;
    int timesHit = 0;
    int requiredHits;
    int stoneIncrement;
    StoneType thisStoneType;

    //public variables
    public GameObject stoneEffectsPrefab;
    public GameObject[] stoneTypes;
    public Collider2D[] stoneColliderArray;

    private void Start()
    {
        // Turn off the stone,
        Activate(false);
        // Randomize the type of stone,
        randomizeStoneType();
        // Turn back on the stone.
        Activate(true);
    }

    private void Update()
    {
        //activate the stone if its time
        if (!isActive)
        {
            if (Time.time - timeSinceMined >= mineCooldown)
            {
                Activate(true);
            }
        }
        //if its active, check if the player can mine
        else
        {
            if (checkForInput)
            {
                if (PlayerController.currentHBSlot == 4 && PlayerController.currentHotbar == Hotbar.Tool && PlayerController.pickaxeUnlocked && Input.GetKeyDown(KeyCode.E))
                {
                    Mine();
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    { checkForInput = true; }
    private void OnTriggerExit2D(Collider2D collision)
    { checkForInput = false; }

    void Mine()
    {
        switch (thisStoneType)
        {
            case StoneType.stoneOne:
            ///change these ints for different stone types
                requiredHits = 1; //input 1 for the stone to be broken in 1 hit, 2 for 2 hits, etc.
                stoneIncrement = 1; //amount of stone given when the stone is broken

                if (timesHit < requiredHits - 1)
                {
                    timesHit++;
                    Instantiate(stoneEffectsPrefab, gameObject.GetComponentInParent<Transform>());
                }
                // On the third hit (0,1,2), "destroy" the object
                else if (timesHit == requiredHits - 1)
                {
                    timesHit = 0;
                    Instantiate(stoneEffectsPrefab, gameObject.GetComponentInParent<Transform>());
                    timeSinceMined = Time.time;
                    mineCooldown = Random.Range(90, 121);
                    PlayerController.playerStoneCount = PlayerController.playerStoneCount + stoneIncrement;
                    Activate(false);
                    randomizeStoneType();
                }
                break;
            case StoneType.iron:
            ///change these ints for different stone types
                requiredHits = 3; //input 1 for the stone to be broken in 1 hit, 2 for 2 hits, etc.
                stoneIncrement = 2; //amount of stone given when the stone is broken

                // increments the hit count and spawns particles
                if (timesHit < requiredHits - 1 && isActive)
                {
                    timesHit++;
                    Instantiate(stoneEffectsPrefab, gameObject.GetComponentInParent<Transform>());
                }
                // On the fifth hit (0,1,2,3,4), destroy the object
                else if (timesHit == requiredHits - 1 && isActive)
                {
                    timesHit = 0;
                    Instantiate(stoneEffectsPrefab, gameObject.GetComponentInParent<Transform>());
                    timeSinceMined = Time.time;
                    mineCooldown = Random.Range(90, 121);
                    PlayerController.playerStoneCount = PlayerController.playerStoneCount + stoneIncrement;
                    Activate(false);
                    randomizeStoneType();
                }
                break;
        }
    }

    void Activate(bool recievedBool)
    {
        isActive = recievedBool;
        switch (thisStoneType)
        {
            case StoneType.stoneOne:
                if (isActive)
                {
                    stoneTypes[0].SetActive(isActive);
                    stoneTypes[1].SetActive(!isActive);
                    stoneColliderArray[0].enabled = true;
                    stoneColliderArray[1].enabled = false;
                }
                else if (!isActive)
                {
                    stoneTypes[0].SetActive(isActive);
                    stoneTypes[1].SetActive(isActive);
                    stoneColliderArray[0].enabled = false;
                    stoneColliderArray[1].enabled = false;
                }
                break;
            case StoneType.iron:
                if (isActive)
                {
                    stoneTypes[0].SetActive(!isActive);
                    stoneTypes[1].SetActive(isActive);
                    stoneColliderArray[0].enabled = false;
                    stoneColliderArray[1].enabled = true;
                }
                else if (!isActive)
                {                
                    stoneTypes[0].SetActive(isActive);
                    stoneTypes[1].SetActive(isActive);
                    stoneColliderArray[0].enabled = false;
                    stoneColliderArray[1].enabled = false;
                }
                break;
        }
    }

    private void randomizeStoneType()
    {
        int randomStone = Random.Range(0, 100);
        switch (randomStone)
        {
            // 80% of the time it will be a normal stone
            case var n when n < 80:
                thisStoneType = StoneType.stoneOne;    
                break;
            // 20% of the time it will be an iron
            case var n when n < 100:
                thisStoneType = StoneType.iron;
                break;
        }
    }
}

public enum StoneType
{
    stoneOne = 0,
    iron,


    /*
    stoneTwo,
    coal,
    gold,
    copper,
    silver,
    sapphire, 
    ruby,
    emerald,
    */
    
    /*//minecraft
    coal,
    iron,
    gold,
    redstone,
    lapislazuli,
    diamond,
    netherite,
    */

    /*//terraria
    copper,
    tin,
    iron,
    lead,
    silver,
    tungsten,
    gold,
    platinum,
    demonite,
    crimtane,
    hellstone,
    cobalt,
    palladium,
    mithril,
    orichalcum,
    titanium,
    adamantium,
    hallowed,
    chlorophyte,
    shroomite,
    spectre,
    solarFragment,
    nebulaFragment,
    vortexFragment,
    stardustFragment,
    luminite,
    */

    /*//periodic table of the elements
    hydrogen,
    helium,
    boron,
    carbon, 
    nitrogen,
    oxygen,
    flourine,
    neon,
    sodium,
    magnesium, 
    aluminum,
    silicon,
    phosphorus,
    sulfur,
    chlorine,
    argon,
    potassium,
    calcium,
    scandium,
    vanadium,
    chromium,
    manganese,
    nickel,
    zinc,
    gallium,
    germanium,
    arsenic,
    selenium, 
    bromine, 
    krypton,
    rubidium,
    strontium,
    yttrium,
    zirconium,
    nobium,
    molubidium,
    techinetium,
    rutherfurdium,
    rhodium,
    cadmium,
    indium,
    antimony,
    tellurium,
    iodine,
    xenon,
    cesium,
    barium,
    lanthanum,
    cerium,
    prasiodimium,
    neodymium,
    promethium,
    samarium,
    europium,
    gadalinium,
    turbium,
    disprosium,
    hallmium,
    erbium,
    thulium,
    ytterbium,
    lutium,
    halfnium,
    tantilum,
    ranium,
    osmium,
    eredium,
    mercury,
    thallium,
    bismuth,
    polonium,
    astatine,
    radon,
    francium,
    radium,
    actinium,
    thorium,
    protactinium,
    uranium,
    neptunium,
    plutonium,
    americium,
    curium,
    bergellium,
    californium,
    einsteinium,
    fernium,
    mendelevium,
    nobellium,
    lorwencium,
    rutherfordium,
    dubnium,
    seaborgium,
    borium,
    hassium,
    meitnerium,
    darmstadtium,
    roentgenium,
    capernicium,
    nihonium,
    flerovium,
    mosscovium,
    livermorium,
    tennessine,
    oganesson,*/
}