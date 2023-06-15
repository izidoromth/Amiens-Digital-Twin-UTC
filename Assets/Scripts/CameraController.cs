using Assets.Scripts;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    bool rightButtonPressed { get => Input.GetMouseButton(1); }
    bool leftButtonPressed { get => Input.GetMouseButton(0); }
    bool scrollPressed { get => Input.GetMouseButton(2); }
    float sensivity = 2.5f;
    float speed_multiplier = 0.1f;
    Vector3 xzPlanTransformVector = new Vector3(1, 0, 1);
    float x = 0;
    void Update()
    {
        if (UIUtils.IsPointerOverUIElement())
            return;

        if (leftButtonPressed)
        {
            float mouseY = -Input.GetAxis("Mouse Y");
            float mouseX = -Input.GetAxis("Mouse X");

            transform.position += transform.right * mouseX * transform.position.y * speed_multiplier;
            transform.position += Vector3.Scale(transform.forward, xzPlanTransformVector) * mouseY * transform.position.y * speed_multiplier;
        }

        if (rightButtonPressed)
        {
            float mouseY = Input.GetAxis("Mouse Y");
            float mouseX = Input.GetAxis("Mouse X");

            transform.eulerAngles += new Vector3(-mouseY * sensivity, mouseX * sensivity, 0);

            if (transform.eulerAngles.x > 90)
                transform.eulerAngles = new Vector3(0, transform.eulerAngles.y);
        }

        if (scrollPressed)
        {
            float mouseY = -Input.GetAxis("Mouse Y");
            float mouseX = -Input.GetAxis("Mouse X");

            transform.position += transform.up * mouseY * transform.position.y * speed_multiplier;
            transform.position += transform.right * mouseX * transform.position.y * speed_multiplier;
        }

        transform.position += transform.forward * Input.mouseScrollDelta.y * transform.position.y * speed_multiplier;
    }
}
