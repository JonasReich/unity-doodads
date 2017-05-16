using UnityEngine;

namespace UnityDoodats
{
	/// <summary>
	/// Provides extension methods for Unity LayerMasks
	/// </summary>
	public static class LayerMaskExtensions
	{
		/// <summary>
		/// Check whether a given layer is part of the LayerMask
		/// </summary>
		public static bool Contains (this LayerMask mask, int layer)
		{
			return ((mask.value & (1 << layer)) > 0);
		}
	}
}
