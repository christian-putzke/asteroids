using UnityEngine;
using System.Collections.Generic;

/**
 * A character which extends the flying object class and extends it with misiles
 */
public abstract class Character : FlyingObject
{
	/**
	 * The characters weapon transform
	 */
	public Transform weaponTransform;


	/**
	 * The delay between the missiles
	 */
	public float delayBetweenMissiles = 0.1f;


	/**
	 * The timestamp when the last missile was shot
	 */
	protected float lastMissileShot = 0f;


	/**
	 * Is called if the character is too long outside of the viewport
	 */
	protected override void TooLongOutsideOfViewport() {}


	/**
	 * Is called after a hit by a collider
	 */
	public override void HitByCollider(Collider2D collider)
	{
		base.HitByCollider(collider); // Call the base method to use standard missile handling

		if (collider.CompareTag(Config.Tags.flyingObject))
		{
			this.Destroy(false);
		}
	}


	/**
	 * Activates the character
	 */
	public override void Activate(Vector2 position, Vector2 velocity) {}


	/**
	 * Destroies the character
	 */
	public override void Destroy(bool addScore)
	{
		AudioManager.Instance.Play(this.destructionAudioClip);
		this.Deactivate();
		
		if (addScore == true)
		{
			GameManager.Instance.player.AddScore(this.score);
		}
	}


	/**
	 * Deactivates the character
	 */
	public override void Deactivate()
	{
		this.gameObject.SetActive(false);
		this.CancelInvoke();
	}


	/**
	 * Fires a missile
	 */
	public void FireMissile(Vector3 position, Quaternion rotation, Vector2 force)
	{
		if (this.CanFireMissile())
		{
			this.lastMissileShot	= Time.time;
			var missile				= MissileManager.Instance.GetMissile(this.flyingObject);

			missile.Activate(position, rotation, force);
		}
	}


	/**
	 * Returns true if the character can fire a missile
	 */
	public bool CanFireMissile()
	{
		return (this.lastMissileShot + this.delayBetweenMissiles <= Time.time);
	}
}
