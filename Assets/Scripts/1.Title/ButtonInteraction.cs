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

    public Sprite active; //������ ����
    public Sprite hover;
    public Sprite inactive;

    public bool isActive;

    public ButtonType buttonType; //��ư Ÿ�� ���ϴ� ���ε� �˻��غ���
    public ButtonException buttonException; // ��ư����? �̰ŵ� �˻�

    public void Start()
    {
        EventTrigger eventTrigger = gameObject.AddComponent<EventTrigger>();

        myImage = transform.GetChild(0).GetComponent<Image>();

        if (buttonType == ButtonType.Text) //��ư Ÿ���� text�� ���� ���ٸ�
        {
            myText = transform.GetChild(1).GetComponent<TMP_Text>();
        }

        EventTrigger.Entry entry = new EventTrigger.Entry(); //�̺�Ʈ Ʈ���� ��Ʈ��? �Ӥ��ŵ� ���� �˻� �� ���غ���
        entry.eventID = EventTriggerType.PointerEnter; 
        entry.callback.AddListener((data) => { OnPointerEnter(); });
        eventTrigger.triggers.Add(entry);

        EventTrigger.Entry entry3 = new EventTrigger.Entry();
        entry3.eventID = EventTriggerType.PointerExit;
        entry3.callback.AddListener((data) => { OnPointerExit(); });
        GetComponent<EventTrigger>().triggers.Add(entry3);

        if (buttonType != ButtonType.Hover) //��ư Ÿ���� ȣ���� �����ʴٸ�
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
