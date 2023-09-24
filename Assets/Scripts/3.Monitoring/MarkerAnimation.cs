using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerAnimation : MonoBehaviour
{
    public float maxHeight = 1.7f;
    public float minHeight = 1.5f;

    public float count = 0;

    public Vector3 startPos;
    public Vector3 endPos;

    public void OnEnable()
    {
        startPos = Vector3.up * minHeight;
        endPos = Vector3.up * maxHeight;

        transform.localPosition = startPos;
    }


    private void Update()
    {
        if (transform.localPosition.y >= maxHeight)
        {
            endPos = Vector3.up * minHeight;
            startPos = Vector3.up * maxHeight;
            count = 0;
        }
        else if (transform.localPosition.y <= minHeight)
        {

            startPos = Vector3.up * minHeight;
            endPos = Vector3.up * maxHeight;
            count = 0;
        }

        count += 1f * Time.deltaTime;

        transform.localPosition = Vector3.Slerp(startPos, endPos, count);
    }
}
