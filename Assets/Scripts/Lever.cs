using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Lever : MonoBehaviour 
{
	[Header("References")]
	public OvrAvatar ovrAvatar;
	public GameObject handPos;

	[Header("Balancing")]
	public float maxAngle = 30f;

	[Header("Private")]
	private bool isGrabbed = false;
	private bool canBeGrabbed = false;
	private Quaternion originalRot;
	private Tween returnAtOrigin;

	// Use this for initialization
	void Start () 
	{
		originalRot = transform.localRotation;
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Grab
		if ( canBeGrabbed 
		&& !isGrabbed 
		&& OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.Touch) > 0.8f )
		{
			isGrabbed = true;
			ovrAvatar.LeftHandCustomPose = handPos.transform;
		}

		//ungrab
		if ( isGrabbed 
		&& OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.Touch) < 0.1f )
		{
			UnGrab();
		}

		if ( isGrabbed )
		{
			Transform handTransform = ovrAvatar.GetHandTransform( OvrAvatar.HandType.Left, OvrAvatar.HandJoint.HandBase );
			Vector3 direction = handTransform.position - transform.position;
			Debug.DrawLine(transform.position, handTransform.position, Color.blue);
			// Debug.DrawRay(transform.position, direction * 2, Color.blue );
			transform.LookAt(handTransform.position);
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if ( other.CompareTag("Player") 
		&& ( null == returnAtOrigin || !returnAtOrigin.IsPlaying() ))
		{
			canBeGrabbed = true;
		}		
	}

	void OnTriggerExit(Collider other)
	{
		if ( other.CompareTag("Player") )
		{
			canBeGrabbed = false;
		}
	}

	void UnGrab()
	{
		isGrabbed = false;
		returnAtOrigin = transform.DOLocalRotateQuaternion(originalRot, 1f);
		ovrAvatar.LeftHandCustomPose = null;
	}

	public float GetSpeedY()
	{
		float y = transform.localRotation.eulerAngles.y;
		if ( y > 180)
			y -= 360;

		return (y / maxAngle);
	}

	public float GetSpeedX()
	{
		float x = transform.localRotation.eulerAngles.x;
		if ( x > 180)
			x -= 360;
		return (x / maxAngle);
	}

	public bool IsGrabbed()
	{
		return isGrabbed;
	}
}
