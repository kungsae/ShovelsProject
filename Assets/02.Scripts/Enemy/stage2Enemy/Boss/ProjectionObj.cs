using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectionObj : MonoBehaviour
{
    private float speed = 20;
    private void Awake()
    {

    }
    void Start()
    {
        Destroy(this.gameObject, 5f);
    }
    private void FixedUpdate()
    {
        transform.position += transform.right * speed * Time.deltaTime;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.name);
        Destroy(this.gameObject);
    }
}
