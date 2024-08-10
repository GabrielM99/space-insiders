using Game.Items;
using UnityEngine;

namespace Game.Entities
{
	/// <summary>
	/// An enemy ship.
	/// </summary>
	public class Alien : Ship
	{
		[Header(nameof(Alien))]
		[SerializeField, Min(0)] private int _points;

		[Space]
		[SerializeField] private Drop _dropPrefab;
		[SerializeField, Range(0f, 1f)] private float _dropChance;
		[SerializeField] private Loot[] _loots;

		public int points { get => _points; }

		private Drop dropPrefab { get => _dropPrefab; }
		private float dropChance { get => _dropChance; }
		private Loot[] loots { get => _loots; }

		public override void Destroy()
		{
			base.Destroy();
			DropLoot();
		}

		private void DropLoot()
		{
			if (Random.value > dropChance)
			{
				return;
			}

			float totalWeight = 0f;

			foreach (Loot loot in loots)
			{
				totalWeight += loot.weight;
			}

			float randomWeight = Random.Range(0f, totalWeight);

			foreach (Loot loot in loots)
			{
				randomWeight -= loot.weight;

				if (randomWeight <= 0f)
				{
					Drop drop = level.Spawn(dropPrefab, transform.position, Quaternion.identity);
					drop.item = loot.item;
					break;
				}
			}
		}
	}
}