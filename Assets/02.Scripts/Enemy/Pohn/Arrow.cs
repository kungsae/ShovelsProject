using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    SpriteRenderer sprite;
	private void Awake()
	{
        sprite = GetComponent<SpriteRenderer>();

    }
	void Start()
    {
        bool dir = GameManager.instance.player.transform.position.x - transform.position.x > 0 ? true : false;
        sprite.flipX = dir;
    }
	private void OnTriggerEnter2D(Collider2D collision)
	{
        Debug.Log(collision.gameObject.name);
        Destroy(this.gameObject);
	}

}
