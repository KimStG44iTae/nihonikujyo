using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class UserCrashManager : MonoBehaviour
{
    //�浹 ����Ʈ
    public GameObject crashList;

    //����Ʈ ������ �θ� �Ǵ� ������Ʈ
    public GameObject contentPosition;

    //�浹 ui ������
    public GameObject crashInfoPrefeb;

    //������ ������Ʈ ����Ʈ 
    public List<GameObject> crashInfoList;

    //�浹���� ������� ȣ��
    public void DeleteCrashInfo(GameObject user1, GameObject user2)
    {
        //UserInfo userInfo1 = user1.GetComponent<UserInfo>();
        //UserInfo userInfo2 = user2.GetComponent<UserInfo>();

        //for (int i = 0; i < crashInfoList.Count; i++)
        //{
        //    string id1 = crashInfoList[i].transform.GetChild(0).GetComponent<TMP_Text>().text;
        //    string id2 = crashInfoList[i].transform.GetChild(1).GetComponent<TMP_Text>().text;

        //    if ((userInfo1.userID.ToString() == id1) && (userInfo2.userID.ToString() == id2))
        //    {
        //        if (crashInfoList[i].transform.GetChild(3).GetComponent<TMP_Text>().text == "-")
        //        {
        //            crashInfoList[i].transform.GetChild(3).GetComponent<TMP_Text>().text = DateTime.Now.ToString("HH:mm:ss");
        //            break;
        //        }
        //    }
        //}
    }

    //�浹 �߻� �ÿ� ȣ��
    public void AddCrashInfo(GameObject user1, GameObject user2)
    {
        //UserInfo userInfo1 = user1.GetComponent<UserInfo>();
        //UserInfo userInfo2 = user2.GetComponent<UserInfo>();

        //int crashCount = crashInfoList.Count+1;

        //GameObject crashInfo = Instantiate(crashInfoPrefeb);

        //RectTransform contextSize = contentPosition.GetComponent<RectTransform>();

        //contextSize.sizeDelta = new Vector2(contextSize.sizeDelta.x, (100 * crashCount));

        //crashInfo.transform.SetParent(contentPosition.transform);

        //crashInfo.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, +50 - 100 * crashCount, 0);
        //crashInfo.GetComponent<RectTransform>().sizeDelta = new Vector2(contextSize.sizeDelta.x, 100);

        //crashInfo.GetComponent<RectTransform>().localScale = Vector3.one;

        //crashInfo.transform.GetChild(0).GetComponent<TMP_Text>().text = userInfo1.userID.ToString();
        //crashInfo.transform.GetChild(1).GetComponent<TMP_Text>().text = userInfo2.userID.ToString();
        //crashInfo.transform.GetChild(2).GetComponent<TMP_Text>().text = DateTime.Now.ToString("HH:mm:ss");
        //crashInfo.transform.GetChild(3).GetComponent<TMP_Text>().text = "-";

        //crashInfoList.Add(crashInfo);
    }

    //�浹 ����Ʈ ui ON/OFF
    public void CrashListOff()
    {
        crashList.SetActive(false);
    }

    public void CrashListOn()
    {
        crashList.SetActive(true);
    }

}
