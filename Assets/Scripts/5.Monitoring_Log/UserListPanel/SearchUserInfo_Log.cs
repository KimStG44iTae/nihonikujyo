using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchUserInfo_Log : MonoBehaviour
{
    public string userId;
    public string userName;
    public string birth_day;

    //유저 선택후 유저리스트에 호출
    public void SelectUser()
    {
        Monitoring_LogManager.instance.userID_Log.SetUserID(this);
    }
}
