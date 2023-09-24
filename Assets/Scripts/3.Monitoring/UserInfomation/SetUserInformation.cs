using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SetUserInformation : MonoBehaviour
{
    public TMP_Text user;
    public TMP_Text zone;
    public TMP_Text pos;
    public TMP_Text heartRate;
    public TMP_Text motion;
    public TMP_Text startTime;
    public TMP_Text vaccine;

    public UserInfo userInfo;

    public GameObject fallingButton;

    public bool isFalling;

    //ui text ����
    public void SetInformationAll(UserInfo userObject)
    {
        if (isFalling)
        {
            fallingButton.SetActive(true);
        }
        else
            fallingButton.SetActive(false);

        userInfo = userObject;

        user.text = userInfo.userID.ToString();
        zone.text = userInfo.zone;
        pos.text = userInfo.pos.ToString();
        heartRate.text = userInfo.heartRate;

        SetInformationMotion();

        startTime.text = userInfo.startTime;

        vaccine.text = userInfo.vaccine;

        if (userInfo.isFalling)
        {
            isFalling = true;
            fallingButton.SetActive(true);
        }
        //else
        //{
        //    fallingButton.SetActive(false);
        //}
    }

    public void FallingCheckEnd()
    {
        isFalling = false;
        userInfo.fallingCheck = true;
        fallingButton.SetActive(false);
        userInfo.dangerousMark.SetActive(false);
    }

    //������ ���� ��ġ,uwb,zone ����
    public void Update()
    {
        SetInformationWatch();
        SetInformationUWB();
        SetInformationZone();
        SetInformationMotion();
    }

    //��ġ ���� ������Ʈ
    public void SetInformationWatch()
    {
        heartRate.text = userInfo.heartRate;
    }

    //uwb ���� ������Ʈ
    public void SetInformationUWB()
    {
        pos.text = userInfo.pos.ToString();
    }

    //zone ���� ������Ʈ
    public void SetInformationZone()
    {
        zone.text = userInfo.zone;
    }

    //��� ���� ������Ʈ
    public void SetMultiData(UserInfo userObject)
    {
        userInfo = userObject;

        zone.text = userInfo.zone;
        pos.text = userInfo.pos.ToString();
        heartRate.text = userInfo.heartRate;
        SetInformationMotion();
        //motion.text = userInfo.motion;

        if (userInfo.isFalling)
        {
            isFalling = true;
            fallingButton.SetActive(true);
        }
        else
        {
            //fallingButton.SetActive(false);
        }
    }

    public void SetInformationMotion()
    {
        switch (userInfo.motion)
        {
            case "0":
                motion.text = "�з��ܵ���";
                break;
            case "10":
                motion.text = "��ġ������-����";
                break;
            case "11":
                motion.text = "��ġ������-������1";
                break;
            case "12":
                motion.text = "��ġ������-������2";
                break;
            case "13":
                motion.text = "��ġ������-������3";
                break;
            case "20":
                motion.text = "���������-����";
                break;
            case "21":
                motion.text = "���������-������1";
                break;
            case "22":
                motion.text = "���������-������2";
                break;
            case "23":
                motion.text = "���������-������3";
                break;
            case "30":
                motion.text = "���帮��Ʈ-����";
                break;
            case "31":
                motion.text = "���帮��Ʈ-������1";
                break;
            case "32":
                motion.text = "���帮��Ʈ-������2";
                break;
            case "33":
                motion.text = "���帮��Ʈ-������3";
                break;
            case "40":
                motion.text = "����Ʈ-����";
                break;
            case "41":
                motion.text = "����Ʈ-������1";
                break;
            case "42":
                motion.text = "����Ʈ-������2";
                break;
            case "43":
                motion.text = "����Ʈ-������3";
                break;

            default:
                motion.text = "������";
                break;
        }
    }

}
