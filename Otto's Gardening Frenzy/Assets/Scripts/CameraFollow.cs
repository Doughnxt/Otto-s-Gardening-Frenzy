using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraFollow : MonoBehaviour
{
    public Transform target; // Target's transform component that the camera will follow

    Vector3 velocity = Vector3.zero; // Velocity used to help smooth camera movement
    public float smoothTime = .15f; // Easing value

    // Minimum and Maximum x and y values for the camera
    public float YMaxValue = 0; 
    public float YMinValue = 0;
    public float XMaxValue = 0;
    public float XMinValue = 0;

    // Fixed Update is called every fixed framerarte frame
    void FixedUpdate()
    {
        Vector3 targetPos = target.position; // Target position
        targetPos.y = Mathf.Clamp(target.position.y, YMinValue, YMaxValue); // Clamps the positions able to be moved in the y axis to between the y min and max values
        targetPos.x = Mathf.Clamp(target.position.x, XMinValue, XMaxValue); // Clamps the positions able to be moved in the x axis to between the x min and max values
        targetPos.z = transform.position.z; // z position
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime); // Moves the camera smoothly to the target's position
    }
}
