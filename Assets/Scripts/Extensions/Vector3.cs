using UnityEngine;

namespace NAMESPACE
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
		/// <returns></returns>
		static public Vector3 Set (this Vector3 vector, float xyz)
		{
			return Set(vector, xyz, xyz, xyz);
		}

		static private Vector3 Set (Vector3 vector, float x, float y, float z)
		{
			vector.Set(x, y, z);
			return vector;
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
