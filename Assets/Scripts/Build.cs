using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Build : MonoBehaviour
{
    //test!!!!!
    Transform selectedPos;
    Quaternion rotation;
    public GameObject[] tilePrefabs;
    public GameObject[] pathPrefabs;
    int[] tileStorage;
    public GameObject button;
    GameObject thisGO;
    GameManager gameManager;

    bool isBuilt = false;

    // Start is called before the first frame update
    void Start()
    {
        tileStorage = new int[21 * 11];
        thisGO = gameObject;   
    }

    // Update is called once per frame
    void Update()
    {
        if(!PlayerController.buildModeActive)
        {
            if (button.activeSelf)
            {
                button.SetActive(false);
            }
        }
        if(PlayerController.buildModeActive)
        {
            if (!button.activeSelf && !isBuilt)
            {
                button.SetActive(true);
            }
        }
    }

    public int[] tileArray()
    {
        return tileStorage;
    }

    public void BuildTile()
    {
        if(!isBuilt)
        {

            if (PlayerController.currentHBSlot == 0 && PlayerController.playerLandCount > 0)
            {
                rotation = Quaternion.Euler(0, 0, 0);

                int type = 0;

                PlayerController.playerLandCount--;

                BuildTile(type, rotation);
            }
            else if (PlayerController.currentHBSlot == 1 && PlayerController.playerPathCount > 0)
            {
                rotation = Quaternion.Euler(0, 0, PlayerController.currentRotation * -90);

                int type = PlayerController.dirtPathSelection;
                
                PlayerController.playerPathCount--;

                BuildTile(type, rotation);
            }
        }
    }

    public void BuildTile(int type, Quaternion rotation)
    {
        thisGO.transform.rotation = rotation;
        GameObject newLand = Instantiate(pathPrefabs[PlayerController.dirtPathSelection], thisGO.transform);
        isBuilt = true;
        button.SetActive(false);

        int xPos = (int)thisGO.transform.position.x + 11;
        int yPos = (int)thisGO.transform.position.y;
        int tileType = PlayerController.dirtPathSelection + 1;
        int rotationInt = PlayerController.currentRotation;

        int index = (xPos + (yPos - 1) * 21) - 1;

        Vector2 pos = new Vector2(xPos, yPos);

        int value = SaveTile(pos, tileType, rotationInt);

        gameManager.updateTileArray(index, value);
    }

    public int SaveTile(Vector2 tilePosition, int tileType, int tileRotation)
    {
        int tileInt = 0;

        tileInt = (int)tilePosition.x * 100000 + (int)tilePosition.y * 1000 + tileType * 10 + tileRotation;

        return tileInt;
    }

    public void UnsaveTile(Vector3 position)
    {
        int index = ((int)position.x + 11 + ((int)position.y - 1) * 21) - 1;
        gameManager.updateTileArray(index, 0);
    }

    public void DeletedTile(GameObject tile, int tileType)
    {
        UnsaveTile(tile.transform.position);

        if(tileType == 0)
        { PlayerController.playerLandCount++; }
        else { PlayerController.playerPathCount++; }
        button.SetActive(false);
        isBuilt = false;
    }
}
