using UnityEngine;

namespace Game.Items
{
	/// <summary>
	/// Items with a drop chance.
	/// </summary>
	[System.Serializable]
	public class Loot
	{
		[SerializeField] private Item _item;
		[SerializeField, Min(0f)] private float _weight;

		public Item item { get => _item; }
		public float weight { get => _weight; }
	}
}