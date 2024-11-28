using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;

public class CameraAdjust : MonoBehaviour
{
    private CameraFollow _camera; // Camera to adjust
    // New min and max values for x and y
    public float yMax; 
    public float yMin;
    public float xMax;
    public float xMin;

    private void Start()
    {
        _camera = GameObject.Find("Main Camera").GetComponent<CameraFollow>(); // Camera script
    }

    // Called when a 2D collider enters
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) // Checks if collision was player
        {
            // Sets the x and y max and min values in the camera script to the nex x and y max and min values
            _camera.YMaxValue = yMax;
            _camera.YMinValue = yMin;
            _camera.XMaxValue = xMax;
            _camera.XMinValue = xMin;
        }
    }
}