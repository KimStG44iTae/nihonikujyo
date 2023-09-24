using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class ReceiveLogData : MonoBehaviour
{
    UnityWebRequest www;

    string url;

    public LogDataList logDataList;
    public LogDataList tempLogDataList;

    string pubkey;

    public DateRecord tempDateRecord;

    public List<GameObject> recordObjectList = new List<GameObject>();
    public GameObject recordPrefeb;
    public GameObject recodListPanel;
    public GameObject content;

    public List<GameObject> fallingObjectList = new List<GameObject>();
    public GameObject fallingPrefeb;
    public GameObject fallingListPanel;
    public GameObject fallingContent;

    public void RecodListPanelOff()
    {
        UIManager_Log.instance.BackGroundOff();
        recodListPanel.SetActive(false);
    }

    public void StartReceiveRecord(string date)
    {
        url = Monitoring_LogManager.instance.url;

        pubkey = Monitoring_LogManager.instance.pubkey;

        CallLogData tempCallLogData = new CallLogData();
        tempCallLogData.date = date;
        tempCallLogData.userID = int.Parse(Monitoring_LogManager.instance.userID_Log.rightId.text);

        recodListPanel.SetActive(true);

        StartCoroutine(ReceiveRecord(tempCallLogData));

    }

    public IEnumerator ReceiveRecord(CallLogData callLogData)
    {
        string sendUrl = url + "/Log/Management_Info";

        //선택한 날짜와 선택한 사람의 데이터를 json화
        string sendData = JsonUtility.ToJson(callLogData);

        byte[] jsonbyte = Encoding.UTF8.GetBytes(sendData);

        www = UnityWebRequest.Post(sendUrl, sendData);

        www.SetRequestHeader("Content-Type", "application/json");
        www.certificateHandler = new CertPublicKey { PUB_KEY = pubkey };
        www.uploadHandler = new UploadHandlerRaw(jsonbyte);

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            byte[] result = www.downloadHandler.data;

            string resultstr = Encoding.UTF8.GetString(result);

            www.Dispose();

            if (resultstr == "empty")
            {
                Monitoring_LogManager.instance.uIManager_Log.ErrorMessageOn("해당 날짜에 들어온 기록이 없습니다.");
                for (int i = recordObjectList.Count - 1; i >= 0; i--)
                {
                    Destroy(recordObjectList[i]);
                }

                recordObjectList.Clear();
            }
            else if (resultstr == "error")
            {
                Monitoring_LogManager.instance.uIManager_Log.ErrorMessageOn("서버 에러(출입 기록)");
            }
            else
            {
                tempDateRecord = JsonUtility.FromJson<DateRecord>(resultstr);
                MakeRecodList();
            }
        }
        else
        {
            Monitoring_LogManager.instance.uIManager_Log.ErrorMessageOn("서버 연결 에러");
        }


    }

    public void MakeRecodList()
    {
        for (int i = recordObjectList.Count - 1; i >= 0; i--)
        {
            Destroy(recordObjectList[i]);
        }

        recordObjectList.Clear();

        if (tempDateRecord != null)
        {
            RectTransform rectTransform = content.GetComponent<RectTransform>();

            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, tempDateRecord.record.Count * 100);

            for (int i = 0; i < tempDateRecord.record.Count; i++)
            {
                GameObject tempEnter = Instantiate(recordPrefeb);

                tempEnter.transform.SetParent(content.transform);

                recordObjectList.Add(tempEnter);

                tempEnter.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, - 50 - 100 * i, 0);
                tempEnter.GetComponent<RectTransform>().sizeDelta = new Vector2(rectTransform.sizeDelta.x, 100);
                tempEnter.GetComponent<RectTransform>().localScale = Vector3.one;

                string record = tempDateRecord.record[i].entry_time + " ~ " + tempDateRecord.record[i].exit_time;

                tempEnter.transform.GetChild(0).GetComponent<TMP_Text>().text = record;
                tempEnter.transform.GetComponent<RecodInfo>().id = tempDateRecord.id;
                tempEnter.transform.GetComponent<RecodInfo>().date = tempDateRecord.date;
                tempEnter.transform.GetComponent<RecodInfo>().record = tempDateRecord.record[i];

            }
        }
    }


    //서버로부터 로그 데이터 정보를 불러온다(시간, 위치 , userID)
    public void StartReceiveLogData(CallLogData data)
    {
        url = Monitoring_LogManager.instance.url;

        pubkey = Monitoring_LogManager.instance.pubkey;

        //CallLogData tempCallLogData = new CallLogData();
        //tempCallLogData.date = date;
        //tempCallLogData.userID = int.Parse(Monitoring_LogManager.instance.userID_Log.rightId.text);
        RecodListPanelOff();

        StartCoroutine(StartReceive(data));
    }

    

    //데이터 요청 및 수신
    IEnumerator StartReceive(CallLogData callLogData)
    {
       // string sendUrl = "http://192.168.0.45:5000" + "/LogData";
        string sendUrl = url + "/Log/Monitor_Info";

        //선택한 날짜와 선택한 사람의 데이터를 json화
        string sendData = JsonUtility.ToJson(callLogData);

        byte[] jsonbyte = Encoding.UTF8.GetBytes(sendData);

        www = UnityWebRequest.Post(sendUrl, sendData);

        www.SetRequestHeader("Content-Type", "application/json");
        www.certificateHandler = new CertPublicKey { PUB_KEY = pubkey };
        www.uploadHandler = new UploadHandlerRaw(jsonbyte);

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            byte[] result = www.downloadHandler.data;

            string resultstr = Encoding.UTF8.GetString(result);

            print(resultstr);

            if (resultstr == "error")
            {
                Monitoring_LogManager.instance.uIManager_Log.ErrorMessageOn("서버 에러(로그 데이터 수신)");
            }
            else if (resultstr == "empty")
            {
                Monitoring_LogManager.instance.uIManager_Log.ErrorMessageOn("데이터가 없습니다.");
            }
            else
            {
                //서버로부터 받은 데이터 클래스로 변경
                tempLogDataList = JsonUtility.FromJson<LogDataList>(resultstr);

                Relocation();
            }
        }
        else
        {
            Monitoring_LogManager.instance.uIManager_Log.ErrorMessageOn("서버 연결 에러");
        }
    }

    //서버로부터 받은 데이터를 잘 사용할 수 있게 시간을 초로 분류하고
    // 빈 시간이 있을 경우 앞뒤 시간의 위치를 비교하여 보간한 뒤 데이터 저장
    public void Relocation()
    {
        if (tempLogDataList == null || tempLogDataList.logData.Count == 0) 
        {
            return;
        }

        logDataList = new LogDataList();

        string startTime = tempLogDataList.logData[0].time;

        string[] startTimeSplit = startTime.Split(':');

        int startHour = int.Parse(startTimeSplit[0]);
        int startMin = int.Parse(startTimeSplit[1]);
        int startSec = int.Parse(startTimeSplit[2]);

        int startTime_by_Second = startHour * 3600 + startMin * 60 + startSec;

        string exitTime = tempLogDataList.logData[tempLogDataList.logData.Count - 1].time;

        string[] exitTimeSplit = exitTime.Split(':');

        int exitHour = int.Parse(exitTimeSplit[0]);
        int exitMin = int.Parse(exitTimeSplit[1]);
        int exitSec = int.Parse(exitTimeSplit[2]);

        int exitTime_by_Second = exitHour * 3600 + exitMin * 60 + exitSec;

        int index = exitTime_by_Second - startTime_by_Second;

        int count = 0;

        //시간을 초 단위로 변경하는 코드
        for (int i = 0; i < index; i++)
        {
            int indexTime = startTime_by_Second + i;

            int tempHour = indexTime / 3600;
            int tempMin = (indexTime % 3600) / 60;
            int tempSec = (indexTime % 3600) % 60;
           
            string hTime = tempHour.ToString();
            string mTime = tempMin.ToString();
            string sTime = tempSec.ToString();

            if (tempHour < 10)
            {
                hTime = "0" + hTime;
            }
            if (tempMin < 10)
            {
                mTime = "0" + mTime;
            }
            if (tempSec < 10)
            {
                sTime = "0" + sTime;
            }

            string tempTime = hTime + ":" + mTime + ":" + sTime;

            LogData tempLogData = new LogData();

            if (tempLogDataList.logData[count].time == tempTime)
            {
                tempLogData.time = tempLogDataList.logData[count].time;
                tempLogData.userID = tempLogDataList.logData[count].userID;
                tempLogData.pos = tempLogDataList.logData[count].pos;
                tempLogData.fall = tempLogDataList.logData[count].fall;

                count++;
            }
            else
            {
                tempLogData.time = tempTime;
                tempLogData.userID = -1;
            }


            logDataList.logData.Add(tempLogData);

        }

        if (logDataList.logData.Count >0)
        {

            int tempUserID = logDataList.logData[0].userID;

            //빈 시간이 있는 경우 보간하는 코드
            for (int i = 0; i < logDataList.logData.Count; i++)
            {
                if (logDataList.logData[i].userID == -1)
                {
                    Vector3 firstPos = logDataList.logData[i - 1].pos;

                    int emptyCount = 0;

                    for (int j = i; j < logDataList.logData.Count; j++)
                    {
                        if (logDataList.logData[j].userID != -1)
                        {
                            break;
                        }
                        emptyCount++;
                    }

                    Vector3 secondPos;

                    if (i+emptyCount > logDataList.logData.Count-1)
                    {
                        secondPos = logDataList.logData[logDataList.logData.Count-1].pos;
                    }
                    else
                        secondPos = logDataList.logData[i + emptyCount].pos;

                    for (int j = 0; j < emptyCount; j++)
                    {
                        float div = 1.0f / (float)(emptyCount + 1) * (float)(1 + j);
                        Vector3 emptyVector = Vector3.Lerp(firstPos, secondPos, div);

                        logDataList.logData[i + j].userID = tempUserID;
                        logDataList.logData[i + j].pos = emptyVector;
                    }

                }
            }

            SetFallingData(logDataList);
        }


        //시간 세팅
        Monitoring_LogManager.instance.inputTime_Pos.SetTime();
        Monitoring_LogManager.instance.inputTime.SetTime();
    }

    public void FallingListPanelOn()
    {
        UIManager_Log.instance.BackGroundOn();
        fallingListPanel.SetActive(true);
    }

    public void FallingListPanelOff()
    {
        UIManager_Log.instance.BackGroundOff();
        fallingListPanel.SetActive(false);
    }

    public void SetFallingData(LogDataList logDataList)
    {
        for (int i = fallingObjectList.Count - 1; i >= 0; i--)
        {
            Destroy(fallingObjectList[i]);
        }

        fallingObjectList.Clear();

        if (logDataList != null)
        {
            int fallCount = 0;

            RectTransform rectTransform = fallingContent.GetComponent<RectTransform>();


            for (int i = 0; i < logDataList.logData.Count; i++)
            {
                if (logDataList.logData[i].fall == "1")
                {
                    GameObject tempEnter = Instantiate(fallingPrefeb);

                    tempEnter.transform.SetParent(fallingContent.transform);

                    fallingObjectList.Add(tempEnter);

                    tempEnter.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -50 - 100 * fallCount, 0);
                    tempEnter.GetComponent<RectTransform>().sizeDelta = new Vector2(rectTransform.sizeDelta.x, 100);
                    tempEnter.GetComponent<RectTransform>().localScale = Vector3.one;

                    string record = logDataList.logData[i].time;

                    tempEnter.transform.GetChild(0).GetComponent<TMP_Text>().text = record;

                    fallCount++;
                }
            }
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, fallCount * 100);

        }
    }
}

[Serializable]
public class CallLogData
{
    public int userID;
    public string date;
    public string entry_time;
    public string exit_time;
}

[Serializable]
public class LogDataList
{
    public List<LogData> logData = new List<LogData>();
}

[Serializable]
public class LogData
{
    public int userID;
    public string time;
    public Vector3 pos;
    public string fall;
}


[Serializable]
public class DateRecord
{
    public int id;
    public string date;
    public List<Record> record;
}

[Serializable]
public class Record
{
    public string entry_time;
    public string exit_time;
}