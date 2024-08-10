using TMPro;
using UnityEngine;

namespace Game.UI
{
	/// <summary>
	/// A value text.
	/// </summary>
	[RequireComponent(typeof(TextMeshProUGUI))]
	public class ValueGraphics : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI _textMesh;

		[Space]
		[SerializeField] private Animator _animator;
		[SerializeField] private string _animationTrigger = "Set Value";

		private TextMeshProUGUI textMesh { get => _textMesh; set => _textMesh = value; }

		private Animator animator { get => _animator; set => _animator = value; }
		private string animationTrigger { get => _animationTrigger; }

		#region Unity
		private void Reset()
		{
			textMesh = GetComponent<TextMeshProUGUI>();
			animator = GetComponent<Animator>();
		}
		#endregion

		/// <summary>
		/// Sets the value of the text.
		/// </summary>
		public void SetValue(int value)
		{
			textMesh.text = value.ToString();

			if (animator != null)
			{
				animator.SetTrigger(animationTrigger);
			}
		}
	}
}