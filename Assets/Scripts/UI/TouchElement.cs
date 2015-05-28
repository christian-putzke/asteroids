using UnityEngine;
using UnityEngine.EventSystems;

/**
 * The UI namespace
 */
namespace UI
{
	/**
	 * An UI touch element
	 */
	public class TouchElement : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
	{
		/**
		 * Is true if the element is touched
		 */
		private bool touched = false;


		/**
		 * The finger id which touches the current element
		 */
		private int fingerId;
		
		
		/**
		 * Handles the on pointer down event
		 */
		public void OnPointerDown(PointerEventData pointerEventData)
		{
			if (this.touched == false)
			{
				this.touched	= true;
				this.fingerId	= pointerEventData.pointerId;
			}
		}
		
		/**
		 * Handles the on pointer up event
		 */
		public void OnPointerUp(PointerEventData pointerEventData)
		{
			if (pointerEventData.pointerId == this.fingerId)
			{
				this.touched = false;
			}
		}
		
		
		/**
		 * Returns true if the current element is touched
		 */
		public bool IsTouched()
		{
			return this.touched;
		}


		/**
		 * Returns the current touching finger id
		 */
		public int GetFingerId()
		{
			return this.fingerId;
		}
	}
}