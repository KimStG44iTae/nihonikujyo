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

    //��ġ ���, �˻�, ���� �κ� �ʱ�ȭ
    public void ResetWatchManager()
    {
        watchList = InsertOperationManager.instance.insertWatchList.watchList;

        watchIdText.text = "";
        mac_addrText.text = "";
        writeWatchIdField.text = "";
        registerWatchIdField.text = "";
    }

    //��ġ ���� inputfield ����
    public void SetWatchID(string tagId)
    {
        watchList = InsertOperationManager.instance.insertWatchList.watchList;

        writeWatchIdField.text = tagId;
    }

    //��ġID�� ��ġ ����Ʈ �˻�
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

        //��ġ id�� ������ ���� �޼���
        InsertOperationManager.instance.insertUIManager.ErrorMessageOn("��ġ id�� �����ϴ�.");
    }

    //��ġ mac �ּҷ� ����Ʈ �̻�
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
        //��ġ mac �ּҰ� ������ ���� �޼���
        InsertOperationManager.instance.insertUIManager.ErrorMessageOn("��ġ Mac �ּҰ� �����ϴ�.");
    }

    //�Է� �� ������� ��ȯ true/false
    public bool IsCollectStyle(string input)
    {
        if (input.Length == 16)
        {
            //string[] splitText = input.Split(':');

            //if (splitText.Length != 6)
            //{
            //    InsertOperationManager.instance.insertUIManager.ErrorMessageOn("�Է� ������ �����ʽ��ϴ�. \n(ex: 12:34:56:78:90:10)");
            //    return false;
            //}

            //for (int i = 0; i < splitText.Length; i++)
            //{
            //    if (splitText[i].Length != 2)
            //    {
            //        InsertOperationManager.instance.insertUIManager.ErrorMessageOn("�Է� ������ �����ʽ��ϴ�. \n(ex: 12:34:56:78:90:10)");
            //        return false;
            //    }
            //}
        }
        else
        {
            InsertOperationManager.instance.insertUIManager.ErrorMessageOn("���ڼ��� �����ʽ��ϴ�(16����). \n(ex: 0123456789ABCDEF)");
            //InsertOperationManager.instance.insertUIManager.ErrorMessageOn("���ڼ��� �����ʽ��ϴ�. \n(ex: 12:34:56:78:90:10)");
            return false;
        }

        return true;
    }


    //��ġ ���� ��û
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

    //��ġ ���� ��û - ��ġ mac �ּҸ� �Ķ���ͷ� ����
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

            //������ ��ġ mac �ּҰ� ������ ���� �޼���
            if (results == "empty")
            {
                InsertOperationManager.instance.insertUIManager.ErrorMessageOn("��ġ mac �ּҰ� �����ϴ�.");
            }
            else if (results == "used")
            {
                InsertOperationManager.instance.insertUIManager.ErrorMessageOn("��ġ mac �ּҸ�\n���� ������Դϴ�.");
            }
            else if (results == "error")
            {
                InsertOperationManager.instance.insertUIManager.ErrorMessageOn("���� ����(��ġ ����)");
            }
            else
            {
                InsertOperationManager.instance.insertWatchList.ReceiveWatchData();
            }
        }
        else
        {
            InsertOperationManager.instance.insertUIManager.ErrorMessageOn("���� ���� ����");
        }

    }

    //��ġ ��� ��û
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

    //��ġ ��� ��û - ��ġ mac �ּҸ� �Ķ���ͷ� ����
    public IEnumerator SendWatchRegister()
    {
        string sendUrl = url + "/Management/InsertWatch/" + registerWatchIdField.text;

        UnityWebRequest www = UnityWebRequest.Get(sendUrl);

        www.certificateHandler = new CertPublicKey { PUB_KEY = pubkey };

        yield return www.SendWebRequest();

        byte[] result = www.downloadHandler.data;

        string results = Encoding.UTF8.GetString(result);

        //������ ��ġ mac �ּҰ� �ߺ��Ǹ� ���� �޼���
        if (results == "duplicated")
        {
            InsertOperationManager.instance.insertUIManager.ErrorMessageOn("��ġ mac �ּҰ� �ߺ��Ǿ����ϴ�.");
        }
        else if (results == "used") 
        {
            InsertOperationManager.instance.insertUIManager.ErrorMessageOn("��ġ mac �ּҸ� ���� ������Դϴ�.");
        }
        else
        {
            InsertOperationManager.instance.insertWatchList.ReceiveWatchData();
        }
    }
}
