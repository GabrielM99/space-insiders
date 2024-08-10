using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.UI
{
	/// <summary>
	/// The pause screen.
	/// </summary>
	public class PauseScreen : MonoBehaviour
	{
		[SerializeField] private string _menuScene;

		private string menuScene { get => _menuScene; }

		#region Unity
		private void OnEnable()
		{
			Pause();
		}

		private void OnDisable()
		{
			Unpause();
		}
		#endregion

		/// <summary>
		/// Toggles the pause screen on or off.
		/// </summary>
		public void Toggle()
		{
			if (gameObject.activeSelf)
			{
				Close();
			}
			else
			{
				Open();
			}
		}

		/// <summary>
		/// Closes the pause screen.
		/// </summary>
		public void Close()
		{
			gameObject.SetActive(false);
		}

		/// <summary>
		/// Exit to the menu.
		/// </summary>
		public void ExitToMenu()
		{
			SceneManager.LoadScene(menuScene);
		}

		/// <summary>
		/// Opens the pause screen.
		/// </summary>
		private void Open()
		{
			gameObject.SetActive(true);
		}

		/// <summary>
		/// Pauses the game.
		/// </summary>
		private void Pause()
		{
			Time.timeScale = 0f;
		}

		/// <summary>
		/// Unpauses the game.
		/// </summary>
		private void Unpause()
		{
			Time.timeScale = 1f;
		}
	}
}