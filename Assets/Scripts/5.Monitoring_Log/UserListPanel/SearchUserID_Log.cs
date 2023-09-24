using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using System.Text;

public class SearchUserID_Log : MonoBehaviour
{
    //유저 이름 입력 판넬
    public GameObject userNameInputPanel;
    public TMP_InputField userNameText;
    public TMP_Text selectUserNameText;

    public UserList_Log search_UserList;
    public List<UserID_Log_Info> search_UserInfoList = new List<UserID_Log_Info>();

    //이름 검색 ui on/off
    public void NameInputPanelOff()
    {
        userNameInputPanel.SetActive(false);
        Monitoring_LogManager.instance.uIManager_Log.BackGroundOff();
    }

    public void NameInputPanelOn()
    {
        userNameInputPanel.SetActive(true);
        Monitoring_LogManager.instance.uIManager_Log.BackGroundOn();
    }

    //검색한 유저 이름으로 유저리스트 초기화(동명이인 포함)
    public void SetUserList()
    {
        selectUserNameText.text = userNameText.text;

        search_UserInfoList.Clear();

        UserList_Log tempUserList = Monitoring_LogManager.instance.userID_Log.userList;

        for (int i = 0; i < tempUserList.userLogList.Count; i++)
        {
            if (userNameText.text == tempUserList.userLogList[i].name)
            {
                search_UserInfoList.Add(tempUserList.userLogList[i]);
            }
        }

        if (search_UserInfoList.Count == 0)
        {
            Monitoring_LogManager.instance.uIManager_Log.ErrorMessageOn("검색한 이름이 없습니다.");
        }
        else
        {
            search_UserList.userLogList = search_UserInfoList;
            Monitoring_LogManager.instance.userID_Log.UpdateIdList(search_UserList);
        }

        NameInputPanelOff();
        userNameText.text = "";
    }
}
