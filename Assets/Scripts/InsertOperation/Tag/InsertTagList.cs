using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class InsertTagList : MonoBehaviour
{
    public TagList tagList;

    public GameObject tagListPrefeb;

    public List<GameObject> tagObjectList = new List<GameObject>();

    public GameObject listPosition;

    string url = "";

    public string pubkey;

    //���� �±� ����Ʈ
    public void ResetTagList()
    {
        ReceiveTagData();
    }

    //�±� ����Ʈ ������ ��û
    public void ReceiveTagData()
    {
        url = InsertOperationManager.instance.url;
        pubkey = InsertOperationManager.instance.pubkey;

        StartCoroutine(ReceiveData());
    }

    // ������ ��û
    public IEnumerator ReceiveData()
    {
        string sendUrl = url + "/Management/Get_All_UWB_List";
        //string sendUrl = "http://192.168.0.45:5000/TagList";

        UnityWebRequest www = UnityWebRequest.Get(sendUrl);

        www.certificateHandler = new CertPublicKey { PUB_KEY = pubkey };

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {

            byte[] result = www.downloadHandler.data;

            string results = Encoding.UTF8.GetString(result);

            www.Dispose();

            //�±� ����Ʈ�� ������ ��� ����
            if (results == "empty")
            {
                DestroyTagList();
            }
            else if (results == "error")
            {
                InsertOperationManager.instance.insertUIManager.ErrorMessageOn("������ ���� �±� ����Ʈ�� �ҷ� ���� ��" +
                    "�߽��ϴ�.");
            }
            else
            {
                tagList = JsonUtility.FromJson<TagList>(results);
                UpdateTagList();
            }
        }
        else
        {
            InsertOperationManager.instance.insertUIManager.ErrorMessageOn("���� ���� ����");
        }

    }

    //�±� ����Ʈ ����
    public void DestroyTagList()
    {
        for (int i = tagObjectList.Count- 1; i >= 0; i--)
        {
            Destroy(tagObjectList[i]);
        }

        tagObjectList.Clear();
    }

    //�±� ����Ʈ ����
    public void UpdateTagList()
    {
        DestroyTagList();

        RectTransform contextSize = listPosition.GetComponent<RectTransform>();

        if (tagList != null)
        {
            contextSize.sizeDelta = new Vector2(contextSize.sizeDelta.x, tagList.tagList.Length * 100);

            for (int i = 0; i < tagList.tagList.Length; i++)
            {
                GameObject tempTagList = Instantiate(tagListPrefeb);

                //��ġ �� ũ��, �θ� ����
                tempTagList.transform.SetParent(listPosition.transform);

                tempTagList.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -50 -100 * i,0);
                tempTagList.GetComponent<RectTransform>().sizeDelta = new Vector2(contextSize.sizeDelta.x, 100);

                tempTagList.GetComponent<RectTransform>().localScale = Vector3.one;

                //������ �ݿ�
                tempTagList.GetComponent<InsertTagInfo>().tagID = tagList.tagList[i].tagID;
                tempTagList.GetComponent<InsertTagInfo>().id = tagList.tagList[i].ID;

                tempTagList.GetComponent<InsertTagInfo>().SetText();
                tagObjectList.Add(tempTagList);
            }
        }
    }
}