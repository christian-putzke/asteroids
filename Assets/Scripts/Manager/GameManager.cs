using UnityEngine;

/**
 * The game manager handles the game and provides easy access to relevant objects
 */
public class GameManager : Singleton<GameManager>
{
	/**
	 * The player
	 */
	public Player player;


	/**
	 * The alien
	 */
	public Alien alien;


	/**
	 * Start with this amount of asteroids
	 */
	public int asteroidStartAmount = 4;


	/**
	 * This is the minimum amount of active asteroids
	 * Exception: On game start there can be less asteroids
	 */
	public int minActiveAsteroids = 6;


	/**
	 * Defines after how many seconds the next alien will spawn
	 */
	public float alienSpawnTimer = 30f;


	/**
	 * Starts a new game
	 * - Shows the ingame ui
	 * - Spawns random start asteroids
	 * - Triggers the alien spawn
	 * - Activates the players space ship
	 */
	public void StartNewGame()
	{
		UIManager.Instance.Show(Enum.UIPanel.Ingame);

		this.player.Activate();
		this.InitializeAsteroids(this.asteroidStartAmount);
		this.Invoke("SpawnAlien", this.alienSpawnTimer);
	}


	/**
	 * Initializes the given amount of asteroids
	 */
	public void InitializeAsteroids(int amount)
	{
		AsteroidManager.Instance.DeactivateAllAsteroids();

		for (var index = 0; index < amount; index ++)
		{
			var asteroidSize = (Enum.AsteroidSize) Random.Range(0, 3);
			AsteroidManager.Instance.SpawnAsteroid(asteroidSize, this.GetValidSpawnCoordinate());
		}
	}


	/**
	 * Spawns new asteroids outside the viewport until the asteroid amount reaches the minimum active asteroid amount
	 */
	public void SpawnNewAsteroids()
	{
		if (AsteroidManager.Instance.GetActiveAsteroidAmount() < this.minActiveAsteroids)
		{
			var spawnAmount = this.minActiveAsteroids - AsteroidManager.Instance.GetActiveAsteroidAmount();
			for (var index = 0; index < spawnAmount; index ++)
			{
				var asteroidSize = (Enum.AsteroidSize) Random.Range(0, 3);
				AsteroidManager.Instance.SpawnAsteroid(asteroidSize);
			}
		}
	}
	

	/**
	 * Spawns the alien if it's not already active
	 */
	private void SpawnAlien()
	{
		if (!this.alien.IsActive())
		{
			var playerPosition	= GameManager.Instance.player.transform.position;
			var spawnPosition	= this.GetValidSpawnCoordinateOutsideTheViewport();
			
			playerPosition.x	-= spawnPosition.x;
			playerPosition.y	-= spawnPosition.y;
			
			this.alien.Activate(spawnPosition, playerPosition.normalized);
		}

		this.Invoke("SpawnAlien", this.alienSpawnTimer);
	}


	/**
	 * Returns a valid spawn point inside the viewport
	 */
	public Vector2 GetValidSpawnCoordinate()
	{
		var randomScreenCoordinate	= new Vector2(Random.Range(0f, Screen.width), Random.Range(0f, Screen.height));
		var randomWorldCoordinate	= Camera.main.ScreenToWorldPoint(randomScreenCoordinate);

		RaycastHit2D hit = Physics2D.Raycast(randomWorldCoordinate, -Vector2.up);
		if (hit.collider != null && !hit.collider.CompareTag(Config.Tags.viewport))
		{
			return this.GetValidSpawnCoordinate();
		}
		else
		{
			return randomWorldCoordinate;
		}
	}


	/**
	 * Returns a valid spawn point outside of the viewport
	 */
	public Vector2 GetValidSpawnCoordinateOutsideTheViewport()
	{
		var randomScreenCoordinate	= new Vector2(Random.Range(0f - (Screen.width * 0.4f), Screen.width * 1.4f), Random.Range(0f * (Screen.height * 0.2f), Screen.height * 1.2f));
		var randomWorldCoordinate	= Camera.main.ScreenToWorldPoint(randomScreenCoordinate);
		
		RaycastHit2D hit = Physics2D.Raycast(randomWorldCoordinate, -Vector2.up);
		if (hit.collider != null)
		{
			return this.GetValidSpawnCoordinateOutsideTheViewport();
		}
		else
		{
			return randomWorldCoordinate;
		}
	}


	/**
	 * Is called if the game is over
	 */
	public void GameOver()
	{
		// Stops the alien respawn
		this.CancelInvoke();

		UIManager.Instance.GetPanel<UI.Panel.Ingame>(Enum.UIPanel.Ingame).ShowGameOverScreen();
	}
}
