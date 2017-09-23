//-------------------------------------------
// (c) 2017 - Jonas Reich
//-------------------------------------------

using UnityEditor;
using UnityEngine;

namespace Doodads.Editor
{
	/// <summary>
	/// Controll the time scale of the game from inside the editor
	/// </summary>
	public static class TimeScaleController
	{
		static GUIStyle style;

		static bool initialized;
		static void Init()
		{
			style = new GUIStyle {normal = {textColor = Color.yellow}};
			initialized = true;
		}


		[MenuItem("Tools/Time Scale/x0.5 (Slow Motion) &y")]
		public static void SlowMotion() { SetTimeScale(0.5f); }

		[MenuItem("Tools/Time Scale/x1 (Default) &x")]

		public static void Normal() { SetTimeScale(1f); }

		[MenuItem("Tools/Time Scale/x2 (Fast forward) &c")]
		public static void FastForward() { SetTimeScale(2f); }


		static void SetTimeScale(float timeScale)
		{
			if (!initialized) Init();

			Time.timeScale = timeScale;
			Time.fixedDeltaTime = 0.02f * timeScale;

			SceneView.onSceneGUIDelegate -= OnScene;
			if (timeScale != 1.0f)
				SceneView.onSceneGUIDelegate += OnScene;
		}

		static void OnScene(SceneView sceneview)
		{
			Handles.BeginGUI();
			GUILayout.BeginArea(new Rect(20, 80, 150, 120));
			GUILayout.BeginHorizontal();
			GUILayout.Label("Time scale: x" + Time.timeScale.ToString(), style);
			GUILayout.EndHorizontal();
			GUILayout.EndArea();
			Handles.EndGUI();
		}
	}
}
