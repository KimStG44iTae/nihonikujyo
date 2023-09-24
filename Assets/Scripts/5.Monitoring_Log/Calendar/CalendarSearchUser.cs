using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//���˸���Ʈ���� �̸� �˻��� ���� ��ũ��Ʈ
public class CalendarSearchUser : MonoBehaviour
{
    public TMP_InputField inputUserName;

    public GameObject inputNamePanel;
    public GameObject userListPanel;

    public GameObject contentPosition;

    public GameObject searchUserListPrefeb;

    public List<GameObject> userListObject = new List<GameObject>();

    public UserList_Log userList;

    //�̸� �˻� ui on/off
    public void inputNamePanelOn()
    {
        inputUserName.text = "";
        inputNamePanel.SetActive(true);
    }

    public void inputNamePanelOff()
    {
        inputNamePanel.SetActive(false);
    }

    //�̸� ����Ʈ ui on/off
    public void userListPanelOn()
    {
        userListPanel.SetActive(true);
        inputNamePanelOff();
        SearchUserName();
    }

    public void userListPanelOff()
    {
        userListPanel.SetActive(false);
    }

    //��������Ʈ���� �˻��� �̸��� �ش��ϴ�(�������� ����) ����� ����Ʈ ����
    public void SearchUserName()
    {
        print("tt");
        for (int i = userListObject.Count - 1; i >= 0; i--)
        {
            Destroy(userListObject[i]);
        }

        userListObject.Clear();
        userList.userLogList.Clear();

        List<GameObject> tempObjectList = Monitoring_LogManager.instance.userID_Log.userIDObjectList;
        UserList_Log tempList = Monitoring_LogManager.instance.userID_Log.userList;

        for (int i = 0; i < tempObjectList.Count; i++)
        {
            if (inputUserName.text == tempObjectList[i].GetComponent<SearchUserInfo_Log>().userName)
            {
                userList.userLogList.Add(tempList.userLogList[i]);
            }
        }

        if (userList.userLogList.Count == 0)
        {
            Monitoring_LogManager.instance.uIManager_Log.ErrorMessageOn("�˻��� �̸��� �����ϴ�.");
        }
        else
        {
            RectTransform contextSize = contentPosition.GetComponent<RectTransform>();

            contextSize.sizeDelta = new Vector2(contextSize.sizeDelta.x, (100 * userList.userLogList.Count));

            for (int i = 0; i < userList.userLogList.Count; i++)
            {
                GameObject tempUserInfo = Instantiate(searchUserListPrefeb);

                tempUserInfo.transform.SetParent(contentPosition.transform);

                tempUserInfo.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -50 - 100 * i, 0);
                tempUserInfo.GetComponent<RectTransform>().sizeDelta = new Vector2(contextSize.sizeDelta.x, 100);

                tempUserInfo.GetComponent<RectTransform>().localScale = Vector3.one;
                userListObject.Add(tempUserInfo);

                tempUserInfo.transform.GetChild(0).GetComponent<TMP_Text>().text = userList.userLogList[i].userID;
                tempUserInfo.transform.GetChild(1).GetComponent<TMP_Text>().text = userList.userLogList[i].name;
                tempUserInfo.transform.GetChild(2).GetComponent<TMP_Text>().text = userList.userLogList[i].birth_day;


                tempUserInfo.transform.GetComponent<ContactSearchInfo>().userId = userList.userLogList[i].userID;
                tempUserInfo.transform.GetComponent<ContactSearchInfo>().name = userList.userLogList[i].name;
                tempUserInfo.transform.GetComponent<ContactSearchInfo>().birth_day = userList.userLogList[i].birth_day;
            }
        }

    }

    //�̸��� �����ϸ� ���˸���Ʈ �޷¿� �̸� ����
    public void SetName(ContactSearchInfo info)
    {
        Monitoring_LogManager.instance.calendarController_Contact.SetName(info);
        userListPanelOff();
    }
}
