//-------------------------------------------
// (c) 2017 - Jonas Reich
//-------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Doodads.Examples
{
	/// <summary>
	/// Used to showcase the funcitonality of my generic grid implementation and various 2D pathfinindg algorithms
	/// </summary>
	public class PathfindingTestGrid : MonoBehaviour
	{
		public PathfindingTestTile tilePrefab;

		[SerializeField, HideInInspector]
		Grid grid;

		[SerializeField]
		int width, height;
		[SerializeField]
		XY origin, target;

		public PathfindingAlgorithms.Algorithm algorithm;

		LayerMask tileLayerMask;


		void Awake ()
		{
			tileLayerMask = tileLayerMask.Add(tilePrefab.gameObject.layer);
		}

		void Update ()
		{
			RaycastHit hit;
			if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 300f, tileLayerMask))
				target = new XY((int)hit.point.x, (int)hit.point.y);
			else
				target = XY.invalid;

			// Restore colors
			foreach (var item in grid)
				item.meshRenderer.material.color = Color.white;
			grid[origin].meshRenderer.material.color = Color.black;

			// Search
			var path = PathfindingAlgorithms.Search(algorithm, grid, origin, ExitCondition);
			
			if (path != null)
			{
				foreach (var item in path)
					item.meshRenderer.material.color = Color.red;

				grid[target].meshRenderer.material.color = Color.blue;
			}
		}


		public void Refresh ()
		{
			while (transform.childCount != 0)
				DestroyImmediate(transform.GetChild(0).gameObject);

			grid = new Grid(width, height, tilePrefab, transform);
		}


		bool ExitCondition<T> (IGrid<T> grid, XY tile)
		{
			return tile.x == target.x && tile.y == target.y;
		}

		[Serializable]
		public class Grid : ComponentGridWithCosts<PathfindingTestTile>
		{
			public Grid (int columnCount, int rowCount, PathfindingTestTile prefab, Transform root) : base(columnCount, rowCount, prefab, root)
			{
			}
		}
	}
}
