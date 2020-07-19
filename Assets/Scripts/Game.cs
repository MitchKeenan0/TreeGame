using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
	public CanvasGroup gameUI;
	public CanvasGroup endScreen;

    void Awake()
    {
		Cursor.lockState = CursorLockMode.Locked;
		Application.targetFrameRate = 60;
		SetEndScreen(false);
    }

	void SetEndScreen(bool value)
	{
		endScreen.alpha = value ? 1f : 0f;
		endScreen.interactable = value;
		endScreen.blocksRaycasts = value;

		gameUI.alpha = !value ? 1f : 0f;
		gameUI.interactable = !value;
		gameUI.blocksRaycasts = !value;
	}

	public void GameOver()
	{
		Time.timeScale = 0.2f;
		SetEndScreen(true);
	}

	public void ResetGame()
	{
		UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
		SceneManager.LoadScene(0);
		Time.timeScale = 1f;
	}
}
