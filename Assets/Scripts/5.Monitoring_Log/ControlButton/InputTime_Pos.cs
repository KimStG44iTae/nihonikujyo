using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InputTime_Pos : MonoBehaviour
{
    public TMP_Text startTime;
    public TMP_Text exitTime;
    public TMP_Text nowTime;

    public int oriStartAllSec;
    public int oriExitAllSec;

    public int nowHour;
    public int nowMin;
    public int nowSec;

    public int nowtAllSec;

    public Slider timeSlider;

    public LogDataList logDataList;

    public GameObject player;

    public Image stopImage;
    public Image playImage;

    public Sprite stopInactive;
    public Sprite stopActive;

    public Sprite playInactive;
    public Sprite playActive;

    public void OnEnable()
    {
        player.GetComponent<LineRenderer>().enabled = false;
    }

    private void Start()
    {
        ConvertTime();
    }

    public void SetTime()
    {
        logDataList = Monitoring_LogManager.instance.receiveLogData.logDataList;
        startTime.text = logDataList.logData[0].time;
        exitTime.text = logDataList.logData[logDataList.logData.Count-1].time;
        ConvertTime();
    }

    //시간을 초단위로 환산, 슬라이더 바 max,min 초기화
    public void ConvertTime()
    {
        oriStartAllSec = ConvertingSeceond(startTime.text);
        oriExitAllSec = ConvertingSeceond(exitTime.text);

        timeSlider.minValue = oriStartAllSec;
        timeSlider.maxValue = oriExitAllSec;

        timeSlider.value = oriStartAllSec;

        nowtAllSec = oriStartAllSec;
    }

    //시간을 초단위로 환산
    public int ConvertingSeceond(string time)
    {
        string[] timeSplit = time.Split(':');

        int hour = int.Parse(timeSplit[0]);
        int min = int.Parse(timeSplit[1]);
        int sec = int.Parse(timeSplit[2]);

        int secTime = hour * 3600 + min * 60 + sec;

        return secTime;
    }

    //슬라이더바 값 수정 될때 마다 위치 호출 및 현재 시간 세팅
    public void ChangeSliderValue()
    {
        nowtAllSec = (int)timeSlider.value;
        CalculateTime();
    }


    public void ChangeNowPoint(int count)
    {
        nowtAllSec += count;

        if (nowtAllSec >= oriExitAllSec)
        {
            nowtAllSec = oriExitAllSec;
        }

        if (nowtAllSec <= oriStartAllSec)
        {
            nowtAllSec = oriStartAllSec;
        }
        CalculateTime();

        timeSlider.value = nowtAllSec;
    }

    //초단위 시간을 다시 시간으로 환산 후 세팅, 시간에 따른 위치 세팅
    public void CalculateTime()
    {
        nowHour = nowtAllSec / 3600;
        nowMin = (nowtAllSec % 3600) / 60;
        nowSec = (nowtAllSec % 3600) % 60;

        string hTime = nowHour.ToString();
        string mTime = nowMin.ToString();
        string sTime = nowSec.ToString();

        if (nowHour < 10)
        {
            hTime = "0" + hTime;
        }
        if (nowMin < 10)
        {
            mTime = "0" + mTime;
        }
        if (nowSec < 10)
        {
            sTime = "0" + sTime;
        }

        nowTime.text = hTime + ":" + mTime + ":" + sTime;

        int index = nowtAllSec - oriStartAllSec;

        if (logDataList.logData.Count > 0)
        {
            Vector3 nowPos = new Vector3(logDataList.logData[index].pos.x, 1, logDataList.logData[index].pos.y);

            player.transform.position = nowPos;
        }
    }

    public bool isPlay;

    public void StartPlayButton()
    {
        isPlay = true;

        playImage.sprite = playActive;
        stopImage.sprite = stopInactive;

        StartCoroutine(StartPlay());
    }

    public void EndPlayButton()
    {
        playImage.sprite = playInactive;
        stopImage.sprite = stopActive;
        isPlay = false;
    }

    IEnumerator StartPlay()
    {
        yield return  null;

        for (int i = nowtAllSec; i < oriExitAllSec; i++)
        {
            if (!isPlay)
            {
                break;
            }
            timeSlider.value++;
            yield return new WaitForSeconds(0.1f);
        }

        playImage.sprite = playInactive;
        stopImage.sprite = stopActive;

        isPlay = false;

    }
}
