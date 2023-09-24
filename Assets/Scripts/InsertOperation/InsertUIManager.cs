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

    //태그 등록, 삭제, 검색 ui
    public void TagPageOn()
    {
        tagBackGround.SetActive(true);
        watchBackGround.SetActive(false);
        tagButton.GetComponent<ButtonInteraction>().OnSelect();
        watchButton.GetComponent<ButtonInteraction>().OnDeSelect();
    }

    //워치 등록, 삭제, 검색 ui
    public void WatchPageOn()
    {
        tagBackGround.SetActive(false);
        watchBackGround.SetActive(true);
        tagButton.GetComponent<ButtonInteraction>().OnDeSelect();
        watchButton.GetComponent<ButtonInteraction>().OnSelect();
    }

    //태그 검색 ui on/off
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

    //ID로 검색
    public void SearchIDPageOn()
    {
        searchId.SetActive(true);
        searchTagId.SetActive(false);
    }
    //tagID로 검색
    public void SearchTagIdPageOn()
    {
        searchId.SetActive(false);
        searchTagId.SetActive(true);
    }

    //워치 검색 ui on/off
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

    //워치id 로 검색
    public void Search_WatchID_PageOn()
    {
        searchWatchId.SetActive(true);
        searchWatchMac_addr.SetActive(false);
    }

    //워치 mac 주소로 검색
    public void SearchWatch_Mac_addr_PageOn()
    {
        searchWatchId.SetActive(false);
        searchWatchMac_addr.SetActive(true);
    }

    //에러 메세지
    public void ErrorMessageOn(string s)
    {
        errorMessgae.SetActive(true);
        errorMessgaeText.text = s;
    }

    public void ErrorMessageOff()
    {
        errorMessgae.SetActive(false);
    }

    //씬 초기화 - 리스트 초기화, 매니저 초기화
    public void ResetScene()
    {
        InsertOperationManager.instance.insertTagList.ResetTagList();
        InsertOperationManager.instance.insertWatchList.ResetWatchList();
        ResetSearchManager();
    }

    //매니저 초기화
    public void ResetSearchManager()
    {
        InsertOperationManager.instance.insertWatchManager.ResetWatchManager();
        InsertOperationManager.instance.insertTagManager.ResetTagManager();

    }

}
