//-------------------------------------------
// (c) 2017 - Jonas Reich
//-------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UnityDoodats
{
	[System.Serializable]
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
			CreateTiles(root);
		}

		public void CreateTiles(Transform root)
		{
			for (int y = 0; y < Height; y++)
			{
				for (int x = 0; x < Width; x++)
				{
					T tNew = GameObject.Instantiate<T>(prefab, new Vector3(x,y), root.rotation);

					tNew.name = y.ToString() + " " + x.ToString();
					this[x, y] = tNew;
					tNew.transform.parent = root;
				}
			}
		}
	}
}
