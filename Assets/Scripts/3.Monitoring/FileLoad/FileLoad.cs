using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class FileLoad : MonoBehaviour
{
    public string pubkey;

    private string url = "";

    UnityWebRequest www;

    public GameObject fileListPanel;

    public GameObject GymSet;

    public ButtonInteraction buttonInteraction;

    //폴더 리스트 화면에 표시
    public void StartFileListReceive()
    {
        pubkey = MonitoringSceneManager.instance.pubkey;
        url = MonitoringSceneManager.instance.url;
        fileListPanel.SetActive(true);
        StartCoroutine(ReceiveFileList());
        MonitoringSceneManager.instance.backGround.SetActive(true);
    }

    //파일 리스트 off
    public void ExitFileList()
    {
        MonitoringSceneManager.instance.backGround.SetActive(false);
        fileListPanel.SetActive(false);
        buttonInteraction.OffSelect();

    }

    //폴더 리스트 요청
    IEnumerator ReceiveFileList()
    {
        www = UnityWebRequest.Get(url+ "/FileUpload/List");

        www.certificateHandler = new CertPublicKey { PUB_KEY = pubkey };

        www.timeout = 3;

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            byte[] results = www.downloadHandler.data;

            string resultStr = Encoding.UTF8.GetString(results);

            if (resultStr == "empty")
            {
                MonitoringSceneManager.instance.ErrorMessageOn("파일리스트가 없습니다.");
            }
            else if (resultStr == "error")
            {
                MonitoringSceneManager.instance.ErrorMessageOn("서버로부터 데이터를 불러오지 못 했습니다.");
            }
            else
            {
                Data data = JsonUtility.FromJson<Data>(resultStr);

                if (data != null)
                {
                    fileListPanel.GetComponent<FileLoadList>().GetList(data);
                }
            }
        }
        else
        {
            MonitoringSceneManager.instance.ErrorMessageOn("서버 연결 에러");
        }

        www.Dispose();
    }

    //모델 데이터 수신 시작
    public void StartReceive(string filename)
    {
        if (MonitoringSceneManager.instance.model != null)
        {
            Destroy(MonitoringSceneManager.instance.model);
            MonitoringSceneManager.instance.model = null;
        }

        StartCoroutine(ReceiveData(filename));

    }

    //모델 데이터 수신 시작
    IEnumerator ReceiveData(string filename)
    {
        string sendUrl = url + "/FileUpload/ReceiveData/" + filename;

        www = UnityWebRequest.Get(sendUrl);
        www.certificateHandler = new CertPublicKey { PUB_KEY = pubkey };

      //  print(sendUrl);

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            byte[] results = www.downloadHandler.data;

            string resultStr = Encoding.UTF8.GetString(results);

            if (resultStr == "error")
            {
                MonitoringSceneManager.instance.ErrorMessageOn("서버로부터 데이터를 불러오지 못 했습니다.");
            }
            else
            {
                ModelData mData = JsonUtility.FromJson<ModelData>(resultStr);

                ShareData.instance.jsonData = mData.data;

                GetComponent<MakeModel>().StartMakeModel();

                GetComponent<MakeModel>().Model.transform.position = Vector3.zero;
                MonitoringSceneManager.instance.model = GetComponent<MakeModel>().Model;

                //GymSet.SetActive(true);

                ExitFileList();
            }
        }
        else
        {
            MonitoringSceneManager.instance.ErrorMessageOn("서버 연결 에러");
        }

        www.Dispose();
    }
}
