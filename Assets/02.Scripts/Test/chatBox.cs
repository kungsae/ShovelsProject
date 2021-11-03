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

    private bool isTalking = false;
    private bool skipChat = false;
    private bool checkChat = false;
    public float time;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
    }
    public void Talk(string[] chat,Transform chatPoint)
    {
        if (!isTalking)
        {
            foreach (var item in chat)
            {
                chatQueue.Enqueue(item);
            }
            isTalking = true;
            StartCoroutine(outText(chatPoint));
        }
        else if (!skipChat)
        {
            skipChat = true;
        }
        else
        {
            checkChat = true;
        }
    }

    IEnumerator outText(Transform chatPoint)
    {
        yield return null;
		while (chatQueue.Count > 0)
		{
            time = 0;
            checkChat = false;
            skipChat = false;
            currentChat = chatQueue.Dequeue();
            StartCoroutine(Typing(currentChat));
			for (int i = 0; i <= currentChat.Length; i++)
			{
                chatBoxSize(chatPoint);
                if (skipChat)
                {
                    break;
                }
                yield return new WaitForSeconds(0.15f);
            }
            text.text = currentChat;
            chatBoxSize(chatPoint);
            yield return new WaitForSeconds(1f);
            while (!checkChat&& time<3)
			{
                yield return null;
            }
        }
        Destroy(gameObject);
    }
    IEnumerator Typing(string _text)
    {
		for (int i = 0; i <= _text.Length; i++)
		{
            text.text = _text.Substring(0, i);
			if (skipChat)
			{
				break;
			}
			yield return new WaitForSeconds(0.15f);
        }
		text.text = _text;
	}
    private void chatBoxSize(Transform chatPoint)
    {
        float x = text.preferredWidth;
        x = (x > 8) ? 8 : x + 0.7f;
        chatBoxObject.transform.localScale = new Vector3(x, text.preferredHeight + 0.3f);
        transform.position = new Vector2(chatPoint.position.x, chatPoint.position.y + text.preferredHeight / 2);
    }
}
