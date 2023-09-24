using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Security.Cryptography.X509Certificates;
using System.Text;

public class OperationSceneManager : MonoBehaviour
{
    public static OperationSceneManager instance;

    public MemberListManager memberListManager;
    public UpdateManager updateManager;
    public RegisterManager registerManager;
    public OperationUIManager operationUIManager;

    public SearchTagID searchTagID;
    public SearchWatchID searchWatchID;
    public SearchUserID searchUserID;

    public string title = "Title";
    public string insertOperation = "InsertOperation";

    public string pubkey;
    public string url = "";

    public void Start()
    {
        if (instance == null)
        {
            instance = this;
        }

        //인증서 키 생성
        //TextAsset tx = Resources.Load<TextAsset>("day");
        //byte[] by = Encoding.UTF8.GetBytes(tx.ToString());

        //X509Certificate2 certificate = new X509Certificate2(by);
        //pubkey = certificate.GetPublicKeyString();

        // memberListManager.ReceiveMemberData();

        pubkey = NetworkManager.instance.pubkey;
        url = NetworkManager.instance.url;
    }

    public void LoadScene(int index)
    {
        switch (index)
        {
            case 0:
                SceneManager.LoadScene(title);
                break;

            case 1:
                SceneManager.LoadScene(insertOperation);
                break;

            default:
                break;
        }
    }

}
