using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class ReceiveUWBData : MonoBehaviour
{
    string url = "";
    public string pubkey;

    UnityWebRequest www;

    public UWBDataList uwbList;

    //사용자 오브젝트 리스트
    public List<GameObject> playerList = new List<GameObject>();

    private void OnDisable()
    {
        if (www != null)
        {
            www.Dispose();
        }
    }

    //uwb 데이터 수신
    public void RecevieData()
    {
        url = MonitoringSceneManager.instance.url;
        pubkey = MonitoringSceneManager.instance.pubkey;

        StartCoroutine(RecevieUWBData());
    }

    //uwb 데이터 수신 후 데이터 분배
    public IEnumerator RecevieUWBData()
    {
        playerList = MonitoringSceneManager.instance.userManager.userList;

        string parameter = url + "/UWB/uwb_info";
        www = UnityWebRequest.Get(parameter);
        www.certificateHandler = new CertPublicKey { PUB_KEY = pubkey };

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            byte[] results = www.downloadHandler.data;
            string resultStr = Encoding.UTF8.GetString(results);

            uwbList = JsonUtility.FromJson<UWBDataList>(resultStr);

            www.Dispose();

            for (int i = 0; i < playerList.Count; i++)
            {
                for (int j = 0; j < uwbList.uwbInfoList.Length; j++)
                {
                    if (playerList[i].GetComponent<UserInfo>().tagID == uwbList.uwbInfoList[j].tagId)
                    {
                        Vector3 usbPos = new Vector3((float)uwbList.uwbInfoList[j].position[0], 0, (float)uwbList.uwbInfoList[j].position[1]);

                        playerList[i].transform.position = usbPos;

                        playerList[i].GetComponent<UserInfo>().SetUWBData(uwbList.uwbInfoList[i]);
                        break;
                    }
                }
            }
        }

        yield return null;
    }
}
