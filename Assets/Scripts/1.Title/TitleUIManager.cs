using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleUIManager : MonoBehaviour
{
    public GameObject mapProjectPanel;
    public GameObject operationPanel;
    public GameObject tag_watch_Panel;

    public GameObject mapClientButton;
    public GameObject operationClientButton;
    public GameObject tag_watchClientButton;

    public void Update()
    {
        if (!mapClientButton.GetComponent<ButtonInteraction>().isActive)
        {
            mapProjectPanel.SetActive(false);
        }
        else
        {
            mapProjectPanel.SetActive(true);
        }

        if (!operationClientButton.GetComponent<ButtonInteraction>().isActive)
        {
            operationPanel.SetActive(false);
        }
        else
        {
            operationPanel.SetActive(true);
        }

        if (!tag_watchClientButton.GetComponent<ButtonInteraction>().isActive)
        {
            tag_watch_Panel.SetActive(false);
        }
        else
        {
            tag_watch_Panel.SetActive(true);
        }
    }

    public void OperationClientOn()
    {
        operationPanel.SetActive(true);
        OperationSceneManager.instance.memberListManager.ReceiveMemberData();
    }

    public void OperationClientOff()
    {
        operationPanel.SetActive(false);
    }

    public void Tag_WatchClientOn()
    {
        tag_watch_Panel.SetActive(true);
        InsertOperationManager.instance.insertUIManager.TagPageOn();
        InsertOperationManager.instance.insertTagList.ReceiveTagData();
        InsertOperationManager.instance.insertWatchList.ReceiveWatchData();
    }

    public void Tag_WatchClientOff()
    {
        tag_watch_Panel.SetActive(false);
    }

    public void MapClientOn()
    {
        TitleSceneManager.instance.titleFileLoad.StartFileListReceive();
        mapProjectPanel.SetActive(true);
    }

    public void UpdateUrl()
    {
        string url = NetworkManager.instance.url;

        InsertOperationManager.instance.url = url;
        OperationSceneManager.instance.url = url;
        TitleSceneManager.instance.url = url;
    }
}
