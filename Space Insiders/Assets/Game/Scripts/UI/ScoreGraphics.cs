using TMPro;
using UnityEngine;

namespace Game.UI
{
	/// <summary>
	/// The graphics of a score.
	/// </summary>
	public class ScoreGraphics : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI _textMesh;

		public TextMeshProUGUI textMesh { get => _textMesh; }
	}
}