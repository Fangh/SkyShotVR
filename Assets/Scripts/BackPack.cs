using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackPack : MonoBehaviour 
{
	[Header("References")]
	public Transform attachTo;
	public Transform player;

	[Header("Private")]
	public static BackPack Instance;

	BackPack()
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
		transform.rotation = player.rotation;
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
