using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class ReceiveUserData : MonoBehaviour
{
    string url = "";
    public string pubkey;

    UnityWebRequest www;

    //사용자 오브젝트 리스트
    public List<GameObject> playerList = new List<GameObject>();

    //사용자 정보 수신
    public void RecevieData()
    {
        url = MonitoringSceneManager.instance.url;
        pubkey = MonitoringSceneManager.instance.pubkey;

        StartCoroutine(RecevieUserData());
    }

    //사용자 정보 수신 후 반영
    public IEnumerator RecevieUserData()
    {
        playerList = MonitoringSceneManager.instance.userManager.userList;

        string parameter = url + "/User/user_info";
        www = UnityWebRequest.Get(parameter);
        www.certificateHandler = new CertPublicKey { PUB_KEY = pubkey };

        yield return www.SendWebRequest();

        byte[] results = www.downloadHandler.data;
        string resultStr = Encoding.UTF8.GetString(results);

        if (resultStr != "empty")
        {
            UserDataList userList = JsonUtility.FromJson<UserDataList>(resultStr);

            www.Dispose();

            for (int i = 0; i < playerList.Count; i++)
            {
                for (int j = 0; j < userList.userList.Length; j++)
                {
                    if (playerList[i].GetComponent<UserInfo>().userID == userList.userList[j].userID)
                    {
                        playerList[i].GetComponent<UserInfo>().SetUserData(userList.userList[j]);
                        break;
                    }
                }
            }
        }

        yield return null;
    }


}
