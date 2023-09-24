using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Vector3 startHitPos;

    //2d 카메라 이동
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            startHitPos = Input.mousePosition;
            return;
        }
        else if (!Input.GetMouseButton(1))
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel") * 3f;
            if (transform.position.y - scroll < 2f)
            {
            }
            else
                transform.position += new Vector3(0, -scroll, 0);
            return; 
        }

        Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - startHitPos);
        Vector3 move = new Vector3(pos.x * (2 - transform.position.y) * 1.92f, 0, pos.y * (2 - transform.position.y) * 1.08f);
        startHitPos = Input.mousePosition;
        transform.position += move;
    }
}
