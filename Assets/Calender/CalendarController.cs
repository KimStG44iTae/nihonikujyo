using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CalendarController : MonoBehaviour
{
    public GameObject _calendarPanel;
    public TMP_Text _yearNumText;
    public TMP_Text _monthNumText;

    public GameObject _item;

    public List<GameObject> _dateItems = new List<GameObject>();
    const int _totalDateNum = 42;

    private DateTime _dateTime;
    public static CalendarController _calendarInstance;

    public Color selectColor;

    public Color deSelectColor;

    //달력 라이브러리 초기화
    public void StartInit()
    {
        ColorUtility.TryParseHtmlString("#F47E75", out selectColor);
        ColorUtility.TryParseHtmlString("#2A2A2A", out deSelectColor);

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
            item.transform.localPosition = new Vector3((i % 7) * 36 + startPos.x, startPos.y - (i / 7) * 30, startPos.z);

            _dateItems.Add(item);
        }

        _dateTime = DateTime.Now;

        CreateCalendar();

        // _calendarPanel.SetActive(false);
    }

    void CreateCalendar()
    {
        DateTime firstDay = _dateTime.AddDays(-(_dateTime.Day - 1));
        int index = GetDays(firstDay.DayOfWeek);

        _yearNumText.text = _dateTime.Year.ToString();
        _monthNumText.text = _dateTime.Month.ToString("D2");

        int date = 0;

        int count = index+1;

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
                    _dateItems[i].GetComponent<Image>().color = Color.white;

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

                    count++;
                    date++;

                    string tempDate = _yearNumText.text + "-" + _monthNumText.text + "-" + date.ToString("D2");

                    if (tempDate == dayText)
                    {
                        _dateItems[i].GetComponent<Image>().color = selectColor;
                        label.color = deSelectColor;
                    }
                }
            }
        }

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
        //_calendarPanel.transform.position = new Vector3(965, 475, 0);//Input.mousePosition-new Vector3(0,120,0);
    }

    TMP_Text _target;

    //Item 클릭했을 경우 Text에 표시.
    public void OnDateItemClick(string day)
    {
        //_target.text = _yearNumText.text + "-" + _monthNumText.text + "-" + int.Parse(day).ToString("D2");
        dayText = _yearNumText.text + "-" + _monthNumText.text + "-" + int.Parse(day).ToString("D2");

        //   ExitCalendar();
        Monitoring_LogManager.instance.receiveLogData.StartReceiveRecord(dayText);
        //Monitoring_LogManager.instance.receiveLogData.StartReceiveRecord(_target.text);
        CreateCalendar();
    }

    GameObject selectObject;

    string dayText = "";

    public void SelectDate(GameObject select)
    {
        return;
        if (selectObject != null)
        {
            selectObject.GetComponent<Image>().color = Color.white;
        }

        selectObject = select;
        selectObject.GetComponent<Image>().color = selectColor;
    }

    public void ExitCalendar()
    {
        _calendarPanel.SetActive(false);
        Monitoring_LogManager.instance.uIManager_Log.BackGroundOff();
    }
}
