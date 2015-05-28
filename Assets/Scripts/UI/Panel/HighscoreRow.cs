using UnityEngine;
using UnityEngine.UI;

/**
 * The UI panel namespace
 */
namespace UI.Panel
{
	/**
	 * Handles a row within the highscore list
	 */
	public class HighscoreRow : MonoBehaviour
	{
		/**
		 * The rank
		 */
		public Text rank;


		/**
		 * The players name
		 */
		public Text playerName;


		/**
		 * The score
		 */
		public Text score;


		/**
		 * Sets the row display data
		 */
		public void SetRowData(int rank, Struct.Score scoreStruct)
		{
			this.rank.text			= rank + ".";
			this.playerName.text	= scoreStruct.name;
			this.score.text			= scoreStruct.score.ToString("n0");
		}
	}
}