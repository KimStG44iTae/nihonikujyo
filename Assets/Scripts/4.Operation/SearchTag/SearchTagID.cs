using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class SearchTagID : MonoBehaviour
{
    //�±� id �ǳ�
    public GameObject TagIDPanel;

    //�±� id ���� ������
    public GameObject tagIDPrefeb;
    public GameObject tagIDPosition;

    //���� ������ �±� ������Ʈ ����Ʈ
    public List<GameObject> tagIDObjectList;

   // public string url = "http://192.168.0.10:5000";
    string url = "";
    public string pubkey;

    public TagList tagList;

    //�±� id inputfied
    public TMP_InputField tagIDInputfield;
    public TMP_InputField tagIDInputfield_update;

    //�±� id ����Ʈ ����
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

    //�±� id �� �ݿ�
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

    //�±� id ����Ʈ ���� �� �±� id �� �ݿ�
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
                OperationSceneManager.instance.operationUIManager.ErrorMessgaeOn("����� �� �ִ� �±װ� �����ϴ�.");
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
            OperationSceneManager.instance.operationUIManager.ErrorMessgaeOn("���� ���� ����");
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

    //�±� id ����Ʈ ������Ʈ ����
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