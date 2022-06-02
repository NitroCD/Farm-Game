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
    int[] increments = new int[6];
    int thisOre;
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
                    MineController();
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    { checkForInput = true; }
    private void OnTriggerExit2D(Collider2D collision)
    { checkForInput = false; }

    void MineController()
    {
        switch (thisStoneType)
        {
            case StoneType.stoneOne: 
                thisOre = (int)StoneType.stoneOne;
                requiredHits = 1;
                increments[thisOre] = 1;
                Mine(thisOre, requiredHits);
                break;
            case StoneType.iron:
                thisOre = (int)StoneType.iron;
                requiredHits = 2;
                increments[thisOre] = 1;
                Mine(thisOre, requiredHits);
                break;
            case StoneType.coal:
                thisOre = (int)StoneType.coal;
                requiredHits = 2;
                increments[thisOre] = 1; 
                Mine(thisOre, requiredHits);
                break;
            case StoneType.gold:
                thisOre = (int)StoneType.gold;
                requiredHits = 3;
                increments[thisOre] = 1;
                Mine(thisOre, requiredHits);
                break;
            case StoneType.copper:
                thisOre = (int)StoneType.copper;
                requiredHits = 2;
                increments[thisOre] = 1;
                Mine(thisOre, requiredHits);
                break;
            case StoneType.sapphire:
                thisOre = (int)StoneType.sapphire;
                requiredHits = 5;
                increments[thisOre] = 1;
                Mine(thisOre, requiredHits);
                break;
                ///when adding new ores:
                // 1. make sure the StoneType.(stonetype) in the case matches thisOre
                // 2. change the required hits, input 1 for the stone to be broken in 1 hit, 2 for 2 hits, etc.
                // 3. change the increments int to the amount of ore given when the stone is broken
        }
    }

    void Mine(int ore, int hits)
    {
        if (timesHit < hits - 1)
        {
            timesHit++;
            Instantiate(stoneEffectsPrefab, gameObject.GetComponentInParent<Transform>());
        }
        else if (timesHit == hits - 1)
        {
            timesHit = 0;
            Instantiate(stoneEffectsPrefab, gameObject.GetComponentInParent<Transform>());
            timeSinceMined = Time.time;
            mineCooldown = Random.Range(90, 121);
            PlayerController.playerOreCount[ore] += increments[ore];
            Activate(false);
            randomizeStoneType();
        }
    }

    void Activate(bool recievedBool)
    {
        isActive = recievedBool;
        int stone = (int)thisStoneType;

        // these arrays must be the same length
        for (int i = 0; i < stoneTypes.Length; i++)
        {
            stoneTypes[i].SetActive(false);
            stoneColliderArray[i].enabled = false;
        }
        if (isActive)
        {
            stoneTypes[stone].SetActive(true);
            stoneColliderArray[stone].enabled = true;
        }
    }

    private void randomizeStoneType()
    {
        int randomStone = Random.Range(0, 100);
        switch (randomStone)
        {
            //when adding new stones, the percent that it will spawn = (n value - previous n value)
            case var n when n < 75:
                thisStoneType = StoneType.stoneOne;    
                break;
            case var n when n < 85:
                thisStoneType = StoneType.iron;
                break;
            case var n when n < 90:
                thisStoneType = StoneType.coal;
                break;
            case var n when n < 94:
                thisStoneType = StoneType.gold;
                break;
            case var n when n < 99:
                thisStoneType = StoneType.copper;
                break;
            case var n when n < 100:
                thisStoneType = StoneType.sapphire;
                break;
        }
    }
}

public enum StoneType
{
    stoneOne = 0,
    iron,
    coal,
    gold,
    copper,
    sapphire,

    /* //other stones
    stoneTwo,
    silver,
    ruby,
    emerald,
    */
    
    /*//minecraft
    coal,
    iron,
    gold,
    redstone,
    lapisLazuli,
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
    diamond,
    amber,
    ruby,
    emerald,
    sapphire,
    topaz,
    amethyst,
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