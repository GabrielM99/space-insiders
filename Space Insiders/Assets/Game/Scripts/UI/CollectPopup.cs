using System.Collections;
using TMPro;
using UnityEngine;

namespace Game.UI
{
	/// <summary>
	/// A popup that is shown when the player collects items and points
	/// </summary>
	public class CollectPopup : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI _textMesh;
		[SerializeField, Min(0f)] private float _duration = 0.75f;
		[SerializeField, Min(0f)] private float _ascentSpeed = 3f;

		public TextMeshProUGUI textMesh { get => _textMesh; }

		private float duration { get => _duration; }
		private float ascentSpeed { get => _ascentSpeed; }

		private IEnumerator Start()
		{
			float timer = 0f;

			while (timer < duration)
			{
				timer += Time.deltaTime;
				textMesh.color = new Color(Random.value, Random.value, Random.value);
				transform.position += ascentSpeed * Time.deltaTime * Vector3.up;
				yield return null;
			}

			Destroy(gameObject);
		}
	}
}