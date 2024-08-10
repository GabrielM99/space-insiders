using System.Collections.Generic;
using Game.Entities;
using Game.UI;
using Game.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
	/// <summary>
	/// A game level.
	/// </summary>
	[RequireComponent(typeof(EdgeCollider2D))]
	public class Level : MonoBehaviour
	{
		[SerializeField] private EdgeCollider2D _collider;

		[Space]
		[SerializeField, Min(1)] private int _value = 1;
		[SerializeField, Range(0f, 1f)] private float _difficultyCurve = 0.1f;

		[Space]
		[SerializeField] private Player _player;
		[SerializeField] private Drone _drone;
		[SerializeField] private Shield[] _shields;

		[Space]
		[SerializeField] private Transform _alienParent;
		[SerializeField, Min(0)] private int _alienCols;
		[SerializeField] private List<Alien> _alienRows;
		[SerializeField] private Vector2 _alienSpacing;
		[SerializeField] private Vector2 _alienOffset;

		[Space]
		[SerializeField] private Range _alienSpawnChance;
		[SerializeField] private Range _alienHorizontalSpeed;
		[SerializeField] private Range _alienVerticalSpeed;
		[SerializeField] private Range _alienLifeMultiplier;
		[SerializeField] private Range _alienShootCooldown;
		[SerializeField] private Range _alienDestroySpeedDelta;

		[Space]
		[SerializeField] private ValueGraphics _valueGraphics;

		private int value
		{
			get => _value;

			set
			{
				_value = value;
				valueGraphics.SetValue(value);
			}
		}
		private float difficultyCurve { get => _difficultyCurve; }

		private Player player { get => _player; }
		private Drone drone { get => _drone; }
		private Shield[] shields { get => _shields; }

		private Transform alienParent { get => _alienParent; }
		private int alienCols { get => _alienCols; }
		private List<Alien> alienRows { get => _alienRows; }
		private Vector2 alienSpacing { get => _alienSpacing; }
		private Vector2 alienOffset { get => _alienOffset; }
		private List<Alien> aliens { get; set; }
		private Vector2 alienMovementDirection { get; set; }
		private Timer alienShootCooldownTimer { get; set; }

		private Range alienSpawnChance { get => _alienSpawnChance; set => _alienSpawnChance = value; }
		private Range alienHorizontalSpeed { get => _alienHorizontalSpeed; set => _alienHorizontalSpeed = value; }
		private Range alienVerticalSpeed { get => _alienVerticalSpeed; }
		private Range alienLifeMultiplier { get => _alienLifeMultiplier; }
		private Range alienShootCooldown { get => _alienShootCooldown; set => _alienShootCooldown = value; }
		private Range alienDestroySpeedDelta { get => _alienDestroySpeedDelta; }

		private ValueGraphics valueGraphics { get => _valueGraphics; }

		#region Unity
		private void Awake()
		{
			aliens = new List<Alien>();
			alienShootCooldownTimer = new Timer(true);
		}

		private void Start()
		{
			// Aliens will start moving to the right.
			alienMovementDirection = Vector2.right;

			Assign(player);
			Assign(drone);

			Generate();
		}

		private void Update()
		{
			AlienShoot();
		}

		private void OnCollisionEnter2D(Collision2D other)
		{
			Entity collidedEntity = other.gameObject.GetEntity();

			if (collidedEntity == null)
			{
				return;
			}

			// Destroys out-of-bounds drops.
			if (collidedEntity is Drop)
			{
				collidedEntity.Destroy();
				return;
			}

			// Checks whether an alien collided with the level boundaries.
			if (collidedEntity is Alien collidedAlien && aliens.Contains(collidedAlien))
			{
				// The direction of collision relative to the level center.
				Vector2 direction = -other.GetContact(0).normal;

				// Aliens collided on left or right.
				if (direction == Vector2.left || direction == Vector2.right)
				{
					// The aliens already collided on this side of the level.
					if (direction == alienMovementDirection)
					{
						return;
					}

					alienMovementDirection = direction;

					// Applies a downwards force so the aliens approach the player.
					foreach (Alien alien in aliens)
					{
						alien.movementDirection = direction;
						alien.Move(Vector2.down * GetDifficultyValue(alienVerticalSpeed));
					}
				}
				// Aliens reached the bottom.
				else if (direction == Vector2.up)
				{
					Restart();
				}
			}
		}
		#endregion

		/// <summary>
		/// Spawns an entity in the level.
		/// </summary>
		public T Spawn<T>(T prefab, Vector3 position, Quaternion rotation, Transform parent = null) where T : Entity
		{
			Entity entity = Instantiate(prefab, position, rotation);
			Assign(entity, parent);
			return entity as T;
		}

		/// <summary>
		/// Restarts the level.
		/// </summary>
		public void Restart()
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}

		/// <summary>
		/// Gets a value from a range based on the current level difficulty.
		/// </summary>
		private float GetDifficultyValue(Range range)
		{
			return range.min + ((1f - Mathf.Exp(-difficultyCurve * (value - 1))) * range.max);
		}

		/// <summary>
		/// Assignes an entity in the level.
		/// </summary>
		private void Assign(Entity entity, Transform parent = null)
		{
			entity.level = this;
			entity.transform.parent = parent == null ? transform : parent;
		}

		/// <summary>
		/// Generates the level.
		/// </summary>
		private void Generate()
		{
			alienShootCooldownTimer.duration = GetDifficultyValue(alienShootCooldown);
			alienShootCooldownTimer.Reset();

			if (aliens.Count > 0)
			{
				foreach (Alien alien in aliens)
				{
					alien.Destroy();
				}
			}

			if (drone.life.isEmpty)
			{
				drone.gameObject.SetActive(true);

				int life = drone.life.maxValue * (int)GetDifficultyValue(alienLifeMultiplier);
				drone.life.maxValue = life;
				drone.life.value = life;

				drone.life.Maximize();
			}

			foreach (Shield shield in shields)
			{
				shield.gameObject.SetActive(true);
				shield.life.Maximize();
			}

			for (int y = 0; y < alienRows.Count; y++)
			{
				for (int x = 0; x < alienCols; x++)
				{
					if (Random.value <= GetDifficultyValue(alienSpawnChance))
					{
						Alien alien = Spawn(alienRows[y], new Vector2(x - alienCols / 2 + alienOffset.x + x * alienSpacing.x, y - alienRows.Count / 2 + alienOffset.y + y * alienSpacing.y), Quaternion.identity, alienParent);

						alien.defaultSpeed = GetDifficultyValue(alienHorizontalSpeed);
						alien.movementDirection = alienMovementDirection;

						int life = alien.life.maxValue * (int)GetDifficultyValue(alienLifeMultiplier);
						alien.life.maxValue = life;
						alien.life.value = life;

						alien.onDestroyed += OnAlienDestroyed;

						aliens.Add(alien);
					}
				}
			}

			// No aliens were generated, so try again.
			if (aliens.Count == 0)
			{
				Generate();
			}
		}

		/// <summary>
		/// Generates the next level.
		/// </summary>
		private void Next()
		{
			Generate();
			value++;
		}

		/// <summary>
		/// Selects a random alien to shoot.
		/// </summary>
		private void AlienShoot()
		{
			alienShootCooldownTimer.Run();

			// Are the aliens under cooldown?
			if (!alienShootCooldownTimer.isDone)
			{
				return;
			}

			if (aliens.Count == 0)
			{
				return;
			}

			Alien alien = aliens[Random.Range(0, aliens.Count)];

			if (alien != null)
			{
				alien.Shoot(Vector2.down);
			}
		}

		/// <summary>
		/// Called when a alien is destroyed.
		/// </summary>
		private void OnAlienDestroyed(Entity entity)
		{
			if (!Application.isPlaying)
			{
				return;
			}

			aliens.Remove(entity as Alien);

			// All aliens were destroyed.
			if (aliens.Count == 0)
			{
				Next();
				return;
			}

			foreach (Alien alien in aliens)
			{
				alien.defaultSpeed += GetDifficultyValue(alienDestroySpeedDelta);
			}
		}
	}
}