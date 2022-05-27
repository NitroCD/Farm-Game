using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public Transform[] menuTransforms;
    Vector2[] initialPositions;
    Vector2 zeroPosition;
    bool open = false;
    bool move = false;
    bool positionsSnapped = false;
    float timeLerped = 0f;
    float lerpDuration = 5f;

    void Start()
    {
        //get the origin position
        zeroPosition = GetComponentInParent<Transform>().position;

        //save all the initial positions of the objects
        initialPositions = new Vector2[menuTransforms.Length];
        for (int i = 0; i < menuTransforms.Length; i++)
        {
            initialPositions[i] = menuTransforms[i].position;
        }

        SnapPositions();
    }

    public void toggleOpen()
    {
        //SnapPositions();
        if (timeLerped != 5f && timeLerped != 0f)
        {
            timeLerped = lerpDuration - timeLerped;
        }
        open = !open;
        move = true;
    }

    private void Update()
    {
        if (move)
        {
            SmoothMovement();
        }
        else if (!positionsSnapped)
        {
            SnapPositions();
        }
    }

    void SmoothMovement()
    {
        for (int i = 0; i < menuTransforms.Length; i++)
        {
            if (timeLerped <= lerpDuration)
            {
                if (open)
                {
                    menuTransforms[i].position = Vector3.Lerp(zeroPosition, initialPositions[i], timeLerped / lerpDuration);
                }
                else if (!open)
                {
                    menuTransforms[i].position = Vector3.Lerp(initialPositions[i], zeroPosition, timeLerped / lerpDuration);
                }
                timeLerped += Time.deltaTime;
            }
        }
        if (timeLerped > lerpDuration)
        {
            move = false;
            SnapPositions();
            timeLerped = 0f;
        }
    }

    void SnapPositions()
    {
        positionsSnapped = true;
        for (int i = 0; i < menuTransforms.Length; i++)
        {
            if (open)
            {
                menuTransforms[i].transform.position = initialPositions[i];
            }
            else if (!open)
            {
                menuTransforms[i].transform.position = zeroPosition;
            }
        }
    }
}