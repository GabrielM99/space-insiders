using UnityEngine;

namespace Game
{
	/// <summary>
	/// Useful extension methods.
	/// </summary>
	public static class Extensions
	{
		/// <summary>
		/// Checks if the layer of a gameobject is contained in a layer mask.
		/// </summary>
		public static bool CompareLayers(this GameObject gameObject, LayerMask layerMask)
		{
			return ((1 << gameObject.layer) & layerMask) != 0;
		}

		///
		/// Gets the entity of an object.
		/// </summary>
		public static Entity GetEntity(this GameObject gameObject)
		{
			return Entity.GetEntity(gameObject);
		}

		/// <summary>
		/// Gets the entity of an object.
		/// </summary>
		public static T GetEntity<T>(this GameObject gameObject) where T : Entity
		{
			return Entity.GetEntity<T>(gameObject);
		}

		/// <summary>
		/// Checks if the object is an entity of a type.
		/// </summary>
		public static bool IsEntity<T>(this GameObject gameObject) where T : Entity
		{
			return GetEntity(gameObject) is T;
		}

		/// <summary>
		/// Tries getting the entity of an object.
		/// </summary>
		public static bool TryGetEntity<T>(this GameObject gameObject, out T entity) where T : Entity
		{
			return Entity.TryGetEntity(gameObject, out entity);
		}
	}
}