using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonitorFileListInfo : MonoBehaviour
{
    public string filename;

    //Ŭ���ϸ� ���� �̸� ���� - ���� �� �� ������ �޾ƿ��� �޼��� ȣ��
    public void SendFileName()
    {
        MonitoringSceneManager.instance.fileLoad.StartReceive(filename);
    }
}
