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
	using UnityEngine;
	using UnityEditor;

	public class TetrahedronEditor : Editor
	{
		[MenuItem("GameObject/3D Object/Tetrahedron")]
		static void Create ()
		{
			GameObject gameObject = new GameObject("Tetrahedron");
			Tetrahedron s = gameObject.AddComponent<Tetrahedron>();
			s.Initialize();
		}
	}
}
