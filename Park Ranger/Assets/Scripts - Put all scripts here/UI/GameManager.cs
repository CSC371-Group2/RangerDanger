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
    private GameObject deathScreen;
    private Transform player;
    private float depletionRate = GameSettings.oilDepleteRate;

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
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        lantern = GameObject.Find("Lantern").GetComponent<Light>();
        deathScreen = GameObject.Find("Death Screen");
        player = GameObject.Find("Ranger D. Danger").GetComponent<Transform>();

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

    public void PlaceTorch()
    {
        torches.Add(Instantiate(torch, player.transform.position, player.transform.rotation));
        oilSlider.value -= 20;
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
