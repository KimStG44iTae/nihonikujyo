using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;


public class SearchWatchID : MonoBehaviour
{
    //워치 id 리스트 판넬
    public GameObject watchIDPanel;

    //워치 id 생성 프리펍
    public GameObject watchIDPrefeb;
    public GameObject watchIDPosition;

    public List<GameObject> tagIDObjectList;

    //string url = "http://192.168.0.10:5000";
    string url = "";
    public string pubkey;

    WatchList watchList;

    //워치 id field
    public TMP_InputField register_watchIDInputfield;
    public TMP_InputField info_watchIDInputfield;

    //워치 id리스트 수신
    public void ReceiveWatchID()
    {
        url = OperationSceneManager.instance.url;
        pubkey = OperationSceneManager.instance.pubkey;
        watchIDPanel.SetActive(true);
        StartCoroutine(ReceiveData());
    }

    public void PanelOff()
    {
        watchIDPanel.SetActive(false);
    }

    //워치 id 클릭 후 field에 값 세팅
    public void SetWatchID(string watchid)
    {
        if (OperationSceneManager.instance.operationUIManager.nowStatus == OperationUIManager.Status.Register)
        {
            register_watchIDInputfield.text = watchid;
        }
        else
        {
            info_watchIDInputfield.text = watchid;
        }

        watchIDPanel.SetActive(false);
    }

    //워치 id리스트 수신 후 값 세팅
    public IEnumerator ReceiveData()
    {
        string reciveURL = url + "/Management/searchWatchList";

        UnityWebRequest www = UnityWebRequest.Get(reciveURL);
        www.certificateHandler = new CertPublicKey { PUB_KEY = pubkey };
        www.timeout = 2;

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            byte[] result = www.downloadHandler.data;

            string resultStr = Encoding.UTF8.GetString(result);

            if (resultStr == "empty")
            {
                OperationSceneManager.instance.operationUIManager.ErrorMessgaeOn("사용할 수 있는 워치가 없습니다.");
                DestroyWatchList();
            }
            else
            {
                watchList = JsonUtility.FromJson<WatchList>(resultStr);
                UpdateSearchWatchList();

                //if (watchList != null)
                //{
                //    UpdateSearchWatchList();
                //}
            }

        }
        else
        {
            OperationSceneManager.instance.operationUIManager.ErrorMessgaeOn("서버 연결 에러");
        }

        www.Dispose();
    }

    public void DestroyWatchList()
    {
        for (int i = tagIDObjectList.Count - 1; i >= 0; i--)
        {
            Destroy(tagIDObjectList[i]);
        }

        tagIDObjectList.Clear();
    }

    //워치 id 리스트 생성
    public void UpdateSearchWatchList()
    {
        DestroyWatchList();

        if (watchList.watchList.Length > 0 )
        {
            RectTransform contextSize = watchIDPosition.GetComponent<RectTransform>();

            contextSize.sizeDelta = new Vector2(contextSize.sizeDelta.x, (100 * watchList.watchList.Length));

            for (int i = 0; i < watchList.watchList.Length; i++)
            {
                GameObject tempInfo = Instantiate(watchIDPrefeb);

                tempInfo.transform.SetParent(watchIDPosition.transform);

                tempInfo.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -50 - 100 * i, 0);
                tempInfo.GetComponent<RectTransform>().sizeDelta = new Vector2(contextSize.sizeDelta.x, 100);

                tempInfo.GetComponent<RectTransform>().localScale = Vector3.one;
                tagIDObjectList.Add(tempInfo);

                tempInfo.transform.GetChild(0).GetComponent<TMP_Text>().text = watchList.watchList[i].watchID.ToString();

                tempInfo.GetComponent<WatchListInfo>().watchid = watchList.watchList[i].watchID.ToString();
            }
        }
    }
}

[Serializable]
public class WatchID
{
    public int watchID;
    public string mac_addr;
}

[Serializable]
public class WatchList
{
    public WatchID[] watchList;
}
