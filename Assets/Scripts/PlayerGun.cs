using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGun : MonoBehaviour 
{
	[Header("References")]
	public GameObject bulletPrefab;
	public Transform shootPos;
	public AudioClip shotSFX;
	public AudioClip dryShotSFX;
	public AudioClip reloadSFX;
	
	[Header("Balancing")]
	public float shootCooldown = 0.08f;
	public int magazineSize = 20;


	[Header("Private")]
	private float shootCurrentCooldown = 0f;
	private bool canShoot = false;
	private AudioSource audioSource;
	private int magazineContent = 0;

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
		if (shootCurrentCooldown <= 0)
		{
			shootCurrentCooldown = shootCooldown;
			canShoot = true;
		}
		else
			shootCurrentCooldown -= Time.deltaTime;

		if (Input.GetMouseButton(0) && canShoot) // you can stay clicked to shoot quickly
		{
			if ( magazineContent > 0)
				Shoot();
		}
		if (Input.GetMouseButtonDown(0) && magazineContent <= 0) //play dry shot only at each click
		{
			audioSource.PlayOneShot( dryShotSFX );
		}
		if ( Input.GetKeyDown( KeyCode.R ) && !Input.GetMouseButton(0) ) //can reload but not when shooting
		{
			audioSource.PlayOneShot( reloadSFX );
			magazineContent = magazineSize;
		}
	}

	void Shoot()
	{
		magazineContent --;
		audioSource.PlayOneShot( shotSFX );
		canShoot = false;
		GameObject b = Instantiate( bulletPrefab, shootPos.position, shootPos.rotation );
		b.GetComponent<TrailRenderer>().startColor = Color.red;
		b.GetComponent<Bullet>().myOwner = gameObject;
		shootPos.GetComponent<ParticleSystem>().Play();
	}
}
