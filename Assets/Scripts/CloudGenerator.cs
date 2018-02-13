using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudGenerator : MonoBehaviour 
{
	[Header("Refenrences")]
	public GameObject[] cloudsPrefab;

	[Header("Balancing")]
	public int nbClouds = 200;
	public float altitudeRange = 100;
	public float xRange = 100;
	public float zRange = 100;

	// Use this for initialization
	void Start () 
	{
		for(int i = 0; i < nbClouds; i++)
		{
			GameObject o = Instantiate(cloudsPrefab[Random.Range(0,cloudsPrefab.Length)],
			new Vector3(Random.Range(-xRange, xRange), Random.Range(-altitudeRange, altitudeRange), Random.Range(-zRange, zRange)),
			Quaternion.Euler(0, Random.Range(0,360), 0));
			o.transform.parent = transform;
		}
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
}
