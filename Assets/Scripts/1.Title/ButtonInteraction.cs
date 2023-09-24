using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ButtonInteraction : MonoBehaviour
{
    public enum ButtonType
    {
        Image, Text, Hover
    };

    public enum ButtonException
    {
        Off, On
    };


    Image myImage;

    TMP_Text myText;

    public Sprite active; //아이콘 지정
    public Sprite hover;
    public Sprite inactive;

    public bool isActive;

    public ButtonType buttonType; //버튼 타입 정하는 거인듯 검색해볼것
    public ButtonException buttonException; // 버튼예외? 이거도 검색

    public void Start()
    {
        EventTrigger eventTrigger = gameObject.AddComponent<EventTrigger>();

        myImage = transform.GetChild(0).GetComponent<Image>();

        if (buttonType == ButtonType.Text) //버튼 타입이 text의 값과 같다면
        {
            myText = transform.GetChild(1).GetComponent<TMP_Text>();
        }

        EventTrigger.Entry entry = new EventTrigger.Entry(); //이벤트 트리거 엔트리? ㅣㅇ거도 내일 검색 조 ㅁ해볼것
        entry.eventID = EventTriggerType.PointerEnter; 
        entry.callback.AddListener((data) => { OnPointerEnter(); });
        eventTrigger.triggers.Add(entry);

        EventTrigger.Entry entry3 = new EventTrigger.Entry();
        entry3.eventID = EventTriggerType.PointerExit;
        entry3.callback.AddListener((data) => { OnPointerExit(); });
        GetComponent<EventTrigger>().triggers.Add(entry3);

        if (buttonType != ButtonType.Hover) //버튼 타입이 호버와 같지않다면
        {
            GetComponent<Button>().onClick.AddListener(() => ButtonClick());
        }
    }

    public void OffSelect()
    {
        ButtonInteraction[] buttonInteractions = GameObject.FindObjectsOfType<ButtonInteraction>();

        for (int i = 0; i < buttonInteractions.Length; i++)
        {
            if (buttonInteractions[i].isActive && (buttonInteractions[i].buttonException != ButtonException.On))
            {
                buttonInteractions[i].isActive = false;
                buttonInteractions[i].OnDeSelect();
            }
        }
    }

    public void ButtonClick()
    {
        if (isActive)
        {
            OnDeSelect();
            return;
        }
        OffSelect();
        
        OnSelect();
    }

    public void OnSelect()
    {
        isActive = true;
        myImage.sprite = active;

        if (buttonType == ButtonType.Text)
        {
            Color color;
            ColorUtility.TryParseHtmlString("#F47E75", out color);
            myText.color = color;
        }
    }

    public void OnPointerEnter()
    {
        if (isActive)
        {
            return;
        }
        myImage.sprite = hover;
    }

    public void OnDeSelect()
    {
        myImage.sprite = inactive;
        isActive = false;

        if (buttonType == ButtonType.Text)
        {
            Color color;
            ColorUtility.TryParseHtmlString("#000000", out color);
            myText.color = color;
        }
        
    }

    public void OnPointerExit()
    {
        if (isActive)
        {
            return;
        }

        myImage.sprite = inactive;

        if (buttonType == ButtonType.Text)
        {
            Color color;
            ColorUtility.TryParseHtmlString("#000000", out color);
            myText.color = color;
        }
    }


}
