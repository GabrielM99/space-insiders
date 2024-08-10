using UnityEngine;

namespace Game.UI
{
	/// <summary>
	/// The scoreboard screen.
	/// </summary>
	public class ScoreboardScreen : MonoBehaviour
	{
		[SerializeField] private ScoreGraphics[] _scoreGraphics;

		private ScoreGraphics[] scoreGraphics { get => _scoreGraphics; }

		#region Unity
		private void OnEnable()
		{
			UpdateGraphics();
		}
		#endregion

		/// <summary>
		/// Updates the scoreboard graphics.
		/// </summary>
		private void UpdateGraphics()
		{
			SaveData saveData = GameManager.saveData;

			for (int i = 0; i < saveData.scores.Count; i++)
			{
				ScoreData scoreData = saveData.scores[i];
				scoreGraphics[i].textMesh.text = $"{i + 1}. {scoreData.name} - {scoreData.value}";
			}
		}
	}
}