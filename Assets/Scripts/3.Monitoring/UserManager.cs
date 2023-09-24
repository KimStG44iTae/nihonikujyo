using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class UserManager : MonoBehaviour
{
    string url = "";
    public string pubkey;

    UnityWebRequest www;

    //사용자 매칭 정보
    public MemberList memberList;

    //사용자 오브젝트 리스트
    public List<GameObject> userList = new List<GameObject>();

    //생성할 사용자 프리펍
    public GameObject playerPrefb;

    public bool isStart;

    public ButtonInteraction buttonInteraction;

    //모든 사용자의 zone 데이터 전송
    public void SendZone()
    {
        for (int i = 0; i < userList.Count; i++)
        {
            userList[i].GetComponent<UserInfo>().SendUserZone();
        }
    }

    //모든 사용자의 zone 데이터 수신
    public void ReceiveZone()
    {
        for (int i = 0; i < userList.Count; i++)
        {
            userList[i].GetComponent<UserInfo>().RecevieZoneData();
        }
    }

    //데이터 수신 시작
    public void RecevieData()
    {
        url = MonitoringSceneManager.instance.url;

        StartCoroutine(RecevieManagementData());
    }

    private void OnDisable()
    {
        if (www !=null)
        {
            www.Dispose();
        }
    }

    //사용자 매칭 정보 호출 및 수신
    public IEnumerator RecevieManagementData()
    {
        if (isStart || (MonitoringSceneManager.instance.model == null))
        {
            print("이미 실행중 or 모델링 없음");
        }
        else
        {
            isStart = true;
            buttonInteraction.ButtonClick();
            buttonInteraction.enabled = false;

            while (true)
            {
                string parameter = url + "/Management/userList";
                www = UnityWebRequest.Get(parameter);
                www.certificateHandler = new CertPublicKey { PUB_KEY = pubkey };

                yield return www.SendWebRequest();

                if (www.result == UnityWebRequest.Result.Success)
                {
                    byte[] result = www.downloadHandler.data;

                    string resultStr = Encoding.UTF8.GetString(result);

                    if (resultStr != "empty")
                    {
                        memberList = JsonUtility.FromJson<MemberList>(resultStr);

                        www.Dispose();

                        //사용자 리스트 점검
                        CheckCount();

                        SendZone();
                        MonitoringSceneManager.instance.receiveMultiData.StartReceiveMultiData();
                    }
                    else
                    {
                        for (int i = userList.Count - 1; i >= 0; i--)
                        {
                            Destroy(userList[i]);
                            userList.RemoveAt(i);
                        }
                    }
                }
                else
                {
                    isStart = false;
                    www.Dispose();
                    break;
                }

                yield return new WaitForSeconds(1f);
            }
        }

    }

    //사용자 리스트 점검(추가, 삭제, 유지)
    public void CheckCount()
    {
        //사용자가 늘어나면 오브젝트 새로 생성하고 정보 요청
        if (memberList.memberList.Length - userList.Count > 0)
        {
            List<int> index = new List<int>();

            for (int i = 0; i < memberList.memberList.Length; i++)
            {
                bool isIn = false;

                for (int j = 0; j < userList.Count; j++)
                {
                    if (userList[j].GetComponent<UserInfo>().tagID == memberList.memberList[i].tagID)
                    {
                        isIn = true;
                    }
                }
                if (!isIn)
                {
                    index.Add(i);
                }
            }

            for (int i = 0; i < index.Count; i++)
            {
                GameObject newPlayer = Instantiate(playerPrefb);

                newPlayer.GetComponent<UserInfo>().tagID = memberList.memberList[index[i]].tagID;
                newPlayer.GetComponent<UserInfo>().watchID = memberList.memberList[index[i]].watchID;
                newPlayer.GetComponent<UserInfo>().userID = memberList.memberList[index[i]].userID;
                newPlayer.GetComponent<UserInfo>().startTime = memberList.memberList[index[i]].entry_time;

                userList.Add(newPlayer);
            }

            MonitoringSceneManager.instance.receiveUserData.RecevieData();

        }
        //사용자가 줄어들면 오브젝트 삭제
        else if (memberList.memberList.Length - userList.Count < 0)
        {
            List<int> index = new List<int>();

            for (int i = 0; i < userList.Count; i++)
            {
                bool isIn = false;

                for (int j = 0; j < memberList.memberList.Length; j++)
                {
                    if (userList[i].GetComponent<UserInfo>().tagID == memberList.memberList[j].tagID)
                    {
                        isIn = true;
                    }
                }
                if (!isIn)
                {
                    index.Add(i);
                }
            }

            index.Sort();

            for (int i = index.Count-1; i >= 0; i--)
            {
                Destroy(userList[index[i]]);
                userList.RemoveAt(index[i]);
            }
        }

    }
}
