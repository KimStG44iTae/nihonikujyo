using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LineDistance : MonoBehaviour
{
    Vector3 startPoint;
    Vector3 endPoint;

    public TMP_Text distanceText;

    public void Start()
    {
        SetDiatance();
    }

    //선 길이 반영
    public void SetDiatance()
    {
        GameObject subLine = gameObject;

        int startPointId = gameObject.transform.parent.GetComponent<LineInfo>().startId;
        int endPointId = gameObject.transform.parent.GetComponent<LineInfo>().endId;

        startPoint = ShareData.instance.jsonData.GetPosition(startPointId);
        endPoint = ShareData.instance.jsonData.GetPosition(endPointId);

        float distance = Vector3.Distance(startPoint, endPoint);

        Vector3 mainPos = new Vector3(distance / 2, 0, 0);

        subLine.GetComponent<LineRenderer>().SetPosition(0, -mainPos + new Vector3(0, 0, 0.25f));
        subLine.GetComponent<LineRenderer>().SetPosition(1, -mainPos + new Vector3(0, 0, 0.5f));
        subLine.GetComponent<LineRenderer>().SetPosition(2, -mainPos + new Vector3(0, 0, 0.4f));
        subLine.GetComponent<LineRenderer>().SetPosition(3, mainPos + new Vector3(0, 0, 0.4f));
        subLine.GetComponent<LineRenderer>().SetPosition(4, mainPos + new Vector3(0, 0, 0.5f));
        subLine.GetComponent<LineRenderer>().SetPosition(5, mainPos + new Vector3(0, 0, 0.25f));

        subLine.transform.position = new Vector3((startPoint.x + endPoint.x) / 2f,0.2f, (startPoint.z + endPoint.z) / 2f) + new Vector3(0, 0, 0.01f);

        float angle = Mathf.Atan2((startPoint.x - endPoint.x), (startPoint.z - endPoint.z)) * Mathf.Rad2Deg;

        subLine.transform.rotation = Quaternion.Euler(new Vector3(0, angle - 90f, 0));

        distanceText.text = distance.ToString("F2");
    }
}
