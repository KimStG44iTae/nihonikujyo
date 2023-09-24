using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class InsertWatchList : MonoBehaviour
{
    public WatchList watchList;

    public GameObject watchListPrefeb;

    public List<GameObject> watchObjectList = new List<GameObject>();

    public GameObject listPosition;

    string url = "";

    public string pubkey;

    //���� ��ġ ����Ʈ
    public void ResetWatchList()
    {
        ReceiveWatchData();
    }

    //��ġ ����Ʈ ������ ��û
    public void ReceiveWatchData()
    {
        url = InsertOperationManager.instance.url;
        pubkey = InsertOperationManager.instance.pubkey;

        StartCoroutine(ReceiveData());
    }

    // ������ ��û
    public IEnumerator ReceiveData()
    {
        string sendUrl = url + "/Management/Get_All_Watch_List";
        //string sendUrl = "http://192.168.0.45:5000/WatchList";

        UnityWebRequest www = UnityWebRequest.Get(sendUrl);

        www.certificateHandler = new CertPublicKey { PUB_KEY = pubkey };

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            byte[] result = www.downloadHandler.data;

            string results = Encoding.UTF8.GetString(result);

            www.Dispose();

            //��ġ ����Ʈ�� ������ ��� ����
            if (results == "empty")
            {
                DestroyWatchList();
            }
            else if (results == "error")
            {
                InsertOperationManager.instance.insertUIManager.ErrorMessageOn("������ ���� �±� ����Ʈ�� �ҷ� ���� ��" +
                    "�߽��ϴ�.");
            }
            else
            {
                watchList = JsonUtility.FromJson<WatchList>(results);
                UpdateWatchList();
            }
        }
        else
        {
            InsertOperationManager.instance.insertUIManager.ErrorMessageOn("���� ���� ����");
        }

    }

    //��ġ ����Ʈ ����
    public void DestroyWatchList()
    {
        for (int i = watchObjectList.Count - 1; i >= 0; i--)
        {
            Destroy(watchObjectList[i]);
        }

        watchObjectList.Clear();
    }

    //��ġ ����Ʈ ����
    public void UpdateWatchList()
    {
        DestroyWatchList();

        RectTransform contextSize = listPosition.GetComponent<RectTransform>();

        if (watchList != null)
        {
            contextSize.sizeDelta = new Vector2(contextSize.sizeDelta.x, watchList.watchList.Length * 100);

            for (int i = 0; i < watchList.watchList.Length; i++)
            {
                GameObject tempTagList = Instantiate(watchListPrefeb);

                //��ġ �� ũ��, �θ� ����
                tempTagList.transform.SetParent(listPosition.transform);

                tempTagList.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -50 - 100 * i, 0);
                tempTagList.GetComponent<RectTransform>().sizeDelta = new Vector2(contextSize.sizeDelta.x, 100);

                tempTagList.GetComponent<RectTransform>().localScale = Vector3.one;

                //������ �ݿ�
                tempTagList.GetComponent<InsertWatchInfo>().watchID = watchList.watchList[i].watchID;
                tempTagList.GetComponent<InsertWatchInfo>().mac_addr = watchList.watchList[i].mac_addr;

                tempTagList.GetComponent<InsertWatchInfo>().SetText();
                watchObjectList.Add(tempTagList);
            }
        }
    }
}
