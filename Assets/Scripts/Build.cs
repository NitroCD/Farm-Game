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
    public GameManager gameManager;

    bool isBuilt = false;

    // Start is called before the first frame update
    void Start()
    {
        tileStorage = new int[21 * 11];
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
    //Called whenever the player wants to place a tile (Clicks on a tile button)
    public void SetBuildInfo()
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

                int type = PlayerController.dirtPathSelection + 1;
                
                PlayerController.playerPathCount--;

                BuildTile(type, rotation);
            }
        }
    }
    //Called from SetBuildInfo and GameManager.LoadTiles, handles the instantiation of a tile when the player builds it or loads a built tile
    public void BuildTile(int type, Quaternion rotation)
    {
        int tileType = 0;
        gameObject.transform.rotation = rotation;
        if(type == 0)
        {
            GameObject newLand = Instantiate(tilePrefabs[0], gameObject.transform);
        }
        else
        {
            GameObject newLand = Instantiate(pathPrefabs[type - 1], gameObject.transform);
            tileType = PlayerController.dirtPathSelection + 1;
        }
        isBuilt = true;
        button.SetActive(false);
        

        int xPos = (int)gameObject.transform.position.x + 11;
        int yPos = (int)gameObject.transform.position.y;
        int rotationInt = PlayerController.currentRotation;

        int index = (xPos + (yPos - 1) * 21) - 1;

        Vector2 pos = new Vector2(xPos, yPos);

        int value = SaveTile(pos, tileType, rotationInt);

        gameManager.updateTileArray(index, value);
    }
    //Adds the current tile to an array that is later used for saving in the GameManager
    public int SaveTile(Vector2 tilePosition, int tileType, int tileRotation)
    {
        int tileInt = 0;

        tileInt = (int)tilePosition.x * 100000 + (int)tilePosition.y * 1000 + tileType * 10 + tileRotation;

        return tileInt;
    }
    //Takes a tile out of the saving array whenever it is deleted
    public void UnsaveTile(Vector3 position)
    {
        int index = ((int)position.x + 11 + ((int)position.y - 1) * 21) - 1;
        gameManager.updateTileArray(index, 0);
    }
    //Runs when the player deletes a tile
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
