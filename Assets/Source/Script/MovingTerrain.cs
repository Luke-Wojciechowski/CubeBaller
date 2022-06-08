using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTerrain : MonoBehaviour
{
     
    private float _up = 8;
    private float _down = -8;
    private float speed = 0.02f;
    [SerializeField]
    private bool upDirection = true;
 
    private Vector3 _start;
    private void Awake()
    {
        _down += transform.position.y;
        _up += transform.position.y;
        _start = transform.position;
        QAgent.OnCrush += Reboot;
    }

    private void OnDestroy()
    {
        QAgent.OnCrush -= Reboot;
    }

    private void FixedUpdate()
    {
        if (upDirection)
        {
            transform.position += Vector3.up*speed;
            if (transform.position.y > _up)
            {
                upDirection = false;
            }
        }
        else
        {
            transform.position -= Vector3.up*speed;
            if (transform.position.y<_down)
            {
                upDirection = true;
            }
        }
            
    }

    private void Reboot()
    {
        transform.position = _start;
    }
}
