using Game.Entities;
using UnityEngine;

namespace Game.Effects
{
	public abstract class Effect : ScriptableObject
	{
		[Header(nameof(Effect))]
		[SerializeField, Min(0f)] private float _duration;

		public float duration { get => _duration; }

		/// <summary>
		/// Called when the effect starts.
		/// </summary>
		public virtual void OnStart(Player player) { }
		/// <summary>
		/// Called every frame the effect is active.
		/// </summary>
		public virtual void OnUpdate(Player player) { }
		/// <summary>
		/// Called when the effect stops.
		/// </summary>
		public virtual void OnStop(Player player) { }
	}
}