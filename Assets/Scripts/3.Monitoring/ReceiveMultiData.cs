using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class ReceiveMultiData : MonoBehaviour
{
    UnityWebRequest www;

    string url = "";
    string pubkey = "";

    public List<GameObject> playerList = new List<GameObject>();

    public List<GameObject> cctvList = new List<GameObject>();

    private void OnDisable()
    {
        if (www != null)
        {
            www.Dispose();
        }
    }

    public void Add_CCTVList(GameObject cctv)
    {
        cctvList.Add(cctv);
    }

    //watch, tag, userInfo, zone 정보 모두를 한번에 받아옴
    public void StartReceiveMultiData()
    {
        url = MonitoringSceneManager.instance.url;
        pubkey = MonitoringSceneManager.instance.pubkey;
        playerList = MonitoringSceneManager.instance.userManager.userList;
        StartCoroutine(ReceiveData());
    }

    public IEnumerator ReceiveData()
    {
        string sendUrl = url + "/Monitor/MonitorInfo";
       // string sendUrl = "http://192.168.0.45:5000" + "/ContactData";

        www = UnityWebRequest.Get(sendUrl);
        www.certificateHandler = new CertPublicKey { PUB_KEY = pubkey };

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            byte[] result = www.downloadHandler.data;

            string resultstr = Encoding.UTF8.GetString(result);

            print(resultstr);

            if (resultstr != "empty")
            {
                MultiDataList multiDataList = JsonUtility.FromJson<MultiDataList>(resultstr);

                www.Dispose();

                //생성되어 있는 user 오브젝트에 데이터 모두 배치
                for (int i = 0; i < playerList.Count; i++)
                {
                    for (int j = 0; j < multiDataList.multiData.Length; j++)
                    {
                        MultiData tempMultiData = multiDataList.multiData[j];

                        if (playerList[i].GetComponent<UserInfo>().userID == tempMultiData.userID)
                        {
                            Vector3 uwbPos = new Vector3((float)tempMultiData.position_X, 0, (float)tempMultiData.position_Y);

                            playerList[i].transform.position = uwbPos;

                            playerList[i].GetComponent<UserInfo>().SetMultiData(tempMultiData);
                            break;
                        }
                    }
                }
            }
        }
        yield return null;
    }

}

