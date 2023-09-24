using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager_Log : MonoBehaviour
{
    public GameObject timePosPanel; 
    public GameObject timeRangePanel;
    public GameObject userListPanel;
    public GameObject userContactPanel;

    public GameObject objCamera;

    public GameObject button3D;
    public GameObject button2D;

    public ButtonInteraction buttonTimePos;
    public ButtonInteraction buttonTimeRange;

    public GameObject contactButton;
    public GameObject logButton;

    public static UIManager_Log instance;

    public bool isLogStatus;

    public GameObject backGroundPanel;

    public GameObject errorMessage;
    public TMP_Text errorMessageText;

    public bool is2D;

    //특정 동작 중에 다른 ui 건드리지 못하게 하는 벽 on / off
    public void BackGroundOn()
    {
        backGroundPanel.SetActive(true);
    }

    public void BackGroundOff()
    {
        backGroundPanel.SetActive(false);
    }

    public void ErrorMessageOn(string message)
    {
        BackGroundOn();
        errorMessageText.text = message;
        errorMessage.SetActive(true);
    }


    public void ErrorMessageOff()
    {  
        BackGroundOff();
        errorMessage.SetActive(false);
    }


    public void Start()
    {
        isLogStatus = true;
        is2D = true;

        buttonTimePos.OnSelect();

        if (instance == null)
        {
            instance = this;
        }
    }

    public void OffSelect()
    {
        ButtonInteraction[] buttonInteractions = GameObject.FindObjectsOfType<ButtonInteraction>();

        for (int i = 0; i < buttonInteractions.Length; i++)
        {
            if (buttonInteractions[i].isActive && (buttonInteractions[i].buttonException != ButtonInteraction.ButtonException.On))
            {
                buttonInteractions[i].isActive = false;
                buttonInteractions[i].OnDeSelect();

                break;
            }
        }
    }

    //시간별 동선 확인 on / off
    public void TimePosPanelOn()
    {
        OffSelect();
       // Monitoring_LogManager.instance.tutorial.StartShow("날짜를 선택한 후 슬라이더 바 또는 버튼을 이용해 시간별 위치를 확인할 수 있습니다." ,1);
        timePosPanel.SetActive(true);
        TimeRangePanelOff();
        buttonTimePos.OnSelect();

    }
    public void TimePosPanelOff()
    {
        buttonTimePos.OnDeSelect();
        timePosPanel.SetActive(false);
    }

    //시간대 범위 지정 동선 확인 on / off
    public void TimeRangePanelOn()
    {
        OffSelect();
       // Monitoring_LogManager.instance.tutorial.StartShow("날짜를 선택한 후 값 입력 또는 버튼을 이용하여 시간 범위안의 동선을 확인할 수 있습니다.", 1);
        timeRangePanel.SetActive(true);
        TimePosPanelOff();
        buttonTimeRange.OnSelect();

    }
    public void TimeRangePanelOff()
    {
        buttonTimeRange.OnDeSelect();
        timeRangePanel.SetActive(false);
    }

    //동선을 확인하기 위한 UI on / off
    public void UserListPanelOn()
    {
        OffSelect();

        if (!is2D)
        {
            button3D.SetActive(true);
            button2D.SetActive(false);
            objCamera.GetComponent<CameraMove3D>().enabled = false;
            is2D = true;
        }

        contactButton.SetActive(true);
        logButton.SetActive(false);
        userListPanel.SetActive(true);
        ContactListPanelOff();
    }
    public void UserListPanelOff()
    {
        userListPanel.SetActive(false);
    }

    //접촉리스트를 확인하기 위한 UI on / off
    public void ContactListPanelOn()
    {
        OffSelect();

        if (!is2D)
        {
            button3D.SetActive(true);
            button2D.SetActive(false);
            objCamera.GetComponent<CameraMove3D>().enabled = false;
            is2D = true;
        }

        if (!objCamera.GetComponent<CameraMove3D>().enabled)
        {
         //   Monitoring_LogManager.instance.tutorial.StartShow("아래의 왼쪽 버튼을 누르면 이름 검색이 가능하고 날짜를 선택하여 범위를 설정한 후 완료버튼을 클릭한다.", 1);
        }
        contactButton.SetActive(false);
        logButton.SetActive(true);
        userContactPanel.SetActive(true);
        UserListPanelOff();
        isLogStatus = false;
    }

    public void ContactListPanelOff()
    {
        userContactPanel.SetActive(false);
    }

    //3d 전환 후 실제 모델의 위치를 보기 위한 ui on / off
    public void Mode3DOn()
    {
        OffSelect();

        //  Monitoring_LogManager.instance.tutorial.StartShow("WASD로 이동, Q,E로 위 아래 이동이 가능하며 오른쪽 판넬을 통해 동선을 조회할 수 있다.", 1);
        ContactListPanelOff();
        UserListPanelOff();
        button3D.SetActive(false);
        button2D.SetActive(true);
        objCamera.GetComponent<CameraMove3D>().enabled = true;
        is2D = false;
    }

    //2d 전환 후 리스트를 보기 위한 ui on / off
    public void Mode3DOff()
    {
        OffSelect();

        if (isLogStatus)
        {
            UserListPanelOn();
        }
        else
        {
            ContactListPanelOn();
        }

        button3D.SetActive(true);
        button2D.SetActive(false);
        objCamera.GetComponent<CameraMove3D>().enabled = false;
        is2D = true;

    }
}
