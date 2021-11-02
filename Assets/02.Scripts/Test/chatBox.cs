using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class chatBox : MonoBehaviour
{
    public Queue<string> chatQueue = new Queue<string>();
    public string currentChat;
    public TextMeshPro text;
    public GameObject chatBoxObject; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Talk(string[] chat,Transform chatPoint)
    {
		foreach (var item in chat)
		{
            chatQueue.Enqueue(item);
        }
        StartCoroutine(outText(chatPoint));
    }

    IEnumerator outText(Transform chatPoint)
    {
        yield return null;
		while (chatQueue.Count > 0)
		{
            currentChat = chatQueue.Dequeue();
            StartCoroutine(Typing(currentChat));
			for (int i = 0; i <= currentChat.Length; i++)
			{
                float x = text.preferredWidth;
                x = (x > 8) ? 8 : x + 0.7f;
                chatBoxObject.transform.localScale = new Vector3(x, text.preferredHeight + 0.3f);
                transform.position = new Vector2(chatPoint.position.x, chatPoint.position.y + text.preferredHeight / 2);
                yield return new WaitForSeconds(0.15f);
            }
            yield return new WaitForSeconds(2f);
            //text.text = currentChat;

        }
        Destroy(gameObject);
    }
    IEnumerator Typing(string _text)
    {
		for (int i = 0; i <= _text.Length; i++)
		{
            text.text = _text.Substring(0, i);
            yield return new WaitForSeconds(0.15f);

        }
    }
}
