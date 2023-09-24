using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class UserInfo : MonoBehaviour
{
    public string zone;
    public string motion;

    //userInfo
    public int userID;
    public string startTime;
    public string vaccine;

    //watch
    public string heartRate;
    public int watchID;

    //tag
    public Vector3 pos;
    public string tagID;

    public bool isFalling = true;
    public bool fallingCheck;

    string url = "";
    public string pubkey;

    //zone ����Ʈ
    public List<string> triggerList = new List<string>();

    public GameObject vaccinePosition;

    public GameObject vaccineNo;
    public GameObject vaccineYes1;
    public GameObject vaccineYes2;

    public GameObject dangerousMark;

    public GameObject fallingText;

    //zone �� ������
    public void SendUserZone()
    {
        pubkey = MonitoringSceneManager.instance.pubkey;
        url = MonitoringSceneManager.instance.url;

        StartCoroutine(SendData());
    }

    //������ zone ����Ʈ ����
    public IEnumerator SendData()
    {
        //while(true)
        {
            string sendUrl = url + "/Zone_Insert";

            UserZoneData userZoneData = new UserZoneData();
            userZoneData.userID = userID;
            userZoneData.zone_data = triggerList;
            userZoneData.date = DateTime.Now.ToString("yyyy-MM-dd'/'HH:mm:ss");

            string sendData = JsonUtility.ToJson(userZoneData);

            byte[] sendbyte = Encoding.UTF8.GetBytes(sendData);

            UnityWebRequest www = UnityWebRequest.Post(sendUrl, sendData);

            www.uploadHandler = new UploadHandlerRaw(sendbyte);
            www.SetRequestHeader("Content-Type", "application/json");
            www.certificateHandler = new CertPublicKey { PUB_KEY = pubkey };

            yield return www.SendWebRequest();

            byte[] receive = www.downloadHandler.data;

            string results = Encoding.UTF8.GetString(receive);

            www.Dispose();

            yield return new WaitForSeconds(0.02f);
        }

    }

    //������ ���� zone ������ ����
    public void RecevieZoneData()
    {
        StartCoroutine(StartReceiveZoneData());
    }

    //������ ���� zone ������ ������ �� ������ �ݿ�
    public IEnumerator StartReceiveZoneData()
    {
       // while(true)
        {
            string parameter = url + "/Zone/zone_info/" + userID;
            UnityWebRequest www = UnityWebRequest.Get(parameter);
            www.certificateHandler = new CertPublicKey { PUB_KEY = pubkey };

            yield return www.SendWebRequest();

            byte[] results = www.downloadHandler.data;
            string resultStr = Encoding.UTF8.GetString(results);

            UserZoneData userZoneData = JsonUtility.FromJson<UserZoneData>(resultStr);

            www.Dispose();

            string zoneName = "";

            for (int i = 0; i < userZoneData.zone_data.Count; i++)
            {
                zoneName += userZoneData.zone_data[i];

                if (i != userZoneData.zone_data.Count-1)
                {
                    zoneName += ",";
                }
            }

            zone = zoneName;

            if (MonitoringSceneManager.instance.setUserInformation.userInfo == this)
            {
                MonitoringSceneManager.instance.setUserInformation.SetInformationZone();
            }

            yield return new WaitForSeconds(0.02f);

        }

    }

    //uwb ������ ����
    public void SetUWBData(UWBData uwbData)
    {
        pos = new Vector3((float)uwbData.position[0], 1, (float)uwbData.position[1]);
        tagID = uwbData.tagId;
    }

    //��ġ ������ ����
    public void SetWatchData(WatchData watchData)
    {
        watchID = watchData.watchID;
        heartRate = watchData.heart_Rate.ToString();
    }

    //���� ������ ����
    public void SetUserData(UserData userData)
    {
        GameObject tempVaccineObj;

        vaccine = userData.covid;

        switch (vaccine)
        {
            case "������":
                tempVaccineObj = Instantiate(vaccineNo);
                tempVaccineObj.transform.SetParent(vaccinePosition.transform);
                tempVaccineObj.transform.localPosition = Vector3.zero;
                tempVaccineObj.transform.localScale = Vector3.one * 0.1f;
                vaccine = "������";

                break;
            case "����":
                tempVaccineObj = Instantiate(vaccineYes1);
                tempVaccineObj.transform.SetParent(vaccinePosition.transform);
                tempVaccineObj.transform.localPosition = Vector3.zero;
                tempVaccineObj.transform.localScale = Vector3.one * 0.1f;
                vaccine = "����";

                break;
            default:
                break;
        }

        //print(userData.userID + " + " + userData.covid + " + " + vaccine);
    }

    public void SetMultiData(MultiData multiData)
    {
        string zoneStr = string.Empty;
        for (int i = 0; i < multiData.zone.Count - 1; i++)
        {
            zoneStr += multiData.zone[i] + ", ";
        }

        if (multiData.zone.Count > 0)
        {
            zoneStr += multiData.zone[multiData.zone.Count - 1];
        }

        zone = zoneStr;

        heartRate = multiData.heart_rate.ToString();
        pos = new Vector3((float)multiData.position_X, 1, (float)multiData.position_Y);

        motion = multiData.motion;

        if (multiData.fall && fallingCheck)
        {
            CheckStatus();
            FallingStart();
        }

        if (MonitoringSceneManager.instance.setUserInformation.userInfo == this)
        {
            MonitoringSceneManager.instance.setUserInformation.SetMultiData(this);
        }
    }

    public void FallingStart()
    {
        isFalling = true;
        fallingCheck = false;

        fallingText.SetActive(true);

        StartCoroutine(FallingCheck());
    }

    public IEnumerator FallingCheck()
    {
        while (true)
        {
            yield return null;

            fallingText.transform.Rotate(0, 1, 0);

            if (fallingCheck)
            {
                break;
            }
        }
        fallingText.SetActive(false);
        isFalling = false;
        fallingCheck = true;
    }

    public void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Alpha1))
        //{
        //    CheckStatus("����");
        //}

        //if (Input.GetKeyDown(KeyCode.Alpha2))
        //{
        //    CheckStatus("ss");
        //}

        //if (Input.GetKeyDown(KeyCode.Alpha3) && fallingCheck)
        //{
        //    FallingStart();
        //    CheckStatus("����");
        //    MonitoringSceneManager.instance.setUserInformation.SetMultiData(this);
        //}
    }

    private void CheckStatus()
    {
        dangerousMark.SetActive(true);

        List<GameObject> cctvList = MonitoringSceneManager.instance.receiveMultiData.cctvList;

        if (cctvList.Count >= 0)
        {

            GameObject tempCCTV = cctvList[0];

            Vector3 cctvPos = tempCCTV.transform.position;

            pos = transform.position;

            for (int i = 0; i < cctvList.Count; i++)
            {
                if (Vector3.Distance(pos, cctvPos) > Vector3.Distance(pos, cctvList[i].transform.position))
                {
                    tempCCTV = cctvList[i];
                    cctvPos = tempCCTV.transform.position;
                }
            }

            MonitoringSceneManager.instance.moveCamera.transform.position = cctvPos + Vector3.up;
            MonitoringSceneManager.instance.moveCamera.transform.LookAt(gameObject.transform);

            MonitoringSceneManager.instance.userSelect.OutLineOff();
            MonitoringSceneManager.instance.userSelect.OutLineOn(tempCCTV.name);

            MonitoringSceneManager.instance.userSelect.StartSendId(tempCCTV.name);
        }
    }

    //zone�� �ִ��� üũ
    private void OnTriggerStay(Collider other)
    {
        if (other.tag != "Zone")
        {
            return;
        }

        bool isIn = false;

        for (int i = 0; i < triggerList.Count; i++)
        {
            if (triggerList[i] == other.name)
            {
                isIn = true;
            }
        }

        if (!isIn)
        {
            triggerList.Add(other.name);
        }
    }

    //zone���� ���� �� �߻�
    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "Zone")
        {
            return;
        }

        for (int i = 0; i < triggerList.Count; i++)
        {
            if (triggerList[i] == other.name)
            {
                triggerList.RemoveAt(i);
                break;
            }
        }
    }
}

