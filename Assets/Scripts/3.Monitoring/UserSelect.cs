using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using UnityEngine.UI;

public class UserSelect : MonoBehaviour
{
    public Camera cam;

    public GameObject playerData;

    public SetUserInformation setUserInformation;

    float weight;

    [DllImport("__Internal")]
    private static extern void SendId(string str);

    public GameObject selectCam;

    public bool isMouseInUi;

    private void Start()
    {
        playerData.SetActive(false);
    }

    public void StartSendId(string message)
    {
#if !UNITY_EDITOR
        SendId(message);
#endif
    }

    //유저 선택 후 속성 창 띄움
    void Update()
    {
        if (Screen.fullScreen)
        {
            weight = 1f;

        }
        else
        {
            weight = 1.5f;
        }

        Ray ray = cam.ScreenPointToRay(Input.mousePosition * weight);
        RaycastHit hit;

        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out hit))
            {
                if (isMouseInUi)
                {
                    return;
                }

                if (hit.transform.tag == "Player")
                {
                    playerData.SetActive(true);
                    setUserInformation.SetInformationAll(hit.transform.GetComponent<UserInfo>());
                }
                else if (hit.transform.tag == "CCTV")
                {
                    if (selectCam != null)
                    {
                        selectCam.transform.GetComponent<OutLineInfo>().OutLineOff();
                    }
                    selectCam = hit.transform.gameObject;
                    hit.transform.GetComponent<OutLineInfo>().OutLineOn();
                    StartSendId(hit.transform.name);
                }
                else
                {

                    playerData.SetActive(false);

                    if (selectCam != null)
                    {
                        selectCam.transform.GetComponent<OutLineInfo>().OutLineOff();
                    }

                    StartSendId("reset");
                    playerData.SetActive(false);
                }
            }
            else
            {
                if (!isMouseInUi)
                {
                    if (selectCam != null)
                    {
                        selectCam.transform.GetComponent<OutLineInfo>().OutLineOff();
                    }
                    StartSendId("reset");

                    playerData.SetActive(false);
                }
                else
                {

                }

            }
        }
    }

    public void OutLineOff()
    {
        print("OutLinsOff");
        if (selectCam != null)
        {
            selectCam.transform.GetComponent<OutLineInfo>().OutLineOff();
            selectCam = null;
        }
    }

    public void OutLineOn(string ip)
    {
        print(ip);
        if (selectCam != null)
        {
            selectCam.transform.GetComponent<OutLineInfo>().OutLineOff();
        }

        GameObject tempCam = GameObject.Find(ip);

        if (tempCam != null)
        {
            selectCam = tempCam;
            selectCam.transform.GetComponent<OutLineInfo>().OutLineOn();
        }
    }
}
