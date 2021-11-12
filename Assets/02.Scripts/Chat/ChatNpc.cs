using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatNpc : MonoBehaviour
{
    public string[] message;
    public Transform charTr;
    public GameObject chatBox;

    public GameObject nowChatBox;

    private GameObject player = null;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X)&& player == GameManager.instance.player)
        {
            Debug.Log("B");
            Talk();
        }
    }
	private void OnTriggerEnter2D(Collider2D collision)
	{
        player = collision.gameObject;

    }
	private void OnTriggerExit2D(Collider2D collision)
	{
        player = null;

    }
	public void Talk()
    {
        if(nowChatBox==null)
        nowChatBox = Instantiate(chatBox);

        nowChatBox.GetComponent<chatBox>().Talk(message, charTr);
    }
}
