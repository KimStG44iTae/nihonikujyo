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

    //�ð��� �ʴ����� ȯ��, �����̴� �� max,min �ʱ�ȭ
    public void ConvertTime()
    {
        oriStartAllSec = ConvertingSeceond(startTime.text);
        oriExitAllSec = ConvertingSeceond(exitTime.text);

        timeSlider.minValue = oriStartAllSec;
        timeSlider.maxValue = oriExitAllSec;

        timeSlider.value = oriStartAllSec;

        nowtAllSec = oriStartAllSec;
    }

    //�ð��� �ʴ����� ȯ��
    public int ConvertingSeceond(string time)
    {
        string[] timeSplit = time.Split(':');

        int hour = int.Parse(timeSplit[0]);
        int min = int.Parse(timeSplit[1]);
        int sec = int.Parse(timeSplit[2]);

        int secTime = hour * 3600 + min * 60 + sec;

        return secTime;
    }

    //�����̴��� �� ���� �ɶ� ���� ��ġ ȣ�� �� ���� �ð� ����
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

    //�ʴ��� �ð��� �ٽ� �ð����� ȯ�� �� ����, �ð��� ���� ��ġ ����
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
