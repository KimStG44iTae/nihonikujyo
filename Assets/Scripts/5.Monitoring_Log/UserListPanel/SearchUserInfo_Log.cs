using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchUserInfo_Log : MonoBehaviour
{
    public string userId;
    public string userName;
    public string birth_day;

    //���� ������ ��������Ʈ�� ȣ��
    public void SelectUser()
    {
        Monitoring_LogManager.instance.userID_Log.SetUserID(this);
    }
}
