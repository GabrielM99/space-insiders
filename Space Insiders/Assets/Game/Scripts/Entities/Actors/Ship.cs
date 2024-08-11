using Game.Utils;
using UnityEngine;

namespace Game.Entities
{
	/// <summary>
	/// An agent capable of shooting.
	/// </summary>
	[RequireComponent(typeof(Life))]
	public class Ship : Actor, IDamageable
	{
		[Header(nameof(Ship))]
		[SerializeField] private Life _life;

		[Space]
		[SerializeField] private Projectile[] _projectilePrefabs;
		[SerializeField, Min(0f)] private float _shootCooldown;

		public Life life { get => _life; private set => _life = value; }

		public float defaultShootSpeedMultiplier { get; set; } = 1f;
		public float currentShootSpeedMultiplier { get; set; }

		private Projectile[] projectilePrefabs { get => _projectilePrefabs; }
		private float shootCooldown { get => _shootCooldown; }
		private Timer shootCooldownTimer { get; set; }

		#region Unity
		protected override void Reset()
		{
			base.Reset();
			life = GetComponent<Life>();
		}

		protected override void Awake()
		{
			base.Awake();
			shootCooldownTimer = new Timer(shootCooldown);
		}

		protected override void Start()
		{
			base.Start();
			currentShootSpeedMultiplier = defaultShootSpeedMultiplier;
		}

		protected override void Update()
		{
			base.Update();
			shootCooldownTimer.Run(Time.deltaTime * currentShootSpeedMultiplier);
			currentShootSpeedMultiplier = defaultShootSpeedMultiplier;
		}
		#endregion

		public virtual void TakeDamage(int damage)
		{
			life.value -= damage;

			if (life.isEmpty)
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
			if (!shootCooldownTimer.isDone)
			{
				return;
			}

			shootCooldownTimer.Reset();

			// Selects a random projectile prefab from the array of projectiles.
			Projectile projectilePrefab = projectilePrefabs[Random.Range(0, projectilePrefabs.Length)];
			// The origin should be outside the agent, so we calculate it based on the collider's bounds.
			Vector2 origin = direction * Mathf.Max(collider.bounds.size.x, collider.bounds.size.y) + (Vector2)transform.position;
			Projectile projectile = level.Spawn(projectilePrefab, origin, Quaternion.Euler(0f, 0f, Vector2.SignedAngle(Vector2.up, direction)));
			projectile.Shoot(this, direction);
		}
	}
}