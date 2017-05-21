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
		public XY origin;
		public XY target;

		void Awake ()
		{
			grid = new ComponentGridWithCosts<MeshRenderer>(2, 2, prefab, transform);
		}

		public void Update()
		{
			if (Input.GetKeyDown(KeyCode.Space))
				StartCoroutine(SearchForTarget());
		}

		IEnumerator SearchForTarget()
		{
			foreach (var item in grid)
				item.material.color = Color.white;

			var path = PathfindingAlgorithms.BreathFirstSearch(origin, target, grid);
			foreach (var item in path)
			{
				item.material.color = Color.red;
				yield return new WaitForSeconds(0.5f);
			}

			grid[origin].material.color = Color.black;
			grid[target].material.color = Color.blue;
		}
	}
}
