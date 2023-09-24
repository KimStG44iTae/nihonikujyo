using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FileLoadList : MonoBehaviour
{
    public List<GameObject> buttonList;

    public Button upButton;
    public Button downButton;

    public Data dataList;

    int count = 0;

    public void GetList(Data data)
    {
        dataList = data;
        SetList();
    }

    public void SetList()
    {
        ButtonOff();

        for (int i = 0; i < buttonList.Count; i++)
        {
            buttonList[i].SetActive(true);

            string filename = dataList.name[count + i];
            filename = filename.Replace(".json", "");

            buttonList[i].transform.GetChild(0).GetComponent<TMP_Text>().text = filename;
            if (count + i >= dataList.name.Length - 1)
            {
                break;
            }
        }
    }

    public void ButtonOff()
    {
        for (int i = 0; i < buttonList.Count; i++)
        {
            buttonList[i].SetActive(false);
        }
    }

    public void NextPage()
    {
        if (count+6 < dataList.name.Length)
        {
            count += buttonList.Count;
            SetList();
            ArrowColor();
        }
    }


    public void PreViewPage()
    {
        if (count > 0)
        {
            count -= buttonList.Count;
            SetList();
            ArrowColor();
        }
    }

    public void ArrowColor()
    {

        if (count + 6 >= dataList.name.Length)
        {
            Color color;
            ColorUtility.TryParseHtmlString("#E4E4E4", out color);
            downButton.transform.GetChild(0).GetComponent<TMP_Text>().color = color;
        }
        else
        {
            Color color;
            ColorUtility.TryParseHtmlString("#1A1E27", out color);
            downButton.transform.GetChild(0).GetComponent<TMP_Text>().color = color;
        }

        if (count - 6 < 0)
        {
            Color color;
            ColorUtility.TryParseHtmlString("#E4E4E4", out color);
            upButton.transform.GetChild(0).GetComponent<TMP_Text>().color = color;
        }
        else
        {
            Color color;
            ColorUtility.TryParseHtmlString("#1A1E27", out color);
            upButton.transform.GetChild(0).GetComponent<TMP_Text>().color = color;
        }
    }

    public void SendFileName_FileInOutput(TMP_Text tmpText)
    {
        MapSceneManager.instance.fileInOutput.StartReceive(tmpText.text);
        MapSceneManager.instance.uiManager.BackPanelChangeOff();
    }

    public ButtonInteraction buttonInteraction;

    public void SendFileName_FileLoad_Monitoring(TMP_Text tmpText)
    {
        MonitoringSceneManager.instance.fileLoad.StartReceive(tmpText.text);
        buttonInteraction.OnDeSelect();
    }
}
