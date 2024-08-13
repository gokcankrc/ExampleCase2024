using UnityEngine;
using UnityEditor;
using Game.Board;

public class SetLevelEditor : EditorWindow
{
	private int _levelNumber = 1;

	[MenuItem("Tools/Set Level")]
	public static void ShowWindow()
	{
		GetWindow<SetLevelEditor>("Set Level");
	}

	private void OnGUI()
	{
		GUILayout.Label("Set Level", EditorStyles.boldLabel);

		_levelNumber = EditorGUILayout.IntField("Level Number", _levelNumber);

		if (GUILayout.Button("Set Level"))
		{
			SetLevel(_levelNumber);
		}
	}

	private void SetLevel(int levelNumber)
	{
		ProgressionManager.SetLevel(levelNumber);
		Debug.Log("Level set to " + levelNumber);
	}
}
