using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private float speed = 10;
	private void Awake()
	{
    }
	void Start()
    {
        Vector3 dir = GameManager.instance.player.transform.position - transform.position;
        transform.rotation = Quaternion.FromToRotation(Vector3.right, dir);
        Destroy(this.gameObject,5f);
    }
	private void FixedUpdate()
	{
        transform.position += transform.right * speed*Time.deltaTime;
	}
	private void OnTriggerEnter2D(Collider2D collision)
	{
        Debug.Log(collision.gameObject.name);
        Destroy(this.gameObject);
	}
}
