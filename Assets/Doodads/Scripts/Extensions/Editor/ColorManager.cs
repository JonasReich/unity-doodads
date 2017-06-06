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
	/// Save a list of colors in a scriptable prefab asset
	/// </summary>
	public class ColorManager : EditorWindow
	{
		const string NAME = "colortable";
		const string PATH = "Assets/Doodads/Resources/" + NAME + ".asset";
		static ColorTable colorTable;
		

		[MenuItem("Tools/Color Manager")]
		private static void ShowWindow ()
		{
			var window = GetWindow<ColorManager>("Colors");
			window.Show();
		}

		private void OnEnable ()
		{
			LoadColors();
		}

		private void OnFocus ()
		{
			LoadColors();
		}

		private void OnLostFocus ()
		{
			SaveColors();
		}

		private void OnDisable ()
		{
			SaveColors();
		}

		private void OnDestroy ()
		{
			SaveColors();
		}

		void LoadColors ()
		{
			if (colorTable != null)
				return;
			
			colorTable = Resources.Load<ColorTable>(NAME);

			if(colorTable == null)
			{
				colorTable = CreateInstance<ColorTable>();
				colorTable.Init();
				AssetDatabase.CreateAsset(colorTable, PATH);
				AssetDatabase.SaveAssets();
				AssetDatabase.Refresh();
			}
		}

		void SaveColors ()
		{
			EditorUtility.SetDirty(colorTable);
			AssetDatabase.SaveAssets();
		}

		private void OnGUI ()
		{
			GUILayout.Space(15);

			if (colorTable != null)
				for (int i = 0; i < colorTable.colors.Count; i++)
				{
					EditorGUILayout.BeginHorizontal();
					GUILayout.Space(5);
					if (GUILayout.Button("","ol minus",  GUILayout.Width(15), GUILayout.Height(15)))
					{
						colorTable.colors.Remove(colorTable.colors[i]);
						break;
					}

					colorTable.colors[i] = EditorGUILayout.ColorField(colorTable.colors[i]);


					GUILayout.Space(5);
					if (GUILayout.Button("copy"))
					{
						EditorGUIUtility.systemCopyBuffer = "#" + ColorUtility.ToHtmlStringRGBA(colorTable.colors[i]);
					}
					if (GUILayout.Button("paste"))
					{
						Color c;
						string colorName = EditorGUIUtility.systemCopyBuffer;
						if (colorName.StartsWith("#") == false)
							colorName = "#" + colorName;
						ColorUtility.TryParseHtmlString(colorName, out c);
						colorTable.colors[i] = c;
						break;
					}

					EditorGUILayout.EndHorizontal();
				}
			GUILayout.Box("", new GUILayoutOption[] { GUILayout.ExpandWidth(true), GUILayout.Height(1) });
			EditorGUILayout.BeginHorizontal();

			GUILayout.Space(5);
			if (GUILayout.Button("", "ol plus", GUILayout.Width(20), GUILayout.Height(15)))
			{
				colorTable.colors.Add(Color.white);
				
			}

			EditorGUILayout.EndHorizontal();
		}
	}
}
