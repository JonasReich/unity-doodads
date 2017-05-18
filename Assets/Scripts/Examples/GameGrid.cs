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
		public Sprite whiteSprite, blackSprite;

		Grid<SpriteRenderer> chessboard;


		private void Awake()
		{
			chessboard = new Grid<SpriteRenderer>(8, 8);
			// Unity crashes after leaving the constructor

			for (int x = 0; x < chessboard.Width; x++)
			{
				for (int y = 0; y < chessboard.Height; y++)
				{
					chessboard[x,y] = Instantiate(transform).gameObject.AddComponent<SpriteRenderer>();
					chessboard[x, y].transform.localPosition = new Vector2(x, y);
				}
			}

			int i = 0;
			foreach (var cell in chessboard)
			{
				i++;
				cell.sprite = (i % 2 == 0) ? whiteSprite : blackSprite;
			}
		}
	}
}
