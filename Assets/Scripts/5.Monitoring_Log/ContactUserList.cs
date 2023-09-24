using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class ContactUserList : MonoBehaviour
{
    UnityWebRequest www;

    string url;

    //서버로 요청할 사람의 id 와 접촉했는지 확인하고 싶은 날짜를 배열로 담음
    SelectContactData selectContactData;

    public GameObject contextPostion;

    public GameObject contactUserPrefeb;

    public ContactContainer contactDataContainer;

    public List<GameObject> userList = new List<GameObject>();

    public List<ContactResultDataList> resultList = new List<ContactResultDataList>();

    public string pubkey;

    //접촉리스트 호출
    public void ReceiveContactData(SelectContactData con)
    {
        url = Monitoring_LogManager.instance.url;
        pubkey = Monitoring_LogManager.instance.pubkey;

    //서버로 요청할 사람의 id 와 접촉했는지 확인하고 싶은 날짜를 배열로 담음
        selectContactData = con; 

        StartCoroutine(StartReceiveContactData());
    }

    //접촉리스트 호출
    IEnumerator StartReceiveContactData()
    {
       // string sendUrl = "http://192.168.0.45:5000" + "/ContactData";
        string sendUrl = url + "/Log/Search_Contactor";

        string sendData = JsonUtility.ToJson(selectContactData);

        print(sendData);

        byte[] jsonbyte = Encoding.UTF8.GetBytes(sendData);

        www = UnityWebRequest.Post(sendUrl, sendData);
        www.SetRequestHeader("Content-Type", "application/json");
        www.uploadHandler = new UploadHandlerRaw(jsonbyte);
        www.certificateHandler = new CertPublicKey { PUB_KEY = pubkey };

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            byte[] result = www.downloadHandler.data;

            string resultstr = Encoding.UTF8.GetString(result);

            //데이터 있는지 없는지 확인
            if (resultstr == "empty")
            {
                Monitoring_LogManager.instance.uIManager_Log.ErrorMessageOn("접촉한 사람이 없습니다.");
            }
            else if (resultstr == "error")
            {
                Monitoring_LogManager.instance.uIManager_Log.ErrorMessageOn("서버 에러(접촉리스트 호출)");
            }
            else
            {
                contactDataContainer = JsonUtility.FromJson<ContactContainer>(resultstr);

                DivideDate();
            }
        }
        else
        {
            Monitoring_LogManager.instance.uIManager_Log.ErrorMessageOn("서버 연결 에러");
        }
    }

    //날짜별로 데이터를 분류
    public void DivideDate()
    {
        resultList.Clear();

        DestroyList();

        for (int i = 0; i < contactDataContainer.contactContainer.Count; i++)
        {
            ContactDataList contactDataList = contactDataContainer.contactContainer[i];

            CalculateData(contactDataList);
        }
    }

    //하루 단위로 분류된 데이터를 계산(해당 날짜에 접촉한 사람을 분 : 초 단위로 계산)
    //초단위로 분류되어 있으므로 id 를 키로 두고 초를 count해서 저장
    public void CalculateData(ContactDataList contactDataList)
    {
        ContactResultDataList contactResultDataList = new ContactResultDataList();

        for (int i = 0; i < contactDataList.contactDataList.Count; i++)
        {
            ContactData tempContactData = contactDataList.contactDataList[i];

            for (int j = 0; j < tempContactData.contactUserList.Count; j++)
            {
                if (tempContactData.contactUserList[j] == -1)
                {
                    break;
                }

                bool isIn = false;
                int contactIndex = -1;

                for (int k = 0; k < contactResultDataList.contactResultDataList.Count; k++)
                {
                    if (contactResultDataList.contactResultDataList[k].userID == tempContactData.contactUserList[j])
                    {
                        isIn = true;
                        contactIndex = k;
                        break;
                    }
                }

                if (isIn)
                {
                    contactResultDataList.contactResultDataList[contactIndex].timeSecond += 1;
                }
                else
                {
                    ContactResultData contactResultData = new ContactResultData();
                    contactResultData.userID = tempContactData.contactUserList[j];
                    contactResultData.timeSecond += 1;
                    contactResultData.date = tempContactData.date;

                    UserList_Log userList_Log = Monitoring_LogManager.instance.userID_Log.userList;

                    //접촉자의 id와 이름 매칭
                    for (int k = 0; k < userList_Log.userLogList.Count; k++)
                    {
                        if (userList_Log.userLogList[k].userID == contactResultData.userID.ToString())
                        {
                            contactResultData.name = userList_Log.userLogList[k].name;
                            break;
                        }
                    }

                    contactResultDataList.contactResultDataList.Add(contactResultData);
                }
            }
        }

        resultList.Add(contactResultDataList);

        UpdateContactList(contactResultDataList);
    }

    public int posY = -50;

    public int sizeY = 0;

    //접촉리스트 생성 및 세팅 - 데이터 분배
    public void UpdateContactList(ContactResultDataList contactResultDataList)
    {
        RectTransform contextSize = contextPostion.GetComponent<RectTransform>();

        contextSize.sizeDelta = new Vector2(contextSize.sizeDelta.x, sizeY + (100 * contactResultDataList.contactResultDataList.Count));

        sizeY = sizeY + (100 * contactResultDataList.contactResultDataList.Count);

        for (int i = 0; i < contactResultDataList.contactResultDataList.Count; i++)
        {
            ContactResultData tempContactResultData = contactResultDataList.contactResultDataList[i];

            GameObject tempContact = Instantiate(contactUserPrefeb);

            tempContact.transform.SetParent(contextPostion.transform);

            tempContact.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, posY, 0);
            tempContact.GetComponent<RectTransform>().sizeDelta = new Vector2(contextSize.sizeDelta.x, 100);

            tempContact.GetComponent<RectTransform>().localScale = Vector3.one;
            userList.Add(tempContact);

            posY = posY - 100;

            int min = tempContactResultData.timeSecond / 60;
            int sec = tempContactResultData.timeSecond % 60;

            string mTime = min.ToString();
            string  sTime = sec.ToString();

            if (min < 10)
            {
                mTime = "0" + mTime;
            }
            if (sec < 10)
            {
                sTime = "0" + sTime;
            }

            tempContact.transform.GetChild(0).GetComponent<TMP_Text>().text = tempContactResultData.userID.ToString();
            tempContact.transform.GetChild(1).GetComponent<TMP_Text>().text = tempContactResultData.name;
            tempContact.transform.GetChild(2).GetComponent<TMP_Text>().text = mTime + "분 "+ sTime +"초";
            tempContact.transform.GetChild(3).GetComponent<TMP_Text>().text = tempContactResultData.date;
        }
    }

    //리스트 삭제
    public void DestroyList()
    {
        posY = -50;
        sizeY = 0;

        for (int i = userList.Count - 1; i >= 0; i--)
        {
            Destroy(userList[i]);
        }
        userList.Clear();
    }

    public TMP_InputField inputName;
    public TMP_Text resultName;
    public GameObject inputNamePanel;

    //리스트 내에서 알고 싶은 접촉자의 이름을 검색하면 동명이인과 함께 다 나옴
    public void SearchName()
    {
        DestroyList();

        ContactResultDataList tempContactResultDataList = new ContactResultDataList();

        tempContactResultDataList.contactResultDataList = new List<ContactResultData>();

        string searchName = inputName.text;

        for (int i = 0; i < resultList.Count; i++)
        {
            for (int j = 0; j < resultList[i].contactResultDataList.Count; j++)
            {
                if (resultList[i].contactResultDataList[j].name == searchName)
                {
                    tempContactResultDataList.contactResultDataList.Add(resultList[i].contactResultDataList[j]);
                }
            }
        }

        if (tempContactResultDataList.contactResultDataList.Count == 0)
        {
            Monitoring_LogManager.instance.uIManager_Log.ErrorMessageOn("입력한 사람이랑 접촉하지 않았습니다.");
        }
        else
        {
            UpdateContactList(tempContactResultDataList);
        }

        resultName.text = searchName;

        InputPanelOff();
    }

    public void InputPanelOn()
    {
        inputNamePanel.SetActive(true);
    }

    public void InputPanelOff()
    {
        inputNamePanel.SetActive(false);
    }

    //접촉리스트 초기화
    public void ResetContactUserList()
    {
        DestroyList();
        for (int i = 0; i < resultList.Count; i++)
        {
            UpdateContactList(resultList[i]);
        }
    }
}

//접촉 데이터 - 초단위로 끊어져있으며 그 시간에 접촉한 사람들이 리스트로 저장
[Serializable]
public class ContactData
{
    public List<int> contactUserList;
    public string time;
    public string date;
}

[Serializable]
public class ContactDataList    //초단위로 모아진 데이터를 모아놓은 곳
{
    public List<ContactData> contactDataList;
}

[Serializable]
public class ContactContainer //초단위로 모아진 데이터를 모아놓은 곳을 날짜별로 모아놓은 곳
{
    public List<ContactDataList> contactContainer;
}

//서버로 부터 받은 데이터를 실제 클라이언트에서 사용할 데이터 양식으로 변경
[Serializable]
public class ContactResultData
{
    public int userID;
    public string name;
    public int timeSecond;
    public string date;

    public ContactResultData()
    {
        timeSecond = 0;
    }
}

[Serializable]
public class ContactResultDataList
{
    public List<ContactResultData> contactResultDataList;

    public ContactResultDataList()
    {
        contactResultDataList = new List<ContactResultData>();
    }
}