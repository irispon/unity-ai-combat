using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour {

    public Canvas m_MainCanvas;
    public Button m_StartButton;
    public Button m_ExitButton;

	// Use this for initialization
	void Start () 
	{
        //m_MainCanvas = this.GetComponent<Canvas>();
        //m_StartButton = this.GetComponent<Button>();
        //m_ExitButton = this.GetComponent<Button>();

    }

    public void StartPress()
    {
        SceneManager.LoadScene(0);
    }
	
	public void ExitPress () 
	{
        Application.Quit();
	}
}
