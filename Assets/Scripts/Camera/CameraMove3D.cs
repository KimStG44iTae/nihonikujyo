using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove3D : MonoBehaviour
{
    public float mSpeed;
    public float rotateSpeed;
    public float zoomSpeed;
    public Camera viewcamera;

    Vector3 originPos;
    Quaternion originRot;

    //3d 카메라 이동
    public void Awake()
    {
        originPos = transform.position;
        originRot = transform.rotation;
    }

    public void PositionReset()
    {
        transform.position = originPos;
        transform.rotation = originRot;
    }

    public void Update()
    {
        Zoom();
        Rotate();
        Move();
    }

    void Move()
    {
        Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        moveDirection = transform.TransformDirection(moveDirection);

        moveDirection *= Time.fixedDeltaTime;

        moveDirection *= mSpeed;

        moveDirection += transform.position;

        transform.position = moveDirection;

        if (Input.GetKey(KeyCode.Q))
        {
            transform.position -= Vector3.up * 0.01f * mSpeed;
        }

        if (Input.GetKey(KeyCode.E))
        {
            transform.position += Vector3.up * 0.01f * mSpeed;
        }

    }
    private void Zoom()
    {
        float distance = Input.GetAxis("Mouse ScrollWheel") * -1 * zoomSpeed;
        if (distance != 0)
        {
            viewcamera.fieldOfView += distance;
        }
    }
    private void Rotate()
    {
        if (Input.GetMouseButton(1))
        {
            transform.Rotate(0f, Input.GetAxis("Mouse X") * rotateSpeed, 0f, Space.World);
            transform.Rotate(-Input.GetAxis("Mouse Y") * rotateSpeed, 0f, 0f);
        }
    }
}
