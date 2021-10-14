using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
	private Animator animator;
	private Rigidbody2D rigid;
	private PlayerMove playerMove;

	// Start is called before the first frame update
	private void Awake()
	{
		rigid = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		playerMove = GetComponent<PlayerMove>();
	}

	// Update is called once per frame
	void Update()
    {
        animator.SetBool("isGround", playerMove.isGround);
        animator.SetFloat("velocity", rigid.velocity.y);
    }
}
