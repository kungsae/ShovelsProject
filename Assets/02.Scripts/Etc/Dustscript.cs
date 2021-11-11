using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dustscript : MonoBehaviour
{
    void Start()
    {
        Invoke("destroy", 2f);
    }

    private void destroy()
    {
        Destroy(gameObject);
    }
}   
