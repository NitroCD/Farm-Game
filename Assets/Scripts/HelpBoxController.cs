using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpBoxController : MonoBehaviour
{
    public GameObject[] HelpBoxes;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < HelpBoxes.Length; i++)
        {
            HelpBoxes[i].SetActive(false);
        }
    }

    public void ShowHelpBox(int boxNumber)
    {
        HelpBoxes[boxNumber].SetActive(true);
    }
}
