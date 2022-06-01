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
    int tileStorageIndex;
    public GameObject button;
    GameObject thisGO;

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

    public void BuildTile()
    {
        if(!isBuilt)
        {

            if (PlayerController.currentHBSlot == 0 && PlayerController.playerLandCount > 0)
            {
                rotation = Quaternion.Euler(0, 0, 0);
                thisGO.transform.rotation = rotation;
                GameObject newLand = Instantiate(tilePrefabs[PlayerController.currentHBSlot], thisGO.transform);
                PlayerController.playerLandCount--;
                button.SetActive(false);
                isBuilt = true;

                int xPos = (int)thisGO.transform.position.x + 11;
                int yPos = (int)thisGO.transform.position.y;
                int tileType = 0;
                int rotationInt = PlayerController.currentRotation / 90;

                tileStorageIndex = (xPos + (yPos - 1) * 21) - 1;

                Vector2 pos = new Vector2(xPos, yPos);

                Debug.Log(tileStorageIndex);
                Debug.Log(tileStorage.Length);
                tileStorage[tileStorageIndex] = SaveTile(pos, tileType, rotationInt);

                Debug.Log("Tile " + tileStorageIndex + " was set to " + tileStorage[tileStorageIndex]);
            }
            else if (PlayerController.currentHBSlot == 1 && PlayerController.playerPathCount > 0)
            {
                rotation = Quaternion.Euler(0, 0, PlayerController.currentRotation * -90);
                thisGO.transform.rotation = rotation;
                GameObject newLand = Instantiate(pathPrefabs[PlayerController.dirtPathSelection], thisGO.transform);
                PlayerController.playerPathCount--;
                button.SetActive(false);
                isBuilt = true;

                int xPos = (int)thisGO.transform.position.x + 11;
                int yPos = (int)thisGO.transform.position.y;
                int tileType = 0;
                int rotationInt = PlayerController.currentRotation / 90;

                tileStorageIndex = (xPos + (yPos - 1) * 21) - 1;

                Vector2 pos = new Vector2(xPos, yPos);

                tileStorage[tileStorageIndex] = SaveTile(pos, tileType, rotationInt);

                Debug.Log("Tile " + tileStorageIndex + " was set to " + tileStorage[tileStorageIndex]);
            }
        }
    }

    public int SaveTile(Vector2 tilePosition, int tileType, int tileRotation)
    {
        int tileInt = 0;

        tileInt = (int)tilePosition.x * 100000 + (int)tilePosition.y * 1000 + tileType * 10 + tileRotation;

        return tileInt;
    }

    public void UnsaveTile(Vector3 position)
    {
        tileStorageIndex = ((int)position.x + 11 + ((int)position.y - 1) * 21) - 1;
        Debug.Log(tileStorageIndex);
        tileStorage[tileStorageIndex] = 0;
        Debug.Log("Tile " + tileStorageIndex + " was set to " + tileStorage[tileStorageIndex]);
    }

    public void DeletedTile(GameObject tile, int tileType)
    {
        UnsaveTile(tile.transform.position);
        

        if(tileType == 1)
        { PlayerController.playerLandCount++; }
        button.SetActive(false);
        isBuilt = false;
    }
}
