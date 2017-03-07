using UnityEngine;

namespace UnityDoodats
{
	/// <summary>
	/// Provides extension methods for Unity LayerMasks
	/// </summary>
	public static class LayerMaskExtensions
	{
		/// <summary>
		/// Check whether a given layer is part of a LayerMask
		/// </summary>
		/// <param name="mask">LayerMask to test against</param>
		/// <param name="layer">Layer to test</param>
		public static bool IsInLayerMask (this LayerMask mask, int layer)
		{
			return ((mask.value & (1 << layer)) > 0);
		}

		/// <summary>
		/// Check whether a given GameObject is part of a LayerMask
		/// </summary>
		/// <param name="mask">LayerMask to test against</param>
		/// <param name="layer">GameObject to test</param>
		public static bool IsInLayerMask (this LayerMask mask, GameObject obj)
		{
			return ((mask.value & (1 << obj.layer)) > 0);
		}
	}
}
