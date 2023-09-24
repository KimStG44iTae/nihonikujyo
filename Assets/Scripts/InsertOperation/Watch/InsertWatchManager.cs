using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using System.Text;

public class InsertWatchManager : MonoBehaviour
{
    public WatchList watchList;

    public TMP_InputField watchIdText;
    public TMP_InputField mac_addrText;

    public TMP_InputField writeWatchIdField;
    public TMP_InputField registerWatchIdField;

    public string url;
    public string pubkey;

    //워치 등록, 검색, 삭제 부분 초기화
    public void ResetWatchManager()
    {
        watchList = InsertOperationManager.instance.insertWatchList.watchList;

        watchIdText.text = "";
        mac_addrText.text = "";
        writeWatchIdField.text = "";
        registerWatchIdField.text = "";
    }

    //워치 삭제 inputfield 세팅
    public void SetWatchID(string tagId)
    {
        watchList = InsertOperationManager.instance.insertWatchList.watchList;

        writeWatchIdField.text = tagId;
    }

    //워치ID로 워치 리스트 검색
    public void SearchWatchID()
    {
        watchList = InsertOperationManager.instance.insertWatchList.watchList;

        if (watchList != null)
        {
            for (int i = 0; i < watchList.watchList.Length; i++)
            {
                if (watchList.watchList[i].watchID.ToString() == watchIdText.text)
                {
                    writeWatchIdField.text = watchList.watchList[i].mac_addr;
                    InsertOperationManager.instance.insertUIManager.SearchWatchPageOff();
                    return;
                }
            }
        }

        //워치 id가 없으면 에러 메세지
        InsertOperationManager.instance.insertUIManager.ErrorMessageOn("워치 id가 없습니다.");
    }

    //워치 mac 주소로 리스트 겁색
    public void SearchMac_addr()
    {
        watchList = InsertOperationManager.instance.insertWatchList.watchList;

        if (watchList != null)
        {
            for (int i = 0; i < watchList.watchList.Length; i++)
            {
                if (watchList.watchList[i].mac_addr.ToString() == mac_addrText.text)
                {
                    writeWatchIdField.text = watchList.watchList[i].mac_addr;
                    InsertOperationManager.instance.insertUIManager.SearchWatchPageOff();
                    return;
                }
            }
        }
        //워치 mac 주소가 없으면 에러 메세지
        InsertOperationManager.instance.insertUIManager.ErrorMessageOn("워치 Mac 주소가 없습니다.");
    }

    //입력 값 장싱인지 반환 true/false
    public bool IsCollectStyle(string input)
    {
        if (input.Length == 16)
        {
            //string[] splitText = input.Split(':');

            //if (splitText.Length != 6)
            //{
            //    InsertOperationManager.instance.insertUIManager.ErrorMessageOn("입력 형식이 맞지않습니다. \n(ex: 12:34:56:78:90:10)");
            //    return false;
            //}

            //for (int i = 0; i < splitText.Length; i++)
            //{
            //    if (splitText[i].Length != 2)
            //    {
            //        InsertOperationManager.instance.insertUIManager.ErrorMessageOn("입력 형식이 맞지않습니다. \n(ex: 12:34:56:78:90:10)");
            //        return false;
            //    }
            //}
        }
        else
        {
            InsertOperationManager.instance.insertUIManager.ErrorMessageOn("글자수가 맞지않습니다(16글자). \n(ex: 0123456789ABCDEF)");
            //InsertOperationManager.instance.insertUIManager.ErrorMessageOn("글자수가 맞지않습니다. \n(ex: 12:34:56:78:90:10)");
            return false;
        }

        return true;
    }


    //워치 삭제 요청
    public void DeleteWatch()
    {
        if (writeWatchIdField.text == "")
        {
            return;
        }

        if (!IsCollectStyle(writeWatchIdField.text))
        {
            return;
        }

        url = InsertOperationManager.instance.url;
        pubkey = InsertOperationManager.instance.pubkey;

        StartCoroutine(SendDeleteWatch());

        ResetWatchManager();
    }

    //워치 삭제 요청 - 워치 mac 주소를 파라미터로 전송
    public IEnumerator SendDeleteWatch()
    {
        string sendUrl = url + "/Management/DeleteWatch/" + writeWatchIdField.text;

        UnityWebRequest www = UnityWebRequest.Get(sendUrl);

        www.certificateHandler = new CertPublicKey { PUB_KEY = pubkey };

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            byte[] result = www.downloadHandler.data;

            string results = Encoding.UTF8.GetString(result);

            //전송한 워치 mac 주소가 없으면 에러 메세지
            if (results == "empty")
            {
                InsertOperationManager.instance.insertUIManager.ErrorMessageOn("워치 mac 주소가 없습니다.");
            }
            else if (results == "used")
            {
                InsertOperationManager.instance.insertUIManager.ErrorMessageOn("워치 mac 주소를\n현재 사용중입니다.");
            }
            else if (results == "error")
            {
                InsertOperationManager.instance.insertUIManager.ErrorMessageOn("서버 에러(워치 삭제)");
            }
            else
            {
                InsertOperationManager.instance.insertWatchList.ReceiveWatchData();
            }
        }
        else
        {
            InsertOperationManager.instance.insertUIManager.ErrorMessageOn("서버 연결 에러");
        }

    }

    //워치 등록 요청
    public void RegisterWatch()
    {
        if (registerWatchIdField.text == "")
        {
            return;
        }

        if (!IsCollectStyle(registerWatchIdField.text))
        {
            return;
        }

        url = InsertOperationManager.instance.url;
        pubkey = InsertOperationManager.instance.pubkey;

        StartCoroutine(SendWatchRegister());

        ResetWatchManager();

    }

    //워치 등록 요청 - 워치 mac 주소를 파라미터로 전송
    public IEnumerator SendWatchRegister()
    {
        string sendUrl = url + "/Management/InsertWatch/" + registerWatchIdField.text;

        UnityWebRequest www = UnityWebRequest.Get(sendUrl);

        www.certificateHandler = new CertPublicKey { PUB_KEY = pubkey };

        yield return www.SendWebRequest();

        byte[] result = www.downloadHandler.data;

        string results = Encoding.UTF8.GetString(result);

        //전송한 워치 mac 주소가 중복되면 에러 메세지
        if (results == "duplicated")
        {
            InsertOperationManager.instance.insertUIManager.ErrorMessageOn("워치 mac 주소가 중복되었습니다.");
        }
        else if (results == "used") 
        {
            InsertOperationManager.instance.insertUIManager.ErrorMessageOn("워치 mac 주소를 현재 사용중입니다.");
        }
        else
        {
            InsertOperationManager.instance.insertWatchList.ReceiveWatchData();
        }
    }
}
