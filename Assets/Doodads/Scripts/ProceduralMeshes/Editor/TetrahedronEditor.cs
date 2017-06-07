//-------------------------------------------
// (c) 2017 - Jonas Reich
//-------------------------------------------

using UnityEditor;
using UnityEngine;

namespace Doodads.Editor
{
	public class TetrahedronEditor : UnityEditor.Editor
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
