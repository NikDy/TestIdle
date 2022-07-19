using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InputController : MonoBehaviour
{
    // Start is called before the first frame update

    public delegate void PlayerInputVector(Vector2 direction);
    public event PlayerInputVector OnInput;

    [SerializeField]private DynamicJoystick joystick;

        
    // Update is called once per frame
    void FixedUpdate()
    {
        OnInput(joystick.Direction);
    }
}
