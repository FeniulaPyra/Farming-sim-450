using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	public GameObject helpOverlay;
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
		helpOverlay.SetActive(!helpOverlay.activeSelf);
	}
	public void QuitGame()
	{
		Application.Quit();
	}
}
