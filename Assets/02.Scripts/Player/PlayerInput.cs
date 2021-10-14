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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        xMove = Input.GetAxisRaw(xAxisName);
        jump = Input.GetKeyDown(KeyCode.C);
        attack = Input.GetKeyDown(KeyCode.Z);
    }
}
