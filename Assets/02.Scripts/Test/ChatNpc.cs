using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatNpc : MonoBehaviour
{
    public string[] message;
    public Transform charTr;
    public GameObject chatBox;

    public GameObject nowChatBox;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log("B");
            Talk();
        }
    }
	public void Talk()
    {
        if(nowChatBox==null)
        nowChatBox = Instantiate(chatBox);

        nowChatBox.GetComponent<chatBox>().Talk(message, charTr);
    }
}
