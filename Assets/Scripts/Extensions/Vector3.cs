using UnityEngine;

namespace UnityDoodats
{
	/// <summary>
	/// Provides extension methods for Unity's Vector3 class
	/// </summary>
	static public class Vector3Extensions
	{
		/// <summary>
		/// Set x, y, and z components of an existing Vector3 (single parameter).
		/// </summary>
		/// <param name="xyz">Length of all 3 axises</param>
		static public void Set (this Vector3 vector, float xyz)
		{
			vector.Set(xyz, xyz, xyz);
		}

		/// <summary>
		/// Clamps a Vector3 between a minimum and maximum length
		/// </summary>
		static public void Clamp(this Vector3 vector, float min, float max)
		{
			if (vector.magnitude > max)
				vector = vector.normalized * max;
			else if (vector.magnitude < min)
				vector = vector.normalized * min;
		}
	}
}
