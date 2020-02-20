using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Slider oilSlider;
    public TextMeshProUGUI oilPercent;
    public float maxOil = 100;
    public Light lantern;
    public bool isRunning;
    public bool outOfOil;
    public GameObject deathScreen;
    public float depletionRate = 5.0f;
    public float oilUp = 20;
    // Start is called before the first frame update
    void Start()
    {
        isRunning = true;
        outOfOil = false;
        oilSlider.gameObject.SetActive(true);
        lantern.gameObject.SetActive(true);
        oilSlider.maxValue = maxOil;
        oilSlider.minValue = 0;
        oilSlider.value = maxOil;
        oilPercent.text = "Oil: " + oilSlider.value + "%"; 
    }

    // Update is called once per frame
    void Update()
    {
        depletionRate = lantern.intensity/5;
        float oldOil = oilSlider.value;
        if (outOfOil == false)
        {
            oilSlider.value -= (depletionRate * Time.deltaTime);
            if(Mathf.FloorToInt(oldOil) != Mathf.FloorToInt(oilSlider.value))
            {
                FixPercentage(Mathf.FloorToInt(oilSlider.value));
            }
            if(oilSlider.value == 0)
            {
                outOfOil = true;
            }
        }
        else
        {
            lantern.gameObject.SetActive(false);
            deathScreen.SetActive(true);
            oilSlider.gameObject.SetActive(false);
        }
    }

    private void FixPercentage(int percent)
    {
        oilPercent.text = "Oil: " + percent + "%";
    }

    public void RestartGame()
    {
        deathScreen.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
