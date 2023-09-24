using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class Monitoring_LogManager : MonoBehaviour
{
    public string pubkey;

    public static Monitoring_LogManager instance;

    public UserIDList_Log userID_Log;
    public UIManager_Log uIManager_Log;
    public ReceiveLogData receiveLogData;

    public CalendarController_Search calendarController_Search;
    public CalendarController_Contact calendarController_Contact;
    public CalendarController calendarController;
    public ContactUserList contactUserList;
    public CalendarSearchUser calendarSearchUser;

    public Tutorial tutorial;

    public InputTime inputTime;
    public InputTime_Pos inputTime_Pos;

    public GameObject model;

    public string title = "Title";
    public string monitoring = "Monitoring";

    public string url;

    public void Start()
    {
        pubkey = NetworkManager.instance.pubkey;
        url = NetworkManager.instance.url;

        if (instance == null)
        {
            instance = this;
        }

        model = GameObject.Find("Model");

        if (model != null)
        {
            model.transform.position = Vector3.zero;
        }
        title = "Title";
        monitoring = "Monitoring";

        //인증서 키 생성
        //TextAsset tx = Resources.Load<TextAsset>("day");
        //byte[] by = Encoding.UTF8.GetBytes(tx.ToString());

        //X509Certificate2 certificate = new X509Certificate2(by);
        //pubkey = certificate.GetPublicKeyString();

        //userList 호출
        userID_Log.ReceiveUserID();

        calendarController_Search.StartInit();  //날짜 검색 달력 초기화(중간 버튼)
        calendarController_Contact.StartInit(); //접촉리스트 달력 초기화(위에 버튼)
        calendarController.StartInit();         //동선 달력 초기화(오른쪽)
    }

    public void LoadScene(int index)
    {
        model = GameObject.Find("Model");

        if (model != null)
        {
            Destroy(model);
        }

        switch (index)
        {
            case 0:
                SceneManager.LoadScene(title);
                break;

            case 1:
                if (model != null)
                {
                    DontDestroyOnLoad(model);
                }
                SceneManager.LoadScene(monitoring);
                break;

            default:
                break;
        }
    }

}
