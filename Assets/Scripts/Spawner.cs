using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour 
{
	[Header("References")]
	public Transform spawnPos;

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	public float FindNearestEnnemi()
	{
		float distanceMin = float.MaxValue;
		foreach (EnemyBehavior e in SpawnManager.Instance.enemiesList)
		{
			float distance = Vector3.Distance(transform.position, e.transform.position);
			if (distance < distanceMin)
				distanceMin = distance;
		}

		return distanceMin;
	}
}
