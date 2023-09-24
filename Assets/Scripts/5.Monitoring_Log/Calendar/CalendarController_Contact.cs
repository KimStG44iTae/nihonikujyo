using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using System.Text;

public class CalendarController_Contact : MonoBehaviour
{
    public GameObject _calendarPanel;
    public TMP_Text _yearNumText;
    public TMP_Text _monthNumText;

    public GameObject _item;

    public List<GameObject> _dateItems = new List<GameObject>();
    const int _totalDateNum = 42;

    private DateTime _dateTime;
    public static CalendarController_Contact _calendarInstance;

    public Color selectColor;
    public Color selectRangeColor;
    public Color deSelectColor;


    //달력 라이브러리 초기화
    public void StartInit()
    {
        ColorUtility.TryParseHtmlString("#F47E75", out selectColor);
        ColorUtility.TryParseHtmlString("#E4E4E4", out selectRangeColor);
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
            item.transform.localPosition = new Vector3((i % 7) * 36  + startPos.x, startPos.y - (i / 7) * 30, startPos.z);

            _dateItems.Add(item);
        }

        _dateTime = DateTime.Now;

        CreateCalendar();

        _calendarPanel.SetActive(false);
    }

    //현재 선택된 날짜가 범위안에 포함되어 있는지 반환
    public bool CheckDateInSelectRange(string day)
    {
        int tempYear = int.Parse(_yearNumText.text);
        int tempMonth = int.Parse(_monthNumText.text);
        int tempday = int.Parse(day);

        DateTime tempDate = new DateTime(tempYear, tempMonth, tempday);

        if (((tempDate - startDate ).Days >= 0) && ((endDate - tempDate).Days >= 0))
        {
            return true;
        }

        return false;

    }

    //달력 ui 생성 - 선택한 범위 안에 포함된 날짜는 빨간 테두리로 표시
    void CreateCalendar()
    {
        DateTime firstDay = _dateTime.AddDays(-(_dateTime.Day - 1));
        int index = GetDays(firstDay.DayOfWeek);

        int date = 0;

        int countRed = index + 1;


        _yearNumText.text = _dateTime.Year.ToString();
        _monthNumText.text = _dateTime.Month.ToString("D2");

        for (int i = 0; i < _totalDateNum; i++)
        {
            TMP_Text label = _dateItems[i].GetComponentInChildren<TMP_Text>();
            _dateItems[i].GetComponent<Image>().color = Color.white;

            _dateItems[i].SetActive(false);


            if (i >= index)
            {
                DateTime thatDay = firstDay.AddDays(date);
                if (thatDay.Month == firstDay.Month)
                {
                    _dateItems[i].SetActive(true);

                    label.text = (date + 1).ToString();
                    label.color = deSelectColor;

                    if (countRed == 1)
                    {
                        label.color = selectColor;
                    }

                    if (countRed % 7 == 0)
                    {
                        //label.color = selectColor;
                        countRed = 0;
                    }

                    countRed++;
                    date++;

                    if ((startDateText.text != "") && (endDateText.text != ""))
                    {
                        if (CheckDateInSelectRange(date.ToString()))
                        {
                            _dateItems[i].GetComponent<Image>().color = selectRangeColor;
                        }

                        if ((startYear == int.Parse(_yearNumText.text)) && (startMonth == int.Parse(_monthNumText.text)) &&
                            (startDay == date))
                        {
                            _dateItems[i].GetComponent<Image>().color = selectColor;
                            label.color = deSelectColor;

                        }

                        if ((endYear == int.Parse(_yearNumText.text)) && (endMonth == int.Parse(_monthNumText.text)) && (endday == date))
                        {
                            _dateItems[i].GetComponent<Image>().color = selectColor;
                            label.color = deSelectColor;

                        }
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

    public void ShowCalendar()
    {
        ResetDateAndName();

        Monitoring_LogManager.instance.uIManager_Log.BackGroundOn();

        _calendarPanel.SetActive(true);
    }

    public TMP_Text nameText;
    public TMP_Text startDateText;
    public TMP_Text endDateText;

    int startYear;
    int startMonth;
    int startDay;

    int endYear;
    int endMonth;
    int endday;

    int userID;

    public int count = 0;

    DateTime startDate;
    DateTime endDate;

    public TMP_Text resultText;

    //선택한 날짜가 끝 날짜보다 뒤인지 아닌지 반환
    public bool CheckSelectDate_start(string day)
    {
        int tempYear = int.Parse(_yearNumText.text);
        int tempMonth = int.Parse(_monthNumText.text);
        int tempday= int.Parse(day);

        DateTime tempDate = new DateTime(tempYear, tempMonth, tempday);

        if ((tempDate - endDate).Days > 0)
        {
            return false;
        }

        return true;
    }

    //선택한 날짜가 시작 날짜보다 앞인지 아닌지 반환
    public bool CheckSelectDate_end(string day)
    {
        int tempYear = int.Parse(_yearNumText.text);
        int tempMonth = int.Parse(_monthNumText.text);
        int tempday = int.Parse(day);

        DateTime tempDate = new DateTime(tempYear, tempMonth, tempday);

        if ((tempDate - startDate).Days < 0)
        {
            return false;
        }

        return true;
    }

    //값 초기화
    public void ResetDateAndName()
    {
        nameText.text = "";
        startDateText.text = "";
        endDateText.text = "";
        count = 0;
        CreateCalendar();
    }

    //시작 날짜 수정 시작
    public void ChangeStartDate()
    {
        if (startDateText.text != "")
        {
            count = 1;
        }
    }

    //끝 날짜 수정 시작
    public void ChangeEndDate()
    {
        if (endDateText.text != "")
        {
            count = 2;
        }
    }
   
    //날짜 클릭 및 다음 무엇을 수행해야하는지 이번에는 무슨 동작을 수행해야하는지 제어
    public void OnDateItemClick(GameObject targetObject)
    {
        string day = targetObject.GetComponentInChildren<TMP_Text>().text;
        string date = _yearNumText.text + "-" + _monthNumText.text + "-" + int.Parse(day).ToString("D2");

        //첫 클릭시에는 무조건 시작 날짜 - 시작 날짜 값 반영
        if (count == 0)
        {
            startDateText.text = date;
            startYear = int.Parse(_yearNumText.text);
            startMonth = int.Parse(_monthNumText.text);
            startDay = int.Parse(day);
            count = 2;

            startDate = new DateTime(startYear, startMonth, startDay);
        }
        else if (count == 1)    //두번째 클릭부터 시작 날짜를 수정할지 끝날짜를 선택할지 결정
        {
            if (CheckSelectDate_start(day)) //끝날짜보다 선택날짜가 앞일경우 시작날짜 수정
            {
                startDateText.text = date;
                startYear = int.Parse(_yearNumText.text);
                startMonth = int.Parse(_monthNumText.text);
                startDay = int.Parse(day);

                startDate = new DateTime(startYear, startMonth, startDay);
                count = 3;
                CreateCalendar();
            }
            else //끝날짜 보다 선택날짜가 뒤일경우 시작 날짜 수정 후 끝 날짜 초기화
            {
                for (int i = 0; i < _totalDateNum; i++)
                {
                    _dateItems[i].GetComponent<Image>().color = Color.white;
                }
                startDateText.text = date;
                startYear = int.Parse(_yearNumText.text);
                startMonth = int.Parse(_monthNumText.text);
                startDay = int.Parse(day);
                endDateText.text = "";

                startDate = new DateTime(startYear, startMonth, startDay);
                endDate = new DateTime();

                count = 2;
            }
        }
        else if (count == 2)
        {
            if (CheckSelectDate_end(day)) //시작 날짜보다 선택날짜가 뒤일경우 끝 날짜 반영
            {
                endDateText.text = date;
                endYear = int.Parse(_yearNumText.text);
                endMonth = int.Parse(_monthNumText.text);
                endday = int.Parse(day);
                count = 3;
                endDate = new DateTime(endYear, endMonth, endday);
                CreateCalendar();
            }
            else  //시작 날짜보다 선택날짜가 앞일경우 끝 날짜 수정후 시작 날짜 초기화
            {
                endDateText.text = date;
                endYear = int.Parse(_yearNumText.text);
                endMonth = int.Parse(_monthNumText.text);
                endday = int.Parse(day);
                startDateText.text = "";

                count = 1;

                startDate = new DateTime();
                endDate = new DateTime(endYear, endMonth, endday);

                for (int i = 0; i < _totalDateNum; i++)
                {
                    _dateItems[i].GetComponent<Image>().color = Color.white;
                }
            }
        }

    }

    public void SetName(ContactSearchInfo info)
    {
        nameText.text = info.name;
        userID = int.Parse(info.userId);
    }

    //날짜 범위 및 이름 선택 후 서버로 전송할 데이터 생성
    public void CompleteCalendar()
    {
        if ((startDateText.text == "") || (endDateText.text == "") || (nameText.text == "")) 
        {
            return;
        }

        SelectContactData selectContactData = new SelectContactData();
        selectContactData.userID = userID;

        //범위 안의 날짜 모두 리스트에 저장
        for (int i = 0; i < (endDate - startDate).Days +1; i++)
        {
            DateTime tempDate = startDate.AddDays(i);

            string month = tempDate.Month.ToString();
            string day = tempDate.Day.ToString();

            if (tempDate.Month < 10)
            {
                month = "0" + month;
            }
            if (tempDate.Day < 10)
            {
                day = "0" + day;
            }

            string dateStr = tempDate.Year + "-" + month + "-" + day;

            selectContactData.date.Add(dateStr);
        }

        resultText.text = nameText.text + " " + startDateText.text + " ~ " + endDateText.text;

        //서버로 데이터 전송 요청
        Monitoring_LogManager.instance.contactUserList.ReceiveContactData(selectContactData);

        ExitCalendar();
    }

    //달력 종료
    public void ExitCalendar()
    {
        _calendarPanel.SetActive(false);
        Monitoring_LogManager.instance.uIManager_Log.BackGroundOff();
    }

}

//서버로 보낼 데이터
[Serializable]
public class SelectContactData
{
    public List<string> date;

    public int userID;

    public SelectContactData()
    {
        date = new List<string>();
    }
}