using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactSearchInfo : MonoBehaviour
{
    public string userId;
    public string userName;
    public string birth_day;

    //������ ���� ������ ������ �ð��� ���� ui���� ����
    public void SelectUser()
    {
        Monitoring_LogManager.instance.calendarSearchUser.SetName(this);
    }
}
