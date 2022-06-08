using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    
    public float GameSpeed = 1;
    void Update()
    {
        Time.timeScale = GameSpeed;
    }
}
