using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour 
{
	[Header("References")]
	public GameObject explosionPrefab;
	public GameObject shootPrefab;
	public GameObject impactPrefab;
	public AudioClip explosionSFX;
	public AudioClip shootSFX;
	public Transform gun1;
	public Transform gun2;
	public ParticleSystem smokeFX;


	[Header("Balancing")]
	public float distanceMaxFromPlayer = 30f;
	public float distanceMinFromPlayer = 2f;
	public float distanceToShotPlayer = 3f;
	public float distanceToLockPlayer = 10f;
	public float movementSpeed = 4f;
	public float rotationSpeed = 1f;
	public int healthPoints = 3;
	public float shootCooldown = 3f;
	public float fallingTime = 2f;


	[Header("Private")]
	private PlayerController player;
	private Rigidbody rb;
	private float distanceFromPlayer = 0;
	private bool playerIsLocked = false;
	private Vector3 destination = Vector3.zero;
	private bool stopMoving = false;
	private float originalDrag = 0;
	private float shootCurrentCooldown = 0f;
	private AudioSource audioSource;
	private bool isDead = false;

	// Use this for initialization
	void Start () 
	{
		player = PlayerController.Instance;
		rb = GetComponent<Rigidbody>();	
		audioSource = GetComponent<AudioSource>();
		originalDrag = rb.drag;
		shootCurrentCooldown = shootCooldown;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if ( isDead )
			return;

		distanceFromPlayer = Vector3.Distance( transform.position, player.transform.position );
		Debug.DrawLine( transform.position, destination, Color.red );

		//if you are far from player, move freely
		if ( distanceFromPlayer > distanceToLockPlayer && !playerIsLocked )
		{
			// Debug.Log("[Ennemi] Moving around", this);
			MoveAround();
		}
		//if you are near player, go toward them.
		else if ( distanceFromPlayer > distanceMinFromPlayer )
		{
			// Debug.Log("[Ennemi] Locking Player", this);
			//lookatPlayerAnd go toward them. And don't move freely ever again
			LockPlayer();
		}
		else
		{
			// Debug.Log("[Ennemi] Stop Moving", this);
			// Debug.Log( "distanceFromPlayer = " + distanceFromPlayer );
			stopMoving = true;
		}
		// if you are at range, shoot !
		if ( distanceFromPlayer < distanceToShotPlayer )
		{
			ShootAtPlayer();
		}
	}

	//I choosed to move the ennemi with forces to make it drift. So it may not be 4m/s
	//to make it move at 4m/s I should have use transform.Translate(forward * speed * Time.deltaTime)
	void FixedUpdate()
	{
		if ( isDead )
			return;

		if ( stopMoving )
		{
			rb.drag = 10f;
		}
		else
		{
			rb.drag = originalDrag;
			if ( destination != Vector3.zero)
			{
				// Debug.Log("[Ennemi] moving toward destination", this);
				rb.AddRelativeForce( Vector3.forward * movementSpeed );
			}
		}
	}

	void MoveAround()
	{
		//if you do not have a destination, take one random not too far from player
		if ( destination == Vector3.zero )
		{
			destination = Random.insideUnitSphere * distanceMaxFromPlayer;
			destination += player.transform.position;
		}
		if (Vector3.Distance( transform.position, destination) < 1f )
		{
			destination = Vector3.zero;
		}
		SmoothLookAt(destination);
		stopMoving = false;
	}

	void LockPlayer()
	{
		playerIsLocked = true;
		destination = player.transform.position;
		stopMoving = false;
		SmoothLookAt( destination );
	}

	void ShootAtPlayer()
	{
		SmoothLookAt( player.transform.position );
		if ( shootCurrentCooldown <= 0 )
		{
			shootCurrentCooldown = shootCooldown;
			audioSource.PlayOneShot( shootSFX );
			GameObject s1 =Instantiate( shootPrefab, gun1.transform.position, gun1.transform.rotation );
			GameObject s2 =Instantiate( shootPrefab, gun2.transform.position, gun2.transform.rotation );
			s1.GetComponent<Bullet>().myOwner = gameObject;
			s2.GetComponent<Bullet>().myOwner = gameObject;
		}
		else
		{
			shootCurrentCooldown -= Time.deltaTime;
		}
	}

	void SmoothLookAt(Vector3 target)
	{
		Vector3 relativePos = target - transform.position;
        Quaternion rotation = Quaternion.LookRotation( relativePos );
		transform.rotation = Quaternion.Slerp( transform.rotation, rotation, Time.deltaTime * rotationSpeed );
	}

	public void Hit()
	{
		healthPoints--;
		if ( healthPoints <= 0 )
		{
			Fall();
		}
	}

	[ContextMenu ("Fall")] //to test in editor easily
	void Fall()
	{
		rb.drag = 0;
		rb.AddTorque(Random.insideUnitSphere * 500f, ForceMode.Impulse);
		rb.useGravity = true;
		isDead = true;
		smokeFX.Play();
		Invoke("Die", fallingTime);
	}

	public void Die()
	{
		smokeFX.transform.parent = null;
		GameObject.Instantiate( explosionPrefab, transform.position, Quaternion.identity );
		Destroy(gameObject);
	}

	void OnDestroy()
	{		
		SpawnManager.Instance.RemoveEnemy( this );
	}

	void OnCollisionEnter(Collision collisionInfo)
	{
		if ( collisionInfo.collider.CompareTag("Bullet") )
		{
			Hit();
			GameObject i = Instantiate( impactPrefab, collisionInfo.contacts[0].point, Quaternion.identity );
			Vector3 pointToLook = collisionInfo.contacts[0].point - collisionInfo.contacts[0].normal;
			i.transform.LookAt( pointToLook );
			i.transform.parent = transform;
			Destroy( collisionInfo.gameObject );
		}		
	}
}
