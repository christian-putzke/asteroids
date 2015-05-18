using UnityEngine;
using UnityEngine.UI;

/**
 * Handles the ingame UI
 */
public class IngamePanel : Panel
{
	/**
	 * The score text
	 */
	public Text score;


	/**
	 * All life indicators
	 */
	public GameObject[] lifeIndicatorArray;


	/**
	 * The thrust button
	 */
	public TouchElement thrustButton;


	/**
	 * The fire button
	 */
	public TouchElement fireButton;


	/**
	 * The rotation input
	 */
	public TouchElement rotationInput;


	/**
	 * The game over screen
	 */
	public GameObject gameOverScreen;


	/**
	 * The game over screen score
	 */
	public Text gameOverScreenScore;


	/**
	 * The game over player name
	 */
	public InputField gameOverPlayerName;



	/**
	 * Handles the touch element inputs
	 */
	private void Update()
	{
		this.HandleThrustButton();
		this.HandleFireButton();
		this.HandleRotationInput();
	}


	/**
	 * Initalize method of the ingame panel
	 */
	protected override void Initialize() {}
	
	
	/**
	 * Reset method of the ingame panel
	 */
	protected override void Reset()
	{
		this.gameOverScreen.SetActive(false);
	}


	/**
	 * Handles the thrust button
	 * Activates / Deactivates the players space ship thrust on touch
	 * 
	 * Note: Is disabled within the Unity editor because it collides with the Unity editor controls
	 */
	private void HandleThrustButton()
	{
		#if !UNITY_EDITOR
			if(!GameManager.Instance.player.hasThrust && this.thrustButton.IsTouched())
			{
				GameManager.Instance.player.hasThrust = true;
			}
			else if (GameManager.Instance.player.hasThrust && !this.thrustButton.IsTouched())
			{
				GameManager.Instance.player.hasThrust = false;
			}
		#endif
	}


	/**
	 * Handles the fire button
	 * Activates / Deactivates the players missile fire on touch
	 */
	private void HandleFireButton()
	{
		if(this.fireButton.IsTouched())
		{
			GameManager.Instance.player.FireMissile();
		}
	}


	/**
	 * Handles the touch rotation input
	 * Rotates the players space ship depending on where he touches the rotation input element
	 */
	private void HandleRotationInput()
	{
		if(this.rotationInput.IsTouched())
		{
			var touches = Input.touches;
			
			for (int index = 0; index < touches.Length; index++)
			{	
				if (this.rotationInput.GetFingerId() == touches[index].fingerId)
				{
					var touchPosition	= touches[index].position;
					var buttonPosition	= Camera.main.WorldToScreenPoint(this.rotationInput.transform.position);
					
					touchPosition.x	-= buttonPosition.x;
					touchPosition.y	-= buttonPosition.y;
					var playerAngle	= Mathf.Atan2(touchPosition.y, touchPosition.x) * Mathf.Rad2Deg;
					
					GameManager.Instance.player.Rotate(Quaternion.Euler(new Vector3(0, 0, playerAngle - 90f)));
					
					return;
				}
			}
		}
	}


	/**
	 * Shows the game over screen
	 */
	public void ShowGameOverScreen()
	{
		this.gameOverScreenScore.text = this.score.text;
		this.gameOverScreen.SetActive(true);
	}


	/**
	 * Sets UI score text
	 */
	public void SetScore(int score)
	{
		this.score.text = score.ToString("n0");
	}


	/**
	 * Saves the current score and shows the highscore list
	 * Is called by the ok button within the ingame game over UI
	 */
	public void SaveScore()
	{
		HighscoreManager.Instance.AddScore(this.gameOverPlayerName.text, GameManager.Instance.player.score);
		UIManager.Instance.Show(Enum.Panel.Highscore);
	}


	/**
	 * Sets the UI life amount
	 */
	public void SetLife(int life)
	{
		for (var index = 0; index < this.lifeIndicatorArray.Length; index ++)
		{
			if (index < life)
			{
				this.lifeIndicatorArray[index].SetActive(true);
			}
			else if (this.lifeIndicatorArray[index].activeSelf == true)
			{
				this.lifeIndicatorArray[index].SetActive(false);
			}
		}
	}
}