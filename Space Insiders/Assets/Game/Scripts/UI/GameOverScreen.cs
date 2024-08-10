using Game.Entities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
	/// <summary>
	/// The game over screen.
	/// </summary>
	public class GameOverScreen : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI _scoreTextMesh;

		[Space]
		[SerializeField] private TMP_InputField _saveScoreInput;
		[SerializeField] private Button _saveScoreButton;

		private string scoreText { get; set; }
		private int score { get; set; }
		private TextMeshProUGUI scoreTextMesh { get => _scoreTextMesh; }
		private TMP_InputField saveScoreInput { get => _saveScoreInput; }
		private Button saveScoreButton { get => _saveScoreButton; }

		#region Unity
		private void Awake()
		{
			scoreText = scoreTextMesh.text;
		}

		private void OnEnable()
		{
			GameManager.Pause();
		}

		private void OnDisable()
		{
			GameManager.Unpause();
		}
		#endregion

		/// <summary>
		/// Opens the game over screen.
		/// </summary>
		public void Open(int score)
		{
			this.score = score;

			gameObject.SetActive(true);
			scoreTextMesh.text = string.Format(scoreText, score);

			if (score <= 0)
			{
				DisableSaveScore();
			}
		}

		/// <summary>
		/// Saves the score.
		/// </summary>
		public void SaveScore()
		{
			string name = saveScoreInput.text;

			if (!string.IsNullOrEmpty(name))
			{
				GameManager.SaveScore(name, score);
				DisableSaveScore();
			}
		}

		/// <summary>
		/// Disables the ability to save the score.
		/// </summary>
		private void DisableSaveScore()
		{
			saveScoreInput.interactable = false;
			saveScoreButton.interactable = false;
		}
	}
}