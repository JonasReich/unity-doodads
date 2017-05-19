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
	public class GameGrid : Grid<SpriteRenderer>
	{
		public Sprite whiteSprite, blackSprite;

		override public void Awake()
		{
			base.Awake();
			CreateTiles();
			//chessboard.Init(8, 8);
			// Unity crashes after leaving the constructor
			/*
			for (int x = 0; x < columnCount; x++)
			{
				for (int y = 0; y < rowCount; y++)
				{
					tileGrid[x,y] = Instantiate(transform).gameObject.AddComponent<SpriteRenderer>();
					tileGrid[x, y].transform.localPosition = new Vector2(x, y);
				}
			}
			*/
			int i = 0;
			for (int x = 0; x < columnCount; x++)
			{
				for (int y = 0; y < rowCount; y++)
				{
					i++;
					tileGrid[x, y].sprite = (i % 2 == 0) ? whiteSprite : blackSprite;
					tileGrid[x, y].color = (i % 2 == 0) ? Color.white : Color.black;
				}
				i++;
			}

		}
	}
}
