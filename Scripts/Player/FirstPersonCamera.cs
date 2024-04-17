using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Camera mainCamera, Camera;

    [Header("Camera Sensitivity and Field of View")]
    public float mouseSensitivity = 2f;
    public float playerFieldOfView = 75f;
    float cameraVerticalRotation = 0f;


    // Start is called before the first frame update
    void Start()
    {
        //Lock and hide the cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        //Collect Mouse Input
        float inputX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float inputY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        //Rotate the camera around its local X-axis
        cameraVerticalRotation -= inputY;
        cameraVerticalRotation = Mathf.Clamp(cameraVerticalRotation,-90f, 90f);
        transform.localEulerAngles = Vector3.right * cameraVerticalRotation;

        //Rotate the player object and the camera around its Y-axis
        player.Rotate(Vector3.up * inputX);

        //Dynamic FOV
        DynamicFOV();
        mainCamera.fieldOfView = Camera.fieldOfView;
    }

    void DynamicFOV()
    {
        if((Input.GetButton("Horizontal") || Input.GetButton("Vertical")) && Input.GetButton("Fire3"))
            Camera.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, playerFieldOfView + 15f, 10f * Time.deltaTime);
        else
            Camera.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, playerFieldOfView, 10f * Time.deltaTime);
    }
}
