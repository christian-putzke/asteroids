using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/**
 * An asteroid which extends the flying object class
 */
public class Asteroid : FlyingObject
{
	/**
	 * The minimal speed of the asteroid
	 */
	public float minSpeed = 1f;


	/**
	 * The asteroids size
	 */
	public Enum.AsteroidSize size;


	/**
	 * The amount of spawned asteroids after destruction
	 */
	public int spawnAmountAfterDestruction = 2;


	/**
	 * The spawn size after destruction
	 */
	public Enum.AsteroidSize spawnAsteroidSize = Enum.AsteroidSize.Big;


	/**
	 * An array of all asteroid sprites
	 */
	public Sprite[] spriteArray;


	/**
	 * An array of colliders for every asteroid shape within a specific size
	 */
	private PolygonCollider2D[] polygonColliderArray;


	/**
	 * The image component
	 */
	private Image image;


	/**
	 * Loads all needed components on start up
	 */
	protected override void Awake()
	{
		base.Awake();

		this.polygonColliderArray	= this.GetComponents<PolygonCollider2D>();
		this.image					= this.GetComponent<Image>();
	}


	/**
	 * Is called if the asteroid is too long outside of the viewport
	 * Deactives the current one a spawns a new one with the same size and to the players direction
	 */
	protected override void TooLongOutsideOfViewport()
	{
		this.Deactivate();
		AsteroidManager.Instance.SpawnAsteroid(this.size);
	}


	/**
	 * Is called after a hit by a collider
	 */
	public override void HitByCollider(Collider2D collider)
	{
		base.HitByCollider(collider); // Call the base method to use standard missile handling

		if (collider.CompareTag(Config.Tags.flyingObject))
		{
			var flyingObject = collider.GetComponent<FlyingObject>();
			if (flyingObject.flyingObject != Enum.FlyingObject.Asteroid)
			{
				this.Destroy(false);
			}
		}
	}


	/**
	 * Destroies the asteroid and spawns new if configured
	 */
	public override void Destroy(bool addScore = true)
	{
		AudioManager.Instance.Play(this.destructionAudioClip);
		this.Deactivate();

		if (this.spawnAmountAfterDestruction > 0)
		{
			for (var index = 0; index < this.spawnAmountAfterDestruction; index ++)
			{
				AsteroidManager.Instance.SpawnAsteroid(this.spawnAsteroidSize, this.transform.position);
			}
		}
		else
		{
			GameManager.Instance.SpawnNewAsteroids();
		}

		if (addScore == true)
		{
			GameManager.Instance.player.AddScore(this.score);
		}
	}


	/**
	 * Deactivates the asteroid
	 */
	public override void Deactivate()
	{
		this.gameObject.SetActive(false);
	}
	
	
	/**
	 * Activates the asteroid and chooses a random appearance
	 */
	public override void Activate(Vector2 position, Vector2 velocity)
	{
		this.SetRandomAppearance();

		var	speed		= Random.Range(this.minSpeed, this.maxSpeed);
		var rotation	= Random.Range(0f, 359.99f);

		this.gameObject.transform.position	= position;
		this.gameObject.transform.rotation	= Quaternion.Euler(new Vector3(0, 0, rotation));

		this.gameObject.SetActive(true);

		this.rigidbody2DComponent.velocity = velocity.normalized * speed;

		// Set last seen in viewport even if the asteroid is positioned outside of the viewport so that it will be validated correctly
		this.lastSeenInViewport	= Time.time;
	}


	/**
	 * Sets the appearance and the appropriate collider of the asteroid randomly
	 */
	private void SetRandomAppearance()
	{
		var	appearance		= (int) Random.Range(0, this.spriteArray.Length);
		this.image.sprite	= this.spriteArray[appearance];
		this.image.SetNativeSize();

		for (var index = 0; index < this.spriteArray.Length; index ++)
		{
			if (index == appearance)
			{
				this.polygonColliderArray[index].enabled = true;
			}
			else
			{
				this.polygonColliderArray[index].enabled = false;
			}
		}
	}
}
