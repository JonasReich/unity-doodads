//-------------------------------------------
// (c) 2017 - Jonas Reich
//-------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UnityDoodats
{
	/// <summary>
	///
	/// </summary>
	public class ComponentGridWithCosts<T> : ComponentGrid<T>, IGridWithCosts<T> where T : Component
	{
		int[,] entryCosts; // Easiest implementation atm

		public ComponentGridWithCosts(int columnCount, int rowCount, T prefab, Transform root) : base(columnCount, rowCount, prefab, root)
		{
			entryCosts = new int[columnCount, rowCount];
		}

		public int GetCosts(XY A, XY B)
		{
			return entryCosts[B.x, B.y];
		}
	}
}
