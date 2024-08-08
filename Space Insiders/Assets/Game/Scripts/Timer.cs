using Unity.VisualScripting;
using UnityEngine;

namespace Game.Utils
{
	/// <summary>
	/// A simple timer.
	/// </summary>
	[System.Serializable]
	public class Timer
	{
		[SerializeField] private bool _loop = true;
		[SerializeField, Min(0f)] private float _duration;
		[SerializeField, Min(0f)] private float _time;

		public bool isDone => time >= duration;
		public bool loop { get => _loop; set => _loop = value; }
		public float duration { get => _duration; set => _duration = value; }
		public float time { get => _time; set => _time = value; }

		/// <summary>
		/// Constructs a timer.
		/// </summary>
		public Timer(bool loop = false)
		{
			this.loop = loop;
		}

		/// <summary>
		/// Constructs a timer.
		/// </summary>
		public Timer(float duration, bool loop = false)
		{
			this.loop = loop;
			this.duration = duration;
		}

		/// <summary>
		/// Runs the timer for a frame.
		/// </summary>
		public bool Run()
		{
			if (!isDone)
			{
				time += Time.deltaTime;
			}
			else
			{
				if (loop)
				{
					time -= duration;
				}

				return true;
			}

			return false;
		}

		/// <summary>
		/// Resets the timer.
		/// </summary>
		public void Reset()
		{
			time = 0f;
		}
	}
}