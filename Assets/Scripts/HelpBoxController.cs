using UnityEngine;

public class HelpBoxController : MonoBehaviour
{
    public void HelpBoxPressed()
    {
        Destroy(gameObject.GetComponentInParent<GameObject>());
    }
}
