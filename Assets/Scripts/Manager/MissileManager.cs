using UnityEngine;
using System.Collections.Generic;

/**
 * The missile manager handles every missile within the game and pools them
 */
public class MissileManager : Singleton<MissileManager>
{
	/**
	 * The missile prefabs
	 */
	public GameObject[] missilePrefabs;


	/**
	 * The missile container so that new missiles do not clutter our hierachy view
	 */
	public Transform missileContainer;


	/**
	 * The missile object pool so we do not have to inistantiate and destroy them every time
	 */
	private Dictionary<Enum.FlyingObject, List<Missile>> missileObjectPool = new Dictionary<Enum.FlyingObject, List<Missile>>();


	/**
	 * Adds a missile to the object pool by the given flying object
	 */
	private Missile AddMissileToObjectPool(Enum.FlyingObject flyingObject)
	{
		var flyingObjectIndex				= (int) flyingObject;
		var missileGameObject				= this.Instantiate(this.missilePrefabs[flyingObjectIndex]) as GameObject;
		missileGameObject.name				= this.missilePrefabs[flyingObjectIndex].name;
		missileGameObject.transform.SetParent(this.missileContainer, false);
		var missile							= missileGameObject.GetComponent<Missile>();

		if (!this.missileObjectPool.ContainsKey(flyingObject))
		{
			this.missileObjectPool.Add(flyingObject, new List<Missile>());
		}

		this.missileObjectPool[flyingObject].Add(missile);
		
		return missile;
	}


	/**
	 * Returns a pooled missile
	 */
	public Missile GetMissile(Enum.FlyingObject flyingObject)
	{
		if (this.missileObjectPool.ContainsKey(flyingObject))
		{
			for (var index = 0; index < this.missileObjectPool[flyingObject].Count; index ++)
			{
				if (!this.missileObjectPool[flyingObject][index].IsActive())
				{
					return this.missileObjectPool[flyingObject][index];
				}
			}
		}
		
		return this.AddMissileToObjectPool(flyingObject);
	}
}