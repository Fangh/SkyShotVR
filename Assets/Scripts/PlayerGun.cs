using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerGun : MonoBehaviour 
{
	[Header("References")]
	public GameObject bulletPrefab;
	public Transform shootPos;
	public AudioClip shotSFX;
	public AudioClip dryShotSFX;
	public AudioClip reloadSFX;
	public GameObject magazine;
	public Transform orderPos;
	public OvrAvatar ovrAvatar;
	public string bulletMask;
	
	[Header("Balancing")]
	public float shootCooldown = 0.08f;
	public int magazineSize = 20;


	[Header("Private")]
	private float shootCurrentCooldown = 0f;
	private bool canShoot = false;
	private AudioSource audioSource;
	private int magazineContent = 0;
	private bool isGrabbed = false;
	private bool isInOrder = true;

	private Tween orderTween;

	// Use this for initialization
	void Start () 
	{
		shootCurrentCooldown = shootCooldown;
		magazineContent = magazineSize;
		audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if ( shootCurrentCooldown <= 0 )
		{
			shootCurrentCooldown = shootCooldown;
			canShoot = true;
		}
		else
			shootCurrentCooldown -= Time.deltaTime;

		if ( isGrabbed 
		&& OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger, OVRInput.Controller.Touch) > 0.8f 
		&& canShoot ) // you can stay clicked to shoot quickly
		{
			if ( magazineContent > 0)
				Shoot();
			else
			{
				audioSource.clip = dryShotSFX;
				audioSource.Play();
			}
		}
		if ( OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.RTouch) 
		&& OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger, OVRInput.Controller.Touch) < 0.1f
		&& isGrabbed ) //can reload but not when shooting
		{
			audioSource.PlayOneShot( reloadSFX );
			magazineContent = magazineSize;
			magazine.transform.localScale = Vector3.one;
		}

		//when ungrab go to original pos
		if ( !isGrabbed && !isInOrder && ( null == orderTween || !orderTween.IsActive() ) )
		{
			orderTween = transform.DOMove( orderPos.position, 1f ).OnComplete( IsInOrder );
			transform.DORotateQuaternion( orderPos.rotation, 1f );
			GetComponent<OVRGrabbable>().enabled = false;
		}
		// if ( isGrabbed )
		// {
		// 	transform.rotation = ovrAvatar.HandRight.transform.rotation;
		// }
	}

	void LateUpdate()
	{		
		if ( isInOrder )
		{
			transform.position = orderPos.position;
			transform.rotation = orderPos.rotation;
		}
	}

	void IsInOrder()
	{
		isInOrder = true;
		GetComponent<OVRGrabbable>().enabled = true;
	}

	public void GrabEnd()
	{
		ovrAvatar.HandRight.gameObject.SetActive(true);
		isGrabbed = false;
		// transform.parent = null;
	}

	public void GrabBegin()
	{
		ovrAvatar.HandRight.gameObject.SetActive(false);
		isGrabbed = true;
		isInOrder = false;
		// transform.parent = ovrAvatar.transform;
	}

	void Shoot()
	{
		magazine.transform.localScale -= Vector3.up / magazineSize;
		magazineContent --;
		audioSource.PlayOneShot( shotSFX );
		canShoot = false;
		GameObject b = Instantiate( bulletPrefab, shootPos.position, shootPos.rotation );
		b.GetComponent<TrailRenderer>().startColor = Color.red;
		b.GetComponent<Bullet>().myOwner = gameObject;
		b.layer = LayerMask.NameToLayer(bulletMask.ToString());
		shootPos.GetComponent<ParticleSystem>().Play();
	}
}
