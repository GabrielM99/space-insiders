using Game.Utils;
using UnityEngine;

namespace Game.Agents
{
	/// <summary>
	/// An agent capable of shooting.
	/// </summary>
	public class Ship : Agent, IDamageable
	{
		[Header(nameof(Ship))]
		[SerializeField, Min(0)] private int _life = 1;

		[Space]
		[SerializeField] private Projectile[] _projectilePrefabs;
		[SerializeField] private Timer _shootCooldown;

		protected int life { get => _life; private set => _life = value; }

		private Projectile[] projectilePrefabs { get => _projectilePrefabs; }
		private Timer shootCooldown { get => _shootCooldown; }

		protected override void Update()
		{
			base.Update();
			shootCooldown.Run();
		}

		public virtual void TakeDamage(int damage)
		{
			life = Mathf.Max(life - damage, 0);

			if (life <= 0)
			{
				Destroy();
			}
		}

		/// <summary>
		/// Called when a target is hit.
		/// </summary>
		public virtual void OnTargetHit(IDamageable target) { }

		/// <summary>
		/// Shoots in a direction.
		/// </summary>
		public void Shoot(Vector2 direction)
		{
			// Are we under cooldown?
			if (!shootCooldown.isDone)
			{
				return;
			}

			shootCooldown.Reset();

			// Selects a random projectile prefab from the array of projectiles.
			Projectile projectilePrefab = projectilePrefabs[Random.Range(0, projectilePrefabs.Length)];
			// The origin should be outside the agent, so we calculate it based on the collider's bounds.
			Vector2 origin = direction * Mathf.Max(collider.bounds.size.x, collider.bounds.size.y) + (Vector2)transform.position;
			Projectile projectile = Instantiate(projectilePrefab, origin, Quaternion.identity);
			projectile.Shoot(this, direction);

		}
	}
}