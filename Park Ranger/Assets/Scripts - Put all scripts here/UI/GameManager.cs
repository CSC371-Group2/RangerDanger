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
    private bool flare_active = false;

    private float time_under_thresh = 0.0f;
    private int flare_num = 2;

    public bool is_game_paused = false;

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
        check_oil_thresh_time();
        DisplayObjectives();
        shouldDisplayPrompts();
        check_pause();
    }

    private void check_oil_thresh_time()
    {
        if(oldOil < GameSettings.oil_thresh)
        {
            time_under_thresh += Time.deltaTime;
            if(time_under_thresh > GameSettings.oil_feedback_thres_time && flare_num > 0)
            {
                Debug.Log("flare spawn from gm");
                start_flare();
                flare_num--;
            }
        }
        else
        {
            time_under_thresh = 0f;
        }
    }

    private void check_pause()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("escape");
            if (is_game_paused == true)
            {
                Time.timeScale = 1;
                is_game_paused = false;
            }
            else
            {
                Time.timeScale = 0;
                is_game_paused = true;
            }
        }
    }

    private void shouldDisplayPrompts() /* display F/tool prompt for tools and barriers */
    {
        if(F_PROMPT != null)
        {
            if (f_able)
            {
                F_PROMPT.SetActive(true);
            }
            else
            {
                F_PROMPT.SetActive(false);
            }
        }
        
        if (NEED_TOOL_PROMPT != null)
        {
            if (need_tool)
            {
                NEED_TOOL_PROMPT.SetActive(true);
            }
            else
            {
                NEED_TOOL_PROMPT.SetActive(false);
            }
        }
        
    }

    public bool is_flare_active()
    {
        if(flare_active)
        {
            flare_active = false;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void start_flare()
    {
        flare_active = true;
    }

    public void end_flare()
    {
        flare_active = false;
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
        int targetSupply = levelSupplyCount(whichSceneAmI());
        if (supply_count == targetSupply)
        {
            objectiveList.Remove("Find the survival supplies\n");
            objectiveList.Remove("Find the survival supplies: " + (supply_count - 1) + "/" + targetSupply + " found");
        }
        else
        {
            objectiveList.Remove("Find the survival supplies\n");
            objectiveList.Remove("Find the survival supplies: " + (supply_count - 1) + "/" + targetSupply + " found");
            objectiveList.Add("Find the survival supplies: " + supply_count + "/" + targetSupply + " found");
        }
    }

    public int levelSupplyCount(level currentLevel)
    {
        switch (currentLevel)
        {
            case level.LEVEL_TWO:
                return 1;
            case level.LEVEL_THREE:
                return 2;
            default:
                return 0;
        }
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
        if (SceneManager.GetActiveScene().name == "Tutorial")
        {
            return level.TUTORIAL;
        }
        else if (SceneManager.GetActiveScene().name == "Terry_Easy_Maze")
        {
            return level.LEVEL_ONE;
        }
        else if (SceneManager.GetActiveScene().name == "Game")
        {
            return level.LEVEL_TWO;
        }
        else if (SceneManager.GetActiveScene().name == "Level3")
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
                SceneManager.LoadScene("Terry_Easy_Maze");
                break;
            case level.LEVEL_ONE:
                SceneManager.LoadScene("Game");
                break;
            case level.LEVEL_TWO:
                SceneManager.LoadScene("Level3");
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

    public void loadMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public ArrayList LoadObjectives()
    {
        // likely will need two revise scene variables to match up
        ArrayList gameObjectives = new ArrayList(); //arraylist holding all the objectives for each level
        level currentLevel = whichSceneAmI();

        switch (currentLevel)
        {
            case level.TUTORIAL:
                // Tutorial objectives 
                gameObjectives = TutorialObjectives();
                break;
            case level.LEVEL_ONE:
                // tutorial
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
        objectiveList.Add("Find the survival supplies\n");
        objectiveList.Add("Find the lost camper\n");
        objectiveList.Add("Escape the forest\n");
        return objectiveList;
    }

    public ArrayList LevelThreeObjectives()
    {
        ArrayList objectiveList = new ArrayList();
        objectiveList.Add("Find the survival supplies\n");
        objectiveList.Add("Find the lost camper\n");
        objectiveList.Add("Escape the forest\n");
        return objectiveList;
    }

    public ArrayList LevelFourObjectives()
    {
        ArrayList objectiveList = new ArrayList();
        //add objectives
        return objectiveList;
    }
}
