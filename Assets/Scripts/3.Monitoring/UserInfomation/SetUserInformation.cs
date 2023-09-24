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

    //ui text 세팅
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

    //프레임 마다 워치,uwb,zone 수정
    public void Update()
    {
        SetInformationWatch();
        SetInformationUWB();
        SetInformationZone();
        SetInformationMotion();
    }

    //워치 정보 업데이트
    public void SetInformationWatch()
    {
        heartRate.text = userInfo.heartRate;
    }

    //uwb 정보 업데이트
    public void SetInformationUWB()
    {
        pos.text = userInfo.pos.ToString();
    }

    //zone 정보 업데이트
    public void SetInformationZone()
    {
        zone.text = userInfo.zone;
    }

    //모든 정보 업데이트
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
                motion.text = "분류외동작";
                break;
            case "10":
                motion.text = "벤치프레스-정상";
                break;
            case "11":
                motion.text = "벤치프레스-비정상1";
                break;
            case "12":
                motion.text = "벤치프레스-비정상2";
                break;
            case "13":
                motion.text = "벤치프레스-비정상3";
                break;
            case "20":
                motion.text = "숄더프레스-정상";
                break;
            case "21":
                motion.text = "숄더프레스-비정상1";
                break;
            case "22":
                motion.text = "숄더프레스-비정상2";
                break;
            case "23":
                motion.text = "숄더프레스-비정상3";
                break;
            case "30":
                motion.text = "데드리프트-정상";
                break;
            case "31":
                motion.text = "데드리프트-비정상1";
                break;
            case "32":
                motion.text = "데드리프트-비정상2";
                break;
            case "33":
                motion.text = "데드리프트-비정상3";
                break;
            case "40":
                motion.text = "스쿼트-정상";
                break;
            case "41":
                motion.text = "스쿼트-비정상1";
                break;
            case "42":
                motion.text = "스쿼트-비정상2";
                break;
            case "43":
                motion.text = "스쿼트-비정상3";
                break;

            default:
                motion.text = "비정상";
                break;
        }
    }

}
