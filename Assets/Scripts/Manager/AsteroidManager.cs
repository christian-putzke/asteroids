using UnityEngine;
using System.Collections.Generic;

/**
 * The asteroid manager handles the asteroids spawning and object pooling of every asteroid
 */
public class AsteroidManager : Singleton<AsteroidManager>
{
	/**
	 * The asteroid prefabs
	 */
	public GameObject[] asteroidPrefabs;


	/**
	 * The asteroid container so that new asteroids do not clutter our hierachy view
	 */
	public Transform asteroidContainer;


	/**
	 * The asteroid object pool so we do not have to inistantiate and destroy them every time
	 */
	private Dictionary<Enum.AsteroidSize, List<Asteroid>> asteroidObjectPool = new Dictionary<Enum.AsteroidSize, List<Asteroid>>();


	/**
	 * Adds an asteroid to the object pool by the given size
	 */
	private Asteroid AddAsteroidToObjectPool(Enum.AsteroidSize size)
	{
		var asteroidSizeIndex				= (int) size;
		var asteroidGameObject				= this.Instantiate(this.asteroidPrefabs[asteroidSizeIndex]) as GameObject;
		asteroidGameObject.name				= this.asteroidPrefabs[asteroidSizeIndex].name;
		asteroidGameObject.transform.SetParent(this.asteroidContainer, false);
		var asteroid						= asteroidGameObject.GetComponent<Asteroid>();

		if (!this.asteroidObjectPool.ContainsKey(size))
		{
			this.asteroidObjectPool.Add(size, new List<Asteroid>());
		}

		this.asteroidObjectPool[size].Add(asteroid);
		
		return asteroid;
	}


	/**
	 * Deactivates all asteroids
	 */
	public void DeactivateAllAsteroids()
	{
		if (this.GetActiveAsteroidAmount() > 0)
		{
			foreach (KeyValuePair<Enum.AsteroidSize, List<Asteroid>> asteroid in this.asteroidObjectPool)
			{
				for (var index = 0; index < asteroid.Value.Count; index ++)
				{
					asteroid.Value[index].Deactivate();
				}
			}
		}
	}


	/**
	 * Returns the active asteroid amount
	 */
	public int GetActiveAsteroidAmount()
	{
		var activeAsteroidAmount = 0;

		foreach (KeyValuePair<Enum.AsteroidSize, List<Asteroid>> asteroid in this.asteroidObjectPool)
		{
			for (var index = 0; index < asteroid.Value.Count; index ++)
			{
				if (asteroid.Value[index].IsActive())
				{
					activeAsteroidAmount ++;
				}
			}
		}

		return activeAsteroidAmount;
	}


	/**
	 * Returns a pooled asteroid
	 */
	public Asteroid GetAsteroid(Enum.AsteroidSize size)
	{
		if (this.asteroidObjectPool.ContainsKey(size))
		{
			for (var index = 0; index < this.asteroidObjectPool[size].Count; index ++)
			{
				if (!this.asteroidObjectPool[size][index].IsActive())
				{
					return this.asteroidObjectPool[size][index];
				}
			}
		}
		
		return this.AddAsteroidToObjectPool(size);
	}


	/**
	 * Spawns an asteroid
	 * If position and velocity were not set	-> spawn outside the viewport and target the player
	 * If velocity is not set					-> use random velocity
	 */
	public void SpawnAsteroid(Enum.AsteroidSize size, Vector2 position = default(Vector2), Vector2 velocity = default(Vector2))
	{
		if (position == default(Vector2) && velocity == default(Vector2))
		{
			var playerPosition	= GameManager.Instance.player.transform.position;
			var spawnPosition	= GameManager.Instance.GetValidSpawnCoordinateOutsideTheViewport();
			
			playerPosition.x	-= spawnPosition.x;
			playerPosition.y	-= spawnPosition.y;

			position			= spawnPosition;
			velocity			= playerPosition.normalized;
		}
		else if (velocity == default(Vector2))
		{
			velocity = new Vector2(Random.Range(-1, 1f), Random.Range(-1f, 1f));
		}

		var asteroid = AsteroidManager.Instance.GetAsteroid(size);
		asteroid.Activate(position, velocity);
	}
}