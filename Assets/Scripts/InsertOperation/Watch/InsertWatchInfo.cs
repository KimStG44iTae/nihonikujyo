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

    //선택한 워치를 inputfield에 세팅 요청
    public void SelectWatch()
    {
        InsertOperationManager.instance.insertWatchManager.SetWatchID(mac_addr);
    }
}
