using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class ReceiveWatchData : MonoBehaviour
{
    string url = "";
    public string pubkey;

    UnityWebRequest www;

    public WatchDataList watchList;

    //사용자 오브젝트 리스트
    public List<GameObject> playerList = new List<GameObject>();

    private void OnDisable()
    {
        if (www != null)
        {
            www.Dispose();
        }
    }

    //워치 데이터 수신
    public void RecevieData()
    {
        url = MonitoringSceneManager.instance.url;

        playerList = MonitoringSceneManager.instance.userManager.userList;
        pubkey = MonitoringSceneManager.instance.pubkey;

        for (int i = 0; i < playerList.Count; i++)
        {
            int ran = UnityEngine.Random.Range(90, 100);
            playerList[i].GetComponent<UserInfo>().heartRate = ran.ToString();
        }

        // StartCoroutine(RecevieWatchData());
    }

    //워치 데이터 수신 후 반영
    public IEnumerator RecevieWatchData()
    {
        playerList = MonitoringSceneManager.instance.userManager.userList;

        string parameter = url + "/Watch/watch_info";
        www = UnityWebRequest.Get(parameter);
        www.certificateHandler = new CertPublicKey { PUB_KEY = pubkey };

        yield return www.SendWebRequest();

        print(www.result);

        if (www.result == UnityWebRequest.Result.Success)
        {
            byte[] results = www.downloadHandler.data;
            string resultStr = Encoding.UTF8.GetString(results);

            WatchDataList watchList = JsonUtility.FromJson<WatchDataList>(resultStr);
            print(resultStr);

            www.Dispose();

            for (int i = 0; i < playerList.Count; i++)
            {
                for (int j = 0; j < watchList.watchInfoList.Length; j++)
                {
                    if (playerList[i].GetComponent<UserInfo>().watchID == watchList.watchInfoList[j].watchID)
                    {
                        playerList[i].GetComponent<UserInfo>().SetWatchData(watchList.watchInfoList[i]);

                        break;
                    }
                }
            }
        }


        yield return null;
    }
}
