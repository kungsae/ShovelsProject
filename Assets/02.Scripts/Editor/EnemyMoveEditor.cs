using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnemyControl),true)]
public class EnemyMoveEditor : Editor
{
	private void OnSceneGUI()
	{
		EnemyControl enemy = (EnemyControl)target;
		Handles.color = Color.red;
		if (enemy.patrollPoint.Length > 0)
		{
			for (int i = 0; i < enemy.patrollPoint.Length; i++)
			{
				Handles.DrawWireCube(enemy.patrollPoint[i], new Vector3(1, 1));
			}
		}
	}
}
