using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Text;

public class RegisterManager : MonoBehaviour
{
    //field 값
    public TMP_InputField inTimeField;
    public TMP_InputField outTimeField;
    public TMP_InputField tagIdField;
    public TMP_InputField watchIDField;
    public TMP_InputField memberField;

   // public string url = "http://192.168.0.10:5000";
    string url = "";
    public string pubkey;

    //현재 시간 값 반영
    public void WriteNowTime(GameObject inputField)
    {
        inputField.GetComponent<TMP_InputField>().text = DateTime.Now.ToString("yyyy-MM-dd'/'HH:mm:ss");
    }

    //회원 등록 정보 서버로 전송
    public void SendMemberData()
    {
        url = OperationSceneManager.instance.url;
        pubkey = OperationSceneManager.instance.pubkey;

        StartCoroutine(SendData());
    }

    //회원 등록 정보 서버로 전송
    public IEnumerator SendData()
    {
        if ((inTimeField.text != "") && (tagIdField.text != "") && (watchIDField.text != "") && (memberField.text != ""))
        {
            string sendUrl = url + "/Management/userRegister";

            MemberInfomation memberData = new MemberInfomation();
            memberData.entry_time = inTimeField.text;
            memberData.exit_time = outTimeField.text;
            memberData.tagID = tagIdField.text;
            memberData.watchID = int.Parse(watchIDField.text);
            memberData.userID = int.Parse(memberField.text);

            string jsonString = JsonUtility.ToJson(memberData);

            byte[] jsonbyte = Encoding.UTF8.GetBytes(jsonString);

            UnityWebRequest www = UnityWebRequest.Post(sendUrl, jsonString);

            www.uploadHandler = new UploadHandlerRaw(jsonbyte);
            www.SetRequestHeader("Content-Type", "application/json");
            www.certificateHandler = new CertPublicKey { PUB_KEY = pubkey };

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                //string s = Encoding.UTF8.GetString(www.downloadHandler.data);

                OperationSceneManager.instance.memberListManager.ReceiveMemberData();
                InitField();
            }
            else
            {
                OperationSceneManager.instance.operationUIManager.ErrorMessgaeOn("서버 연결 에러");
            }
            www.Dispose();
        }
    }

    public void InitField()
    {
        inTimeField.text = "";
        outTimeField.text = "";
        tagIdField.text = "";
        watchIDField.text = "";
        memberField.text = "";
    }

}
