using UnityEngine;
using UnityEngine.UI;

namespace UnityDoodats.UI
{
	/// <summary>
	/// Manages UI presentation of HP, Mana etc.
	/// </summary>
	public class StatBar : MonoBehaviour
	{
		// ---------------------------------------------
		// The bar should be placed on an own GameObject with all
		// visual elements beneath the StartBar transform.
		//
		// The 'bar' Image has to be set to a fill mode to display
		// current progress, preferrably 'linear (left)'
		// ---------------------------------------------

		/// <summary>
		/// UI Image Component that is filled according to the current percentage
		/// </summary>
		public Image bar;
		/// <summary>
		/// Text component that displays the numerical value/label text
		/// </summary>
		public Text textComponent;
		/// <summary>
		/// Hide/Show the bar on Set() calls:
		/// Hide if value hits 0 or maxValue.
		/// Show if it's inbetween.
		/// </summary>
		public bool autoHide = false;

		// ---------------------------------------------

		/// <summary>
		/// Set the bar to a different absolute value and show the Bar.
		/// </summary>
		/// <param name="value">Current value</param>
		/// <param name="maxValue">Maximum possible value (to show the percentage of)</param>
		/// <param name="label">String that should be printed on the attached text field</param>
		public void SetAndShow (float value, float maxValue, string label = null)
		{
			Show();
			Set(value, maxValue, label);
		}

		/// <summary>
		/// Set the bar to a different absolute value.
		/// </summary>
		/// <param name="value">Current value</param>
		/// <param name="maxValue">Maximum possible value (to show the percentage of)</param>
		/// <param name="label">String that should be printed on the attached text field</param>
		public void Set (float value, float maxValue, string label = null)
		{
			if (autoHide)
				if (value <= 0 || value >= maxValue)
					Hide();
				else
					Show();

			bar.fillAmount = value / maxValue;

			if (textComponent != null)
				textComponent.text = label != null ? label : value + " / " + maxValue;
		}

		public void Show()
		{
			gameObject.SetActive(true);
		}

		public void Hide()
		{
			gameObject.SetActive(false);
		}
	}
}
