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
        Scene currentScene = SceneManager.GetActiveScene();
        objectiveList = LoadObjectives(currentScene.buildIndex);

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
        DisplayObjectives();
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

    public ArrayList LoadObjectives(int scene)
    {
        // likely will need two revise scene variables to match up
        ArrayList gameObjectives = new ArrayList(); //arraylist holding all the objectives for each level

        switch (scene)
        {
            case 2:
                // Level 1 objectives 
                gameObjectives = LevelOneObjectives();
                break;
            case 3:
                // tutorial
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
