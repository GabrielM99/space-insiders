using Game.Entities;
using UnityEngine;

namespace Game.Effects
{
	/// <summary>
	/// Grants a immediate quantity of life to the player.
	/// </summary>
	[CreateAssetMenu(fileName = "New Restoration Effect", menuName = "Game/Effects/Restoration")]
	public class RestorationEffect : Effect
	{
		[Header(nameof(RestorationEffect))]
		[SerializeField] public int _lifeQuantity;

		public int lifeQuantity { get => _lifeQuantity; }

		public override void OnStart(Player player)
		{
			base.OnStart(player);
			player.life.value += lifeQuantity;
		}
	}
}