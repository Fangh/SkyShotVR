using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Radar : MonoBehaviour 
{
	[Header("References")]
	public GameObject playerSprite;
	public GameObject enemySpritePrefab;
	public GameObject hitSprite;

	[Header("Private")]
	public static Radar Instance;
	private List<GameObject> enemiesSpritesList = new List<GameObject>();
	private SpawnManager SM;

	Radar()
	{
		Instance = this;
	}

	// Use this for initialization
	void Start () 
	{
		SM = SpawnManager.Instance;
	}
	
	// Update is called once per frame
	void Update () 
	{
		for ( int i = 0; i < enemiesSpritesList.Count; i++ )
		{
			if (null == enemiesSpritesList[i])
				return;

			Vector3 relativePos = SM.enemiesList[i].transform.position - Player.Instance.transform.position;
			// relativePos.Normalize();
			// Debug.Log( "relative pos = " + relativePos );
			relativePos *= 0.0008f;
			// Debug.Log( "relative pos after scame = " + relativePos );
			
			Debug.DrawLine( playerSprite.transform.position, playerSprite.transform.position + relativePos, Color.cyan );
			enemiesSpritesList[i].transform.localPosition = relativePos;

		}
	}

	public void OnHit(Transform e)
	{
		Vector3 relativePos = e.position - Player.Instance.transform.position;
		hitSprite.transform.LookAt(hitSprite.transform.position + relativePos);
		// Debug.DrawLine(hitSprite.transform.position, hitSprite.transform.position + relativePos, Color.yellow, 1f);
		hitSprite.GetComponentInChildren<SpriteRenderer>().color = Color.white;
		hitSprite.GetComponentInChildren<SpriteRenderer>().DOFade(0,2f);
	}

	public void AddEnemy( GameObject e )
	{
		GameObject s = Instantiate( enemySpritePrefab, transform.position, Quaternion.identity );
		enemiesSpritesList.Add( s );
		s.name = e.GetInstanceID().ToString();
		s.transform.parent = transform;
	}
	
	public void RemoveEnemy( GameObject e )
	{
		// Debug.Log("trying to remove " + e.name, e );
		GameObject o = FindEnemySpriteByID( e.GetInstanceID().ToString() );
		// Debug.Log("I found " + o.name, o);
		enemiesSpritesList.Remove( o );
		Destroy( o );
	}

	GameObject FindEnemySpriteByID(string id)
	{
		// Debug.Log("trying to find an object called " + id);
		foreach (GameObject o in enemiesSpritesList)
		{
			if (o.name == id)
				return o;
		}
		return null;
	}

}