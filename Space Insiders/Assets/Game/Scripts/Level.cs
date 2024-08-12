using System.Collections;
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
	[RequireComponent(typeof(EdgeCollider2D), typeof(AudioSource))]
	public class Level : MonoBehaviour
	{
		[SerializeField] private EdgeCollider2D _collider;

		[Space]
		[SerializeField, Min(1)] private int _value = 1;
		[SerializeField, Range(0f, 1f)] private float _difficultyCurve = 0.1f;

		[Space]
		[SerializeField] private Camera _camera;
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
		[SerializeField] private PauseScreen _pauseScreen;

		[Space]
		[SerializeField] private GameOverScreen _gameOverScreen;
		[SerializeField, Min(0f)] private float _gameOverDelay;

		[Space]
		[SerializeField] private AudioSource _audioSource;
		[SerializeField] private AudioClip _clearSound;

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

		private new Camera camera { get => _camera; }
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
		private PauseScreen pauseScreen { get => _pauseScreen; }

		private GameOverScreen gameOverScreen { get => _gameOverScreen; }
		private Coroutine gameOverCoroutine { get; set; }
		private float gameOverDelay { get => _gameOverDelay; }

		private AudioSource audioSource { get => _audioSource; set => _audioSource = value; }
		private AudioClip clearSound { get => _clearSound; }

		private Coroutine shakeCoroutine { get; set; }

		#region Unity
		private void Reset()
		{
			audioSource = GetComponent<AudioSource>();
		}

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
			Assign(shields);

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
					player.Destroy();
				}
			}
		}
		#endregion

		/// <summary>
		/// Spawns an entity in the level.
		/// </summary>
		public T Spawn<T>(T prefab, Vector3 position, Quaternion rotation, Transform parent = null) where T : Object
		{
			T instance = Instantiate(prefab, position, rotation, parent);

			if (instance is Entity entity)
			{
				Assign(entity);
			}

			return instance;
		}

		/// <summary>
		/// Shakes the level.
		/// </summary>
		public void Shake(float duration, float intensity)
		{
			if (shakeCoroutine == null)
			{
				shakeCoroutine = StartCoroutine(ShakeCoroutine(duration, intensity));
			}
		}

		/// <summary>
		/// The coroutine for shaking the level.
		/// </summary>
		private IEnumerator ShakeCoroutine(float duration, float intensity)
		{
			Vector3 cameraStartPosition = camera.transform.position;

			float time = 0f;

			while (time < duration)
			{
				time += Time.unscaledDeltaTime;
				camera.transform.position = cameraStartPosition + (Vector3)Random.insideUnitCircle * intensity * ((Mathf.Sin(time / duration) + 1) / 2f);
				yield return null;
			}

			camera.transform.position = cameraStartPosition;
			shakeCoroutine = null;
		}

		/// <summary>
		/// Restarts the level.
		/// </summary>
		public void Restart()
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}

		/// <summary>
		/// Ends the game.
		/// </summary>
		public void GameOver()
		{
			if (gameOverCoroutine == null)
			{
				gameOverCoroutine = StartCoroutine(GameOverCoroutine());
			}
		}

		/// <summary>
		/// The coroutine for displaying the game over screen.
		/// </summary>
		private IEnumerator GameOverCoroutine()
		{
			yield return new WaitForSeconds(gameOverDelay);
			gameOverScreen.Open(player.score);
			gameOverCoroutine = null;
		}

		/// <summary>
		/// Pauses or unpauses the level.
		/// </summary>
		public void TogglePause()
		{
			pauseScreen.Toggle();
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
		/// Assignes an array of entities in the level.
		/// </summary>
		private void Assign(Entity[] entities, Transform parent = null)
		{
			foreach (Entity entity in entities)
			{
				Assign(entity, parent);
			}
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
			audioSource.PlayOneShot(clearSound);
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