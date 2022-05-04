using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeEffectController : MonoBehaviour
{
    // Destruction variables
    float destructionTime = 1f;
    float instantiatedTime;

    // Start is called before the first frame update
    void Start()
    {
        instantiatedTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        //change this to disable the animation, then destory it later
        if (Time.time > destructionTime + instantiatedTime)
        {
            Destroy(gameObject);
        }
    }
}
