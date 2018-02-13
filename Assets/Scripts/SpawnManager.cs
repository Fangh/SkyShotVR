using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SpawnManager : MonoBehaviour 
{
	[Header("References")]
	public GameObject enemyPrefab;

	[Header("Balancing")]
	public float spawnCooldown = 3f;
	public int maxEnemies = 10;
	
	[Header("Privates")]
	private List<Spawner> spawnsList = new List<Spawner>();
	public static SpawnManager Instance;
	public List<EnemyBehavior> enemiesList = new List<EnemyBehavior>();
	private float spawnCurrentCooldown;

	SpawnManager()
	{
		Instance = this;
	}

	// Use this for initialization
	void Start () 
	{
		spawnsList.AddRange( GameObject.FindObjectsOfType<Spawner>() );
		spawnCurrentCooldown = spawnCooldown;		
	}
	
	// Update is called once per frame
	void Update () 
	{
		if ( spawnCurrentCooldown <= 0 )
		{
			spawnCurrentCooldown = spawnCooldown;
			if ( enemiesList.Count < maxEnemies )
				Spawn();
		}
		else
		{
			spawnCurrentCooldown -= Time.deltaTime;
		}
	}

	void Spawn()
	{
		//dictionary of each spawner
		Dictionary<Spawner, float> spawnersByDistance = new Dictionary<Spawner, float>();
		foreach( Spawner s in spawnsList )
		{
			float distance = s.FindNearestEnnemi();
			spawnersByDistance.Add(s,distance);
		}
		//sort this dictionary by distance
		List<float> distances = spawnersByDistance.Values.ToList();
        distances.Sort();
		distances.Reverse();

		// //debug to check if it's correct
		// for(int i = 0; i < distances.Count; i++)
		// {
		// 	Debug.Log("my list["+i+"] = " + distances[i] );
		// }

		//keep only the nearest (because I will remove them from spawnerByDistance)
		distances.RemoveRange(0, 2);
		
		//find the nearest distance, and remove the associate spawner
		for(int i = 0; i < distances.Count; i++)
		{
			Spawner myKey = spawnersByDistance.FirstOrDefault( x => x.Value == distances[i] ).Key; // this is not exact if there are equal distances
			spawnersByDistance.Remove(myKey);
		}

		// //debug to check if it's correct
		// for(int i = 0; i < spawnersByDistance.Count; i++)
		// {
		// 	Debug.Log("mySpawnerList = " + spawnersByDistance.ElementAt(i));
		// }


		//now getting distance from player
		float distanceFromPlayer = float.MaxValue;
		Spawner rightSpawner = null;
		foreach(var s in spawnersByDistance)
		{
			//taking only the spawner wich is nearer the player;
			float distance = Vector3.Distance(s.Key.transform.position, BackPack.Instance.transform.position);
			if ( distance < distanceFromPlayer)
			{
				distanceFromPlayer = distance;
				rightSpawner = s.Key;
				// Debug.Log("the right spawner is at " + distanceFromPlayer + "m from player");
				// Debug.Log("right Spawner = " + rightSpawner.name, rightSpawner);
			}
		}

		GameObject e = GameObject.Instantiate( enemyPrefab, rightSpawner.transform.position, rightSpawner.transform.rotation );
		enemiesList.Add( e.GetComponent<EnemyBehavior>() );
		Radar.Instance.AddEnemy( e );
	}

	public void RemoveEnemy( EnemyBehavior e )
	{
		enemiesList.Remove(e);
		Radar.Instance.RemoveEnemy( e.gameObject );
	}
}
