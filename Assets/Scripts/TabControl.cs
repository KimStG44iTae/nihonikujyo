using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

//InputField 이벤트  - tab 눌렀을때 다음 InputField로 이동
public class TabControl : MonoBehaviour
{
    public Selectable next;
    public bool isStart;
    public Selectable end;

    public void Start()
    {
        GetComponent<TMP_InputField>().onSelect.AddListener((s => CheckInputTab()));    //InputField 선택시 이벤트
        GetComponent<TMP_InputField>().onEndEdit.AddListener((s => EndInputEnter()));   //InputField 입력 끝날시 이벤트
    }

    public void CheckInputTab()
    {
        isStart = true;
        StartCoroutine(CheckTab());
    }

    //InputField에서 tab 눌렀는지 계속 확인
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
