using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelpBoxHider : MonoBehaviour
{
    public void Start()
    {
        Debug.Log(Application.persistentDataPath);
    }

    public void HelpBoxPressed()
    {
        gameObject.SetActive(false);
    }

    public void SetHelpBoxText(string incomingText)
    {
        GetComponentInChildren<Text>().text = incomingText;
    }
}
