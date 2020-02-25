using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flicker : MonoBehaviour
{
    Light lght;
    
    float duration;
    public Color r1;
    public Color r2;
    public float flicker_delta = 4f;
    // Start is called before the first frame update
    void Start()
    {
        lght = GetComponent<Light>();     
    }

    // Update is called once per frame
    void Update()
    {
        // set light color
        duration = Random.Range(0f, flicker_delta);
        float t = Mathf.PingPong(Time.time, duration) / duration;
        lght.color = Color.Lerp(r1, r2, t);
    }
}
