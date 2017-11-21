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
    [Range(0, 50)]
    public int maxDistance;

    public bool isEnabled;

    Vector3 oldMousePosition;

    private bool isMoving;
    private bool isRotating;
    private float isZooming;
    private bool mouseHeld;

    private int moveButton = 0;
    private int rotateButton = 1;
    //private int zoomButton = 2;

    // Update is called once per frame
    void Update () {
        //print("Old: " + oldMousePosition + " - New: " + Input.mousePosition);

        if (!isEnabled)
            return;

        isMoving = Input.GetMouseButton(moveButton);
        isRotating = Input.GetMouseButton(rotateButton);
        isZooming = Input.GetAxis("Mouse ScrollWheel");

        Vector3 mousePosition = Camera.main.ScreenToViewportPoint(Input.mousePosition - oldMousePosition);

        if (isMoving)
        {
            float x = -mousePosition.x * moveSpeed;
            float y = -mousePosition.y * moveSpeed;

            Vector3 move = new Vector3(x, y, 0);
            transform.Translate(move, Space.Self);

            Clamp();
        }

        if (isRotating)
        {
            transform.RotateAround(transform.position, transform.right, -mousePosition.y * turnSpeed);
            transform.RotateAround(transform.position, Vector3.up, mousePosition.x * turnSpeed);

            Clamp();
        }

        if(isZooming != 0)
        {
            Vector3 move = transform.forward * zoomSpeed * isZooming;
            transform.Translate(move, Space.World);

            Clamp();
        }

        oldMousePosition = Input.mousePosition;
    }

    private void Clamp()
    {
        float x = Mathf.Clamp(transform.position.x, -maxDistance, GameData.Instance.currentWidth + maxDistance);
        float y = Mathf.Clamp(transform.position.y, 0, Mathf.Max(GameData.Instance.currentWidth, GameData.Instance.currentHeight) + 10);
        float z = Mathf.Clamp(transform.position.z, -maxDistance, GameData.Instance.currentHeight + maxDistance);
        transform.position = new Vector3(x, y, z);

        x = transform.rotation.eulerAngles.x;
        if (x > 225)
            x = 0;
        else if (x > 85)
            x = 85;
        transform.rotation = Quaternion.Euler(x, transform.rotation.eulerAngles.y, 0/*transform.rotation.eulerAngles.z*/);
    }
}
