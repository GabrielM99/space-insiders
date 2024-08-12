using Game.Effects;
using UnityEngine;

namespace Game.Items
{
	/// <summary>
	/// An object that can be collected by the player, granting all sorts of effects.
	/// </summary>
	[CreateAssetMenu(fileName = "New Item", menuName = "Game/Item")]
	public class Item : ScriptableObject
	{
		[SerializeField] private Sprite _sprite;

		[Space]
		[SerializeField] private Effect[] _effects;

		public Sprite sprite { get => _sprite; }

		public Effect[] effects { get => _effects; }
	}
}