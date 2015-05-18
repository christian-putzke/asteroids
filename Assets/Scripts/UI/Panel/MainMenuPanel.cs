using UnityEngine;

/**
 * Handles the main menu UI
 */
public class MainMenuPanel : Panel
{
	/**
	 * Initializes some background asteroids on start up
	 */
	private void Start()
	{
		GameManager.Instance.InitializeAsteroids(10);
	}


	/**
	 * Starts a new game
	 * Is called by the start button within the main menu UI
	 */
	public void StartNewGame()
	{
		GameManager.Instance.StartNewGame();
	}


	/**
	 * Shows the highscore screen
	 * Is called by the highscore button within the main menu UI
	 */
	public void ShowHighscore()
	{
		UIManager.Instance.Show(Enum.Panel.Highscore);
	}


	/**
	 * Initalize method of the main menu panel
	 */
	protected override void Initialize() {}
	
	
	/**
	 * Reset method of the main menu panel
	 */
	protected override void Reset() {}
}