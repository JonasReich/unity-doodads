//-------------------------------------------
// (c) 2017 - Jonas Reich
//-------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UnityDoodats
{
	public class Grid<T> : IGrid<T> where T : Component
	{
		//--------------------------------------
		// Fields
		//--------------------------------------

		T[,] cells;
		T prefab;

		//--------------------------------------
		// Properties
		//--------------------------------------

		public int Width { get { return cells.GetLength(0); } }
		public int Height { get { return cells.GetLength(1); } }
		public int ItemCount
		{
			get
			{
				int i = 0;

				foreach (var item in cells)
					if (item != null)
						i++;

				return i;
			}
		}

		//--------------------------------------
		// Overload [] operators
		//--------------------------------------

		public T this[int x, int y] { get { return cells[x, y]; } set { cells[x, y] = value; } }
		public T this[XY pos] { get { return cells[pos.x, pos.y]; } set { cells[pos.x, pos.y] = value; } }

		//--------------------------------------
		// Setup
		//--------------------------------------

		public Grid(int columnCount, int rowCount, T prefab, Transform root)
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

		//--------------------------------------
		// Get Information
		//--------------------------------------

		public XY GetPosition(T item)
		{
			for (int x = 0; x < Width; x++)
				for (int y = 0; y < Height; y++)
					if (cells[x, y] == item)
						return new XY(x, y);

			return XY.invalid;
		}

		public bool IsValid(XY pos)
		{
			return pos.x > 0 && pos.x < Width && pos.y > 0 && pos.y < Width;
		}

		public T[] AdjacentItems(T item)
		{
			List<T> adjacentTiles = new List<T>();

			foreach (var pos in AdjacentTiles(GetPosition(item)))
				if (this[pos] != null)
					adjacentTiles.Add(this[pos]);

			return adjacentTiles.ToArray();
		}

		public XY[] AdjacentTiles(XY pos)
		{
			List<XY> adjacentTiles = new List<XY>();

			for (int x = -1; x <= +1; x++)
				for (int y = -1; y <= +1; y++)
				{
					XY n = new XY(pos.x + x, pos.y + y);
					if (IsValid(n))
						adjacentTiles.Add(n);
				}

			return adjacentTiles.ToArray();
		}

		//--------------------------------------
		// Modify
		//--------------------------------------

		public void Remove(T t)
		{
			var pos = GetPosition(t);

			if (IsValid(pos))
				this[pos] = null;
		}

		public void Swap(T A, T B)
		{
			Swap(GetPosition(A), GetPosition(B));
		}

		public void Swap(XY A, XY B)
		{
			T temp = this[A];
			this[A] = this[B];
			this[B] = this[A];
		}

		public void Clear()
		{
			for (int x = 0; x < Width; x++)
				for (int y = 0; y < Height; y++)
					this[x, y] = null;
		}

		//--------------------------------------
		// Enumerate
		//--------------------------------------

		public IEnumerator<T> GetEnumerator()
		{
			for (int x = 0; x < Width; x++)
				for (int y = 0; y < Height; y++)
					yield return cells[x, y];
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
