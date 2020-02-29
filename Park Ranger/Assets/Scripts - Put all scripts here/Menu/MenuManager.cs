using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
	public GameObject MainMenu; // 0
	public GameObject LevelSelect; // 1
	public GameObject ControlsPage; // 2
	public GameObject OptionsPage; // 3
	public GameObject CreditsPage; // 4
	private GameObject[] MenuPages;
    private int curr_canvas = 0;


    // Start is called before the first frame update
    void Start()
    {
    	MenuPages = new GameObject[] {MainMenu, LevelSelect, ControlsPage, OptionsPage, CreditsPage};
    	MenuPages[0].SetActive(true);
    	for (int i = 1; i < MenuPages.Length; i ++)
    	{
    		MenuPages[i].SetActive(false);
    	}
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Navigate_Menu(int next)
    {
    	MenuPages[curr_canvas].SetActive(false);
    	MenuPages[next].SetActive(true);
    	curr_canvas = next;
    }

    public void QuitGame() 
    {
		#if UNITY_EDITOR
		    UnityEditor.EditorApplication.isPlaying = false;
		#else
	        Application.Quit();
		#endif
 	}

 	public void LoadLevel(string level)
 	{
 		SceneManager.LoadScene(level);
 	}
}
