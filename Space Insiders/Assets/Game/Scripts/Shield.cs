using Game.Agents;
using UnityEngine;

namespace Game
{
	/// <summary>
	/// Shields are damageable barriers.
	/// </summary>
	public class Shield : Entity, IDamageable
	{
		[SerializeField, Min(0f)] private int _life;
		[SerializeField] private Transform _lifeGraphics;

		private int life { get => _life; set => _life = value; }
		private int startLife { get; set; }
		private Transform lifeGraphics { get => _lifeGraphics; }

		#region Unity
		protected override void Start()
		{
			base.Start();
			startLife = life;
		}

		protected override void OnCollisionEnter2D(Collision2D other)
		{
			base.OnCollisionEnter2D(other);

			if (other.gameObject.IsEntity<Alien>())
			{
				Destroy();
			}
		}
		#endregion

		public void TakeDamage(int damage)
		{
			life -= damage;

			if (life <= 0)
			{
				Destroy();
			}

			lifeGraphics.localScale = new Vector2((float)life / startLife, 1f);
		}
	}
}