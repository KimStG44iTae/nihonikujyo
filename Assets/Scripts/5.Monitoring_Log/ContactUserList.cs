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

    //������ ��û�� ����� id �� �����ߴ��� Ȯ���ϰ� ���� ��¥�� �迭�� ����
    SelectContactData selectContactData;

    public GameObject contextPostion;

    public GameObject contactUserPrefeb;

    public ContactContainer contactDataContainer;

    public List<GameObject> userList = new List<GameObject>();

    public List<ContactResultDataList> resultList = new List<ContactResultDataList>();

    public string pubkey;

    //���˸���Ʈ ȣ��
    public void ReceiveContactData(SelectContactData con)
    {
        url = Monitoring_LogManager.instance.url;
        pubkey = Monitoring_LogManager.instance.pubkey;

    //������ ��û�� ����� id �� �����ߴ��� Ȯ���ϰ� ���� ��¥�� �迭�� ����
        selectContactData = con; 

        StartCoroutine(StartReceiveContactData());
    }

    //���˸���Ʈ ȣ��
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

            //������ �ִ��� ������ Ȯ��
            if (resultstr == "empty")
            {
                Monitoring_LogManager.instance.uIManager_Log.ErrorMessageOn("������ ����� �����ϴ�.");
            }
            else if (resultstr == "error")
            {
                Monitoring_LogManager.instance.uIManager_Log.ErrorMessageOn("���� ����(���˸���Ʈ ȣ��)");
            }
            else
            {
                contactDataContainer = JsonUtility.FromJson<ContactContainer>(resultstr);

                DivideDate();
            }
        }
        else
        {
            Monitoring_LogManager.instance.uIManager_Log.ErrorMessageOn("���� ���� ����");
        }
    }

    //��¥���� �����͸� �з�
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

    //�Ϸ� ������ �з��� �����͸� ���(�ش� ��¥�� ������ ����� �� : �� ������ ���)
    //�ʴ����� �з��Ǿ� �����Ƿ� id �� Ű�� �ΰ� �ʸ� count�ؼ� ����
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

                    //�������� id�� �̸� ��Ī
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

    //���˸���Ʈ ���� �� ���� - ������ �й�
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
            tempContact.transform.GetChild(2).GetComponent<TMP_Text>().text = mTime + "�� "+ sTime +"��";
            tempContact.transform.GetChild(3).GetComponent<TMP_Text>().text = tempContactResultData.date;
        }
    }

    //����Ʈ ����
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

    //����Ʈ ������ �˰� ���� �������� �̸��� �˻��ϸ� �������ΰ� �Բ� �� ����
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
            Monitoring_LogManager.instance.uIManager_Log.ErrorMessageOn("�Է��� ����̶� �������� �ʾҽ��ϴ�.");
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

    //���˸���Ʈ �ʱ�ȭ
    public void ResetContactUserList()
    {
        DestroyList();
        for (int i = 0; i < resultList.Count; i++)
        {
            UpdateContactList(resultList[i]);
        }
    }
}

//���� ������ - �ʴ����� ������������ �� �ð��� ������ ������� ����Ʈ�� ����
[Serializable]
public class ContactData
{
    public List<int> contactUserList;
    public string time;
    public string date;
}

[Serializable]
public class ContactDataList    //�ʴ����� ����� �����͸� ��Ƴ��� ��
{
    public List<ContactData> contactDataList;
}

[Serializable]
public class ContactContainer //�ʴ����� ����� �����͸� ��Ƴ��� ���� ��¥���� ��Ƴ��� ��
{
    public List<ContactDataList> contactContainer;
}

//������ ���� ���� �����͸� ���� Ŭ���̾�Ʈ���� ����� ������ ������� ����
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