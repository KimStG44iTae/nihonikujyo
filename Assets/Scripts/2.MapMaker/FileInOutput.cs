using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
using System;

[Serializable]
public class Data
{
    public string[] name;
    public int length;
}

public class FileInOutput : MonoBehaviour
{
    private static string SavePath => Application.persistentDataPath + "/saves/";


    private string url = "http://192.168.0.100:5000";
    UnityWebRequest www;

    public RectTransform fileListContact;
    public GameObject fileNamePrefeb;

    public GameObject fileListPanel;

    public GameObject flieSavePanel;

    public TMP_InputField fileNameInput;

    public RawImage formImage;
    public string pubkey;

    public Data data;

    public List<GameObject> fileList;

    public void Start()
    {
        pubkey = NetworkManager.instance.pubkey;
        url = NetworkManager.instance.url;
    }

    //폴더 리스트 화면에 표시
    public void StartFileListReceive()
    {
        MapSceneManager.instance.uiManager.BackPanelChangeOn();
        MapSceneManager.instance.uiManager.OffSelect_All();
        MapSceneManager.instance.uiManager.FilePanelOff();

        pubkey = MapSceneManager.instance.pubkey;
        fileListPanel.SetActive(true);
        StartCoroutine(ReceiveFileList());
    }

    //파일 이름입력 시작
    public void StartFileSaveInput()
    {
        MapSceneManager.instance.uiManager.BackPanelChangeOn();

        flieSavePanel.SetActive(true);
    }

    //파일 이름입력 취소
    public void EndFileSaveInput()
    {
        MapSceneManager.instance.uiManager.BackPanelChangeOff();

        MapSceneManager.instance.uiManager.OffSelect_All();

        flieSavePanel.SetActive(false);
    }

    //파일 리스트 off
    public void ExitFileList()
    {
        fileListPanel.SetActive(false);
        MapSceneManager.instance.uiManager.BackPanelChangeOff();
    }

    //폴더 리스트 요청
    IEnumerator ReceiveFileList()
    {
        www = UnityWebRequest.Get(url + "/FileUpload/List");
        www.certificateHandler = new CertPublicKey { PUB_KEY = pubkey };

        yield return www.SendWebRequest();

        byte[] results = www.downloadHandler.data;

        string resultStr = Encoding.UTF8.GetString(results);

        if (www.result == UnityWebRequest.Result.Success)
        {
            if (resultStr == "empty")
            {
                MapSceneManager.instance.uiManager.ErrorMessageOn("파일리스트가 없습니다.");            
                DeleteList();
            }
            else if(resultStr == "error")
            {
                MapSceneManager.instance.uiManager.ErrorMessageOn("서버로부터 데이터를 불러오지 못 했습니다.");
            }
            else
            {
                DeleteList();

                data = JsonUtility.FromJson<Data>(resultStr);

                MapSceneManager.instance.fileLoadList.GetList(data);

                //int count = -50;

                //if (data != null)
                //{
                //    fileListContact.sizeDelta = new Vector2(fileListContact.sizeDelta.x, (100 * data.length));

                //    for (int i = 0; i < data.length; i++)
                //    {
                //        GameObject tempFile = Instantiate(fileNamePrefeb);
                //        tempFile.transform.SetParent(fileListContact.transform);

                //        tempFile.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, count, 0);
                //        tempFile.GetComponent<RectTransform>().sizeDelta = new Vector2(fileListContact.sizeDelta.x, 100);
                //        tempFile.GetComponent<RectTransform>().localScale = Vector3.one;

                //        string filename = data.name[i];

                //        filename = filename.Replace(".json", "");

                //        tempFile.transform.GetChild(0).GetComponent<TMP_Text>().text = filename;
                //        tempFile.transform.GetComponent<FileListInfo>().filename = filename;

                //        fileList.Add(tempFile);

                //        count -= 100;
                //    }
                //}
            }
        }
        else
        {
            MapSceneManager.instance.uiManager.ErrorMessageOn("서버 연결 에러");
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

    //데이터 전송
    public void StartSend()
    {
        //url = MapSceneManager.instance.url;
        //pubkey = MapSceneManager.instance.pubkey;

        string sendFileName = fileNameInput.text;
        StartCoroutine(SendData(sendFileName));
    }
    
    IEnumerator SendData(string sendFileName)
    {
        MapSceneManager.instance.shareData.jsonData.mapName = sendFileName;
        Texture2D rawImageTexture = new Texture2D(1,1);
        if (formImage.texture)
        {
            rawImageTexture = (Texture2D)formImage.texture;

        }

        byte[] sendData = rawImageTexture.EncodeToPNG();

        string sendUrl = url + "/FileUpload/SendData";


        ShareData.instance.jsonData.imagePos = formImage.transform.position;

        ModelData modelData = new ModelData();
        modelData.fileName = sendFileName;
        modelData.data = ShareData.instance.jsonData;
        modelData.imageData = sendData;

        string jsonString = JsonUtility.ToJson(modelData);

        byte[] jsonbyte = Encoding.UTF8.GetBytes(jsonString);

        www = UnityWebRequest.Put(sendUrl, jsonbyte);
        //www = UnityWebRequest.Put("https://192.168.0.45:3000/SendData", jsonbyte);
        www.method = "Post";
        www.SetRequestHeader("Content-Type", "application/json");
        www.SetRequestHeader("ModelName", sendFileName);

        www.certificateHandler = new CertPublicKey { PUB_KEY = pubkey };
        yield return www.SendWebRequest();
        byte[] results = www.downloadHandler.data;

        string resultStr = Encoding.UTF8.GetString(results);

        www.Dispose();
        EndFileSaveInput();
        MapSceneManager.instance.uiManager.InfomationUpdate();
    }

    //데이터 수신
    public void StartReceive(string filename)
    {
       // pubkey = MapSceneManager.instance.pubkey;

        StartCoroutine(ReceiveData(filename));
    }

    IEnumerator ReceiveData(string fileName)
    {
        string sendUrl = url + "/FileUpload/ReceiveData/" + fileName;

        print(sendUrl);
        UnityWebRequest www = UnityWebRequest.Get(sendUrl);

        www.certificateHandler = new CertPublicKey { PUB_KEY = pubkey };

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            byte[] results = www.downloadHandler.data;

            string resultStr = Encoding.UTF8.GetString(results);

            
            if (resultStr == "error")
            {
                MapSceneManager.instance.uiManager.ErrorMessageOn("서버로부터 데이터를 불러오지 못 했습니다.");
            }
            else
            {
                ModelData mData = JsonUtility.FromJson<ModelData>(resultStr);

                ShareData.instance.jsonData = mData.data;

                Texture2D texture2D = new Texture2D(2, 2);
                texture2D.LoadImage(mData.imageData);
                texture2D.Apply();

                formImage.texture = texture2D;

                MapSceneManager.instance.pointManager.PointUpdate();
                MapSceneManager.instance.lineManager.LineUpdate();
                MapSceneManager.instance.cctvManager.UpdateCCTV();
                MapSceneManager.instance.uwbManager.UpdateUWB();
                MapSceneManager.instance.zoneManager.UpdateZone();

                MapSceneManager.instance.markerManager.UpdateMarker();
                MapSceneManager.instance.boardManager.UpdateBoard();

                www.Dispose();

                MapSceneManager.instance.uiManager.SetImageSize(texture2D.width, texture2D.height);

                formImage.transform.localScale *= ShareData.instance.jsonData.ratio;
                formImage.transform.position = ShareData.instance.jsonData.imagePos;

                fileListPanel.SetActive(false);
                MapSceneManager.instance.uiManager.InfomationUpdate();
            }
        }
        else
        {
            MapSceneManager.instance.uiManager.ErrorMessageOn("서버 연결 에러");
        }

        yield return null;

    }

    public void StartDelete()
    {
        //pubkey = MapSceneManager.instance.pubkey;

        StartCoroutine(DeleteData());
    }

    public IEnumerator DeleteData()
    {
        string fileName = "HealthMain8";

        string sendUrl = url + "/FileUpload/DeleteMap/" + fileName;
        print(sendUrl);

        www = UnityWebRequest.Get(sendUrl);

        www.certificateHandler = new CertPublicKey { PUB_KEY = pubkey };

        yield return www.SendWebRequest();

        byte[] result = www.downloadHandler.data;

        string resultStr = Encoding.UTF8.GetString(result);

        print(resultStr);
    }
}

public class ModelData
{
    public string fileName;
    public JsonData data;
    public byte[] imageData;
}