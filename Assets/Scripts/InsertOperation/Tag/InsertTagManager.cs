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

    //�±� ���, �˻�, ���� �κ� �ʱ�ȭ
    public void ResetTagManager()
    {
        tagList = InsertOperationManager.instance.insertTagList.tagList;

        tagIdText.text = "";
        idText.text = "";
        writeTagIdField.text = "";
        registerTagIdField.text = "";
    }

    //�±� ���� inputfield ����
    public void SetTagID(string tagId)
    {
        tagList = InsertOperationManager.instance.insertTagList.tagList;

        writeTagIdField.text = tagId;
    }

    //�±�ID�� �±� ����Ʈ �˻�
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

        //�±� id�� ������ ���� �޼���
        InsertOperationManager.instance.insertUIManager.ErrorMessageOn("�±� id�� �����ϴ�.");
    }

    //ID�� ����Ʈ �̻�
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
        //ID�� ������ ���� �޼���
        InsertOperationManager.instance.insertUIManager.ErrorMessageOn("id�� �����ϴ�.");
    }

    //�Է� �� ������� ��ȯ true/false
    public bool IsCollectStyle(string input)
    {
        //if (input.Length == 11)
        if (input.Length != 16) //���� ����
        {
            //���� ����
            InsertOperationManager.instance.insertUIManager.ErrorMessageOn("���ڼ��� �����ʽ��ϴ�. \n(ex: ABCDEF0123456789)");
            return false;
            
            //string[] splitText = input.Split('-');

            //if (splitText.Length != 4)
            //{
            //    InsertOperationManager.instance.insertUIManager.ErrorMessageOn("�Է� ������ �����ʽ��ϴ�. \n(ex: 12-34-56-78)");
            //    return false;
            //}

            //for (int i = 0; i < splitText.Length; i++)
            //{
            //    if (splitText[i].Length != 2)
            //    {
            //        InsertOperationManager.instance.insertUIManager.ErrorMessageOn("�Է� ������ �����ʽ��ϴ�. \n(ex: 12-34-56-78)");
            //        return false;
            //    }
            //}
        }
        //else
        //{
        //    InsertOperationManager.instance.insertUIManager.ErrorMessageOn("���ڼ��� �����ʽ��ϴ�. \n(ex: 12-34-56-78)");
        //    return false;
        //}

        //if ((input.Length != 11) || (input.Length != 23))
        //{
        //    InsertOperationManager.instance.insertUIManager.ErrorMessageOn("���ڼ��� �����ʽ��ϴ�. \n(ex: 12-34-56-78)");
        //    return false;
        //}

        return true;
    }

    //�±� ���� ��û
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

    //�±� ���� ��û - �±�id�� �Ķ���ͷ� ����
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

            //������ �±�id�� ������ ���� �޼���
            if (results == "empty")
            {
                InsertOperationManager.instance.insertUIManager.ErrorMessageOn("�±�ID�� �����ϴ�.");
            }
            else if (results == "used")
            {
                InsertOperationManager.instance.insertUIManager.ErrorMessageOn("�±�ID�� ���� ������Դϴ�.");
            }
            else if (results == "error")
            {
                InsertOperationManager.instance.insertUIManager.ErrorMessageOn("���� ����(�±� ����)");
            }
            else
            {
                InsertOperationManager.instance.insertTagList.ReceiveTagData();
            }
        }
        else
        {
            InsertOperationManager.instance.insertUIManager.ErrorMessageOn("���� ���� ����");
        }
    }

    //�±� ��� ��û
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

    //�±� ��� ��û - �±�id�� �Ķ���ͷ� ����
    public IEnumerator SendRegisterTag()
    {
        string sendUrl = url + "/Management/InsertUWB/" + registerTagIdField.text;

        UnityWebRequest www = UnityWebRequest.Get(sendUrl);

        www.certificateHandler = new CertPublicKey { PUB_KEY = pubkey };

        yield return www.SendWebRequest();

        byte[] result = www.downloadHandler.data;

        string results = Encoding.UTF8.GetString(result);

        //������ �±�id�� �ߺ��Ǹ� ���� �޼���
        if (results == "duplicated")
        {
            InsertOperationManager.instance.insertUIManager.ErrorMessageOn("�±�ID�� �ߺ��Ǿ����ϴ�.");
        }
        else
        {
            InsertOperationManager.instance.insertTagList.ReceiveTagData();
        }
    }
}
