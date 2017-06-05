using UnityEngine;
using UnityEngine.Events;

namespace Doodads.UnityEvents
{
	/// <summary>
	/// Event component that has UnityEvents for the basic MonoBehaviour messages
	/// </summary>
	public class MonoBehaviourEvents : MonoBehaviour
	{
		public UnityEvent onAwake;
		public UnityEvent onStart;
		public UnityEvent onUpdate;
		public UnityEvent onDestroy;
		public UnityEvent onEnable;
		public UnityEvent onDisable;

		private void Awake ()
		{
			onAwake.Invoke();
		}

		private void Start ()
		{
			onStart.Invoke();
		}

		private void Update ()
		{
			onUpdate.Invoke();
		}

		private void OnDestroy ()
		{
			onDestroy.Invoke();
		}

		private void OnEnable ()
		{
			onEnable.Invoke();
		}

		private void OnDisable ()
		{
			onDisable.Invoke();
		}
	}
}
