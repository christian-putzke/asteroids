using UnityEngine;
using UnityEngine.UI;

/**
 * Handles the viewport behaviour
 */
public class Viewport : MonoBehaviour
{
	/**
	 * The main rect transform from the gameplay object which defines the viewport
	 */
	public RectTransform viewport;

	/**
	 * Adjust the viewport collider to the cameras viewport
	 */
	private void Start()
	{
		var collider		= this.GetComponent<BoxCollider2D>();
		var colliderSize	= collider.size;

		colliderSize.x	= this.viewport.sizeDelta.x;
		colliderSize.y	= this.viewport.sizeDelta.y;

		collider.size = colliderSize;
	}


	/**
	 * Notifies flying objects if they leave the viewport
	 */
	private void OnTriggerExit2D(Collider2D collider)
	{
		if (collider.CompareTag(Config.Tags.flyingObject))
		{
			collider.GetComponent<FlyingObject>().OnLeaveViewport();
		}
	}
}
