using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ī�޶� Ŭ���ϸ� �ƿ����� ������ ���
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

    //�ƿ����� Ű��
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

    //�ƿ����� ����
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
