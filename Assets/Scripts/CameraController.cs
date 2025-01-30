using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] float rotationSpeedX;
    [SerializeField] float rotationSpeedY;

    public GameObject cameraPivot;
    public Transform characterParentTransform;
    //public Transform aimY;

    void Update()
    {
        float rotationX = Input.GetAxis("Mouse X") * rotationSpeedX;
        rotationX *= Time.deltaTime;
        characterParentTransform.transform.Rotate(0, rotationX, 0);

        float rotationY = Input.GetAxis("Mouse Y") * rotationSpeedY;
        rotationY *= Time.deltaTime;
        cameraPivot.transform.Rotate(rotationY, 0, 0);
    }
}
