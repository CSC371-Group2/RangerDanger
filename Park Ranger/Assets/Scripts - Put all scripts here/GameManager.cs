using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Slider oilSlider;
    public float maxOil = 100;
    public Light lantern;
    public bool isRunning;
    public bool outOfOil;
    // Start is called before the first frame update
    void Start()
    {
        isRunning = true;
        outOfOil = false;
        oilSlider.gameObject.SetActive(true);
        oilSlider.maxValue = maxOil;
        oilSlider.minValue = 0;
        oilSlider.value = maxOil;
    }

    // Update is called once per frame
    void Update()
    {
        if (outOfOil == false)
        {
            oilSlider.value -= (10 * Time.deltaTime);
            if(oilSlider.value == 0)
            {
                outOfOil = true;
            }
        }
        else
        {
            lantern.gameObject.SetActive(false);
        }
    }
}
