//-------------------------------------------
// Copyright (c) 2017 - JonasReich
//-------------------------------------------

using System.Collections.Generic;

namespace UnityDoodats
{
	/// <summary>
	/// Extended version of UnityEngine.Random
	/// </summary>
	public static class Random
	{
		/// <summary>
		/// Returns a random float number in an inclusive range defined as Pair
		/// </summary>
		public static float Range (Pair<float, float> range)
		{
			return UnityEngine.Random.Range(range.first, range.second);
		}

		/// <summary>
		/// Returns a random float number between min and max [inclusive]
		/// </summary>
		public static float Range (float min, float max)
		{
			return UnityEngine.Random.Range(min, max);
		}

		/// <summary>
		/// Returns a random int number in an inclusive range defined as Pair
		/// </summary>
		public static int Range (Pair<int, int> range)
		{
			return UnityEngine.Random.Range(range.first, range.second + 1);
		}

		/// <summary>
		/// Returns a random integer number between min and max [inclusive]
		/// </summary>
		public static int Range (int min, int max)
		{
			return UnityEngine.Random.Range(min, max + 1);
		}

		/// <summary>
		/// Returns a random element from an array
		/// </summary>
		public static T Of<T> (T[] array)
		{
			return array[Range(0, array.Length - 1)];
		}

		/// <summary>
		/// Returns a random element from a list
		/// </summary>
		public static T Of<T> (List<T> list)
		{
			return list[Range(0, list.Count - 1)];
		}
	}
}
