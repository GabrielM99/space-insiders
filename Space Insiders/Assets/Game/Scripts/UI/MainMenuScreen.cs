using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.UI
{
	/// <summary>
	/// The main menu screen.
	/// </summary>
	public class MainMenuScreen : MonoBehaviour
	{
		[SerializeField] private string _levelScene;

		private string levelScene { get => _levelScene; }

		/// <summary>
		/// Plays the game.
		/// </summary>
		public void Play()
		{
			SceneManager.LoadScene(levelScene);
		}

		/// <summary>
		/// Exits the game.
		/// </summary>
		public void Exit()
		{
			Application.Quit();
		}
	}
}