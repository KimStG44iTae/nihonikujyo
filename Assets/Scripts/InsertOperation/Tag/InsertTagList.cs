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

    //리셋 태그 리스트
    public void ResetTagList()
    {
        ReceiveTagData();
    }

    //태그 리스트 데이터 요청
    public void ReceiveTagData()
    {
        url = InsertOperationManager.instance.url;
        pubkey = InsertOperationManager.instance.pubkey;

        StartCoroutine(ReceiveData());
    }

    // 데이터 요청
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

            //태그 리스트가 존재할 경우 실행
            if (results == "empty")
            {
                DestroyTagList();
            }
            else if (results == "error")
            {
                InsertOperationManager.instance.insertUIManager.ErrorMessageOn("서버로 부터 태그 리스트를 불러 오지 못" +
                    "했습니다.");
            }
            else
            {
                tagList = JsonUtility.FromJson<TagList>(results);
                UpdateTagList();
            }
        }
        else
        {
            InsertOperationManager.instance.insertUIManager.ErrorMessageOn("서버 연결 에러");
        }

    }

    //태그 리스트 제거
    public void DestroyTagList()
    {
        for (int i = tagObjectList.Count- 1; i >= 0; i--)
        {
            Destroy(tagObjectList[i]);
        }

        tagObjectList.Clear();
    }

    //태그 리스트 생성
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

                //위치 및 크기, 부모 설정
                tempTagList.transform.SetParent(listPosition.transform);

                tempTagList.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -50 -100 * i,0);
                tempTagList.GetComponent<RectTransform>().sizeDelta = new Vector2(contextSize.sizeDelta.x, 100);

                tempTagList.GetComponent<RectTransform>().localScale = Vector3.one;

                //데이터 반영
                tempTagList.GetComponent<InsertTagInfo>().tagID = tagList.tagList[i].tagID;
                tempTagList.GetComponent<InsertTagInfo>().id = tagList.tagList[i].ID;

                tempTagList.GetComponent<InsertTagInfo>().SetText();
                tagObjectList.Add(tempTagList);
            }
        }
    }
}