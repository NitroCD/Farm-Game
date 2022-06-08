using UnityEngine;
using UnityEngine.UI;

public class HelpBoxHider : MonoBehaviour
{
    public BoxNumber thisHelpBox;
    HelpBoxController thisHelpBoxController;

    void Start()
    {
        thisHelpBoxController = GameObject.FindGameObjectWithTag("HelpBoxController").GetComponent<HelpBoxController>();
    }

    public void HelpBoxPressed()
    {
        if (thisHelpBox == BoxNumber.buildOpened1)
        {
            thisHelpBoxController.ShowHelpBox(BoxNumber.buildOpened2);
        }
        if (thisHelpBox == BoxNumber.buildOpened2)
        {
            thisHelpBoxController.ShowHelpBox(BoxNumber.buildOpened3);
        }
        gameObject.SetActive(false);
    }

    /*public void SetHelpBoxText(string incomingText)
    {
        GetComponentInChildren<Text>().text = incomingText;
    }*/
}
