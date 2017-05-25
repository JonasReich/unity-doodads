//-------------------------------------------
// Copyright (c) 2017 - JonasReich
//-------------------------------------------

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

		/// <summary>
		/// Add a layer to the mask (Doesn't modify original)
		/// </summary>
		public static LayerMask Add (this LayerMask mask, int layer)
		{
			return mask | (1 << layer);
		}

		/// <summary>
		/// Remove a layer from the mask (Doesn't modify original)
		/// </summary>
		public static LayerMask Remove (this LayerMask mask, int layer)
		{
			return mask ^ (1 << layer);
		}
	}
}
