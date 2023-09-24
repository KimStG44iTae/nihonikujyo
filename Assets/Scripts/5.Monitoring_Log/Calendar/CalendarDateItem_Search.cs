using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
public class CalendarDateItem_Search : MonoBehaviour {

    //달력에서 클릭한 날짜를 이름 검색 달력으로 전송
    public void OnDateItemClick()
    {
        CalendarController_Search._calendarInstance.OnDateItemClick(gameObject.GetComponentInChildren<TMP_Text>().text);
    }
}
