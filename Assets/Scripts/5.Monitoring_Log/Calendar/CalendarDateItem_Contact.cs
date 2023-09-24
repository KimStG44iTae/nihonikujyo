using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
public class CalendarDateItem_Contact : MonoBehaviour {

    //달력에서 클릭한 날짜를 접촉리스트 달력으로 전송
    public void OnDateItemClick()
    {
        CalendarController_Contact._calendarInstance.OnDateItemClick(gameObject);
    }
}
