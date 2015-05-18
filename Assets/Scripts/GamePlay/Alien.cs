using UnityEngine;
using System.Collections;

/**
 * An alien which extends the character class and extends it with an AI
 */
public class Alien : Character
{
	/**
	 * The minimal speed of the alien
	 */
	public float minSpeed;


	/**
	 * The timer when the velocity of the alien will change automaticly
	 */
	public float velocityChangeTimer = 5f;


	/**
	 * The aliens spawn audio clip
	 */
	public AudioClip spawnAudioClip;


	/**
	 * Fires missiles in the players direction if possible
	 */
	private void Update()
	{
		if (this.IsActive() && this.CanFireMissile())
		{
			var playerPosition	= GameManager.Instance.player.transform.position;
			var alienPosition	= this.weaponTransform.position;
			
			playerPosition.x	-= alienPosition.x;
			playerPosition.y	-= alienPosition.y;
			var angle			= (Mathf.Atan2(playerPosition.y, playerPosition.x) * Mathf.Rad2Deg) - 90f;
			var quaternion		= Quaternion.Euler(new Vector3(0f, 0f, angle));

			this.FireMissile(alienPosition, quaternion, playerPosition.normalized);
		}
	}


	/**
	 * Activates the alien
	 */
	public override void Activate(Vector2 position, Vector2 velocity)
	{
		var	speed							= Random.Range(this.minSpeed, this.maxSpeed);
		this.gameObject.transform.position	= position;
		this.lastSeenInViewport				= Time.time; // Ensures that last seen in viewport will work correctly
		this.lastMissileShot				= Time.time; // Ensures that the alien will fire after a short delay

		AudioManager.Instance.Play(this.spawnAudioClip);
		this.gameObject.SetActive(true);

		this.rigidbody2DComponent.velocity = velocity * speed;
		
		Invoke("ChangeVelocityRandomly", this.velocityChangeTimer);
	}


	/**
	 * Deactivates the alien if he is too long outside of the viewport
	 */
	protected override void TooLongOutsideOfViewport()
	{
		Debug.Log("TooLongOutsideOfViewport!");
		this.Deactivate();
	}


	/**
	 * Changes the velocity of the alien randomly
	 */
	private void ChangeVelocityRandomly()
	{
		var	speed							= Random.Range(this.minSpeed, this.maxSpeed);
		this.rigidbody2DComponent.velocity	= new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * speed;

		Invoke("ChangeVelocityRandomly", this.velocityChangeTimer);
	}
}
