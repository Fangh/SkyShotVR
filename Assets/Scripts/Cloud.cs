using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour 
{
	[Header("Balancing")]
	public float movementSpeed = 5f;

	private float randomAxe = 0;
	// Use this for initialization
	void Start () 
	{
		randomAxe = Random.value;		
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (randomAxe > 0.5f)
			transform.Translate(movementSpeed * Time.deltaTime, 0, 0);
		else
			transform.Translate(0, 0, movementSpeed * Time.deltaTime);
		
	}
}
