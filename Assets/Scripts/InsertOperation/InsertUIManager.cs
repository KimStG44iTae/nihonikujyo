using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InsertUIManager : MonoBehaviour
{
    public GameObject tagBackGround;
    public GameObject watchBackGround;

    public GameObject tagButton;
    public GameObject watchButton;

    public GameObject searchTag;
    public GameObject searchId;
    public GameObject searchTagId;

    public GameObject backGround;

    public GameObject searchWatch;
    public GameObject searchWatchId;
    public GameObject searchWatchMac_addr;

    public GameObject errorMessgae;
    public TMP_Text errorMessgaeText;

    //�±� ���, ����, �˻� ui
    public void TagPageOn()
    {
        tagBackGround.SetActive(true);
        watchBackGround.SetActive(false);
        tagButton.GetComponent<ButtonInteraction>().OnSelect();
        watchButton.GetComponent<ButtonInteraction>().OnDeSelect();
    }

    //��ġ ���, ����, �˻� ui
    public void WatchPageOn()
    {
        tagBackGround.SetActive(false);
        watchBackGround.SetActive(true);
        tagButton.GetComponent<ButtonInteraction>().OnDeSelect();
        watchButton.GetComponent<ButtonInteraction>().OnSelect();
    }

    //�±� �˻� ui on/off
    public void SearchTagPageOn()
    {
        ResetSearchManager();

        backGround.SetActive(true);
        searchTag.SetActive(true);
        SearchIDPageOn();
    }

    public void SearchTagPageOff()
    {
        backGround.SetActive(false);
        searchTag.SetActive(false);
    }

    //ID�� �˻�
    public void SearchIDPageOn()
    {
        searchId.SetActive(true);
        searchTagId.SetActive(false);
    }
    //tagID�� �˻�
    public void SearchTagIdPageOn()
    {
        searchId.SetActive(false);
        searchTagId.SetActive(true);
    }

    //��ġ �˻� ui on/off
    public void SearchWatchPageOn()
    {
        ResetSearchManager();

        backGround.SetActive(true);
        searchWatch.SetActive(true);
        SearchIDPageOn();
    }

    public void SearchWatchPageOff()
    {
        backGround.SetActive(false);

        searchWatch.SetActive(false);
    }

    //��ġid �� �˻�
    public void Search_WatchID_PageOn()
    {
        searchWatchId.SetActive(true);
        searchWatchMac_addr.SetActive(false);
    }

    //��ġ mac �ּҷ� �˻�
    public void SearchWatch_Mac_addr_PageOn()
    {
        searchWatchId.SetActive(false);
        searchWatchMac_addr.SetActive(true);
    }

    //���� �޼���
    public void ErrorMessageOn(string s)
    {
        errorMessgae.SetActive(true);
        errorMessgaeText.text = s;
    }

    public void ErrorMessageOff()
    {
        errorMessgae.SetActive(false);
    }

    //�� �ʱ�ȭ - ����Ʈ �ʱ�ȭ, �Ŵ��� �ʱ�ȭ
    public void ResetScene()
    {
        InsertOperationManager.instance.insertTagList.ResetTagList();
        InsertOperationManager.instance.insertWatchList.ResetWatchList();
        ResetSearchManager();
    }

    //�Ŵ��� �ʱ�ȭ
    public void ResetSearchManager()
    {
        InsertOperationManager.instance.insertWatchManager.ResetWatchManager();
        InsertOperationManager.instance.insertTagManager.ResetTagManager();

    }

}
