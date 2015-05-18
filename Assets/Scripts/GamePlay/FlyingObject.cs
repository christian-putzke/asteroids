using UnityEngine;

/**
 * The flying object ground class which provides some basic features every flying object needs
 * It extends more specific flying objects
 */
public abstract class FlyingObject : MonoBehaviour
{
	/**
	 * The flying object type
	 */
	public Enum.FlyingObject flyingObject;


	/**
	 * The score of the flying object
	 */
	public int score = 0;


	/**
	 * The maximal speed of the flying object
	 */
	public float maxSpeed = 1f;


	/**
	 * The flying objects destruction audio clip
	 */
	public AudioClip destructionAudioClip;


	/**
	 * The rigidbody 2D component of the flying object
	 */
	protected Rigidbody2D rigidbody2DComponent;


	/**
	 * The time when the flying object was last seen within the viewport
	 */
	protected float lastSeenInViewport;


	/**
	 * Loads all needed components and starts the "in view port" verification
	 */
	protected virtual void Awake()
	{
		this.rigidbody2DComponent = this.GetComponent<Rigidbody2D>();

		this.Invoke("VerifyIsInViewport", 1f);
	}


	/**
	 * Returns true if the flying object is active
	 */
	public bool IsActive()
	{
		return this.gameObject.activeSelf;
	}


	/**
	 * Is called if a flying object was hit by a collider
	 */
	public virtual void HitByCollider(Collider2D collider)
	{
		if (collider.CompareTag(Config.Tags.missile))
		{
			var missile = collider.GetComponent<Missile>();

			if (missile.ownedBy != this.flyingObject)
			{
				var addScore = (missile.ownedBy == Enum.FlyingObject.Player);

				missile.Deactivate();
				this.Destroy(addScore);
			}
		}
	}


	/**
	 * Is called if the flying object is too long outside of the viewport
	 */
	protected abstract void TooLongOutsideOfViewport();


	/**
	 * Is called if the flying object is destroied
	 */
	public abstract void Destroy(bool addScore);


	/**
	 * Activates the flying pbject
	 */
	public abstract void Activate(Vector2 position, Vector2 velocity);


	/**
	 * Deactivates the flying pbject
	 */
	public abstract void Deactivate();


	/**
	 * Handles what hapens if a missile enters the flying object collider
	 */
	protected virtual void OnTriggerEnter2D(Collider2D collider)
	{
		this.HitByCollider(collider);
	}


	/**
	 * Handles the flying object if it leaves the viewport
	 * In this case: Teleports the flying object to the other side of the "world"
	 */
	public void OnLeaveViewport()
	{
		var screenPosition		= Camera.main.WorldToScreenPoint(this.transform.position);
		var position			= this.transform.position;

		// Teleport on the x-axis
		if (Screen.width < screenPosition.x || screenPosition.x < 0)
		{
			position.x *= -1;
		}
		// Teleport on the y-axis
		else if (Screen.height < screenPosition.y || screenPosition.y < 0)
		{
			position.y *= -1;
		}
		
		this.transform.position	= position;
	}


	/**
	 * Verifies that the flying object is in the viewport after a teleport
	 */
	private void VerifyIsInViewport()
	{
		if (this.IsActive())
		{
			RaycastHit2D hit = Physics2D.Raycast(this.transform.position, -Vector2.up, Mathf.Infinity, Config.Layer.viewport);
			if (hit.collider != null && hit.collider.CompareTag(Config.Tags.viewport))
			{
				this.lastSeenInViewport = Time.time;
			}
			else if (this.lastSeenInViewport + 5f < Time.time)
			{
				this.TooLongOutsideOfViewport();
			}
		}

		this.Invoke("VerifyIsInViewport", 1f);
	}
}
