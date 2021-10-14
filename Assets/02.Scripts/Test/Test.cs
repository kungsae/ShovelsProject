using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("1"))
        {
            Time.timeScale = 1f;
            Debug.Log("A");
        }
        if (Input.GetKeyDown("2"))
        {
            Time.timeScale = 0.1f;
        }
    }
}
