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
    public GameObject button;
    GameObject thisGO;

    bool isBuilt = false;

    // Start is called before the first frame update
    void Start()
    {
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
            }
            else if (PlayerController.currentHBSlot == 1 && PlayerController.playerPathCount > 0)
            {
                rotation = Quaternion.Euler(0, 0, PlayerController.currentRotation * -90);
                thisGO.transform.rotation = rotation;
                GameObject newLand = Instantiate(pathPrefabs[PlayerController.dirtPathSelection], thisGO.transform);
                PlayerController.playerPathCount--;
                button.SetActive(false);
                isBuilt = true;
            }
        }
    }

    public void DeletedTile(int tileType)
    {
        if(tileType == 1)
        { PlayerController.playerLandCount++; }
        button.SetActive(false);
        isBuilt = false;
    }
}
