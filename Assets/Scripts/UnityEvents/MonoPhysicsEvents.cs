using UnityEngine;
using UnityEngine.Events;

namespace UnityDoodats.UnityEvents
{
	/// <summary>
	/// Event component that has UnityEvents for the 3D Physic messages of MonoBehaviours
	/// </summary>
	public class MonoPhysicsEvents : MonoBehaviour
	{
		[Header("Collisions")]
		public UnityEvent onCollisionEnter;
		public UnityEvent onCollisionStay;
		public UnityEvent onCollisionExit;

		[Header("Trigger")]
		public UnityEvent onTriggerEnter;
		public UnityEvent onTriggerStay;
		public UnityEvent onTriggerExit;


		// ---------------------------
		// 3D Collisions
		// ---------------------------

		private void OnCollisionEnter (Collision collision)
		{
			onCollisionEnter.Invoke();
		}

		private void OnCollisionStay (Collision collision)
		{
			onCollisionStay.Invoke();
		}

		private void OnCollisionExit (Collision collision)
		{
			onCollisionExit.Invoke();
		}

		// ---------------------------
		// 3D Trigger
		// ---------------------------
		private void OnTriggerEnter (Collider other)
		{
			onTriggerEnter.Invoke();
		}

		private void OnTriggerStay (Collider other)
		{
			onTriggerStay.Invoke();
		}

		private void OnTriggerExit (Collider other)
		{
			onTriggerExit.Invoke();
		}
	}
}
