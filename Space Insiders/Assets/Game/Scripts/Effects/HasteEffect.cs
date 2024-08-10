using Game.Entities;
using UnityEngine;

namespace Game.Effects
{
	/// <summary>
	/// Increases movement speed.
	/// </summary>
	[CreateAssetMenu(fileName = "New Haste Effect", menuName = "Game/Effects/Haste")]
	public class HasteEffect : Effect
	{
		[Header(nameof(HasteEffect))]
		[SerializeField, Min(1f)] private float _speedMultiplier = 1f;
		[SerializeField, Min(1f)] private float _accelerationMultiplier = 1f;

		private float speedMultiplier { get => _speedMultiplier; }
		private float accelerationMultiplier { get => _accelerationMultiplier; }

		public override void OnUpdate(Player player)
		{
			base.OnUpdate(player);
			
			player.currentSpeed = player.defaultSpeed * speedMultiplier;
			player.currentAcceleration = player.defaultAcceleration * accelerationMultiplier;
		}
	}
}