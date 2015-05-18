/**
 * The struct class contains every struct used within this project
 * Structs within this project are just simple data container
 */
public class Struct
{
	/**
	 * A score entry
	 */
	public struct Score
	{
		/**
		 * The score of the player
		 */
		public int score;


		/**
		 * The name of the player
		 */
		public string name;


		/**
		 * The Constructor
		 */
		public Score(string name, int score)
		{
			this.name	= name;
			this.score	= score;
		}
	}
}
