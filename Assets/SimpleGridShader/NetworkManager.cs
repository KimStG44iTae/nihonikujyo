using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Networking;

public class NetworkManager : MonoBehaviour
{
    public string pubkey;

    public static NetworkManager instance;

    public string url;

    public TMP_InputField inputUrl;
    public TMP_Text nowUrl;

    public void Start()
    {
        if (instance == null)
        {
            instance = this;
        }

        //인증서 키 생성
        TextAsset tx = Resources.Load<TextAsset>("day");
        byte[] by = Encoding.UTF8.GetBytes(tx.ToString());

        X509Certificate2 certificate = new X509Certificate2(by);
        pubkey = certificate.GetPublicKeyString();

        url = "https://192.168.0.100:5000";
        nowUrl.text = url;

        DontDestroyOnLoad(gameObject);
    }

    public void ChangeURL()
    {
        url = inputUrl.text;
        nowUrl.text = url; 
    }

    //사용자 정보 수신
    public void ResetUserData()
    {
        StartCoroutine(StartResetUserData());
    }

    //사용자 정보 수신 후 반영
    public IEnumerator StartResetUserData()
    {
        string sendUrl = url + "/UserRefresh";
        UnityWebRequest www = UnityWebRequest.Get(sendUrl);
        www.certificateHandler = new CertPublicKey { PUB_KEY = pubkey };

        print(sendUrl);

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            InsertOperationManager.instance.insertUIManager.ErrorMessageOn("서버 연결 에러");
        }

        yield return null;
    }

}
