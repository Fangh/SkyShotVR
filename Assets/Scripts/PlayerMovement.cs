using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerMovement : MonoBehaviour 
{

	[Header("Balancing")]
	public float movementSpeed = 5f;
	public float rotationSpeed = 5f;
	public float smoothReset = 0.3f;

	[Header("Reference")]
	public Lever lever;
	public ParticleSystem particleLeft;
	public ParticleSystem particleRight;

	[Header("Private")]
	private AudioSource audioSource;
	private float currentSpeed = 0f;

	// Use this for initialization
	void Start () 
	{
		audioSource = GetComponent<AudioSource>();
		audioSource.volume = 0;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if( OVRInput.Get(OVRInput.Button.One, OVRInput.Controller.LTouch) || Input.GetKey(KeyCode.UpArrow)  )
		{
			currentSpeed = Mathf.Lerp( currentSpeed, movementSpeed, Time.deltaTime );
			if( audioSource.volume != 0.5f)
			{
				audioSource.DOFade(0.5f, 2f);
			}
		}
		else
		{
			currentSpeed = Mathf.Lerp( currentSpeed, 0, Time.deltaTime );
			if( audioSource.volume != 0 )
			{
				audioSource.DOFade(0, 2f);
			}
		}
		transform.position += transform.forward * currentSpeed * Time.deltaTime ;

		float x = lever.GetSpeedX();
		float y = lever.GetSpeedY();

		var r = particleRight.main;
		var l = particleLeft.main;
		r.startSizeMultiplier = Mathf.Abs( y );
		l.startSizeMultiplier = Mathf.Abs( y );
		r.startSpeedMultiplier = Mathf.Abs( y );
		l.startSpeedMultiplier = Mathf.Abs( y );

		// Debug.Log("x = " + x);
		if (y > 0)
		{
			if (particleLeft.isStopped )
			{
				particleLeft.GetComponent<AudioSource>().DOFade(0.5f, 1);
				particleRight.GetComponent<AudioSource>().DOFade(0f, 1);
				particleLeft.Play();
				particleRight.Stop();
			}
		}
		else if( y < 0 )
		{
			if ( particleRight.isStopped )
			{
				particleRight.GetComponent<AudioSource>().DOFade(0.5f, 1);
				particleLeft.GetComponent<AudioSource>().DOFade(0f, 1);
				particleRight.Play();
				particleLeft.Stop();
			}
		}
		else
		{
			if ( particleRight.isPlaying || particleLeft.isPlaying )
			{
				particleRight.GetComponent<AudioSource>().DOFade(0f, 1);
				particleLeft.GetComponent<AudioSource>().DOFade(0f, 1);
				particleRight.Stop();
				particleLeft.Stop();
			}
		}
		// Debug.Log("x = " + x);

		// Debug.Log("currentRotation = " + transform.rotation.eulerAngles);
		Vector3 newRotation = transform.rotation.eulerAngles;
		if(x == 0)
		{
			newRotation.x = Mathf.Lerp( newRotation.x, 0, Time.deltaTime * smoothReset );
		}
		else
			newRotation.x += x * rotationSpeed * Time.deltaTime;

		newRotation.y += y * rotationSpeed * Time.deltaTime;


		if ( ( newRotation.x < 45
		|| newRotation.x > 315 ) )
		// && ( newRotation.y < 45
		// || newRotation.y > 315) )
		{
			transform.rotation = Quaternion.Euler( newRotation );
		}



		
	}
}
