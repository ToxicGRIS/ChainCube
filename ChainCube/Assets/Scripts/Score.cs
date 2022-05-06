using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
	#region Properties

	[SerializeField] private int currentScore;
	[SerializeField] private AnimationCurve chanceOfCube;
	[SerializeField] private Camera mainCamera;
	[SerializeField] private bool isPlaying;
	[SerializeField] private GameObject cubePrefab;
	[SerializeField] private float maxThrowPositionDelta;
	[SerializeField] private FixedJoint joint;
	[SerializeField] [Range(0, 1000)] private float throwingForce;

	private static event Action win;
	private static event Action gameOver;
	private Controls input;
	private bool isGrabbing;
	private GameObject currentCube;
	private Rigidbody currentRigitbody;

	#endregion
	#region Start

	private void Awake()
	{
		input = new Controls();
	}

	private void OnEnable()
	{
		input.Enable();
	}

	private void Start()
	{
		input.Gameplay.Grab.performed += e => Grab();
		input.Gameplay.Throw.performed += e => { if (isGrabbing) Throw(); };
		SpawnCube();
	}

	private void OnDisable()
	{
		input.Disable();
	}

	#endregion

	private void Grab()
	{
		isGrabbing = true;
		joint.connectedBody = currentRigitbody;
	}

	private void Throw()
	{
		isGrabbing = false;
		currentRigitbody.constraints = RigidbodyConstraints.None;
		currentRigitbody.AddForce(Vector3.forward * throwingForce, ForceMode.VelocityChange);
	}

	private void SpawnCube()
	{
		if (currentCube == null)
		{
			currentCube = Instantiate(cubePrefab, transform.position, Quaternion.identity);
			currentRigitbody = currentCube.GetComponent<Rigidbody>();
			currentRigitbody.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
			currentCube.GetComponent<Cube>().Number = Mathf.RoundToInt(chanceOfCube.Evaluate(UnityEngine.Random.Range(0f, 1f)));
		}
	}

	private void FixedUpdate()
	{
		if (isGrabbing)
		{
			 Vector3 delta = mainCamera.ScreenToViewportPoint(input.Gameplay.CursorDelta.ReadValue<Vector2>()) * Vector3.Distance(transform.position, mainCamera.transform.position);
			Vector3 newPosition = new Vector3(Mathf.Clamp(currentCube.transform.position.x + delta.x, transform.position.x - maxThrowPositionDelta, transform.position.x + maxThrowPositionDelta), currentCube.transform.position.y, currentCube.transform.position.z);
			currentCube.transform.position = newPosition;
			Debug.Log(newPosition);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.attachedRigidbody == currentRigitbody)
		{
			currentRigitbody = null;
			currentCube = null;
		}
		SpawnCube();
	}
}
