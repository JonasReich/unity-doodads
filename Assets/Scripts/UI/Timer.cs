using UnityEngine;
using UnityEngine.UI;

namespace UnityDoodats
{
	/// <summary>
	/// Timer UI wrapper that displays a float time in integer format
	/// </summary>
	public class Timer : MonoBehaviour
	{
		public Text label;
		public Text timeText;
		int oldTime;

		/// <summary>
		/// Update the time to a given value
		/// </summary>
		/// <param name="time">new time value</param>
		public void SetTime (float time, string labelText = null)
		{
			int newTime = (int)time;
			if (newTime < oldTime)
				Tick();

			oldTime = newTime;

			timeText.text = ((int)time).ToString();

			if (labelText != null)
				label.text = labelText;
		}

		public void Show ()
		{
			gameObject.SetActive(true);
		}

		public void Hide ()
		{
			gameObject.SetActive(false);
		}

		void Tick ()
		{
			// insert animation queue here
		}
	}
}
