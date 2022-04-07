using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
		if (Input.GetKeyDown(KeyCode.Escape) && helpOverlay.activeSelf)
		{
			ToggleHelpOverlay();
		}
	}

	public void GoToGameScene()
	{
		SceneManager.LoadScene("GroundScene", LoadSceneMode.Single);
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
