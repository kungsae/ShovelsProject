using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public string xAxisName = "Horizontal";
    public string jumpKeyName = "Jump";
    public float xMove { get; private set; }
    public bool jump { get; private set; }
    public bool attack { get; private set; }
    public bool parrying { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        xMove = Input.GetAxisRaw(xAxisName);
        jump = Input.GetKey(KeyCode.C);
        attack = Input.GetKeyDown(KeyCode.Z);
        parrying = Input.GetKeyDown(KeyCode.X);
    }
}
