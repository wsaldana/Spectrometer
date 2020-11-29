using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSpeed : MonoBehaviour
{

    private float speed = 0.000001f;
    // Update is called once per frame
    void Update()
    {
        Time.timeScale = speed;
    }

    public void AdjustSpeed(float newSpeed)
    {
        speed = newSpeed;
    }
}
