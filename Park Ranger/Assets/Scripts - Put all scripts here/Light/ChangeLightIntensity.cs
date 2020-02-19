using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeLightIntensity : MonoBehaviour
{
    private Light lght_src;
    private float lght_incr = 0.3f;
    private float lght_max; 
    private float lght_min;

    // Start is called before the first frame update
    void Start()
    {
        lght_src = GetComponent<Light>();
        lght_max = 6f - lght_incr;
        lght_min = 0f + lght_incr;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Q))
        {
            if(lght_src.intensity > lght_min)
            {
                lght_src.intensity -= lght_incr;
            }
        }
        else if(Input.GetKey(KeyCode.E))
        {
            if(lght_src.intensity < lght_max)
            {
                lght_src.intensity += lght_incr;
            }
        }
    }
}
