using Game.Entities;
using UnityEngine;

namespace Game.Effects
{
	/// <summary>
	/// Increases the shooting speed.
	/// </summary>
	[CreateAssetMenu(fileName = "New Shoot Effect", menuName = "Game/Effects/Shoot")]
	public class ShootEffect : Effect
	{
		[Header(nameof(ShootEffect))]
		[SerializeField, Min(1f)] private float _speedMultiplier = 1f;

		public float speedMultiplier { get => _speedMultiplier; }

		public override void OnUpdate(Player player)
		{
			base.OnUpdate(player);
			player.currentShootSpeedMultiplier = player.defaultShootSpeedMultiplier * speedMultiplier;
		}
	}
}