using Game.Entities;
using UnityEngine;

namespace Game
{
	/// <summary>
	/// Shields are damageable barriers.
	/// </summary>
	[RequireComponent(typeof(Life))]
	public class Shield : Entity, IDamageable
	{
		[Header(nameof(Shield))]
		[SerializeField] private Life _life;

		public Life life { get => _life; private set => _life = value; }

		#region Unity
		protected override void Reset()
		{
			base.Reset();
			life = GetComponent<Life>();
		}

		protected override void OnCollisionEnter2D(Collision2D other)
		{
			base.OnCollisionEnter2D(other);

			if (other.gameObject.IsEntity<Alien>())
			{
				Destroy();
			}
		}
		#endregion

		public void TakeDamage(int damage)
		{
			life.value -= damage;

			if (life.isEmpty)
			{
				Destroy();
			}
		}
	}
}