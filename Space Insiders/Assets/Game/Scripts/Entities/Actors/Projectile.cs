using UnityEngine;

namespace Game.Entities
{
	/// <summary>
	/// Projectiles can be shot from ships.
	/// </summary>
	public class Projectile : Actor
	{
		[Header(nameof(Projectile))]
		[SerializeField, Min(0)] private int _damage = 1;

		private int damage { get => _damage; }
		private Ship owner { get; set; }

		#region Unity
		protected override void OnTriggerEnter2D(Collider2D other)
		{
			base.OnTriggerEnter2D(other);

			Entity entity = other.gameObject.GetEntity();

			if (entity != null)
			{
				// Ignores collisions with entities the same type as our owner.
				if (entity.GetType() == owner.GetType())
				{
					return;
				}

				// Ignores collisions with drops.
				if (entity is Drop)
				{
					return;
				}

				// Deals damage.
				if (entity is IDamageable damageable)
				{
					damageable.TakeDamage(damage);
					owner.OnTargetHit(damageable);
				}
			}

			Destroy();
		}
		#endregion

		/// <summary>
		/// Shoots the projectile.
		/// </summary>
		public void Shoot(Ship owner, Vector2 direction)
		{
			this.owner = owner;
			// Ignores collisions with the ship who spawned us.
			Physics2D.IgnoreCollision(owner.collider, collider);
			// Applies a direction of movement.
			movementDirection = direction;
		}
	}
}