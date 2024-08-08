using System.Collections.Generic;
using Game.Agents;
using Game.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Level
{
	/// <summary>
	/// A game level.
	/// </summary>
	[RequireComponent(typeof(EdgeCollider2D))]
	public class Level : MonoBehaviour
	{
		[SerializeField] private EdgeCollider2D _collider;

		[Space]
		[SerializeField] private Transform _alienParent;
		[SerializeField, Min(0)] private int _alienCols;
		[SerializeField] private List<Alien> _alienRows;
		[SerializeField] private Vector2 _alienSpacing;
		[SerializeField] private Vector2 _alienOffset;
		[SerializeField] private Timer _alienShootCooldown;
		[SerializeField, Min(0f)] private float _alienVerticalSpeed = 0.5f;
		[SerializeField, Min(0f)] private float _alienDestroySpeedIncrement;

		private new EdgeCollider2D collider { get => _collider; set => _collider = value; }

		private Transform alienParent { get => _alienParent; }
		private int alienCols { get => _alienCols; }
		private List<Alien> alienRows { get => _alienRows; }
		private Vector2 alienSpacing { get => _alienSpacing; }
		private Vector2 alienOffset { get => _alienOffset; }
		private Timer alienShootCooldown { get => _alienShootCooldown; }
		private float alienVerticalSpeed { get => _alienVerticalSpeed; }
		private float alienDestroySpeedIncrement { get => _alienDestroySpeedIncrement; }
		private List<Alien> aliens { get; set; }
		private Vector2 currentAlienMovementDirection { get; set; }

		#region Unity
		private void Reset()
		{
			collider = GetComponent<EdgeCollider2D>();
		}

		private void Awake()
		{
		}

		private void Start()
		{
			// Aliens will start moving to the right.
			currentAlienMovementDirection = Vector2.right;
			SpawnAliens();
		}

		private void Update()
		{
			AlienShoot();
		}

		private void OnCollisionEnter2D(Collision2D other)
		{
			Alien collidedAlien = other.gameObject.GetEntity<Alien>();

			// Checks whether an entity collided with the level boundaries.
			if (collidedAlien != null && aliens.Contains(collidedAlien))
			{
				// The direction of collision relative to the level center.
				Vector2 direction = new Vector2(collider.bounds.center.x - other.transform.position.x, 0).normalized;

				// Aliens collided on left or right.
				if (direction == Vector2.left || direction == Vector2.right)
				{
					// The aliens already collided on this side of the level.
					if (direction == currentAlienMovementDirection)
					{
						return;
					}

					currentAlienMovementDirection = direction;

					// Applies a downwards force so the aliens approach the player.
					foreach (Alien alien in aliens)
					{
						alien.movementDirection = direction;
						alien.Move(Vector2.down * alienVerticalSpeed);
					}
				}
				// Aliens reached the bottom.
				else if (direction == Vector2.down)
				{
					// Game over.
					SceneManager.LoadScene(SceneManager.GetActiveScene().name);
				}
			}
		}
		#endregion

		/// <summary>
		/// Spawns the aliens in the level.
		/// </summary>
		private void SpawnAliens()
		{
			aliens = new List<Alien>();

			for (int y = 0; y < alienRows.Count; y++)
			{
				for (int x = 0; x < alienCols; x++)
				{
					Alien alien = Instantiate(alienRows[y], new Vector2(x - alienCols / 2 + alienOffset.x + x * alienSpacing.x, y - alienRows.Count / 2 + alienOffset.y + y * alienSpacing.y), Quaternion.identity, alienParent);
					alien.movementDirection = currentAlienMovementDirection;
					alien.onDestroyed += OnAlienDestroyed;
					aliens.Add(alien);
				}
			}
		}

		/// <summary>
		/// Selects a random alien to shoot.
		/// </summary>
		private void AlienShoot()
		{
			alienShootCooldown.Run();

			// Are the aliens under cooldown?
			if (!alienShootCooldown.isDone)
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
			aliens.Remove(entity as Alien);

			if (aliens.Count == 0)
			{
				SceneManager.LoadScene(SceneManager.GetActiveScene().name);
				return;
			}

			foreach (Alien alien in aliens)
			{
				alien.defaultSpeed += alienDestroySpeedIncrement;
			}
		}
	}
}