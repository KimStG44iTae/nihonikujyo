using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using System.Text;

public class CalendarController_Search : MonoBehaviour
{
    public GameObject _calendarPanel;
    public TMP_Text _yearNumText;
    public TMP_Text _monthNumText;

    public GameObject _item;

    public List<GameObject> _dateItems = new List<GameObject>();
    const int _totalDateNum = 42;

    private DateTime _dateTime;
    public static CalendarController_Search _calendarInstance;

    public Color selectColor;
    public Color selectRangeColor;

    //달력 라이브러리 초기화
    public void StartInit()
    {
        ColorUtility.TryParseHtmlString("#F47E75", out selectColor);
        ColorUtility.TryParseHtmlString("#E4E4E4", out selectRangeColor);

        _calendarInstance = this;
        Vector3 startPos = _item.transform.localPosition;
        _dateItems.Clear();
        _dateItems.Add(_item);

        for (int i = 1; i < _totalDateNum; i++)
        {
            GameObject item = GameObject.Instantiate(_item) as GameObject;
            item.name = "Item" + (i + 1).ToString();
            item.transform.SetParent(_item.transform.parent);
            item.transform.localScale = Vector3.one;
            item.transform.localRotation = Quaternion.identity;
            item.transform.localPosition = new Vector3((i % 7) * 36  + startPos.x, startPos.y - (i / 7) * 30, startPos.z);

            _dateItems.Add(item);
        }

        _dateTime = DateTime.Now;

        CreateCalendar();

        _calendarPanel.SetActive(false);
    }

    //날짜 계산후 날짜 생성
    void CreateCalendar()
    {
        DateTime firstDay = _dateTime.AddDays(-(_dateTime.Day - 1));
        int index = GetDays(firstDay.DayOfWeek);

        int date = 0;

        int count = index + 1;

        for (int i = 0; i < _totalDateNum; i++)
        {
            TMP_Text label = _dateItems[i].GetComponentInChildren<TMP_Text>();
            _dateItems[i].SetActive(false);

            if (i >= index)
            {
                DateTime thatDay = firstDay.AddDays(date);
                if (thatDay.Month == firstDay.Month)
                {
                    _dateItems[i].SetActive(true);

                    label.text = (date + 1).ToString();

                    if (count == 1)
                    {
                        label.color = selectColor;
                    }

                    if (count % 7 == 0)
                    {
                        //label.color = selectColor;
                        count = 0;
                    }

                    date++;
                    count++;
                }
            }
        }
        _yearNumText.text = _dateTime.Year.ToString();
        _monthNumText.text = _dateTime.Month.ToString("D2");
    }

    int GetDays(DayOfWeek day)
    {
        switch (day)
        {
            case DayOfWeek.Monday: return 1;
            case DayOfWeek.Tuesday: return 2;
            case DayOfWeek.Wednesday: return 3;
            case DayOfWeek.Thursday: return 4;
            case DayOfWeek.Friday: return 5;
            case DayOfWeek.Saturday: return 6;
            case DayOfWeek.Sunday: return 0;
        }

        return 0;
    }
    public void YearPrev()
    {
        _dateTime = _dateTime.AddYears(-1);
        CreateCalendar();
    }

    public void YearNext()
    {
        _dateTime = _dateTime.AddYears(1);
        CreateCalendar();
    }

    public void MonthPrev()
    {
        _dateTime = _dateTime.AddMonths(-1);
        CreateCalendar();
    }

    public void MonthNext()
    {
        _dateTime = _dateTime.AddMonths(1);
        CreateCalendar();
    }

    public void ShowCalendar(TMP_Text target)
    {
        _calendarPanel.SetActive(true);
        _target = target;
        Monitoring_LogManager.instance.uIManager_Log.BackGroundOn();  
    }

    TMP_Text _target;

    UnityWebRequest www;

    public string url;
    public string pubkey;

    //Item 클릭했을 경우 해당 날짜의 유저 리스트 호출
    public void OnDateItemClick(string day)
    {
        _target.text = _yearNumText.text + "-" + _monthNumText.text + "-" + int.Parse(day).ToString("D2");

        url = Monitoring_LogManager.instance.url;
        pubkey = Monitoring_LogManager.instance.pubkey;


        StartCoroutine(ReceiveDateData(_target.text));
    }

    //유저 리스트 호출
    public IEnumerator ReceiveDateData(string date)
    {
        string sendurl = url + "/Log/Management_Info/" + date;

        www = UnityWebRequest.Get(sendurl);
        www.certificateHandler = new CertPublicKey { PUB_KEY = pubkey };
        www.timeout = 2;

        ExitCalendar();

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            byte[] result = www.downloadHandler.data;

            string resultstr = Encoding.UTF8.GetString(result);

            if (resultstr == "empty")
            {
                Monitoring_LogManager.instance.uIManager_Log.ErrorMessageOn("해당 날짜에 들어온 사람이 없습니다.");
            }
            else if (resultstr == "error")
            {
                Monitoring_LogManager.instance.uIManager_Log.ErrorMessageOn("서버 에러(날짜 검색).");
            }
            else
            {
                UserList_Log tempList = JsonUtility.FromJson<UserList_Log>(resultstr);
                //받은 데이터로 유저리스트 초기화
                Monitoring_LogManager.instance.userID_Log.UpdateIdList(tempList);
            }
        }
        else
        {
            Monitoring_LogManager.instance.uIManager_Log.ErrorMessageOn("서버 연결 에러");
        }

        www.Dispose();
    }

    //달력 off
    public void ExitCalendar()
    {
        _calendarPanel.SetActive(false);
        Monitoring_LogManager.instance.uIManager_Log.BackGroundOff();
    }
}
