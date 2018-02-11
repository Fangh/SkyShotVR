using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour 
{
	[Header("Balancing")]
	public float autoDestroyTime = 0.2f; //0.2 at 100m/s = 20m max
	public float speed = 100f; //100m/s

	[Header("Private")]
	public GameObject myOwner;

	// Use this for initialization
	void Start () 
	{
		Destroy( gameObject, autoDestroyTime );
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.Translate(transform.forward * Time.deltaTime * speed, Space.World);
		
	}
}
