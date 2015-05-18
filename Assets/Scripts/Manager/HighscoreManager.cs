using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/**
 * The highscore manager handles the highscore list
 */
public class HighscoreManager : Singleton<HighscoreManager>
{
	/**
	 * The length of the highscore list
	 */
	public int highscoreListLength = 15;


	/**
	 * The highscore list
	 */
	[HideInInspector]
	public List<Struct.Score> highscoreList = new List<Struct.Score>();


	/**
	 * Loads the highscore list from the player prefs
	 */
	private void Start()
	{
		for (var rank = 1; rank <= this.highscoreListLength; rank ++)
		{
			if (PlayerPrefs.HasKey("Rank" + rank + "Name"))
			{
				var name		= PlayerPrefs.GetString("Rank" + rank + "Name");
				var score		= PlayerPrefs.GetInt("Rank" + rank + "Score");
				var scoreStruct	= new Struct.Score(name, score);

				this.highscoreList.Add(scoreStruct);
			}
		}
	}


	/**
	 * Adds the given score to the highscore list if its high enough
	 */
	public void AddScore(string name, int score)
	{
		if (this.IsInHighscoreList(score))
		{
			var scoreStruct = new Struct.Score(name, score);
			this.highscoreList.Add(scoreStruct);
			this.highscoreList = this.highscoreList.OrderByDescending(orderScoreStruct => orderScoreStruct.score).ToList();

			// Remove the the last entry if the highscore list is longer that it should be
			if (this.highscoreList.Count > this.highscoreListLength)
			{
				this.highscoreList.RemoveAt(this.highscoreListLength);
			}

			this.Save();
		}
	}


	/**
	 * Saves the highscore list
	 */
	private void Save()
	{
		for (var rank = 1; rank <= this.highscoreList.Count; rank ++)
		{
			PlayerPrefs.SetString("Rank" + rank + "Name", this.highscoreList[rank - 1].name);
			PlayerPrefs.SetInt("Rank" + rank + "Score", this.highscoreList[rank - 1].score);
		}
	}


	/**
	 * Returns true if the given score is within the highscore list
	 */
	public bool IsInHighscoreList(int score)
	{
		return (this.highscoreList.Count < this.highscoreListLength || score > this.highscoreList.Last().score);
	}
}
