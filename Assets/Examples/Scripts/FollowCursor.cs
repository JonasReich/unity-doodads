//-------------------------------------------
// (c) 2017 - Jonas Reich
//-------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Doodads
{
	/// <summary>
	/// 
	/// </summary>
	public class FollowCursor : MonoBehaviour
	{	
		void Update ()
		{
			transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		}
	}
}
