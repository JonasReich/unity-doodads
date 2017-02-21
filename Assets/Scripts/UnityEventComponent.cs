using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace NAMESPACE
{
	/// <summary>
	/// UnityEvent standalone component
	/// </summary>
	public class UnityEventComponent : MonoBehaviour
	{
		[SerializeField]
		private bool invokeNow;

		public UnityEvent OnEvent;


		void Update()
		{
			if (invokeNow)
			{
				Invoke();
				invokeNow = false;
			}
		}

		public void Invoke() { OnEvent.Invoke(); }
	}
}
