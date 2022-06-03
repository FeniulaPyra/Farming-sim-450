using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class MainMenu : MonoBehaviour
{
	public GameObject helpOverlay;
	public GameObject controls;
	public GameObject gameInfo;
	public GameObject titleScreen;

	private void Start()
	{
		
	}
	private void Update()
	{
        if (Keyboard.current.escapeKey.wasPressedThisFrame == true && helpOverlay.activeSelf)//if (Input.GetKeyDown(KeyCode.Escape) && helpOverlay.activeSelf)
        {
			ToggleHelpOverlay();
		}
	}

	public void GoToGameScene()
	{
		SceneManager.LoadScene("GroundScene", LoadSceneMode.Single);
	}

    public void GoToTutorial()
    {
        SceneManager.LoadScene("TutorialScene", LoadSceneMode.Single);
    }

	public void ToggleHelpOverlay()
	{
		titleScreen.SetActive(!titleScreen.activeSelf);
		helpOverlay.SetActive(!helpOverlay.activeSelf);

		if (helpOverlay.activeSelf)
		{
			gameInfo.SetActive(true);
			controls.SetActive(false);
		}

	}

	public void OpenControls()
	{
		gameInfo.SetActive(false);
		controls.SetActive(true);
	}

	public void OpenGameInfo()
	{
		gameInfo.SetActive(true);
		controls.SetActive(false);
	}

	public void QuitGame()
	{
		Application.Quit();
	}
}
