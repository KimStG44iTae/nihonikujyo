using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//카메라 클릭하면 아웃라인 켜지는 기능
public class OutLineInfo : MonoBehaviour
{
    public List<GameObject> childObject;
    public bool isStart;

    public void Start()
    {
        if (MonitoringSceneManager.instance)
        {
            isStart = true;
            MonitoringSceneManager.instance.receiveMultiData.Add_CCTVList(gameObject);
        }
    }

    //아웃라인 키기
    public void OutLineOn()
    {
        if (!isStart)
        {
            if (MonitoringSceneManager.instance)
            {
                isStart = true;
                MonitoringSceneManager.instance.receiveMultiData.Add_CCTVList(gameObject);
            }
        }

        for (int i = 0; i < childObject.Count; i++)
        {
            childObject[i].GetComponent<Outline>().enabled = true;
        }
    }

    //아웃라인 끄기
    public void OutLineOff()
    {
        if (!isStart)
        {
            if (MonitoringSceneManager.instance)
            {
                isStart = true;
                MonitoringSceneManager.instance.receiveMultiData.Add_CCTVList(gameObject);
            }
        }

        for (int i = 0; i < childObject.Count; i++)
        {
            childObject[i].GetComponent<Outline>().enabled = false;
        }
    }
}
