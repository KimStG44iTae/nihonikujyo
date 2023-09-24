using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerInfo : MonoBehaviour
{
    public int id;

    public GameObject yObject;
    public GameObject zObject;

    public JsonMarker jsonMarker;

    public void SetLocalRotation()
    {
        Vector3 tempY = transform.position + Vector3.up / 25f;

        tempY.x = (float)Math.Round(tempY.x, 3);
        tempY.y = (float)Math.Round(tempY.y, 3);
        tempY.z = (float)Math.Round(tempY.z, 3);

        yObject.transform.position = tempY;

        Vector3 tempZ = transform.position + Vector3.forward;

        tempZ.x = (float)Math.Round(tempZ.x, 3);
        tempZ.y = (float)Math.Round(tempZ.y, 3);
        tempZ.z = (float)Math.Round(tempZ.z, 3);

        zObject.transform.position = tempZ;

        jsonMarker.yAxis = yObject.transform.localPosition;
        jsonMarker.zAxis = zObject.transform.localPosition;
    }

    public void SetOriginPos()
    {
        Vector3 origin = Vector3.zero - jsonMarker.position;
        origin.y = 0;

        jsonMarker.originPos = origin;
    }
}
