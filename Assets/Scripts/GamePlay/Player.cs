using UnityEngine;
using System.Collections;

/**
 * The player which extends the character class
 */
public class Player : Character
{
	/**
	 * The thrust of the players space ship
	 */
	public float thrust = 10f;


	/**
	 * The thrust loss ratio of the players space ship
	 */
	public float thrustLossRatio = 0.02f;


	/**
	 * The max life and start amount of the player
	 */
	public int maxLife = 3;


	/**
	 * The time the player stays indestructible after a hit
	 */
	public float indestructibleTime = 3f;


	/**
	 * Is true if the players space ship has thrust 
	 * Also plays / stops the looped thrust sfx
	 */
	public bool hasThrust
	{
		get
		{
			return this._hasThrust;
		}
		set
		{
			if (value == true)
			{
				this.audioSource.Play();
			}
			else
			{
				this.audioSource.Stop();
			}
			
			this._hasThrust = value;
		}
	}


	/**
	 * The current life amount of the player
	 */
	private int life = 0;


	/**
	 * The time until the player is indestructible
	 */
	private float indestructibleUntil = 0f;


	/**
	 * The players audio source
	 */
	private AudioSource audioSource;


	/**
	 * Is true if the players space ship has thrust 
	 */
	private bool _hasThrust = false;


	/**
	 * Loads needed components
	 */
	protected override void Awake()
	{
		base.Awake();
		
		this.audioSource = this.GetComponent<AudioSource>();
	}


	#if UNITY_EDITOR
		/**
		 * Handle player inputs within the Unity Editor
		 */
		private void Update()
		{
				this.RotateAroundMouse();

				if (Input.GetKeyDown(KeyCode.LeftControl))
				{
					this.hasThrust = true;
				}
				else if (this.hasThrust && Input.GetKeyUp(KeyCode.LeftControl))
				{
					this.hasThrust = false;
				}

				if (Input.GetMouseButton(0))
				{
					this.FireMissile();
				}
		}
	#endif


	/**
	 * Gives thrust to the player or reduces the thrust automaticly to zero
	 */
	private void FixedUpdate()
	{
		if (this.IsActive() && this.hasThrust)
		{
			this.Thrust();
		}
		else
		{
			this.ReduceThrust();
		}
	}


	/**
	 * Fires a missile if possible
	 */
	public void FireMissile()
	{
		if (this.IsActive() && this.lastMissileShot + this.delayBetweenMissiles <= Time.time)
		{
			this.lastMissileShot	= Time.time;
			var missile				= MissileManager.Instance.GetMissile(this.flyingObject);

			missile.Activate(this.weaponTransform.position, this.transform.rotation, this.transform.up);
		}
	}


	/**
	 * Rotates the player around the mouse
	 */
	private void RotateAroundMouse()
	{
		var mousePosition	= Input.mousePosition;
		var playerPosition	= Camera.main.WorldToScreenPoint(this.transform.position);
		
		mousePosition.x	-= playerPosition.x;
		mousePosition.y	-= playerPosition.y;
		var playerAngle	= Mathf.Atan2(mousePosition.y, mousePosition.x) * Mathf.Rad2Deg;
		
		this.transform.rotation = Quaternion.Euler(new Vector3(0, 0, playerAngle - 90f));
	}


	/**
	 * Increases or holds the players thrust
	 */
	public void Thrust()
	{
		if(this.rigidbody2DComponent.velocity.magnitude > this.maxSpeed)
		{
			this.rigidbody2DComponent.velocity = this.rigidbody2DComponent.velocity.normalized * this.maxSpeed;
		}
		else
		{
			this.rigidbody2DComponent.AddForce(this.transform.up * this.thrust);
		}
	}


	/**
	 * Reduces the players thrust
	 */
	private void ReduceThrust()
	{
		if (this.rigidbody2DComponent.velocity.magnitude > 0)
		{
			this.rigidbody2DComponent.velocity *= 1f - this.thrustLossRatio;
		}
	}


	/**
	 * Sets the players rotation
	 */
	public void Rotate(Quaternion rotation)
	{
		this.transform.rotation = rotation;
	}


	/**
	 * Returns true if the player is indistructable
	 */
	public bool IsIndistructable()
	{
		return this.indestructibleUntil > Time.time;
	}


	/**
	 * Adds the given score to the player score
	 */
	public void AddScore(int score)
	{
		this.SetScore(this.score + score);
	}


	/**
	 * Sets the players score
	 */
	private void SetScore(int score)
	{
		this.score = score;

		UIManager.Instance.GetPanel<IngamePanel>(Enum.Panel.Ingame).SetScore(this.score);
	}


	/**
	 * Reduces the players life
	 */
	private void ReduceLife(int life = 1)
	{
		this.SetLife(this.life - life);
	}


	/**
	 * Sets the players lifes
	 */
	private void SetLife(int life)
	{
		this.life = life;

		UIManager.Instance.GetPanel<IngamePanel>(Enum.Panel.Ingame).SetLife(this.life);
	}


	/**
	 * Destroies the player
	 */
	public override void Destroy(bool addScore = false)
	{
		if (!this.IsIndistructable())
		{
			AudioManager.Instance.Play(this.destructionAudioClip);

			this.ReduceLife();
			
			if (this.life == 0)
			{
				this.Deactivate();
				GameManager.Instance.GameOver();
			}
			else
			{
				this.indestructibleUntil					= Time.time + this.indestructibleTime;
				this.transform.position						= Vector3.zero;
				this.rigidbody2DComponent.velocity			= Vector2.zero;
				this.hasThrust								= false;
				
				this.GetComponent<Animator>().SetBool("Indistructable", true);
				this.Invoke("RemoveIndistructable", this.indestructibleTime);
			}
		}
	}


	/**
	 * Activates the player
	 */
	public void Activate()
	{
		this.gameObject.SetActive(true);
		this.Reset();
	}


	/**
	 * Removes the indistructable effect
	 */
	private void RemoveIndistructable()
	{
		this.GetComponent<Animator>().SetBool("Indistructable", false);
	}


	/**
	 * Resets the player object
	 */
	private void Reset()
	{
		this.SetScore(0);
		this.SetLife(this.maxLife);

		this.indestructibleUntil			= 0f;
		this.transform.position				= Vector3.zero;
		this.rigidbody2DComponent.velocity	= Vector2.zero;
	}
}
