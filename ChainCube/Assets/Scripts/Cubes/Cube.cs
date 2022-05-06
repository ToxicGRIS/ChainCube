using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[DisallowMultipleComponent]
public class Cube : MonoBehaviour
{
	#region Properties

	[SerializeField][Min(0)] private int number;
	[SerializeField] private TextMeshPro[] texts;
	[SerializeField] private CubesKit kit;
	[SerializeField] [Range(0, 1000)] float jumpForce;

	private Collider colliderComponent;
	private Material materialComponent;
	private Rigidbody rigidbodyComponent;

	[SerializeField] public int Number 
	{ 
		get => number;
		set
		{
			if (value < 0 || value >= kit.Lenght) throw new System.Exception("Number is out of range.");
			number = value;
			UpdateVisuals();
		}
	}

	#endregion
	#region Start

	private void Awake()
	{
		colliderComponent = GetComponent<Collider>();
		materialComponent = GetComponent<Renderer>().material;
		rigidbodyComponent = GetComponent<Rigidbody>();
	}

	private void Start()
	{
		UpdateVisuals();
	}

	#endregion

	private void OnCollisionEnter(Collision collision)
	{
		Cube other = collision.gameObject.GetComponent<Cube>();
		if (other?.Number == Number && colliderComponent.enabled)
		{
			Vector3 otherCubePosition = collision.transform.position;
			collision.collider.enabled = false;
			Destroy(collision.gameObject);
			transform.position = (transform.position + otherCubePosition) / 2;
			rigidbodyComponent.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
			Number++;
		}
	}

	public void UpdateVisuals()
	{
		materialComponent.color = kit[number].Color;
		foreach (var t in texts)
		{
			t.text = $"{kit[number].Number}";
		}
	}
}
