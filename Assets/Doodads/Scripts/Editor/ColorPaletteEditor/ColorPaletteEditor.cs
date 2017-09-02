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
	/// Save a list of colors in a ColorPalette scriptable object
	/// </summary>
	public class ColorPaletteEditor : EditorWindow
	{
		const string NAME = "colorpalette";
		const string PATH = "Assets/Doodads/Scripts/Editor/ColorPaletteEditor/Resources/" + NAME + ".asset";
		static ColorPalette colorPalette;
		

		[MenuItem("Tools/Color Manager")]
		private static void ShowWindow ()
		{
			var window = GetWindow<ColorPaletteEditor>("Colors");
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
			if (colorPalette != null)
				return;
			
			colorPalette = Resources.Load<ColorPalette>(NAME);

			if(colorPalette == null)
			{
				colorPalette = CreateInstance<ColorPalette>();
				colorPalette.Init();
				AssetDatabase.CreateAsset(colorPalette, PATH);
				AssetDatabase.SaveAssets();
				AssetDatabase.Refresh();
			}
		}

		void SaveColors ()
		{
			EditorUtility.SetDirty(colorPalette);
			AssetDatabase.SaveAssets();
		}

		private void OnGUI ()
		{
			GUILayout.Space(15);

			if (colorPalette != null)
				for (int i = 0; i < colorPalette.colors.Count; i++)
				{
					EditorGUILayout.BeginHorizontal();
					GUILayout.Space(5);
					if (GUILayout.Button("","ol minus",  GUILayout.Width(15), GUILayout.Height(15)))
					{
						colorPalette.colors.Remove(colorPalette.colors[i]);
						break;
					}

					colorPalette.colors[i] = EditorGUILayout.ColorField(colorPalette.colors[i]);


					GUILayout.Space(5);
					if (GUILayout.Button("copy"))
					{
						EditorGUIUtility.systemCopyBuffer = "#" + ColorUtility.ToHtmlStringRGBA(colorPalette.colors[i]);
					}
					if (GUILayout.Button("paste"))
					{
						Color c;
						string colorName = EditorGUIUtility.systemCopyBuffer;
						if (colorName.StartsWith("#") == false)
							colorName = "#" + colorName;
						ColorUtility.TryParseHtmlString(colorName, out c);
						colorPalette.colors[i] = c;
						break;
					}

					EditorGUILayout.EndHorizontal();
				}
			GUILayout.Space(5);
			GUILayout.Box("", new GUILayoutOption[] { GUILayout.ExpandWidth(true), GUILayout.Height(1) });
			GUILayout.Space(2);
			EditorGUILayout.BeginHorizontal();

			GUILayout.Space(5);
			if (GUILayout.Button("", "ol plus", GUILayout.Width(20), GUILayout.Height(15)))
			{
				colorPalette.colors.Add(Color.white);
				
			}

			EditorGUILayout.EndHorizontal();
		}
	}
}
