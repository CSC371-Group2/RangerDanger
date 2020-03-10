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
    public GameObject F_PROMPT;
    public GameObject NEED_TOOL_PROMPT;

    private Transform player;
    private float depletionRate = GameSettings.oilDepleteRate;
    private float torchDepletion = GameSettings.torchDepletion;
    private float oldOil;
    private float oilIncrement = 20f;

    private bool f_able = false; /* should we display prompt to use 'F' to get tool */
    private bool need_tool = false; 

    public bool is_camper_following = false; 
    private int supply_count = 0; /* supplies gathered by player */
    private bool has_tool = false; /* true once we have the tool */

    /* current level described by the enum below 
     * mostly for readability
    */
    private level current; 
    
    public enum level
    {
        TUTORIAL, LEVEL_ONE, LEVEL_TWO, LEVEL_THREE, LEVEL_FOUR, LOADING
    }

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
        //objectiveList = LoadObjectives(currentScene.buildIndex);
        objectiveList = LoadObjectives();

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


        current = whichSceneAmI();
    }

    // Update is called once per frame
    void Update()
    {
        check_oil_level();
        DisplayObjectives();
        shouldDisplayPrompts();
    }

    private void shouldDisplayPrompts() /* display F/tool prompt for tools and barriers */
    {
        if(f_able)
        {
            F_PROMPT.SetActive(true);
        }
        else
        {
            F_PROMPT.SetActive(false);
        }

        if(need_tool)
        {
            NEED_TOOL_PROMPT.SetActive(true);
        }
        else
        {
            NEED_TOOL_PROMPT.SetActive(false);
        }
    }

    public void canF(bool state)
    {
        f_able = state;
    }

    public void set_need_tool_prompt(bool state)
    {
        need_tool = state;
    }

    public void addSupply()
    {
        supply_count++;
        int totalSupplies = suppliesRequired(whichSceneAmI());
        Debug.Log(totalSupplies);
        if (supply_count == totalSupplies)
        {
            objectiveList.Remove("Find the supplies\n");
            objectiveList.Remove("Find the supplies: 1/2 found");
        }
        else if (supply_count == 1)
        {
            objectiveList.Remove("Find the supplies\n");
            objectiveList.Add("Find the supplies: 1/2 found");
        }
    }

    public int suppliesRequired(level currentLevel)
    {
        int supplies;
        switch (currentLevel)
        {
            case level.LEVEL_TWO: //Scene title: Game
                supplies = 1;
                break;
            case level.LEVEL_THREE:
                supplies = 2;
                break;
            default:
                supplies = 0;
                break;
        }
        return supplies;
    }

    public void pickupTool()
    {
        has_tool = true;
    }

    public bool can_unlock()
    {
        return has_tool;
    }

    private level whichSceneAmI()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName == "Tutorial")
        {
            return level.TUTORIAL;
        }
        else if (sceneName == "Game")
        {
            return level.LEVEL_ONE;
        }
        else if (sceneName == "Level3")
        {
            return level.LEVEL_THREE;
        }
        else
        {
            return level.LOADING;
        }
    }

    public void escape()
    {
        switch(current)
        {
            case level.TUTORIAL:
                SceneManager.LoadScene("Game");
                break;
            case level.LEVEL_ONE:
                SceneManager.LoadScene("Menu");
                break;
            case level.LEVEL_THREE:
                SceneManager.LoadScene("Menu");
                break;
            default:
                break;
        }
    }

    public bool check_win_condition()
    {
        switch (current)
        {
            case level.TUTORIAL:
                return is_camper_following;
            case level.LEVEL_ONE:
                return is_camper_following;
            case level.LEVEL_TWO:
                return is_camper_following && supply_count == 1;
            case level.LEVEL_THREE:
                return is_camper_following && supply_count == 2;
            default:
                return false;
        }
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

    public ArrayList LoadObjectives()
    {
        // likely will need two revise scene variables to match up
        level currentLevel = whichSceneAmI();
        ArrayList gameObjectives = new ArrayList(); //arraylist holding all the objectives for each level
        switch (currentLevel)
        {
            case level.TUTORIAL:
                // Tutorial objectives 
                gameObjectives = TutorialObjectives();
                break;
            case level.LEVEL_ONE:
                // level 1 objectives
                gameObjectives = LevelOneObjectives();
                break;
            case level.LEVEL_TWO:
                gameObjectives = LevelTwoObjectives();
                break;
            case level.LEVEL_THREE:
                gameObjectives = LevelThreeObjectives();
                break;
            case level.LEVEL_FOUR:
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

    public void UpdateObjectives(bool eventTitle)
    {
        if (is_camper_following)
        {
            objectiveList.Remove("Find the lost camper\n");
        }
        //switch (eventTitle)
        //{
        //    case is_camper_following == true:
        //        objectiveList.Remove("Find the lost camper\n");
        //        break;
        //    default:
        //        break;
        //}
    }

    public ArrayList TutorialObjectives()
    {
        ArrayList objectiveList = new ArrayList();
        objectiveList.Add("Find the lost camper\n");
        objectiveList.Add("Escape the forest\n");
        return objectiveList;
    }

    public ArrayList LevelOneObjectives()
    {
        ArrayList objectiveList = new ArrayList();
        objectiveList.Add("Find the lost camper\n");
        objectiveList.Add("Find the axe to chop the tree (optional)");
        objectiveList.Add("Escape the forest\n");
        return objectiveList;
    }

    public ArrayList LevelTwoObjectives()
    {
        ArrayList objectiveList = new ArrayList();
        objectiveList.Add("Find the lost camper\n");
        objectiveList.Add("Find the supplies\n");
        objectiveList.Add("Escape the forest\n");
        return objectiveList;
    }

    public ArrayList LevelThreeObjectives()
    {
        ArrayList objectiveList = new ArrayList();
        objectiveList.Add("Find the lost campter\n");
        objectiveList.Add("Escape the forest\n");
        objectiveList.Add("Get through the barrier\n");
        objectiveList.Add("Find the supplies\n");
        return objectiveList;
    }

    public ArrayList LevelFourObjectives()
    {
        ArrayList objectiveList = new ArrayList();
        //add objectives
        return objectiveList;
    }
}
