using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour {

    [Header("Camera Settings")]
    [Range(0, 50)]
    public int turnSpeed;
    [Range(0, 50)]
    public int moveSpeed;
    [Range(0, 50)]
    public int zoomSpeed;

    Vector3 oldMousePosition;

    private bool isMoving;
    private bool isRotating;
    private float zoomFactor;
    private bool mouseHeld;

    private int moveButton = 0;
    private int rotateButton = 1;
    //private int zoomButton = 2;

    // Update is called once per frame
    void Update () {
        //print("Old: " + oldMousePosition + " - New: " + Input.mousePosition);

        isMoving = Input.GetMouseButton(moveButton);
        isRotating = Input.GetMouseButton(rotateButton);
        zoomFactor = Input.GetAxis("Mouse ScrollWheel");

        Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - oldMousePosition);

        if (isMoving)
        {
            Vector3 move = new Vector3(-pos.x * moveSpeed, -pos.y * moveSpeed, 0);
            transform.Translate(move, Space.Self);
        }

        if (isRotating)
        {
            transform.RotateAround(transform.position, transform.right, -pos.y * turnSpeed);
            transform.RotateAround(transform.position, Vector3.up, pos.x * turnSpeed);
        }

        if(zoomFactor != 0)
        {
            Vector3 move = transform.forward * zoomSpeed * zoomFactor;
            transform.Translate(move, Space.World);
        }

        oldMousePosition = Input.mousePosition;
    }
}
