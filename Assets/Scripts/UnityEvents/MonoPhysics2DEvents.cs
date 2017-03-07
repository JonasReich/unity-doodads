using UnityEngine;
using UnityEngine.Events;

namespace UnityDoodats.UnityEvents
{
	/// <summary>
	/// Event component that has UnityEvents for the 2D Physic messages of MonoBehaviours
	/// </summary>
	public class MonoPhysics2DEvents : MonoBehaviour
	{
		[Header("Collisions")]
		public UnityEvent onCollisionEnter2D;
		public UnityEvent onCollisionStay2D;
		public UnityEvent onCollisionExit2D;

		[Header("Trigger")]
		public UnityEvent onTriggerEnter2D;
		public UnityEvent onTriggerExit2D;
		public UnityEvent onTriggerStay2D;


		// ---------------------------
		// 2D Collisions
		// ---------------------------

		private void OnCollisionEnter2D (Collision2D collision)
		{
			onCollisionEnter2D.Invoke();
		}

		private void OnCollisionStay2D (Collision2D collision)
		{
			onCollisionStay2D.Invoke();
		}

		private void OnCollisionExit2D (Collision2D collision)
		{
			onCollisionExit2D.Invoke();
		}

		// ---------------------------
		// 2D Trigger
		// ---------------------------
		private void OnTriggerEnter2D (Collider2D collision)
		{
			onTriggerEnter2D.Invoke();
		}

		private void OnTriggerStay2D (Collider2D collision)
		{
			onTriggerStay2D.Invoke();
		}

		private void OnTriggerExit2D (Collider2D collision)
		{
			onTriggerExit2D.Invoke();
		}
	}
}
