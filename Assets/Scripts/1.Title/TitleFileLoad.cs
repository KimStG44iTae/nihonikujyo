using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class TitleFileLoad : MonoBehaviour
{
    public string pubkey;

    private string url = "";

    UnityWebRequest www;

    public RectTransform projectPostion;
    public GameObject projectPrefeb;

    public List<GameObject> fileList;

    public GameObject mapClientButton;

    //폴더 리스트 화면에 표시
    public void StartFileListReceive()
    {
        pubkey = TitleSceneManager.instance.pubkey;
        url = TitleSceneManager.instance.url;

        StartCoroutine(ReceiveFileList());
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

            print(resultStr);

            if (resultStr == "empty")
            {
              //  MonitoringSceneManager.instance.ErrorMessageOn("파일리스트가 없습니다.");
                DeleteList();
            }
            else if (resultStr == "error")
            {
               // MonitoringSceneManager.instance.ErrorMessageOn("서버로부터 데이터를 불러오지 못 했습니다.");
            }
            else
            {
                Data data = JsonUtility.FromJson<Data>(resultStr);

                int topCount = -120;
                int leftCount = 510;

                if (data != null)
                {
                    DeleteList();

                    for (int i = 1; i < data.length+1; i++)
                    {
                        if (i % 5 == 0)
                        {
                            topCount -= 230;
                            leftCount = 180;
                        }
                        GameObject tempFile = Instantiate(projectPrefeb);
                        tempFile.transform.SetParent(projectPostion.transform);

                        tempFile.GetComponent<RectTransform>().anchoredPosition = new Vector3(leftCount, topCount, 0);
                        tempFile.GetComponent<RectTransform>().localScale = Vector3.one;

                        string filename = data.name[i-1];

                        filename = filename.Replace(".json", "");

                        tempFile.transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = filename;

                        tempFile.GetComponent<ProjectInfo>().projectName = filename;

                        fileList.Add(tempFile);
                        leftCount += 330;
                    }
                }
            }
        }
        else
        {
          //  MonitoringSceneManager.instance.ErrorMessageOn("서버 연결 에러");
        }

        www.Dispose();
    }

    public void DeleteList()
    {
        for (int i = fileList.Count - 1; i >= 0; i--)
        {
            Destroy(fileList[i]);
        }

        fileList.Clear();
    }

    public void StartDelete(string projectName)
    {
        pubkey = TitleSceneManager.instance.pubkey;

        StartCoroutine(DeleteData(projectName));
    }

    public IEnumerator DeleteData(string projectName)
    {
        string fileName = projectName;

        string sendUrl = url + "/FileUpload/DeleteMap/" + fileName;
        print(sendUrl);

        www = UnityWebRequest.Get(sendUrl);

        www.certificateHandler = new CertPublicKey { PUB_KEY = pubkey };

        www.timeout = 3;
     
        yield return www.SendWebRequest();

        print(www.result);

        byte[] result = www.downloadHandler.data;

        string resultStr = Encoding.UTF8.GetString(result);

        print(resultStr);

        www.Dispose();
        StartFileListReceive();
        mapClientButton.GetComponent<ButtonInteraction>().isActive = true;
    }


}
