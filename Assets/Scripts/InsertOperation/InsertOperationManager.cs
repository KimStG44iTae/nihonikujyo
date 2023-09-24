using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InsertOperationManager : MonoBehaviour
{
    public static InsertOperationManager instance;

    public InsertUIManager insertUIManager;
    public InsertTagList insertTagList;
    public InsertTagManager insertTagManager;

    public InsertWatchList insertWatchList;
    public InsertWatchManager insertWatchManager;

    public string title = "Title";
    public string operation = "Operation";

    public string url;
    public string pubkey;

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

        //insertTagList.ReceiveTagData();
        //insertWatchList.ReceiveWatchData();

        pubkey = NetworkManager.instance.pubkey;
        url = NetworkManager.instance.url;
    }

    public void SceneChange(int index)
    {
        switch (index)
        {
            case 0 :
                SceneManager.LoadScene(title);
                break;
            case 1 :
                SceneManager.LoadScene(operation);
                break;
            default:
                break;
        }

    }
}
