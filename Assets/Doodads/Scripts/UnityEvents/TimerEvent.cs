using UnityEngine;
using UnityEngine.Events;

namespace Doodads.UnityEvents
{
	/// <summary>
	/// UnityEvent component that can be used as countdown timer
	/// </summary>
	public class TimerEvent : MonoBehaviour
	{
		public bool startNow = false;
		public float duration = 20f;

		public UnityEvent onStart;
		public UnityEvent onEnd;

		float timer = 0f;

		private void Update ()
		{
			if (startNow)
			{
				startNow = false;
				timer = duration;
				onStart.Invoke();
			}

			if(timer > 0)
			{
				timer -= Time.deltaTime;
				if (timer <= 0)
					onEnd.Invoke();
			}
		}
	}
}
