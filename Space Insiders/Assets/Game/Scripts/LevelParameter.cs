using UnityEngine;

namespace Game
{
	/// <summary>
	/// A range of values.
	/// </summary>
	[System.Serializable]
	public struct Range
	{
		[SerializeField] private float _min;
		[SerializeField] private float _max;

		public float min { get => _min; set => _min = value; }
		public float max { get => _max; set => _max = value; }
	}
}