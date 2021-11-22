using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class PlayerAttributes
{
    [SerializeField]
    private float _walkSpeed = 8.0f;
    [SerializeField]
    private float _jumpForce = 18.0f;

    public float Speed
    {
        get { return _walkSpeed; }
        set { _walkSpeed = value; }
    }

    public float Jump
    {
        get { return _jumpForce; }
        set { _jumpForce = value; }
    }
}
