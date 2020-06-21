using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotator : MonoBehaviour
{
    private float speed;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, speed * Time.deltaTime, 0);

        if (Input.GetKeyDown("a"))
        {
            speed = 50;
        }

        if (Input.GetKeyDown("d"))
        {
            speed = -50;
        }

        if (Input.GetKeyUp("a") || (Input.GetKeyUp("d")))
        {
            speed = 0;
        }

    }
}
