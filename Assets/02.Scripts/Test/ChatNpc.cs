using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatNpc : MonoBehaviour
{
    public string[] message;
    public Transform charTr;
    public GameObject chatBox;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            print("A");
            Talk();

        }
    }
    public void Talk()
    {
        GameObject chat = Instantiate(chatBox);
        chat.GetComponent<chatBox>().Talk(message, charTr);
    }
}
