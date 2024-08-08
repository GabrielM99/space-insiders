using UnityEngine;

namespace Game.Agents
{
	/// <summary>
	/// An enemy ship.
	/// </summary>
	public class Alien : Ship
	{
		[Header(nameof(Alien))]
		[SerializeField, Min(0)] private int _points;

		public int points { get => _points; }
	}
}