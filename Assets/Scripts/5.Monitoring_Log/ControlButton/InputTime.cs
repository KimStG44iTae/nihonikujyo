using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class InputTime : MonoBehaviour
{
    public TMP_Text startTime;
    public TMP_Text exitTime;

    public TMP_InputField startPoint;
    public TMP_InputField exitPoint;

    public int oriStartAllSec;
    public int oriExitAllSec;

    public int startHour;
    public int startMin;
    public int startSec;

    public int startAllSec;

    public int exitHour;
    public int exitMin;
    public int exitSec;

    public int exitAllSec;

    public GameObject backPanel;

    public LogDataList logDataList;

    public GameObject player;

    public void Start()
    {
        backPanel.SetActive(false);
        ConvertTime();
    }

    public void OnEnable()
    {
        player.GetComponent<LineRenderer>().enabled = true;
    }

    //�ð� ������ ���� �� ����, ���� �ð�, �����ð� ����, InputField �� ����
    public void SetTime()
    {
        logDataList = Monitoring_LogManager.instance.receiveLogData.logDataList;
        startTime.text = logDataList.logData[0].time;
        exitTime.text = logDataList.logData[logDataList.logData.Count - 1].time;
        ConvertTime();
        ChangeStartPoint(0);
        ChangeEndPoint(0);
    }

    //�ð��� �ʴ����� ȯ��, InputField �� max,min ����
    public void ConvertTime()
    {
        startPoint.text = startTime.text;
        exitPoint.text = exitTime.text;

        oriStartAllSec = ConvertingSeceond(startPoint.text);
        oriExitAllSec = ConvertingSeceond(exitPoint.text);

        WriteStartTime();
        WriteExitTime();
    }

    //�ʴ��� ȯ��
    public int ConvertingSeceond(string time)
    {
        string[] timeSplit = time.Split(':');

        int hour = int.Parse(timeSplit[0]);
        int min = int.Parse(timeSplit[1]);
        int sec = int.Parse(timeSplit[2]);

        int secTime = hour * 3600 + min * 60 + sec;

        return secTime;
    }

    //���� �ð� ���� - ������ �ð� ���� ���� �ð��� Ž������ ���ϰ� ����
    public void WriteStartTime()
    {
        string time = startPoint.text;
        string[] timeSplit = time.Split(':');

        int count = timeSplit.Length - 1;

        if (count == 2)
        {
            startHour = int.Parse(timeSplit[0]);
            startMin = int.Parse(timeSplit[1]);
            startSec = int.Parse(timeSplit[2]);

            startAllSec = startHour * 3600 + startMin * 60 + startSec;
        }
        else
        {
            startPoint.text = startTime.text;
            backPanel.SetActive(true);

            Monitoring_LogManager.instance.uIManager_Log.ErrorMessageOn("���� �� �� �Է��ϼ̽��ϴ�.");
        }
    }

    //���� �ð� ���� - ������ �ð� ���� ���� �ð��� Ž������ ���ϰ� ����
    public void WriteExitTime()
    {
        string time = exitPoint.text;
        string[] timeSplit = time.Split(':');

        int count = timeSplit.Length - 1;

        if (count == 2)
        {
            exitHour = int.Parse(timeSplit[0]);
            exitMin = int.Parse(timeSplit[1]);
            exitSec = int.Parse(timeSplit[2]);
            exitAllSec = exitHour * 3600 + exitMin * 60 + exitSec;
        }
        else
        {
            exitPoint.text = exitTime.text;
            backPanel.SetActive(true);
            Monitoring_LogManager.instance.uIManager_Log.ErrorMessageOn("���� �� �� �Է��ϼ̽��ϴ�.");
        }
    }

    //���� ���� ���� - �� ������ ���� �ð����� ���� �ð����δ� �� ����. �� ���� ������ �� ���� ���� Ŀ������ ����.
    public void ChangeStartPoint(int count)
    {
        startAllSec += count;

        if (startAllSec < oriStartAllSec)
        {
            startAllSec = oriStartAllSec;
        }
        if (startAllSec >= exitAllSec)
        {
            startAllSec = exitAllSec - 1;
        }

        startHour = startAllSec / 3600;
        startMin = (startAllSec % 3600 )/ 60;
        startSec = (startAllSec % 3600 )% 60;

        string hTime = startHour.ToString();
        string mTime = startMin.ToString();
        string sTime = startSec.ToString();

        if (startHour < 10)
        {
            hTime = "0" + hTime;
        }
        if (startMin < 10)
        {
            mTime = "0" + mTime;
        }
        if (startSec < 10)
        {
            sTime = "0" + sTime;
        }

        startPoint.text = hTime + ":" + mTime + ":" + sTime;

        int forStart = startAllSec - oriStartAllSec;
        int forEnd = exitAllSec - oriStartAllSec;

        //player ���� ����
        player.GetComponent<LineRenderer>().positionCount = forEnd - forStart;

        player.transform.position = new Vector3(logDataList.logData[forStart].pos.x, 1, logDataList.logData[forStart].pos.y);

        int forCount = 0;

        for (int i = forStart; i < forEnd; i++)
        {
            if (player.GetComponent<LineRenderer>().positionCount > 0)
            {
                Vector3 nowPos = new Vector3(logDataList.logData[i].pos.x, 1, logDataList.logData[i].pos.y);

                player.GetComponent<LineRenderer>().SetPosition(forCount, nowPos);
                forCount++;
            }
        }
    }

    //�� ���� ���� - �� ������ ���� �ð����� ū �ð����δ� �� ����. �� ���� ������ ���� ���� ���� �۾��� ���� ����.
    public void ChangeEndPoint(int count)
    {
        exitAllSec += count;

        if (exitAllSec > oriExitAllSec)
        {
            exitAllSec = oriExitAllSec;
        }
        if (exitAllSec <= startAllSec)
        {
            exitAllSec = startAllSec + 1;
        }

        exitHour = exitAllSec / 3600;
        exitMin = (exitAllSec % 3600) / 60;
        exitSec = (exitAllSec % 3600) % 60;

        string hTime = exitHour.ToString();
        string mTime = exitMin.ToString();
        string sTime = exitSec.ToString();

        if (exitHour < 10)
        {
            hTime = "0" + hTime;
        }
        if (exitMin < 10)
        {
            mTime = "0" + mTime;
        }
        if (exitSec < 10)
        {
            sTime = "0" + sTime;
        }

        exitPoint.text = hTime + ":" + mTime + ":" + sTime;

        int forStart = startAllSec - oriStartAllSec;
        int forEnd = exitAllSec - oriStartAllSec;

        player.transform.position = new Vector3(logDataList.logData[forStart].pos.x, 1, logDataList.logData[forStart].pos.y);

        //player ���� ����
        player.GetComponent<LineRenderer>().positionCount = forEnd - forStart;

        int forCount = 0;

        for (int i = forStart; i < forEnd; i++)
        {
            if (player.GetComponent<LineRenderer>().positionCount > 0)
            {
                Vector3 nowPos = new Vector3(logDataList.logData[i].pos.x, 1, logDataList.logData[i].pos.y);

                player.GetComponent<LineRenderer>().SetPosition(forCount, nowPos);
                forCount++;
            }
        }
    }
}
