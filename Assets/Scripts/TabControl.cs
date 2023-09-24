using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

//InputField �̺�Ʈ  - tab �������� ���� InputField�� �̵�
public class TabControl : MonoBehaviour
{
    public Selectable next;
    public bool isStart;
    public Selectable end;

    public void Start()
    {
        GetComponent<TMP_InputField>().onSelect.AddListener((s => CheckInputTab()));    //InputField ���ý� �̺�Ʈ
        GetComponent<TMP_InputField>().onEndEdit.AddListener((s => EndInputEnter()));   //InputField �Է� ������ �̺�Ʈ
    }

    public void CheckInputTab()
    {
        isStart = true;
        StartCoroutine(CheckTab());
    }

    //InputField���� tab �������� ��� Ȯ��
    public IEnumerator CheckTab()
    {
        while(true)
        {
            if (isStart == false)
            {
                break;
            }

            yield return null;
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                next.Select();
                isStart = false;
                break;
            }
        }
    }

    public void EndInputEnter()
    {
        isStart = false; 
        end.Select();
    }
    
}
