using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnemyControl))]
public class EnemyMoveEditor : Editor
{
	private void OnSceneGUI()
	{
		EnemyControl enemy = (EnemyControl)target;

		if (enemy.patrollPoint.Length > 0)
		{
			for (int i = 0; i < enemy.patrollPoint.Length; i++)
			{
				Handles.color = Color.red;
				Handles.DrawWireCube(enemy.patrollPoint[i], new Vector3(1, 1));
			}
		}
	}
}
