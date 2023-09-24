using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InsertWatchInfo : MonoBehaviour
{
    public int watchID;
    public string mac_addr;

    public TMP_Text watchIDText;
    public TMP_Text mac_addrText;

    public void SetText()
    {
        watchIDText.text = watchID.ToString();
        mac_addrText.text = mac_addr;
    }

    //������ ��ġ�� inputfield�� ���� ��û
    public void SelectWatch()
    {
        InsertOperationManager.instance.insertWatchManager.SetWatchID(mac_addr);
    }
}
