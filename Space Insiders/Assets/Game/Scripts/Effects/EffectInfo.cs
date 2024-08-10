using Game.Utils;
using UnityEngine;

namespace Game.Effects
{
	/// <summary>
	/// The info about an active effect.
	/// </summary>
	public class EffectInfo
	{
		public Effect effect { get; private set; }
		public Timer timer { get; private set; }

		/// <summary>
		/// Constructs an effect info.
		/// </summary>
		public EffectInfo(Effect effect)
		{
			this.effect = effect;
			timer = new Timer(effect.duration);
		}
	}
}