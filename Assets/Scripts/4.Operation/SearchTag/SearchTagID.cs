using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class SearchTagID : MonoBehaviour
{
    //태그 id 판넬
    public GameObject TagIDPanel;

    //태그 id 생성 프리펍
    public GameObject tagIDPrefeb;
    public GameObject tagIDPosition;

    //현재 생성된 태그 오브젝트 리스트
    public List<GameObject> tagIDObjectList;

   // public string url = "http://192.168.0.10:5000";
    string url = "";
    public string pubkey;

    public TagList tagList;

    //태그 id inputfied
    public TMP_InputField tagIDInputfield;
    public TMP_InputField tagIDInputfield_update;

    //태그 id 리스트 수신
    public void ReceiveTagID()
    {
        url = OperationSceneManager.instance.url;
        pubkey = OperationSceneManager.instance.pubkey;
        TagIDPanel.SetActive(true);
        StartCoroutine(ReceiveData());
    }

    public void PanelOff()
    {
        TagIDPanel.SetActive(false);
    }

    //태그 id 값 반영
    public void SetTagID(string tagid)
    {
        if (OperationSceneManager.instance.operationUIManager.insertMember.activeSelf)
        {
            tagIDInputfield.text = tagid;
        }
        else if(OperationSceneManager.instance.operationUIManager.updateMember.activeSelf)
        {
            tagIDInputfield_update.text = tagid;
        }
        TagIDPanel.SetActive(false);
    }

    //태그 id 리스트 수신 후 태그 id 값 반영
    public IEnumerator ReceiveData()
    {
        string reciveURL = url + "/Management/searchTagList";

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
                OperationSceneManager.instance.operationUIManager.ErrorMessgaeOn("사용할 수 있는 태그가 없습니다.");
                DestroyTagList();
            }
            else
            {
                tagList = JsonUtility.FromJson<TagList>(resultStr);
                UpdateSearchTagList();
            }
        }
        else
        {
            OperationSceneManager.instance.operationUIManager.ErrorMessgaeOn("서버 연결 에러");
        }

        www.Dispose();
    }

    public void DestroyTagList()
    {
        for (int i = tagIDObjectList.Count - 1; i >= 0; i--)
        {
            Destroy(tagIDObjectList[i]);
        }

        tagIDObjectList.Clear();
    }

    //태그 id 리스트 오브젝트 생성
    public void UpdateSearchTagList()
    {
        DestroyTagList();

        if (tagList.tagList.Length>0)
        {
            RectTransform contextSize = tagIDPosition.GetComponent<RectTransform>();

            contextSize.sizeDelta = new Vector2(contextSize.sizeDelta.x, (100 * tagList.tagList.Length));

            for (int i = 0; i < tagList.tagList.Length; i++)
            {
                GameObject tempTagInfo = Instantiate(tagIDPrefeb);

                tempTagInfo.transform.SetParent(tagIDPosition.transform);

                tempTagInfo.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -50 - 100 * i, 0);
                tempTagInfo.GetComponent<RectTransform>().sizeDelta = new Vector2(contextSize.sizeDelta.x, 100);

                tempTagInfo.GetComponent<RectTransform>().localScale = Vector3.one;
                tagIDObjectList.Add(tempTagInfo);

                tempTagInfo.transform.GetChild(0).GetComponent<TMP_Text>().text = tagList.tagList[i].tagID;

                tempTagInfo.GetComponent<TagListInfo>().tagid = tagList.tagList[i].tagID;
            }
        }
     
    }
}

[Serializable]
public class TagList
{
    public TagID[] tagList;
}

[Serializable]
public class TagID
{
    public string tagID;
    public int ID;
}