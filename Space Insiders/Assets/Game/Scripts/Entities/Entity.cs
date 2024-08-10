using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
	/// <summary>
	/// An object of the world.
	/// </summary>
	public class Entity : MonoBehaviour
	{
		/// <summary>
		/// The mode in which the entity will be destroyed.
		/// </summary>
		public enum DestroyMode
		{
			Destroy,
			Deactivate
		}

		[Header(nameof(Entity))]
		[SerializeField] private DestroyMode _destroyMode;

		private static Dictionary<GameObject, Entity> entityByObject { get; set; }

		public Level level { get; internal set; }
		public Action<Entity> onDestroyed { get; set; }

		private DestroyMode destroyMode { get => _destroyMode; }

		/// <summary>
		/// Statically constructs the entity.
		/// </summary>
		static Entity()
		{
			entityByObject = new Dictionary<GameObject, Entity>();
		}

		#region Unity
		protected virtual void Reset() { }
		protected virtual void OnEnable() { }
		protected virtual void Awake() { Register(); }
		protected virtual void OnDisable() { }
		protected virtual void Start() { }
		protected virtual void Update() { }
		protected virtual void FixedUpdate() { }
		protected virtual void OnCollisionEnter2D(Collision2D other) { }
		protected virtual void OnTriggerEnter2D(Collider2D other) { }
		protected virtual void OnDestroy() { Unregister(); }
		#endregion

		/// <summary>
		/// Gets the entity of an object.
		/// </summary>
		public static Entity GetEntity(GameObject gameObject)
		{
			return entityByObject.GetValueOrDefault(gameObject);
		}

		/// <summary>
		/// Gets the entity of an object.
		/// </summary>
		public static T GetEntity<T>(GameObject gameObject) where T : Entity
		{
			return GetEntity(gameObject) as T;
		}

		/// <summary>
		/// Checks if the object is an entity of a type.
		/// </summary>
		public static bool IsEntity<T>(GameObject gameObject) where T : Entity
		{
			return TryGetEntity<T>(gameObject, out _);
		}

		/// <summary>
		/// Tries getting the entity of an object.
		/// </summary>
		public static bool TryGetEntity<T>(GameObject gameObject, out T entity) where T : Entity
		{
			return (entity = GetEntity<T>(gameObject)) != null;
		}

		/// <summary>
		/// Destroys the entity.
		/// </summary>
		public virtual void Destroy()
		{
			if (destroyMode == DestroyMode.Deactivate)
			{
				gameObject.SetActive(false);
			}
			else if (destroyMode == DestroyMode.Destroy)
			{
				Destroy(gameObject);
			}

			// Notifies the entity was destroyed.
			onDestroyed?.Invoke(this);
		}

		/// <summary>
		/// Registers the entity, so we can access scripts directly without using methods like GetComponent.
		/// </summary>
		private void Register()
		{
			entityByObject.Add(gameObject, this);
		}

		/// <summary>
		/// Unregisters the entity.
		/// </summary>
		private void Unregister()
		{
			entityByObject.Remove(gameObject);
		}
	}
}