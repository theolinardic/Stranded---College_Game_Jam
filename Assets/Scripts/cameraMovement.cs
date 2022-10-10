using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraMovement : MonoBehaviour
{

    public float mouseSens = 100f;

    public Transform playerBody;

    float xRotation = 0f;

    public static bool workbenchMode;
    public bool canMoveCamera;
    bool shouldReset;
    Quaternion newRot;
    // Start is called before the first frame update
    void Start()
    {
        shouldReset = false;
        canMoveCamera = true;
        workbenchMode = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
     //   Debug.Log(canMoveCamera);
        if(canMoveCamera == true)
        {
            if (workbenchMode == false)
            {
                //time.deltatime is used to make sure movement speed doesn't change with framerate
                float mouseX = Input.GetAxis("Mouse X") * mouseSens * Time.deltaTime;
                float mouseY = Input.GetAxis("Mouse Y") * mouseSens * Time.deltaTime;
                if(shouldReset == true)
                {
                    Camera.main.transform.rotation = newRot;
                    shouldReset = false;
                }

                xRotation -= mouseY;
                //clamp locks value between -90 and 90 so camera wont flip
                xRotation = Mathf.Clamp(xRotation, -90f, 90f);
                transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
                playerBody.Rotate(Vector3.up * mouseX);



                RaycastHit hit;
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                // Debug.Log(ray3);
                if (Physics.Raycast(ray, out hit, 10f))
                {

                    if (hit.transform.tag == "hasDescription")
                    {
                        hit.transform.GetComponent<description>().showDescription();
                    }

                }

            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
            }
        }

    }

    public void setRotation(Quaternion rotation)
    {
        newRot = rotation;
        canMoveCamera = true;
    }

}
