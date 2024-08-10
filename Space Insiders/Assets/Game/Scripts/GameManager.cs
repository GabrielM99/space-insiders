using System.Collections.Generic;
using UnityEngine;

namespace Game
{
	/// <summary>
	/// Handles the game initialization and closure.
	/// </summary>
	public class GameManager : MonoBehaviour
	{
		[SerializeField] private SaveData _saveData;

		private const string SAVE_KEY = "Save";

		public static SaveData saveData { get => instance._saveData; private set => instance._saveData = value; }

		private static GameManager instance { get; set; }

		#region Unity
		private void Awake()
		{
			CreateSingleton();
			LoadData();
		}

		private void OnApplicationQuit()
		{
			SaveData();
		}
		#endregion

		/// <summary>
		/// Saves a score.
		/// </summary>
		public static void SaveScore(string name, int score)
		{
			if (score <= 0)
			{
				return;
			}

			saveData.scores.Add(new ScoreData(name, score));
			saveData.scores.Sort((a, b) => a.value.CompareTo(b.value));
		}

		/// <summary>
		/// Creates the singleton.
		/// </summary>
		private void CreateSingleton()
		{
			if (instance != null && instance != this)
			{
				Destroy(gameObject);
				return;
			}

			instance = this;

			DontDestroyOnLoad(gameObject);
		}

		/// <summary>
		/// Loads the game data.
		/// </summary>
		private void LoadData()
		{
			if (PlayerPrefs.HasKey(SAVE_KEY))
			{
				saveData = JsonUtility.FromJson<SaveData>(PlayerPrefs.GetString(SAVE_KEY));
			}
		}

		/// <summary>
		/// Saves the game data.
		/// </summary>
		private void SaveData()
		{
			PlayerPrefs.SetString(SAVE_KEY, JsonUtility.ToJson(saveData));
		}
	}
}