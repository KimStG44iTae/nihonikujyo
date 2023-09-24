using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using System.Text;
using System;

public class UserIDList_Log : MonoBehaviour
{
    //유저 정보 생성 프리펍
    public GameObject userIDPrefeb;
    public GameObject userIDPosition;

    public List<GameObject> userIDObjectList;

    public UserList_Log userList;

    //유저 이름 입력 field
    public TMP_InputField userNameInputfield;

    //string url = "https://192.168.0.10:5000";
    string url = "";

    public string pubkey;

    public TMP_Text rightName;
    public TMP_Text rightId;
    public TMP_Text rightBirthdate;

    //유저 리스트 호출
    public void ReceiveUserID()
    {
        url = Monitoring_LogManager.instance.url;
        pubkey = Monitoring_LogManager.instance.pubkey;

        StartCoroutine(ReceiveData());

    }

    //오른쪽에 시간별 동선 ui에 있는 id 이름 생년월일 수정
    public void SetUserID(SearchUserInfo_Log infoData)
    {
        rightId.text = infoData.userId;
        rightName.text = infoData.userName;
        rightBirthdate.text = infoData.birth_day;
    }

    //유저 리스트 호출
    public IEnumerator ReceiveData()
    {
        //string reciveURL = "http://192.168.0.45:5000" + "/UserListLog";
        string reciveURL = url + "/User/All_User_Info";

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
                Monitoring_LogManager.instance.uIManager_Log.ErrorMessageOn("회원 리스트가 없습니다.");
            }
            else if (resultStr == "error")
            {
                Monitoring_LogManager.instance.uIManager_Log.ErrorMessageOn("서버 에러(회원 리스트)");
            }
            else
            {
                userList = JsonUtility.FromJson<UserList_Log>(resultStr);

                UpdateIdList(userList);
            }
        }
        else
        {
            Monitoring_LogManager.instance.uIManager_Log.ErrorMessageOn("서버 연결 에러");
        }
        www.Dispose();

    }

    //유저 리스트 생성
    public void UpdateIdList(UserList_Log userList)
    {
        for (int i = userIDObjectList.Count - 1; i >= 0; i--)
        {
            Destroy(userIDObjectList[i]);
        }

        userIDObjectList.Clear();

        RectTransform contextSize = userIDPosition.GetComponent<RectTransform>();

        if (userList != null)
        {
            contextSize.sizeDelta = new Vector2(contextSize.sizeDelta.x, (100 * userList.userLogList.Count));

            for (int i = 0; i < userList.userLogList.Count; i++)
            {
                GameObject tempUserInfo = Instantiate(userIDPrefeb);

                tempUserInfo.transform.SetParent(userIDPosition.transform);

                //위치 및 크기 세팅
                tempUserInfo.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -50 - 100 * i, 0);
                tempUserInfo.GetComponent<RectTransform>().sizeDelta = new Vector2(contextSize.sizeDelta.x, 100);

                tempUserInfo.GetComponent<RectTransform>().localScale = Vector3.one;
                userIDObjectList.Add(tempUserInfo);

                tempUserInfo.transform.GetChild(0).GetComponent<TMP_Text>().text = userList.userLogList[i].userID;
                tempUserInfo.transform.GetChild(1).GetComponent<TMP_Text>().text = userList.userLogList[i].name;
                tempUserInfo.transform.GetChild(2).GetComponent<TMP_Text>().text = userList.userLogList[i].birth_day;

                tempUserInfo.GetComponent<SearchUserInfo_Log>().userId = userList.userLogList[i].userID;
                tempUserInfo.GetComponent<SearchUserInfo_Log>().userName = userList.userLogList[i].name;
                tempUserInfo.GetComponent<SearchUserInfo_Log>().birth_day = userList.userLogList[i].birth_day;
            }
        }
       
    }

    public void ResetUserList()
    {
        UpdateIdList(userList);
    }
}

[Serializable]
public class UserList_Log
{
    public List<UserID_Log_Info> userLogList;
}

[Serializable]
public class UserID_Log_Info
{
    public string userID;
    public string name;
    public string birth_day;
}
