//-------------------------------------------
// (c) 2017 - Jonas Reich
//-------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UnityDoodats
{
	/// <summary>
	///
	/// </summary>
	public class PathfindingTestGrid : MonoBehaviour
	{
		public PathfindingTestTile prefab;
		ComponentGridWithCosts<PathfindingTestTile> grid;

		[SerializeField]
		int width, height;
		[SerializeField]
		XY origin, target;


		void Awake ()
		{
			grid = new ComponentGridWithCosts<PathfindingTestTile>(width, height, prefab, transform);
		}

		public void Update ()
		{
			var mouseWorldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			target = new XY((int)mouseWorldPoint.x, (int)mouseWorldPoint.y);


			if (grid.IsValid(target) == false)
				return;

			foreach (var item in grid)
				item.meshRenderer.material.color = Color.white;

			var path = PathfindingAlgorithms.Dijkstra(origin, target, grid);
			grid[origin].meshRenderer.material.color = Color.black;
			grid[target].meshRenderer.material.color = Color.blue;
			foreach (var item in path)
				item.meshRenderer.material.color = Color.red;
		}
	}
}
