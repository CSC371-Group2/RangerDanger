using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Level1_Script : MonoBehaviour
{
    //public string[] objectiveList = { "Find the lost camper\n", "Escape the forest\n" };
    public ArrayList objectiveList = new ArrayList();
    public TextMeshProUGUI objectives;
    // Start is called before the first frame update
    void Start()
    {
        objectiveList.Add("Find the lost camper\n");
        objectiveList.Add("Escape the forest\n");
        objectives.text = "Objectives:\n" + objectiveList[0] + "\n";
    }

    // Update is called once per frame
    void Update()
    {
        objectives.text = objectiveString();
    }

    public string objectiveString()
    {
        string objectiveStr = "";

        foreach (string obj in objectiveList)
        {
            objectiveStr += obj;
        }

        return objectiveStr;
    }

    public void updateObjective(string eventTitle)
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

}
