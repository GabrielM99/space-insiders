using Game.Items;
using UnityEngine;

namespace Game.Entities
{
	/// <summary>
	/// An object that can spawn when an alien dies.
	/// </summary>
	public class Drop : Actor
	{
		[Header(nameof(Drop))]
		[SerializeField] private SpriteRenderer _spriteRenderer;

		[Space]
		[SerializeField] private Item _item;

		public Item item
		{
			get => _item;

			set
			{
				_item = value;
				UpdateGraphics();
			}
		}

		private SpriteRenderer spriteRenderer { get => _spriteRenderer; }

		#region Unity
		protected override void Start()
		{
			base.Start();
			movementDirection = Vector2.down;
		}

		protected override void OnTriggerEnter2D(Collider2D other)
		{
			base.OnTriggerEnter2D(other);

			if (other.gameObject.TryGetEntity(out Player player))
			{
				player.OnCollect(item);
				Destroy(gameObject);
			}
		}
		#endregion

		private void UpdateGraphics()
		{
			if (item != null)
			{
				name = item.name;
				spriteRenderer.sprite = item.sprite;
			}
		}
	}
}