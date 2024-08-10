using Game.Utils;
using UnityEngine;

namespace Game.Entities
{
	/// <summary>
	/// An alien that moves between two points.
	/// </summary>
	public class Drone : Alien
	{
		[Header(nameof(Drone))]
		[SerializeField] private Transform _startWaypoint;
		[SerializeField] private Transform _endWaypoint;

		[Space]
		[SerializeField, Min(0f)] private float _minPatrolInterval = 5f;
		[SerializeField, Min(0f)] private float _maxPatrolInterval = 10f;

		private float minPatrolInterval { get => _minPatrolInterval; }
		private float maxPatrolInterval { get => _maxPatrolInterval; }

		private Transform startWaypoint { get => _startWaypoint; }
		private Vector2 startPosition { get; set; }

		private Transform endWaypoint { get => _endWaypoint; }
		private Vector2 endPosition { get; set; }

		private Timer patrolTimer { get; set; }

		#region Unity
		protected override void Awake()
		{
			base.Awake();

			patrolTimer = new Timer(true);
			startPosition = startWaypoint.position;
			endPosition = endWaypoint.position;
		}

		protected override void OnEnable()
		{
			base.OnEnable();

			transform.position = startPosition;
			RandomizePatrolTimer();
			patrolTimer.Reset();
		}
		#endregion

		protected override void Move()
		{
			base.Move();
			Patrol();
		}

		/// <summary>
		/// Moves between the start and end point.
		/// </summary>
		private void Patrol()
		{
			if (waypoint == null)
			{
				if (patrolTimer.Run())
				{
					if ((Vector2)transform.position == startPosition)
					{
						SetWaypoint(endPosition);
					}
					else if ((Vector2)transform.position == endPosition)
					{
						SetWaypoint(startPosition);
					}

					RandomizePatrolTimer();
				}
			}
		}

		private void RandomizePatrolTimer()
		{
			patrolTimer.duration = Random.Range(minPatrolInterval, maxPatrolInterval);
		}
	}
}