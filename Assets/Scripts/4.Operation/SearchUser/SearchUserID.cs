using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.Networking;
using System.Text;

public class SearchUserID : MonoBehaviour
{
    //���� �̸� �Է� �ǳ�
    public GameObject userNameInputPanel;
    //���� ���� ����Ʈ �ǳ�
    public GameObject userListPanel;

    //���� ���� ���� ������
    public GameObject userIDPrefeb;
    public GameObject userIDPosition;

    public List<GameObject> userIDObjectList;


    public UserList userList;

    //���� �̸� �Է� field
    public TMP_InputField userNameInputfield;

    //���� id field
    public TMP_InputField register_userIDInputfield;
    public TMP_InputField info_userIDInputfield;

    //string url = "http://192.168.0.10:5000";
    string url = "";

    public string pubkey;

    public void PanelOff()
    {
        userListPanel.SetActive(false);
        userNameInputPanel.SetActive(false);
    }

    public void NameInputPanelOn()
    {
        userNameInputPanel.SetActive(true);
    }

    //���� �̸� �Է� (��ư)
    public void ReceiveUserID()
    {
        url = OperationSceneManager.instance.url;
        pubkey = OperationSceneManager.instance.pubkey;

        userNameInputPanel.SetActive(false);
        userListPanel.SetActive(true);
        StartCoroutine(ReceiveData(userNameInputfield.text));
    }

    //���� id �� �Է�
    public void SetUserID(string name)
    {
        if (OperationSceneManager.instance.operationUIManager.nowStatus == OperationUIManager.Status.Register)
        {
            register_userIDInputfield.text = name;
        }
        else
        {
            info_userIDInputfield.text = name;
        }

        userListPanel.SetActive(false);
    }

    //���� ���� ���� �� �� �ݿ�
    public IEnumerator ReceiveData(string name)
    {
        string reciveURL = url + "/Management/searchUserList/"+ name;

        UnityWebRequest www = UnityWebRequest.Get(reciveURL);
        www.certificateHandler = new CertPublicKey { PUB_KEY = pubkey };
        www.timeout = 3;

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            byte[] result = www.downloadHandler.data;

            string resultStr = Encoding.UTF8.GetString(result);

            if (resultStr == "empty")
            {
                OperationSceneManager.instance.operationUIManager.ErrorMessgaeOn("������ ����̰ų� �̸��� �����ϴ�.");
            }
            else
            {
                userList = JsonUtility.FromJson<UserList>(resultStr);
                UpdateSearchTagList();
            }
            //if (userList != null)
            //{
            //    UpdateSearchTagList();
            //}
        }
        else
        {
            OperationSceneManager.instance.operationUIManager.ErrorMessgaeOn("���� ���� ����");
        }

        www.Dispose();
    }

    //���� ���� ������Ʈ ���� �� ����Ʈ��  �߰�
    public void UpdateSearchTagList()
    {
        for (int i = userIDObjectList.Count - 1; i >= 0; i--)
        {
            Destroy(userIDObjectList[i]);
        }

        userIDObjectList.Clear();

        if (userList.userList.Length > 0)
        {
            RectTransform contextSize = userIDPosition.GetComponent<RectTransform>();

            contextSize.sizeDelta = new Vector2(contextSize.sizeDelta.x, (100 * userList.userList.Length));

            for (int i = 0; i < userList.userList.Length; i++)
            {
                GameObject tempUserInfo = Instantiate(userIDPrefeb);

                tempUserInfo.transform.SetParent(userIDPosition.transform);

                tempUserInfo.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -50 - 100 * i, 0);
                tempUserInfo.GetComponent<RectTransform>().sizeDelta = new Vector2(contextSize.sizeDelta.x, 100);

                tempUserInfo.GetComponent<RectTransform>().localScale = Vector3.one;
                userIDObjectList.Add(tempUserInfo);

                tempUserInfo.transform.GetChild(0).GetComponent<TMP_Text>().text = userList.userList[i].userID;
                tempUserInfo.transform.GetChild(1).GetComponent<TMP_Text>().text = userList.userList[i].name;
                tempUserInfo.transform.GetChild(2).GetComponent<TMP_Text>().text = userList.userList[i].birth_day;

                tempUserInfo.GetComponent<SearchUserInfo>().userId = userList.userList[i].userID;
                tempUserInfo.GetComponent<SearchUserInfo>().userName = userList.userList[i].name;
                tempUserInfo.GetComponent<SearchUserInfo>().birth_day = userList.userList[i].birth_day;
            }
        }
       
    }
}

[Serializable]
public class UserList
{
    public UserID[] userList;
}

[Serializable]
public class UserID
{
    public string userID;
    public string name;
    public string birth_day;
}