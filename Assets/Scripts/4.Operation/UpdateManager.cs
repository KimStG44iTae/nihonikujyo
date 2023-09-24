using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Text;

public class UpdateManager : MonoBehaviour
{
    //field값
    public TMP_InputField inTimeField;
    public TMP_InputField outTimeField;
    public TMP_InputField tagIdField;
    public TMP_InputField watchIDField;
    public TMP_InputField memberField;

    string url = "";
    //string url = "http://192.168.0.10:5000";
    public string pubkey;

    public string selectTagID;

    //회원 정보 값 반영
    public void SetMemberInformation(MemberInfomation memberData)
    {
        inTimeField.text = memberData.entry_time;
        outTimeField.text = memberData.exit_time;
        tagIdField.text = memberData.tagID;
        watchIDField.text = memberData.watchID.ToString();
        memberField.text = memberData.userID.ToString();

        selectTagID = memberData.tagID;
    }

    //회원 리스트에서 선택한 회원 수정 요청
    public void SendMemberData()
    {
        url = OperationSceneManager.instance.url;
        pubkey = OperationSceneManager.instance.pubkey;

        StartCoroutine(SendData());
    }

    //회원 리스트에서 선택한 회원 수정 요청
    public IEnumerator SendData()
    {
        string sendUrl = url + "/Management/userUpdate/" + selectTagID;

        MemberInfomation memberData = new MemberInfomation();
        memberData.entry_time = inTimeField.text;
        memberData.exit_time = outTimeField.text;
        memberData.tagID = tagIdField.text;
        memberData.watchID = int.Parse(watchIDField.text);
        memberData.userID = int.Parse(memberField.text);

        string jsonString = JsonUtility.ToJson(memberData);
        print(jsonString);

        byte[] jsonbyte = Encoding.UTF8.GetBytes(jsonString);

        UnityWebRequest www = UnityWebRequest.Post(sendUrl, jsonString);

        www.uploadHandler = new UploadHandlerRaw(jsonbyte);
        www.SetRequestHeader("Content-Type", "application/json");
        www.certificateHandler = new CertPublicKey { PUB_KEY = pubkey };

        yield return www.SendWebRequest();

        string s = Encoding.UTF8.GetString(www.downloadHandler.data);
        print(s);
        www.Dispose();

        OperationSceneManager.instance.memberListManager.ReceiveMemberData();

        ResetMemberInformation();
    }

    //회원 리스트에서 선택한 회원 삭제 요청
    public void DeleteMemberData()
    {
        url = OperationSceneManager.instance.url;
        pubkey = OperationSceneManager.instance.pubkey;


        StartCoroutine(DeleteData());
    }

    //회원 리스트에서 선택한 회원 삭제 요청
    public IEnumerator DeleteData()
    {
        if ((inTimeField.text != "") && (tagIdField.text != "") && (watchIDField.text != "") && (memberField.text != ""))
        {
            string sendUrl = url + "/Management/userDelete/" + selectTagID;

            MemberInfomation memberData = new MemberInfomation();
            memberData.entry_time = inTimeField.text;
            memberData.exit_time = DateTime.Now.ToString("yyyy-MM-dd'/'HH:mm:ss");
            memberData.tagID = tagIdField.text;
            memberData.watchID = int.Parse(watchIDField.text);
            memberData.userID = int.Parse(memberField.text);

            string jsonString = JsonUtility.ToJson(memberData);

            byte[] jsonbyte = Encoding.UTF8.GetBytes(jsonString);

            UnityWebRequest www = UnityWebRequest.Post(sendUrl, jsonString);
            www.certificateHandler = new CertPublicKey { PUB_KEY = pubkey };

            www.uploadHandler = new UploadHandlerRaw(jsonbyte);
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                byte[] result = www.downloadHandler.data;

                string resultStr = Encoding.UTF8.GetString(result);

                www.Dispose();

                OperationSceneManager.instance.operationUIManager.InsertOn();
                OperationSceneManager.instance.memberListManager.ReceiveMemberData();

                ResetMemberInformation();
            }
            else
            {
                OperationSceneManager.instance.operationUIManager.ErrorMessgaeOn("서버 연결 에러");
            }

        }

    }

    //초기화
    public void ResetMemberInformation()
    {
        inTimeField.text = "";
        outTimeField.text = "";
        tagIdField.text = "";
        watchIDField.text = "";
        memberField.text = "";

        selectTagID = "";
    }

}
