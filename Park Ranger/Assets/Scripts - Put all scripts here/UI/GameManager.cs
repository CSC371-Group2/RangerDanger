using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Singleton GameManager stuff
    public static GameManager instance = null;


    public Slider oilSlider;
    public TextMeshProUGUI oilPercent;

    // for placing and tracking torches in map
    public GameObject torch;
    
    private ArrayList torches = new ArrayList();
    private Light lantern;
    private bool outOfOil;
    public GameObject deathScreen;
    private Transform player;
    private float depletionRate = GameSettings.oilDepleteRate;
    private float torchDepletion = GameSettings.torchDepletion;
    private float oldOil;
    private float oilIncrement = 20f;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Object.Destroy(this);
        }

        // so the instance persists between scenes...
        // DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        lantern = GameObject.Find("Lantern").GetComponent<Light>();
        // deathScreen = GameObject.Find("Death Screen");
        player = GameObject.Find("Ranger D. Danger").GetComponent<Transform>();
        oldOil = 100;
        outOfOil = false;
        oilSlider.gameObject.SetActive(true);
        lantern.gameObject.SetActive(true);
        oilSlider.maxValue = GameSettings.startingOil;
        oilSlider.minValue = 0;
        oilSlider.value = GameSettings.startingOil;
        oilPercent.text = "Oil: " + oilSlider.value + "%"; 
    }

    // Update is called once per frame
    void Update()
    {
        check_oil_level();
    }

    private void check_oil_level()
    {
        depletionRate = lantern.intensity/5;
        oldOil = oilSlider.value;
        if (outOfOil == false)
        {
            oilSlider.value -= (depletionRate * Time.deltaTime);
            if(Mathf.FloorToInt(oldOil) != Mathf.FloorToInt(oilSlider.value))
            {
                FixPercentage(Mathf.FloorToInt(oilSlider.value));
            }
            if(oilSlider.value <= 0)
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

    public bool incrementOil()
    {
        oldOil = oilSlider.value;
        if (oldOil > (100 - oilIncrement))
        {
            if (oldOil >= 100)
            {
                    return false;
            }
            else
            {
                oilSlider.value += (100 - oldOil);
                check_oil_level();
                return true;
            }    
        }
        else
        {
            oilSlider.value += oilIncrement;
            check_oil_level();
            return true;
        }        
    }


    public void PlaceTorch()
    {
        oldOil = oilSlider.value;
        if(oldOil >= torchDepletion)
        {
            torches.Add(Instantiate(torch, player.transform.position + (player.transform.forward), player.transform.rotation));
            oilSlider.value -= torchDepletion;
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
