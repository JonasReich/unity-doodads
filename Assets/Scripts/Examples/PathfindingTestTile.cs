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
	/// <summary>
	/// 
	/// </summary>
	public class PathfindingTestTile : MonoBehaviour, IGridTileWithEntryCosts
	{
		public int EntryCosts { get { return entryCosts; } }

		[SerializeField]
		int entryCosts = 1;

		public MeshRenderer meshRenderer;

		private void Update ()
		{
			var newPos = transform.position;
			newPos.z= entryCosts;
			transform.position = newPos; 
		}
	}
}
