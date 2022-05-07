using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
	#region Properties

	[SerializeField] private static int currentScore;
	[SerializeField] private AnimationCurve chanceOfCube;
	[SerializeField] private Camera mainCamera;
	[SerializeField] private bool isPlaying;
	[SerializeField] private GameObject cubePrefab;
	[SerializeField] private float maxThrowPositionDelta;
	[SerializeField] [Range(0, 1000)] private float throwingForce;
	[SerializeField] private TextMeshProUGUI scoreText;

	private Controls input;
	private bool isGrabbing;
	private GameObject currentCube;
	private Rigidbody currentRigitbody;
	private bool isPaused = true;

	private static event Action<int> addScore;

	public int CurrentScore
	{
		get => currentScore;
		private set { currentScore = value; scoreText.text = $"{Localisation.CurrentLocalisation[3]}{currentScore}"; }
	}

	#endregion
	#region Start

	private void Awake()
	{
		input = new Controls();
		AddScoreSubscribe(e => CurrentScore += e);
	}

	private void Start()
	{
		input.Gameplay.Grab.performed += e => Grab();
		input.Gameplay.Throw.performed += e => Throw();

		GameState.GameOverSubscribe(GameOver);
		GameState.StartGameSubscribe(StartPlay);
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
	#region Events

	public static void AddScoreInvoke(int amount)
	{
		addScore?.Invoke(amount);
	}

	public static void AddScoreSubscribe(Action<int> action)
	{
		addScore += action;
	}

	public static void AddScoreUnsubscribe(Action<int> action)
	{
		addScore -= action;
	}

	#endregion

	private void Grab()
	{
		if (isPlaying)
			isGrabbing = true;
	}

	private void Throw()
	{
		if (isGrabbing)
		{
			isGrabbing = false;
			currentRigitbody.constraints = RigidbodyConstraints.None;
			currentRigitbody.AddForce(Vector3.forward * throwingForce, ForceMode.VelocityChange);
		}
	}

	private void SpawnCube()
	{
		if (currentCube == null)
		{
			currentCube = Instantiate(cubePrefab, transform.position, Quaternion.identity);
			currentRigitbody = currentCube.GetComponent<Rigidbody>();
			currentRigitbody.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
			Cube thisCube = currentCube.GetComponent<Cube>();
			thisCube.Number = Mathf.RoundToInt(chanceOfCube.Evaluate(UnityEngine.Random.Range(0f, 1f)));
			CurrentScore += thisCube.Kit[thisCube.Number].Number;
		}
	}

	private void FixedUpdate()
	{
		if (isGrabbing && isPlaying && currentCube)
		{
			 Vector3 delta = mainCamera.ScreenToViewportPoint(input.Gameplay.CursorDelta.ReadValue<Vector2>()) * Vector3.Distance(transform.position, mainCamera.transform.position);
			Vector3 newPosition = new Vector3(Mathf.Clamp(currentCube.transform.position.x + delta.x, transform.position.x - maxThrowPositionDelta, transform.position.x + maxThrowPositionDelta), currentCube.transform.position.y, currentCube.transform.position.z);
			currentCube.transform.position = newPosition;
			Debug.Log(newPosition);
		}
	}

	private void GameOver()
	{
		isPlaying = false;
		GameState.GameOverInvoke();
	}

	public void StartPlay()
	{
		isPlaying = true;
		isPaused = false;
		CurrentScore = 0;
		SpawnCube();
		input.Gameplay.Enable();
	}

	public void SwitchPause()
	{
		if (isPaused)
		{
			isPlaying = true;
			input.Gameplay.Enable();
			isPaused = false;
		}
		else
		{
			isPlaying = false;
			input.Gameplay.Disable();
			isPaused = true;
		}
	}

	#region Trigger

	private void OnTriggerExit(Collider other)
	{
		if (other.attachedRigidbody == currentRigitbody)
		{
			currentRigitbody = null;
			currentCube = null;
			SpawnCube();
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.attachedRigidbody != currentRigitbody && other.gameObject.GetComponent<Cube>()) GameState.GameOverInvoke();
	}

	#endregion
}
