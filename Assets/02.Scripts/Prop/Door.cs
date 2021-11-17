using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private Vector3 closePos;
	private void Awake()
	{
        closePos = transform.position;

    }
	public IEnumerator CloseDoor()
    {
        float time = 0;

		while (true)
		{
            time += Time.deltaTime;
            transform.position = Vector2.Lerp(transform.position, closePos, time);
            if (time >= 1)
            {
                transform.position = closePos;
                break;
            }
            yield return null;
		}
    }
}
