//-------------------------------------------
// (c) 2017 - Jonas Reich
//-------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UnityDoodats
{
	public class ComponentGrid<T> : Grid<T> where T : Component
	{
		[SerializeField]
		protected T prefab;

		//--------------------------------------
		// Setup
		//--------------------------------------

		public ComponentGrid(int columnCount, int rowCount, T prefab, Transform root)
			: base(columnCount, rowCount)
		{
			this.prefab = prefab;

			cells = new T[columnCount, rowCount];
			CreateTiles(root);
		}

		public void CreateTiles(Transform root)
		{
			for (int x = 0; x < Height; x++)
			{
				for (int y = 0; y < Width; y++)
				{
					T tNew = GameObject.Instantiate<T>(prefab, new Vector3(0.5f + y - Width / 2, 0.5f + x - Height / 2), root.rotation);

					tNew.name = x.ToString() + " " + y.ToString();
					cells[y, x] = tNew;
					tNew.transform.parent = root;
				}
			}
		}
	}
}
