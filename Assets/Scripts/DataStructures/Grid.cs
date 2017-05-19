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
	public class Grid<TileType> : MonoBehaviour where TileType : Component
	{
		public TileType prefab;

		public int columnCount;
		public int rowCount;

		public TileType[,] tileGrid;
		protected int tileCount;

		protected class Coordinates
		{
			public int x, y;

			public Coordinates(int _x, int _y)
			{
				x = _x;
				y = _y;
			}
		}

		public virtual void Awake()
		{
			tileGrid = new TileType[columnCount, rowCount];
			tileCount = 0;
		}

		public virtual void Start()
		{
			CreateTiles();
		}


		//SETUP GRID

		protected void CreateTiles()
		{
			for (int x = 0; x < rowCount; x++)
			{
				for (int y = 0; y < columnCount; y++)
				{
					TileType tNew = Instantiate<TileType>(prefab, new Vector3(0.5f + y - columnCount / 2, 0.5f + x - rowCount / 2), transform.rotation);

					tNew.name = x.ToString() + " " + y.ToString();
					tileGrid[y, x] = tNew;
					tNew.transform.parent = this.transform;
					tileCount++;
				}
			}
		}


		//GRID MANIPULATION

		protected Coordinates getGridPosition(TileType originTile)
		{
			for (int x = 0; x < rowCount; x++)
			{
				for (int y = 0; y < columnCount; y++)
				{
					if (originTile == tileGrid[y, x])
					{
						return new Coordinates(y, x);
					}
				}
			}
			return new Coordinates(0, 0);
		}

		protected TileType[] AdjacentTiles(TileType originTile)
		{
			TileType[] tileset = new TileType[9];

			Coordinates pos = getGridPosition(originTile);
			int tileCount = 0;

			for (int x = -1; x <= +1; x++)
			{
				for (int y = -1; y <= +1; y++)
				{
					int checkX = pos.x + x;
					int checkY = pos.y + y;

					//clamp to grid bounds
					checkX = Math.Min(Math.Max(checkX, 0), columnCount - 1);
					checkY = Math.Min(Math.Max(checkY, 0), rowCount - 1);

					tileset[tileCount] = tileGrid[checkX, checkY];

					tileCount++;
				}
			}

			return cleanArray(tileset);
		}

		protected void SwapGridTiles(TileType origin, TileType target)
		{
			Coordinates oPos = getGridPosition(origin);
			Coordinates tPos = getGridPosition(target);

			SwapReferences(ref tileGrid[oPos.x, oPos.y], ref tileGrid[tPos.x, tPos.y]);
		}


		//ADDITIONAL METHODS

		private TileType[] cleanArray(TileType[] inputArray)
		{
			ArrayList outList = new ArrayList();

			for (int i = 0; i < inputArray.Length; i++)
			{
				if (!outList.Contains(inputArray[i]))
				{
					outList.Add(inputArray[i]);
				}
			}

			TileType[] outArray = new TileType[outList.Count];

			for (int i = 0; i < outList.Count; i++)
			{
				outArray[i] = (TileType)outList[i];
			}
			return outArray;
		}

		private void SwapReferences<T>(ref T swap1, ref T swap2)
		{
			T swapVar = swap1;
			swap1 = swap2;
			swap2 = swapVar;
		}
	}

	/*
	/// <summary>
	///
	/// </summary>
	public class Grid<T> : IGrid<T> where T : Component
	{
		private T[,] cells;

		public int Width { get { return cells.GetLength(0); } }
		public int Height { get { return cells.GetLength(1); } }
		public int ItemCount
		{
			get
			{
				int i = 0;

				foreach (var item in this)
					if (item != null)
						i++;

				return i;
			}
		}


		public void Init(int width, int height)
		{
			cells = new T[width, height];
			// Unity crashes after leaving the constructor
		}


		public T this[int x, int y] { get { return cells[x, y]; } set { cells[x, y] = value; } }
		public T this[Vec2i pos] { get { return this[pos.x, pos.y]; } set { this[pos.x, pos.y] = value; } }

		public void Remove(T t)
		{
			var pos = GetPosition(t);

			if (IsValid(pos))
				this[pos] = null;
		}

		public Vec2i GetPosition(T item)
		{
			for (int x = 0; x < Width; x++)
				for (int y = 0; y < Height; y++)
					if (cells[x, y].Equals(item))
						return new Vec2i(x, y);

			return Vec2i.invalid;
		}

		public void Swap(T A, T B)
		{
			Swap(GetPosition(A), GetPosition(B));
		}

		public void Swap(Vec2i A, Vec2i B)
		{
			T temp = this[A];
			this[A] = this[B];
			this[B] = this[A];
		}

		public T[] AdjacentItems(T item)
		{
			List<T> adjacentTiles = new List<T>();

			foreach (var pos in AdjacentTiles(GetPosition(item)))
				if (this[pos] != null)
					adjacentTiles.Add(this[pos]);

			return adjacentTiles.ToArray();
		}

		public Vec2i[] AdjacentTiles(Vec2i pos)
		{
			List<Vec2i> adjacentTiles = new List<Vec2i>();

			for (int x = -1; x <= +1; x++)
				for (int y = -1; y <= +1; y++)
				{
					Vec2i n = new Vec2i(pos.x + x, pos.y + y);
					if (IsValid(n))
						adjacentTiles.Add(n);
				}

			return adjacentTiles.ToArray();
		}

		public bool IsValid(Vec2i pos)
		{
			return pos.x > 0 && pos.x < Width && pos.y > 0 && pos.y < Width;
		}

		public void Clear()
		{
			for (int x = 0; x < Width; x++)
				for (int y = 0; y < Height; y++)
					this[x, y] = null;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return cells.GetEnumerator();
		}

		public IEnumerator<T> GetEnumerator()
		{
			throw new NotImplementedException();
		}
	}
	*/
}
