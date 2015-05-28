using UnityEngine;
using System.Collections.Generic;

/**
 * The UI.Panel namespace
 */
namespace UI.Panel
{
	/**
	 * Handles the highscore UI
	 */
	public class Highscore : UI.Panel.Core
	{
		/**
		 * The highscore row blueprint
		 */
		public GameObject rowBlueprint;


		/**
		 * The highscore list transform
		 */
		public Transform highscoreListTransform;


		/**
		 * The distance between two rows
		 */
		public float distanceBetweenTwoRows = 30f;


		/**
		 * The list of all displayed highscore rows
		 */
		private List<UI.Panel.HighscoreRow> rowList = new List<UI.Panel.HighscoreRow>();


		/**
		 * Is called by the new game button within the highscore UI
		 * Starts a new game
		 */
		public void StarNewGame()
		{
			GameManager.Instance.StartNewGame();
		}


		/**
		 * Is called by the main menu button within the highscore UI
		 * Shows the main menu
		 */
		public void ShowMainMenu()
		{
			UIManager.Instance.Show(Enum.UIPanel.MainMenu);
		}


		/**
		 * Fils the highscore list with the highscore data
		 */
		protected override void Initialize()
		{
			this.HideRows();

			var highscoreList = HighscoreManager.Instance.highscoreList;
			for (var rank = 1; rank <= highscoreList.Count; rank ++)
			{
				var highscoreRow = this.GetHighscoreRow();
				highscoreRow.SetRowData(rank, highscoreList[rank - 1]);
			}
		}


		/**
		 * Hides all available rows
		 */
		private void HideRows()
		{
			for (var index = 0; index < rowList.Count; index ++)
			{
				this.rowList[index].gameObject.SetActive(false);
			}
		}


		/**
		 * Returns a valid highscore list row
		 */
		public UI.Panel.HighscoreRow GetHighscoreRow()
		{
			for (var index = 0; index < this.rowList.Count; index ++)
			{
				if (this.rowList[index].gameObject.activeSelf == false)
				{
					this.rowList[index].gameObject.SetActive(true);
					return this.rowList[index];
				}
			}

			return this.AddHighscoreRow();
		}


		/**
		 * Adds a highscore list row
		 */
		private HighscoreRow AddHighscoreRow()
		{
			var row				= this.Instantiate(this.rowBlueprint) as GameObject;
			var highscoreRow	= row.GetComponent<UI.Panel.HighscoreRow>();
			
			row.SetActive(true);
			row.transform.SetParent(this.highscoreListTransform, false);
			
			var rowPosition				= row.transform.localPosition;
			rowPosition.y				= this.rowBlueprint.transform.localPosition.y - (this.rowList.Count * this.distanceBetweenTwoRows);
			row.transform.localPosition	= rowPosition;
			
			this.rowList.Add(highscoreRow);

			return highscoreRow;
		}

		
		
		/**
		 * Reset method of the highscore panel
		 */
		protected override void Reset() {}
	}
}