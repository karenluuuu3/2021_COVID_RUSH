using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandByCamera : MonoBehaviour
{

    // Just set rotation of the camera in each frame
    void Update()
    {
        transform.Rotate(Vector3.up, 5.0f * Time.deltaTime);
    }
}
