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
	public class ComponentGridWithCosts<T> : ComponentGrid<T>, IGridWithCosts<T> where T : Component, IGridTileWithEntryCosts
	{
		Grid<int> entryCosts; // Easiest implementation atm

		public ComponentGridWithCosts(int columnCount, int rowCount, T prefab, Transform root) : base(columnCount, rowCount, prefab, root)
		{
			entryCosts = new Grid<int>(columnCount, rowCount);
			entryCosts.Initialize(1);
		}

		public int GetCosts(XY A, XY B)
		{
			return cells[B.x, B.y].EntryCosts;
		}
	}

	public interface IGridTileWithEntryCosts
	{
		int EntryCosts { get; }
	}
}
