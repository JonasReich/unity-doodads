using UnityEngine;
using UnityEngine.Events;

namespace Doodads.UnityEventComponents
{
	/// <summary>
	/// Collection of individual event "steps" that are activated sequentially
	/// </summary>
	public class EventStepChain : MonoBehaviour
	{
		public bool advanceNow = false;
		public bool loop = false;
		public UnityEvent[] Steps;

		int currentStep = 0;


		private void Update ()
		{
			if (advanceNow)
			{
				advanceNow = false;
				Next();
			}
		}


		public void Next ()
		{
			if (currentStep >= Steps.Length)
			{
				if (loop)
					currentStep = 0;
				else
					return;
			}

			Steps[currentStep].Invoke();
			currentStep++;
		}
	}
}
