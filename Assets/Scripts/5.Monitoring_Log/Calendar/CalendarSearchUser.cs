using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//접촉리스트에서 이름 검색을 위한 스크립트
public class CalendarSearchUser : MonoBehaviour
{
    public TMP_InputField inputUserName;

    public GameObject inputNamePanel;
    public GameObject userListPanel;

    public GameObject contentPosition;

    public GameObject searchUserListPrefeb;

    public List<GameObject> userListObject = new List<GameObject>();

    public UserList_Log userList;

    //이름 검색 ui on/off
    public void inputNamePanelOn()
    {
        inputUserName.text = "";
        inputNamePanel.SetActive(true);
    }

    public void inputNamePanelOff()
    {
        inputNamePanel.SetActive(false);
    }

    //이름 리스트 ui on/off
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

    //유저리스트에서 검색한 이름에 해당하는(동명이인 포함) 사람들 리스트 생성
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
            Monitoring_LogManager.instance.uIManager_Log.ErrorMessageOn("검색한 이름이 없습니다.");
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

    //이름을 선택하면 접촉리스트 달력에 이름 세팅
    public void SetName(ContactSearchInfo info)
    {
        Monitoring_LogManager.instance.calendarController_Contact.SetName(info);
        userListPanelOff();
    }
}
