using UnityEngine;

namespace Game.Utils
{
	/// <summary>
	/// A wrapper for the animator component, for custom options.
	/// </summary>
	[RequireComponent(typeof(Animator))]
	public class AnimatorWrapper : MonoBehaviour
	{
		[SerializeField] private Animator _animator;
		[Space]
		[SerializeField] private bool _randomizeStart;

		private Animator animator { get => _animator; set => _animator = value; }
		private bool randomizeStart { get => _randomizeStart; }

		#region Unity
		private void Reset()
		{
			animator = GetComponent<Animator>();
		}

		private void Start()
		{
			RandomizeStart();
		}
		#endregion

		private void RandomizeStart()
		{
			if (!randomizeStart)
			{
				return;
			}

			// Gives every starting animation in each layer a random normalized time.
			for (int i = 0; i < animator.layerCount; i++)
			{
				AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(i);
				animator.Play(state.fullPathHash, i, Random.Range(0f, 1f));
			}
		}
	}
}