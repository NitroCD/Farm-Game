[System.Serializable]

public class GameData
{
    public int[] tileArray;
    public float money;
    public int[] seeds;
    public int[] crops;
    public int wood;
    public int[] ores;
    public int paths;
    public int land;
    public bool well;
    public bool bucket;
    public bool axe;
    public bool pickaxe;


    public GameData(int[] allTiles, 
        float playerMoney,
        int[] playerSeeds,
        int[] playerCrops,
        int playerWood,
        int[] playerOres,
        int playerPaths,
        int playerLand, 
        bool wellStatus,
        bool bucketStatus, 
        bool axeStatus,
        bool pickaxeStatus)
    {
        tileArray = allTiles;
        money = playerMoney;
        seeds = playerSeeds;
        crops = playerCrops;
        wood = playerWood;
        ores = playerOres;
        paths = playerPaths;
        land = playerLand;
        well = wellStatus;
        bucket = bucketStatus;
        axe = axeStatus;
        pickaxe = pickaxeStatus;
    }
}


