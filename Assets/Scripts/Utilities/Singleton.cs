using UnityEngine;

/**
 * The singleton will make every class which extends this singleton class to a singleton
 * It can be accessed like this: YourClassName.Instance.publicVariable
 */
public class Singleton<Type> : MonoBehaviour where Type : MonoBehaviour
{
	/**
	 * The singleton instance
	 */
	protected static Type instance;
	
	
	/**
	 * Returns the instance of the class
	 */
	public static Type Instance
	{
		get
		{
			if(Singleton<Type>.instance == null)
			{
				Singleton<Type>.instance = (Type) Singleton<Type>.FindObjectOfType(typeof(Type));
				
				if (Singleton<Type>.instance == null)
				{
					throw new UnityException("Couldn't find an instance of " + typeof(Type));
				}
			}
			
			return Singleton<Type>.instance;
		}
	}
}