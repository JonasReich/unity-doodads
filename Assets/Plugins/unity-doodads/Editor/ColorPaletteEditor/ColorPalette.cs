//-------------------------------------------
// (c) 2017 - Jonas Reich
//-------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Doodads.Editor
{
	/// <summary>
	/// Collection of colors
	/// </summary>
	public class ColorPalette : ScriptableObject, IEnumerable<Color>
	{
		public List<Color> colors;
		
		public void Init ()
		{
			colors = new List<Color>();
			colors.Add(Color.white);
			colors.Add(Color.black);
			colors.Add(Color.blue);
		}

		public IEnumerator<Color> GetEnumerator()
		{
			return colors.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable) colors).GetEnumerator();
		}
	}
}
