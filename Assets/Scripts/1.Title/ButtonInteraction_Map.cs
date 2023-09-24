using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ButtonInteraction_Map : MonoBehaviour
{
    public enum ButtonType
    {
        Image, Text
    };

    Image myImage;

    TMP_Text myText;

    public Sprite active;
    public Sprite hover;
    public Sprite inactive;

    public bool isActive;

    public ButtonType buttonType;

    public void Start()
    {
        EventTrigger eventTrigger = gameObject.AddComponent<EventTrigger>();

        myImage = transform.GetChild(0).GetComponent<Image>();

        if (buttonType == ButtonType.Text)
        {
            myText = transform.GetChild(1).GetComponent<TMP_Text>();
        }

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerEnter;
        entry.callback.AddListener((data) => { OnPointerEnter(); });
        eventTrigger.triggers.Add(entry);

        EventTrigger.Entry entry3 = new EventTrigger.Entry();
        entry3.eventID = EventTriggerType.PointerExit;
        entry3.callback.AddListener((data) => { OnPointerExit(); });
        GetComponent<EventTrigger>().triggers.Add(entry3);

        GetComponent<Button>().onClick.AddListener(() => ButtonClick());

    }

    //public void OffSelect()
    //{
    //    ButtonInteraction[] buttonInteractions = GameObject.FindObjectsOfType<ButtonInteraction>();

    //    for (int i = 0; i < buttonInteractions.Length; i++)
    //    {
    //        if (buttonInteractions[i].isActive)
    //        {
    //            buttonInteractions[i].isActive = false;
    //            buttonInteractions[i].OnDeSelect();

    //            break;
    //        }
    //    }
    //}

    public void ButtonClick()
    {
      //  OffSelect();

        isActive = true;
        myImage.sprite = active;

        if (buttonType == ButtonType.Text)
        {
            Color color;
            ColorUtility.TryParseHtmlString("#F47E75", out color);
            myText.color = color;
        }
    }

    void OnDeSelect()
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

    public void OnPointerEnter()
    {
        if (isActive)
        {
            return;
        }
        myImage.sprite = hover;
    }

    void OnPointerExit()
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
