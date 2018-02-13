using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour 
{

	[Header("Balancing")]
	public float movementSpeed = 5f;
	public float rotationSpeed = 5f;

	[Header("Reference")]
	public Lever lever;

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		if( OVRInput.Get(OVRInput.Button.One, OVRInput.Controller.LTouch)  )
		{
			transform.Translate( Vector3.forward * movementSpeed * Time.deltaTime );
		}

		float x = lever.GetSpeedX();
		float y = lever.GetSpeedY();

		// Debug.Log("x = " + x);

		// Debug.Log("currentRotation = " + transform.rotation.eulerAngles);
		Vector3 newRotation = transform.rotation.eulerAngles;
		newRotation.x += x * rotationSpeed * Time.deltaTime;
		newRotation.y += y * rotationSpeed * Time.deltaTime;

		// if ( ( newRotation.x < 45
		// || newRotation.x > 315 )
		// && ( newRotation.y < 45
		// || newRotation.y > 315) )
		// {
			transform.rotation = Quaternion.Euler( newRotation );
		// }


		
	}
}
