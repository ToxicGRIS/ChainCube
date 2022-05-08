using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
	#region Properties

	[SerializeField] private GameObject GameScreen;
	[SerializeField] private GameObject PauseScreen;
	[SerializeField] private Score score;
	[SerializeField] private GameObject gameOverText;

	private static event Action win;
	private static event Action gameOver;
	private static event Action startGame;

	private Controls input;
	private bool isPaused = true;
	private bool isOver = true;

	#endregion
	#region Events

	public static void GameOverInvoke()
	{
		gameOver?.Invoke();
	}

	public static void GameOverSubscribe(Action action)
	{
		gameOver += action;
	}

	public static void GameOverUnsubscribe(Action action)
	{
		gameOver -= action;
	}

	public static void WinInvoke()
	{
		win?.Invoke();
	}

	public static void WinSubscribe(Action action)
	{
		win += action;
	}

	public static void WinUnsubscribe(Action action)
	{
		win -= action;
	}

	public static void StartGameInvoke()
	{
		startGame?.Invoke();
	}

	public static void StartGameSubscribe(Action action)
	{
		startGame += action;
	}

	public static void StartGameUnsubscribe(Action action)
	{
		startGame -= action;
	}

	#endregion
	#region Start

	private void Awake()
	{
		input = new Controls();
	}

	private void Start()
	{
		input.Menu.Pause.performed += e => SwitchPause();
		GameOverSubscribe(GameOver);
	}

	private void OnEnable()
	{
		input.Enable();
	}

	private void OnDisable()
	{
		input.Disable();
	}

	#endregion

	public void ResumeGame()
	{
		if (isOver)
			StartGame();
		else if (isPaused)
			SwitchPause();
		else
		{
			PauseScreen.SetActive(false);
			GameScreen.SetActive(true);
		}
	}

	public void StartGame()
	{
		StartGameInvoke();
		isPaused = false;
		isOver = false;
		PauseScreen.SetActive(false);
		GameScreen.SetActive(true);
		gameOverText.SetActive(false);
	}

	public void SwitchPause()
	{
		if (!isOver)
		{
			if (isPaused)
			{
				PauseScreen.SetActive(false);
				GameScreen.SetActive(true);
				isPaused = false;
			}
			else
			{
				PauseScreen.SetActive(true);
				GameScreen.SetActive(false);
				isPaused = true;
			}
			score.SwitchPause();
		}
	}

	public void GameOver()
	{
		SwitchPause();
		isOver = true;
		gameOverText.SetActive(true);
	}
}
