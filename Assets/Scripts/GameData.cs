using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int[] tileArray;
    public int money;

    public GameData(int[] allTiles, int playerMoney)
    {
        tileArray = allTiles;
        money = playerMoney;
    }
}


