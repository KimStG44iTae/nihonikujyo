using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactSearchInfo : MonoBehaviour
{
    public string userId;
    public string userName;
    public string birth_day;

    //선택한 유저 정보로 오른쪽 시간별 동선 ui정보 세팅
    public void SelectUser()
    {
        Monitoring_LogManager.instance.calendarSearchUser.SetName(this);
    }
}
