using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemiBehavior : MonoBehaviour 
{
	PlayerController player;
	Rigidbody rb;
	float distanceFromPlayer;

	[Header("Balancing")]
	public float distanceMaxFromPlayer = 30f;
	public float distanceMinFromPlayer = 2f;
	public float distanceToShotPlayer = 3f;
	public float movementSpeed = 4f;
	public float rotationSpeed = 1f;

	// Use this for initialization
	void Start () 
	{
		player = PlayerController.Instance;
		rb = GetComponent<Rigidbody>();		
	}
	
	// Update is called once per frame
	void Update () 
	{
		distanceFromPlayer = Vector3.Distance(transform.position, player.transform.position);
		if (di)
	}

	void MoveAround()
	{

	}

	void LockPlayer()
	{

	}
}
