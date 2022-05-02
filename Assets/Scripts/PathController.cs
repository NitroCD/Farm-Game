using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathController : MonoBehaviour
{
    Build parentScript;
    public int thisType;
    public GameObject deleteButton;
    public bool isBuildable;

    private void Start()
    {
        parentScript = transform.parent.gameObject.GetComponent<Build>();
    }

    private void Update()
    {
        if (PlayerController.buildModeActive && isBuildable)
        {
            deleteButton.SetActive(true);
        }
        else if(isBuildable)
        {
            deleteButton.SetActive(false);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController.touchingDirtPath+=1;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        PlayerController.touchingDirtPath-=1;
    }

    public void DeleteTile(GameObject parent)
    {
        Debug.Log("boo");
        parentScript.DeletedTile(1);

        Destroy(parent);
    }
}
