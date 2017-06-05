//-------------------------------------------
// Copyright (c) 2017 - JonasReich
//-------------------------------------------

using UnityEditor;
using UnityEngine;

namespace Doodads.Editor
{
	[CustomPropertyDrawer(typeof(SceneAttribute))]
	public class SceneDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			if (property.propertyType == SerializedPropertyType.String)
			{
				var sceneObject = GetSceneObject(property.stringValue);
				var scene = EditorGUI.ObjectField(position, label, sceneObject, typeof(SceneAsset), true);
				if (scene == null)
				{
					property.stringValue = "";
				}
				else if (scene.name != property.stringValue)
				{
					var sceneObj = GetSceneObject(scene.name);
					if (sceneObj == null)
					{
						Debug.LogWarning("Scene [" + scene.name + "] cannot be used. Add it to the 'Scenes in Build' list in build settings.");
					}
					else
					{
						property.stringValue = scene.name;
					}
				}
			}
			else
				EditorGUI.LabelField(position, label.text, "Use [Scene] with strings.");
		}

		protected SceneAsset GetSceneObject(string sceneObjectName)
		{
			if (string.IsNullOrEmpty(sceneObjectName))
			{
				return null;
			}

			foreach (var editorScene in EditorBuildSettings.scenes)
			{
				if (editorScene.path.IndexOf(sceneObjectName) != -1)
				{
					return AssetDatabase.LoadAssetAtPath(editorScene.path, typeof(SceneAsset)) as SceneAsset;
				}
			}
			Debug.LogWarning("Scene [" + sceneObjectName + "] cannot be used. Add it to the 'Scenes in Build' list in build settings.");
			return null;
		}
	}
}
