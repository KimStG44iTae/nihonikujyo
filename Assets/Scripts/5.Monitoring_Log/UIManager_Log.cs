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

    //Ư�� ���� �߿� �ٸ� ui �ǵ帮�� ���ϰ� �ϴ� �� on / off
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

    //�ð��� ���� Ȯ�� on / off
    public void TimePosPanelOn()
    {
        OffSelect();
       // Monitoring_LogManager.instance.tutorial.StartShow("��¥�� ������ �� �����̴� �� �Ǵ� ��ư�� �̿��� �ð��� ��ġ�� Ȯ���� �� �ֽ��ϴ�." ,1);
        timePosPanel.SetActive(true);
        TimeRangePanelOff();
        buttonTimePos.OnSelect();

    }
    public void TimePosPanelOff()
    {
        buttonTimePos.OnDeSelect();
        timePosPanel.SetActive(false);
    }

    //�ð��� ���� ���� ���� Ȯ�� on / off
    public void TimeRangePanelOn()
    {
        OffSelect();
       // Monitoring_LogManager.instance.tutorial.StartShow("��¥�� ������ �� �� �Է� �Ǵ� ��ư�� �̿��Ͽ� �ð� �������� ������ Ȯ���� �� �ֽ��ϴ�.", 1);
        timeRangePanel.SetActive(true);
        TimePosPanelOff();
        buttonTimeRange.OnSelect();

    }
    public void TimeRangePanelOff()
    {
        buttonTimeRange.OnDeSelect();
        timeRangePanel.SetActive(false);
    }

    //������ Ȯ���ϱ� ���� UI on / off
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

    //���˸���Ʈ�� Ȯ���ϱ� ���� UI on / off
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
         //   Monitoring_LogManager.instance.tutorial.StartShow("�Ʒ��� ���� ��ư�� ������ �̸� �˻��� �����ϰ� ��¥�� �����Ͽ� ������ ������ �� �Ϸ��ư�� Ŭ���Ѵ�.", 1);
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

    //3d ��ȯ �� ���� ���� ��ġ�� ���� ���� ui on / off
    public void Mode3DOn()
    {
        OffSelect();

        //  Monitoring_LogManager.instance.tutorial.StartShow("WASD�� �̵�, Q,E�� �� �Ʒ� �̵��� �����ϸ� ������ �ǳ��� ���� ������ ��ȸ�� �� �ִ�.", 1);
        ContactListPanelOff();
        UserListPanelOff();
        button3D.SetActive(false);
        button2D.SetActive(true);
        objCamera.GetComponent<CameraMove3D>().enabled = true;
        is2D = false;
    }

    //2d ��ȯ �� ����Ʈ�� ���� ���� ui on / off
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
