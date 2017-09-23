//-------------------------------------------
// (c) 2017 - Jonas Reich
//-------------------------------------------

using UnityEngine;

namespace Doodads
{
	/// <summary>
	/// Generic Grid that instantiates prefab copies on construction
	/// </summary>
	[System.Serializable]
	public class ComponentGrid<T> : Grid<T> where T : Component
	{
		[SerializeField, HideInInspector]
		protected T prefab;

		//--------------------------------------
		// Setup
		//--------------------------------------

		public ComponentGrid(int width, int height, T prefab, Transform root)
			: base(width, height)
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
					T tNew = GameObject.Instantiate<T>(prefab, root.position + new Vector3(x,y), root.rotation);

					tNew.name = y.ToString() + " " + x.ToString();
					this[x, y] = tNew;
					tNew.transform.parent = root;
				}
			}
		}
	}
}
