using UnityEngine;

namespace NAMESPACE
{
	/// <summary>
	/// Provides methods for array serialization that are not available from Unitys JsonUtility
	/// </summary>
	public class JsonArrayUtility
	{
		/// <summary>
		/// Generate an array of objects from a json string
		/// </summary>
		/// <typeparam name="T">serializable type</typeparam>
		/// <param name="json">json string (e.g. parsed from txt file)</param>
		/// <returns></returns>
		public static T[] FromJsonArray<T> (string json)
		{
			string data = "{ \"array\": " + json + "}";
			Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(data);
			return wrapper.array;
		}

		/// <summary>
		/// Generate a json text string from an array of objects
		/// </summary>
		/// <typeparam name="T">serializable type</typeparam>
		/// <param name="array">array to be serialized</param>
		/// <returns></returns>
		public static string ToJsonArray<T> (T[] array)
		{
			Wrapper<T> wrapper = new Wrapper<T>(array);
			return JsonUtility.ToJson(wrapper, true);
		}

		// Wrapper for array de-/serialization
		[System.Serializable]
		private class Wrapper<T>
		{
			public T[] array;
			public Wrapper (T[] array) { this.array = array; }
		}
	}
}
