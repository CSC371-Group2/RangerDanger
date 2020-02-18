using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Slider oilSlider;
    public float maxOil = 100;
    // Start is called before the first frame update
    void Start()
    {
        oilSlider.maxValue = maxOil;
        oilSlider.minValue = 0;
        oilSlider.value = maxOil;
    }

    // Update is called once per frame
    void Update()
    {
        oilSlider.value -= (2 * Time.deltaTime);
    }
}
