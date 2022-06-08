using UnityEngine;

public class HelpBoxController : MonoBehaviour
{
    public GameObject[] HelpBoxes;
    public bool[] BoxesShown;

    // Start is called before the first frame update
    void Start()
    {
        BoxesShown = new bool[HelpBoxes.Length];
        for (int i = 0; i < HelpBoxes.Length; i++)
        {
            HelpBoxes[i].SetActive(false);
            BoxesShown[i] = false;
        }
    }

    public void ShowHelpBox(BoxNumber incomingNumber)
    {
        //if the box has been shown already, dont bother showing it again
        if (BoxesShown[(int)incomingNumber])
        {
            return;
        }
        //otherwise, show the box
        else
        {
            HelpBoxes[(int)incomingNumber].SetActive(true);
            BoxesShown[(int)incomingNumber] = true;
        }
    }
}

public enum BoxNumber
{
    startGame = 0, //done
    seedsUnlocked, 
    stoneUnlocked, //done
    buildOpened1, //done
    buildOpened2, //done
    buildOpened3, //done
}