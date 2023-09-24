using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonitorFileListInfo : MonoBehaviour
{
    public string filename;

    //클릭하면 파일 이름 전송 - 전송 후 모델 데이터 받아오는 메서드 호출
    public void SendFileName()
    {
        MonitoringSceneManager.instance.fileLoad.StartReceive(filename);
    }
}
