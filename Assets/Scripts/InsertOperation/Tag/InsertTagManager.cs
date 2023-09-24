using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using System.Text;

public class InsertTagManager : MonoBehaviour
{
    public TagList tagList;

    public TMP_InputField tagIdText;
    public TMP_InputField idText;

    public TMP_InputField writeTagIdField;
    public TMP_InputField registerTagIdField;

    public string url;
    public string pubkey;

    //태그 등록, 검색, 삭제 부분 초기화
    public void ResetTagManager()
    {
        tagList = InsertOperationManager.instance.insertTagList.tagList;

        tagIdText.text = "";
        idText.text = "";
        writeTagIdField.text = "";
        registerTagIdField.text = "";
    }

    //태그 삭제 inputfield 세팅
    public void SetTagID(string tagId)
    {
        tagList = InsertOperationManager.instance.insertTagList.tagList;

        writeTagIdField.text = tagId;
    }

    //태그ID로 태그 리스트 검색
    public void SearchTagID()
    {
        tagList = InsertOperationManager.instance.insertTagList.tagList;

        if (tagList != null)
        {
            for (int i = 0; i < tagList.tagList.Length; i++)
            {
                if (tagList.tagList[i].tagID == tagIdText.text)
                {
                    writeTagIdField.text = tagList.tagList[i].tagID;
                    InsertOperationManager.instance.insertUIManager.SearchTagPageOff();
                    return;
                }
            }
        }

        //태그 id가 없으면 에러 메세지
        InsertOperationManager.instance.insertUIManager.ErrorMessageOn("태그 id가 없습니다.");
    }

    //ID로 리스트 겁색
    public void SearchID()
    {
        tagList = InsertOperationManager.instance.insertTagList.tagList;

        if (tagList != null)
        {
            for (int i = 0; i < tagList.tagList.Length; i++)
            {
                if (tagList.tagList[i].ID.ToString() == idText.text)
                {
                    writeTagIdField.text = tagList.tagList[i].tagID;
                    InsertOperationManager.instance.insertUIManager.SearchTagPageOff();
                    return;
                }
            }
        }
        //ID가 없으면 에러 메세지
        InsertOperationManager.instance.insertUIManager.ErrorMessageOn("id가 없습니다.");
    }

    //입력 값 장싱인지 반환 true/false
    public bool IsCollectStyle(string input)
    {
        //if (input.Length == 11)
        if (input.Length != 16) //동래 전용
        {
            //동래 전용
            InsertOperationManager.instance.insertUIManager.ErrorMessageOn("글자수가 맞지않습니다. \n(ex: ABCDEF0123456789)");
            return false;
            
            //string[] splitText = input.Split('-');

            //if (splitText.Length != 4)
            //{
            //    InsertOperationManager.instance.insertUIManager.ErrorMessageOn("입력 형식이 맞지않습니다. \n(ex: 12-34-56-78)");
            //    return false;
            //}

            //for (int i = 0; i < splitText.Length; i++)
            //{
            //    if (splitText[i].Length != 2)
            //    {
            //        InsertOperationManager.instance.insertUIManager.ErrorMessageOn("입력 형식이 맞지않습니다. \n(ex: 12-34-56-78)");
            //        return false;
            //    }
            //}
        }
        //else
        //{
        //    InsertOperationManager.instance.insertUIManager.ErrorMessageOn("글자수가 맞지않습니다. \n(ex: 12-34-56-78)");
        //    return false;
        //}

        //if ((input.Length != 11) || (input.Length != 23))
        //{
        //    InsertOperationManager.instance.insertUIManager.ErrorMessageOn("글자수가 맞지않습니다. \n(ex: 12-34-56-78)");
        //    return false;
        //}

        return true;
    }

    //태그 삭제 요청
    public void DeleteTag()
    {
        if (writeTagIdField.text == "")
        {
            return;
        }

        if (!IsCollectStyle(writeTagIdField.text))
        {
            return;
        }

        url = InsertOperationManager.instance.url;
        pubkey = InsertOperationManager.instance.pubkey;

        StartCoroutine(SendDeleteTag());
    }

    //태그 삭제 요청 - 태그id를 파라미터로 전송
    public IEnumerator SendDeleteTag()
    {
        string sendUrl = url + "/Management/DeleteUWB/" + writeTagIdField.text;

        UnityWebRequest www = UnityWebRequest.Get(sendUrl);

        www.certificateHandler = new CertPublicKey { PUB_KEY = pubkey };

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            byte[] result = www.downloadHandler.data;

            string results = Encoding.UTF8.GetString(result);

            //전송한 태그id가 없으면 에러 메세지
            if (results == "empty")
            {
                InsertOperationManager.instance.insertUIManager.ErrorMessageOn("태그ID가 없습니다.");
            }
            else if (results == "used")
            {
                InsertOperationManager.instance.insertUIManager.ErrorMessageOn("태그ID를 현재 사용중입니다.");
            }
            else if (results == "error")
            {
                InsertOperationManager.instance.insertUIManager.ErrorMessageOn("서버 에러(태그 삭제)");
            }
            else
            {
                InsertOperationManager.instance.insertTagList.ReceiveTagData();
            }
        }
        else
        {
            InsertOperationManager.instance.insertUIManager.ErrorMessageOn("서버 연결 에러");
        }
    }

    //태그 등록 요청
    public void RegisterTag()
    {
        if (registerTagIdField.text == "")
        {
            return;
        }

        if (!IsCollectStyle(registerTagIdField.text))
        {
            return;
        }

        url = InsertOperationManager.instance.url;
        pubkey = InsertOperationManager.instance.pubkey;

        StartCoroutine(SendRegisterTag());
    }

    //태그 등록 요청 - 태그id를 파라미터로 전송
    public IEnumerator SendRegisterTag()
    {
        string sendUrl = url + "/Management/InsertUWB/" + registerTagIdField.text;

        UnityWebRequest www = UnityWebRequest.Get(sendUrl);

        www.certificateHandler = new CertPublicKey { PUB_KEY = pubkey };

        yield return www.SendWebRequest();

        byte[] result = www.downloadHandler.data;

        string results = Encoding.UTF8.GetString(result);

        //전송한 태그id가 중복되면 에러 메세지
        if (results == "duplicated")
        {
            InsertOperationManager.instance.insertUIManager.ErrorMessageOn("태그ID가 중복되었습니다.");
        }
        else
        {
            InsertOperationManager.instance.insertTagList.ReceiveTagData();
        }
    }
}
