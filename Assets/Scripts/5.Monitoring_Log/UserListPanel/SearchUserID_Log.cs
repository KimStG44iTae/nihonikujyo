using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using System.Text;

public class SearchUserID_Log : MonoBehaviour
{
    //���� �̸� �Է� �ǳ�
    public GameObject userNameInputPanel;
    public TMP_InputField userNameText;
    public TMP_Text selectUserNameText;

    public UserList_Log search_UserList;
    public List<UserID_Log_Info> search_UserInfoList = new List<UserID_Log_Info>();

    //�̸� �˻� ui on/off
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

    //�˻��� ���� �̸����� ��������Ʈ �ʱ�ȭ(�������� ����)
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
            Monitoring_LogManager.instance.uIManager_Log.ErrorMessageOn("�˻��� �̸��� �����ϴ�.");
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
