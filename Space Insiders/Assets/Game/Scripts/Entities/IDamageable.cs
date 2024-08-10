using UnityEngine;

namespace Game
{
	/// <summary>
	/// Interface for objects that can be damaged.
	/// </summary>
	public interface IDamageable
	{
		/// <summary>
		/// Makes the object take damage.
		/// </summary>
		void TakeDamage(int damage);
	}
}