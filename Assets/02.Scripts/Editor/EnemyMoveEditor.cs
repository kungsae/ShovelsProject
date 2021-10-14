using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnemyMove))]
public class EnemyMoveEditor : Editor
{
	private void OnSceneGUI()
	{
		EnemyMove enemy = (EnemyMove)target;

		for (int i = 0; i < enemy.patrollPoint.Length; i++)
		{
			Handles.color = Color.red;
			Handles.DrawWireCube(enemy.patrollPoint[i], new Vector3(1, 1));
		}

	}
}
