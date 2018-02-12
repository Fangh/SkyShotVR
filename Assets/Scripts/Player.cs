using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour 
{
	[Header("References")]
	public Transform attachTo;

	[Header("Private")]
	public static Player Instance;

	Player()
	{
		Instance = this;
	}

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.position = attachTo.position;
	}

	void OnTriggerEnter(Collider other)
	{
		if( other.CompareTag("Bullet") )
		{
			Radar.Instance.OnHit( other.GetComponent<Bullet>().myOwner.transform );
			Debug.Log("ouch !");
			Destroy( other.gameObject );
		}
	}
}
