using UnityEngine;

/**
 * The missile of a character
 */
public class Missile : MonoBehaviour
{
	/**
	 * The missiles lifetime
	 */
	public float lifetime = 1f;


	/**
	 * The missiles speed
	 */
	public float speed = 500f;


	/**
	 * The layer mask which is used for the collision raycast
	 */
	public LayerMask collisionRaycastLayerMask;


	/**
	 * The missile is owned by the specified flying object
	 */
	public Enum.FlyingObject ownedBy;


	/**
	 * The missiles rigidbody 2D
	 */
	private Rigidbody2D rigidbody2DComponent;


	/**
	 * The missiles collider
	 */ 
	private Collider2D collider2DComponent;


	/**
	 * The missiles despawn time
	 */
	private float despawnTime;


	/**
	 * The previous rigidbody2D position
	 */
	private Vector3 previousRigidbody2DPosition;


	/**
	 * Deactivate the missile on instantiation
	 * Grep the some needed components
	 */
	private void Awake()
	{
		this.rigidbody2DComponent			= this.GetComponent<Rigidbody2D>();
		this.collider2DComponent			= this.GetComponent<Collider2D>();
		this.previousRigidbody2DPosition	= this.rigidbody2DComponent.position;

		this.Deactivate();
	}

	
	/**
	 * Deactivates the missile after it reaches the despawn time
	 * Also uses raycast to extend the collision detection for "high" speed objects to help the slow physics collision detection
	 */
	private void FixedUpdate()
	{
		if (this.IsActive())
		{
			var raycastToPosition	= this.transform.position - this.previousRigidbody2DPosition;
			RaycastHit2D hit		= Physics2D.Raycast(this.previousRigidbody2DPosition, raycastToPosition, Vector2.Distance(this.transform.position, this.previousRigidbody2DPosition), this.collisionRaycastLayerMask.value);

			if (hit.collider != null && hit.collider.CompareTag(Config.Tags.flyingObject))
			{
				var flyingObject = hit.collider.GetComponent<FlyingObject>();
				if (this.ownedBy != flyingObject.flyingObject)
				{
					flyingObject.HitByCollider(this.collider2DComponent);
					this.rigidbody2DComponent.position = hit.point;
				}
			}

			this.previousRigidbody2DPosition = this.rigidbody2DComponent.position;

			if (this.despawnTime <= Time.time)
			{
				this.Deactivate();
			}
		}
	}


	/**
	 * Deactivates the missile
	 */
	public void Deactivate()
	{
		this.gameObject.SetActive(false);
		this.rigidbody2DComponent.velocity = Vector2.zero;
	}


	/**
	 * Activates the missile and sets position and rotation and adds a force to the rigidbody
	 */
	public void Activate(Vector3 position, Quaternion rotation, Vector2 force)
	{
		this.gameObject.transform.position	= position;
		this.gameObject.transform.rotation	= rotation;
		this.despawnTime					= Time.time + this.lifetime;
		this.rigidbody2DComponent.position	= position;
		this.previousRigidbody2DPosition	= this.rigidbody2DComponent.position;

		this.gameObject.SetActive(true);

		this.rigidbody2DComponent.AddForce(force * this.speed);
	}


	/**
	 * Returns true if the missile is active
	 */
	public bool IsActive()
	{
		return this.gameObject.activeSelf;
	}
}
