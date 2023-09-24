using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchUserInfo : MonoBehaviour
{
    public string userId;
    public string userName;
    public string birth_day;

    public void SelectUser()
    {
        OperationSceneManager.instance.searchUserID.SetUserID(userId);
    }
}
