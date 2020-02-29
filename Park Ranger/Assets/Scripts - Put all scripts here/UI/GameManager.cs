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


    public TextMeshProUGUI objectives;
    public ArrayList objectiveList;

    // for placing and tracking torches in map
    public GameObject torch;
    
    private ArrayList torches = new ArrayList();
    private Light lantern;
    private bool outOfOil;
    private GameObject deathScreen;
    private Transform player;
    private float depletionRate = GameSettings.oilDepleteRate;
    public int scene;

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
        // loads objectives
        Scene currentScene = SceneManager.GetActiveScene();
        scene = currentScene.buildIndex;

        objectiveList = loadObjectives(scene);
        

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
        OilUpdate();
        DisplayObjectives();
    }


    public void OilUpdate()
    {
        depletionRate = lantern.intensity / 5;
        float oldOil = oilSlider.value;
        if (outOfOil == false)
        {
            oilSlider.value -= (depletionRate * Time.deltaTime);
            if (Mathf.FloorToInt(oldOil) != Mathf.FloorToInt(oilSlider.value))
            {
                FixPercentage(Mathf.FloorToInt(oilSlider.value));
            }
            if (oilSlider.value <= 0)
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


    public ArrayList loadObjectives(int scene)
    {
        // likely will need two revise scene variables to match up
        ArrayList gameObjectives = new ArrayList(); //arraylist holding all the objectives for each level

        switch (scene)
        {
            case 2:
                // Level 1 objectives 
                gameObjectives = LevelOneObjectives();
                break;
            case 4:
                gameObjectives = LevelTwoObjectives();
                break;
            case 6:
                gameObjectives = LevelThreeObjectives();
                break;
            case 8:
                gameObjectives = LevelFourObjectives();
                break;
            default:
                break;
        }

        return gameObjectives;
    }

    public void DisplayObjectives()
    {
        objectives.text = ObjectiveString();
    }

    public string ObjectiveString()
    {
        string objectiveStr = "";

        foreach (string obj in objectiveList)
        {
            objectiveStr += obj;
        }

        return objectiveStr;
    }

    public void UpdateObjectives(string eventTitle)
    {
        switch (eventTitle)
        {
            case "camperFound":
                objectiveList.Remove("Find the lost camper\n");
                break;
            default:
                break;
        }
    }

    public ArrayList LevelOneObjectives()
    {
        ArrayList objectiveList = new ArrayList();
        objectiveList.Add("Find the lost camper\n");
        objectiveList.Add("Escape the forest\n");
        return objectiveList;
    }

    public ArrayList LevelTwoObjectives()
    {
        ArrayList objectiveList = new ArrayList();
        //add objectives
        return objectiveList;
    }

    public ArrayList LevelThreeObjectives()
    {
        ArrayList objectiveList = new ArrayList();
        //add objectives
        return objectiveList;
    }

    public ArrayList LevelFourObjectives()
    {
        ArrayList objectiveList = new ArrayList();
        //add objectives
        return objectiveList;
    }
}
