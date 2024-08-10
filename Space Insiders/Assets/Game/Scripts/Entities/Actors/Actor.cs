using UnityEngine;

namespace Game.Entities
{
	/// <summary>
	/// Actors are movable objects.
	/// </summary>
	[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
	public class Actor : Entity
	{
		[Header(nameof(Actor))]
		[SerializeField] private Rigidbody2D _rigidbody;
		[SerializeField] private Collider2D _collider;

		[Space]
		[SerializeField, Min(0f)] private float _defaultSpeed = 5f;
		[SerializeField, Min(0f)] private float _defaultAcceleration = 10f;

		public new Collider2D collider { get => _collider; private set => _collider = value; }
		public float defaultSpeed { get => _defaultSpeed; set => _defaultSpeed = value; }
		public float currentSpeed { get; set; }
		public float currentAcceleration { get; set; }
		public float defaultAcceleration { get => _defaultAcceleration; set => _defaultAcceleration = value; }
		public Vector2 movementDirection { get; set; }

		protected Vector2? waypoint { get; private set; }

		private new Rigidbody2D rigidbody { get => _rigidbody; set => _rigidbody = value; }
		private Vector2 internalVelocity { get; set; }

		#region Unity
		protected override void Reset()
		{
			base.Reset();
			rigidbody = GetComponent<Rigidbody2D>();
			collider = GetComponent<Collider2D>();
		}

		protected override void Start()
		{
			base.Start();
			RestoreDefaultMovementValues();
		}

		protected override void FixedUpdate()
		{
			base.FixedUpdate();
			CalculateInternalVelocity();
			Move();
		}
		#endregion

		/// <summary>
		/// Moves the actor by an amount of distance.
		/// </summary>
		public void Move(Vector2 amount)
		{
			SetWaypoint(rigidbody.position + amount);
		}

		/// <summary>
		/// Sets a waypoint where the actor will move to.
		/// </summary>
		public void SetWaypoint(Vector2? waypoint)
		{
			this.waypoint = waypoint;
		}

		/// <summary>
		/// Moves the actor.
		/// </summary>
		protected virtual void Move()
		{
			rigidbody.velocity = Vector2.zero;

			if (waypoint == null)
			{
				rigidbody.MovePosition(rigidbody.position + internalVelocity * Time.fixedDeltaTime);
			}
			// The actor will ignore its movement direction while following any waypoint.
			else
			{
				Vector2 newPosition = Vector2.MoveTowards(rigidbody.position, waypoint.Value, defaultSpeed * Time.fixedDeltaTime);

				// Reached our destination.
				if (newPosition == waypoint)
				{
					waypoint = null;
				}

				rigidbody.MovePosition(newPosition);
			}
		}

		/// <summary>
		/// Calculates the internal velocity of the actor, which independs from the rigidbody.
		/// </summary>
		private void CalculateInternalVelocity()
		{
			Vector2 internalVelocity = this.internalVelocity;

			internalVelocity.x = Mathf.MoveTowards(internalVelocity.x, movementDirection.x * currentSpeed, currentAcceleration * Time.fixedDeltaTime);
			internalVelocity.y = Mathf.MoveTowards(internalVelocity.y, movementDirection.y * currentSpeed, currentAcceleration * Time.fixedDeltaTime);

			RestoreDefaultMovementValues();

			this.internalVelocity = internalVelocity;
		}

		/// <summary>
		/// Restores both current speed and acceleration values to their original values. This is used so other systems are able to modify those values each frame to only temporarily change the actor's movement.
		/// </summary>
		private void RestoreDefaultMovementValues()
		{
			currentSpeed = defaultSpeed;
			currentAcceleration = defaultAcceleration;
		}
	}
}