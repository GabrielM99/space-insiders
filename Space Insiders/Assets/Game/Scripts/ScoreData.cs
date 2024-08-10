using UnityEngine;

namespace Game
{
	/// <summary>
	/// The data of a score.
	/// </summary>
	[System.Serializable]
	public class ScoreData
	{
		public string name;
		public int value;

		public ScoreData(string name, int value)
		{
			this.name = name;
			this.value = value;
		}
	}
}