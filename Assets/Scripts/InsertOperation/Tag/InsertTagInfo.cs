using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InsertTagInfo : MonoBehaviour
{
    public int id;
    public string tagID;

    public TMP_Text idText;
    public TMP_Text tagIdText;

    public void SetText()
    {
        idText.text = id.ToString();
        tagIdText.text = tagID;
    }

    //������ �±׸� inputfield�� ���� ��û
    public void SelectTag()
    {
        InsertOperationManager.instance.insertTagManager.SetTagID(tagID);
    }
}
