using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    GameObject[] menuObjects;
    int xIncrement;
    float timeLerped = 0f;
    float lerpduration = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        menuObjects = GetComponentsInChildren<GameObject>();
        xIncrement = 30;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OpenMenu()
    {
        for (int i = 0; i < menuObjects.Length; i++)
        {
            //smoothly move each UI element down
        }
    }

    void CloseMenu()
    {
        for (int i = 0; i < menuObjects.Length; i++)
        {
            //smoothly move each UI element up
        }
    }
}
/*
        if (timeLerped <= lerpDuration)
        {
            Vector3 smoothPosition = Vector3.Lerp(transform.position, plotCameraPosition.position, timeLerped / lerpDuration);
            mainCameraGO.transform.position = smoothPosition + new Vector3(0f, 0f, -1f);

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
            //zoomedIn = false;
        }
*/