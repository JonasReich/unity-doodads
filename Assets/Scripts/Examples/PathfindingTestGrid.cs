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
		public MeshRenderer prefab;
		ComponentGridWithCosts<MeshRenderer> grid;

		public int width, height;

		public XY origin;
		public XY target;

		void Awake ()
		{
			grid = new ComponentGridWithCosts<MeshRenderer>(width, height, prefab, transform);
		}

		public void Update ()
		{
			var mouseWorldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

			foreach (var item in grid)
				item.material.color = Color.white;

			var path = PathfindingAlgorithms.BreathFirstSearch(origin, new XY((int)mouseWorldPoint.x, (int)mouseWorldPoint.y), grid);
			grid[origin].material.color = Color.black;
			grid[target].material.color = Color.blue;
			foreach (var item in path)
				item.material.color = Color.red;

			if (Input.GetKeyDown(KeyCode.Space))
				StartCoroutine(SearchForTarget());
		}

		IEnumerator SearchForTarget()
		{
			foreach (var item in grid)
				item.material.color = Color.white;

			var path = PathfindingAlgorithms.BreathFirstSearch(origin, target, grid);
			grid[origin].material.color = Color.black;
			grid[target].material.color = Color.blue;
			foreach (var item in path)
			{
				item.material.color = Color.red;
				yield return new WaitForSeconds(0.5f);
			}

		}
	}
}
