using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Text;
using System;

public class MemberListManager : MonoBehaviour
{
    public MemberList memberList;

    //ȸ�� ���� ���� ������
    public GameObject infoPrefeb;
    public GameObject infoPosition;

    public List<GameObject> memberObjectList;
    string url = "";

    public int count;
    public string pubkey;

    //���� �ִ� ȸ�� ����Ʈ �� tagid, watchid ��Ī ���� ����
    public void ReceiveMemberData()
    {
        url = OperationSceneManager.instance.url;
        pubkey = OperationSceneManager.instance.pubkey;
        StartCoroutine(ReceiveData());
    }

    //���� �ִ� ȸ�� ����Ʈ �� tagid, watchid ��Ī ���� ����
    public IEnumerator ReceiveData()
    {
        string receiveUrl = url + "/Management/userList";

        UnityWebRequest www = UnityWebRequest.Get(receiveUrl);
        www.certificateHandler = new CertPublicKey { PUB_KEY = pubkey };

        yield return www.SendWebRequest();

        string result = Encoding.UTF8.GetString(www.downloadHandler.data);

        www.Dispose();

        if (result != "empty")
        {
            memberList = JsonUtility.FromJson<MemberList>(result);

            UpdateMemberList();
        }
        else
        {
            DestroyMemberList();
        }
    }

    public void DestroyMemberList()
    {
        for (int i = memberObjectList.Count - 1; i >= 0; i--)
        {
            Destroy(memberObjectList[i]);
        }
        memberObjectList.Clear();
    }

    //ȸ�� ����Ʈ �����͸� �̿��� ȸ�� ���� ������Ʈ ���� ����Ʈ�� �߰�
    public void UpdateMemberList()
    {
        DestroyMemberList();

        RectTransform contextSize = infoPosition.GetComponent<RectTransform>();

        if (memberList != null)
        {
            contextSize.sizeDelta = new Vector2(contextSize.sizeDelta.x, (100 * memberList.memberList.Length));

            for (int i = 0; i < memberList.memberList.Length; i++)
            {
                GameObject tempInfo = Instantiate(infoPrefeb);

                tempInfo.transform.SetParent(infoPosition.transform);

                tempInfo.GetComponent<RectTransform>().localPosition = new Vector3(0, -50 - 100 * i, 0);

                tempInfo.GetComponent<RectTransform>().localScale = Vector3.one;
                memberObjectList.Add(tempInfo);

                string tempTime = memberList.memberList[i].entry_time;

                string time = tempTime.Replace("/", " /");

                tempInfo.transform.GetChild(0).GetComponent<TMP_Text>().text = memberList.memberList[i].userID.ToString();
                tempInfo.transform.GetChild(1).GetComponent<TMP_Text>().text = memberList.memberList[i].tagID;
                tempInfo.transform.GetChild(2).GetComponent<TMP_Text>().text = memberList.memberList[i].watchID.ToString();
                tempInfo.transform.GetChild(3).GetComponent<TMP_Text>().text = time;

                tempInfo.GetComponent<MemberInfo>().memberInfo = memberList.memberList[i];
            }
        }
       
    }
}

[Serializable]
public class MemberInfomation
{
    public string tagID;
    public int watchID;
    public int userID;
    public string entry_time;
    public string exit_time;
}

[Serializable]
public class MemberList
{
    public MemberInfomation[] memberList;
}