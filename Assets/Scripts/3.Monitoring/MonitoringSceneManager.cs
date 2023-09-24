using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using TMPro;

public class MonitoringSceneManager : MonoBehaviour
{
    public string pubkey;

    public static MonitoringSceneManager instance;

    public GameObject model;

    public FileLoad fileLoad;
    public UserManager userManager;
    public ReceiveUserData receiveUserData;
    public SetUserInformation setUserInformation;
    public UserCrashManager userCrashManager;
    public ReceiveMultiData receiveMultiData;
    public UserSelect userSelect;

    public string title = "Title";
    public string mapCleint = "MapClient";
    public string monitoringLog = "Monitoring_Log";

    public string url = "https://192.168.0.100:5000";

    public GameObject backGround;   //특정 동작 이외에 클릭 못하게 하는 배경
    public GameObject errorMessage;
    public TMP_Text errorMessageText;

    public GameObject moveCamera;

    public void Start()
    {
        if (instance == null)
        {
            instance = this;
        }

        //model = GameObject.Find("Model");

        if (model != null)
        {
            model.transform.position = Vector3.zero;
            //fileLoad.GymSet.SetActive(true);
        }

        title = "Title";
        mapCleint = "MapClient";
        monitoringLog = "Monitoring_Log";

        pubkey = NetworkManager.instance.pubkey;
        url = NetworkManager.instance.url;

        ////인증서 키 생성
        //TextAsset tx = Resources.Load<TextAsset>("day");
        //byte[] by = Encoding.UTF8.GetBytes(tx.ToString());

        //X509Certificate2 certificate = new X509Certificate2(by);
        //pubkey = certificate.GetPublicKeyString();
    }

    public void LoadScene(int index)
    {
        model = GameObject.Find("Model");

        switch (index)
        {
            case 0:
                SceneManager.LoadScene(title);

                if (model != null)
                {
                    Destroy(model);
                }
                break;

            case 1:
                SceneManager.LoadScene(mapCleint);

                if (model != null)
                {
                    Destroy(model);
                }
                break;

            case 2:

                if (model == null)
                {
                    ErrorMessageOn("파일 선택 후 클릭해주세요.");
                }
                else
                {
                    DontDestroyOnLoad(model);
                    SceneManager.LoadScene(monitoringLog);
                }
                break;

            default:
                break;
        }
    }

    public void ErrorMessageOn(string message)
    {
        errorMessageText.text = message;
        errorMessage.SetActive(true);
        backGround.SetActive(true);
    }

    public void ErrorMessageOff()
    {
        errorMessage.SetActive(false);
        backGround.SetActive(false);
    }

}
