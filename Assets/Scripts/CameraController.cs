using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float sensitivity = 100f;
    public float xClamp = 80f; // Wie weit hoch/runter
    public float yClamp = 60f; // Wie weit links/rechts (Begrenzung!)

    float xRotation = 0f;
    float yRotation = 0f;

    void Update()
    {
        if (GameManager.Instance.CurrentState != GameState.FreeLook) return;

        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -xClamp, xClamp);

        yRotation += mouseX;
        yRotation = Mathf.Clamp(yRotation, -yClamp, yClamp);

        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }
}