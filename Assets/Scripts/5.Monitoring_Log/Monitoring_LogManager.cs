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

        //������ Ű ����
        //TextAsset tx = Resources.Load<TextAsset>("day");
        //byte[] by = Encoding.UTF8.GetBytes(tx.ToString());

        //X509Certificate2 certificate = new X509Certificate2(by);
        //pubkey = certificate.GetPublicKeyString();

        //userList ȣ��
        userID_Log.ReceiveUserID();

        calendarController_Search.StartInit();  //��¥ �˻� �޷� �ʱ�ȭ(�߰� ��ư)
        calendarController_Contact.StartInit(); //���˸���Ʈ �޷� �ʱ�ȭ(���� ��ư)
        calendarController.StartInit();         //���� �޷� �ʱ�ȭ(������)
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
