using System;
using UnityEngine;

namespace Game
{
	/// <summary>
	/// The life of an object.
	/// </summary>
	public class Life : MonoBehaviour
	{
		[SerializeField, Min(0)] private int _maxValue = 1;
		[SerializeField, Min(0)] private int _value = 1;

		[Space]
		[SerializeField] private Transform _graphics;
		[SerializeField] private bool _displayFull;

		public int value
		{
			get => _value;

			set
			{
				_value = Math.Clamp(value, 0, maxValue);
				onValueChanged?.Invoke(_value);
				UpdateGraphics();
			}
		}
		public int maxValue
		{
			get => _maxValue;

			set
			{
				_maxValue = value;
				UpdateGraphics();
			}
		}
		public bool isEmpty => value <= 0;
		public bool isFull => value >= maxValue;

		public Action<int> onValueChanged { get; set; }

		private bool displayFull { get => _displayFull; }

		private Transform graphics { get => _graphics; }

		#region Unity
		private void Start()
		{
			UpdateGraphics();
		}
		#endregion

		/// <summary>
		/// Sets the life to its maximum value.
		/// </summary>
		public void Maximize()
		{
			value = maxValue;
		}

		/// <summary>
		/// Updates the life graphics.
		/// </summary>
		private void UpdateGraphics()
		{
			if (graphics != null)
			{
				// Graphics won't be displayed if life is full. 
				graphics.gameObject.SetActive(displayFull || !isFull);
				graphics.localScale = new Vector2((float)value / maxValue, 1f);
			}
		}
	}
}