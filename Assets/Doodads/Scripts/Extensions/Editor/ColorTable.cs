//-------------------------------------------
// (c) 2017 - Jonas Reich
//-------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Doodads.Editor
{
	/// <summary>
	/// 
	/// </summary>
	public class ColorTable : ScriptableObject
	{
		public List<Color> colors;
		
		public void Init ()
		{
			colors = new List<Color>();
			colors.Add(Color.white);
			colors.Add(Color.black);
			colors.Add(Color.blue);
		}
	}
}
