//-------------------------------------------
// Copyright (c) 2017 - JonasReich
//-------------------------------------------

using UnityEditor;
using UnityEngine;

namespace Doodads.Editor
{
	[CustomPropertyDrawer(typeof(FloatPair))]
	public class FloatPairDrawer : PairDrawer<float> { }

	[CustomPropertyDrawer(typeof(IntPair))]
	public class IntPairDrawer : PairDrawer<int> { }

	[CustomPropertyDrawer(typeof(StringPair))]
	public class StringPairDrawer : PairDrawer<string> { }

	[CustomPropertyDrawer(typeof(Pair<GameObject>))]
	public class GameObjectPairDrawer : PairDrawer<GameObject> { }

	public class PairDrawer<T> : PairDrawer<T, T> { }

	public class PairDrawer<T, U> : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(position, label, property);
			position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
			var indent = EditorGUI.indentLevel;
			EditorGUI.indentLevel = 0;

			var firstRect = new Rect(position.x, position.y, (position.width / 2) - 5, position.height);
			EditorGUI.PropertyField(firstRect, property.FindPropertyRelative("first"), GUIContent.none);

			var secondRect = new Rect(position.x + (position.width / 2) + 5, position.y, (position.width / 2) - 5, position.height);
			EditorGUI.PropertyField(secondRect, property.FindPropertyRelative("second"), GUIContent.none);

			EditorGUI.indentLevel = indent;
			EditorGUI.EndProperty();
		}
	}
}
