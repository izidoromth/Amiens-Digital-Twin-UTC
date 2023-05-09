using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    bool rightButtonPressed = false;
    bool leftButtonPressed = false;
    float sensivity = 2.5f;
    float speed = 25.0f;
    Vector3 xzPlanTransformVector = new Vector3(1, 0, 1);
    float x = 0;
    void Update()
    {
        if (UIUtils.IsPointerOverUIElement())
            return;

        if (Input.GetMouseButtonDown(0))
        {
            leftButtonPressed = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            leftButtonPressed = false;
        }

        if (Input.GetMouseButtonDown(1))
        {
            rightButtonPressed = true;
        }
        else if (Input.GetMouseButtonUp(1))
        {
            rightButtonPressed = false;
        }

        if (leftButtonPressed)
        {
            float mouseY = Input.GetAxis("Mouse Y");
            float mouseX = Input.GetAxis("Mouse X");

            transform.eulerAngles += new Vector3(-mouseY * sensivity, mouseX * sensivity, 0);

            if (transform.eulerAngles.x > 90)
                transform.eulerAngles = new Vector3(0, transform.eulerAngles.y);
        }

        if (rightButtonPressed)
        {
            float mouseY = -Input.GetAxis("Mouse Y");
            float mouseX = -Input.GetAxis("Mouse X");

            transform.position += transform.right * mouseX * speed;
            transform.position += Vector3.Scale(transform.forward, xzPlanTransformVector) * mouseY * speed;
        }

        transform.position += transform.forward * Input.mouseScrollDelta.y * speed;
    }
}
