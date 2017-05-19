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
	public class GameGrid : MonoBehaviour
	{
		public SpriteRenderer prefab;
		public Sprite whiteSprite, blackSprite;
		Grid<SpriteRenderer> grid;

		public void Awake()
		{
			grid = new Grid<SpriteRenderer>(8,8,prefab,transform);

			int i = 0;
			for (int x = 0; x < grid.Width; x++)
			{
				for (int y = 0; y < grid.Height; y++)
				{
					i++;
					grid[x, y].sprite = (i % 2 == 0) ? whiteSprite : blackSprite;
					grid[x, y].color = (i % 2 == 0) ? Color.white : Color.black;
				}
				i++;
			}

		}
	}
}
