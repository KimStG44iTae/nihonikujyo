using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIManager : MonoBehaviour
{
    public GameObject leftPanel;
    public GameObject rightPanel;

    public GameObject panel3D;
    public GameObject panel2D;

    //도면 이미지
    public RawImage drawingImage;

    //리셋 ui
    public GameObject resetUI;

    public GameObject backPanel;

    public GameObject errorMessage;


    public GameObject filePanel;

    public ButtonInteraction fileButton;

    public bool isDraw;

    List<GameObject> imageList = new List<GameObject>();

    public TMP_Text projectName;
    public TMP_Text pointCount;
    public TMP_Text zoneCount;
    public TMP_Text cctvCount;
    public TMP_Text uwbCount;
    public TMP_Text markerCount;
    public TMP_Text boardCount;
    public TMP_Text fireexitCount;

    public void InfomationUpdate()
    {
        if (MapSceneManager.instance.shareData.jsonData.mapName == "")
        {
            projectName.text = "Empty";
        }
        else
        {
            projectName.text = MapSceneManager.instance.shareData.jsonData.mapName;
        }

        pointCount.text = MapSceneManager.instance.shareData.jsonData.points.Count.ToString();
        zoneCount.text = MapSceneManager.instance.shareData.jsonData.zones.Count.ToString();
        cctvCount.text = MapSceneManager.instance.shareData.jsonData.cctvs.Count.ToString();
        uwbCount.text = MapSceneManager.instance.shareData.jsonData.uwbs.Count.ToString();
        markerCount.text = MapSceneManager.instance.shareData.jsonData.markers.Count.ToString();
        boardCount.text = MapSceneManager.instance.shareData.jsonData.boards.Count.ToString();
        fireexitCount.text = MapSceneManager.instance.shareData.jsonData.fireexits.Count.ToString();
    }

    public void FilePanelOn()
    {
        if (filePanel.activeSelf)
        {
            FilePanelOff();
            OffSelect_NoFilePanel();
        }
        else
            filePanel.SetActive(true);
    }

    public void OffSelect_NoFilePanel()
    {
        ButtonInteraction[] buttonInteractions = GameObject.FindObjectsOfType<ButtonInteraction>();

        for (int i = 0; i < buttonInteractions.Length; i++)
        {
            if (buttonInteractions[i].isActive && (fileButton != buttonInteractions[i]))
            {
                buttonInteractions[i].isActive = false;
                buttonInteractions[i].OnDeSelect();
                break;
            }
        }
    }

    public void OffSelect_All()
    {
        ButtonInteraction[] buttonInteractions = GameObject.FindObjectsOfType<ButtonInteraction>();

        for (int i = 0; i < buttonInteractions.Length; i++)
        {
            if (buttonInteractions[i].isActive)
            {
                buttonInteractions[i].isActive = false;
                buttonInteractions[i].OnDeSelect();
                break;
            }
        }
    }

    private void Start()
    {
        for (int i = 0; i < panel2D.transform.childCount; i++)
        {
            imageList.Add(panel2D.transform.GetChild(i).gameObject);
        }
    }


    public void FilePanelOff()
    {
        filePanel.SetActive(false);
    }

    public void BackPanelChangeOn()
    {
        backPanel.SetActive(true);
    }

    public void BackPanelChangeOff()
    {
        backPanel.SetActive(false);
    }

    public void EndIsDraw()
    {
         isDraw = false;
    }

    public void Panel2D_InteractiveOff()
    {
        for (int i = 0; i < imageList.Count; i++)
        {
            imageList[i].GetComponent<Button>().interactable = false;
        }
    }

    public void Panel2D_InteractiveOn()
    {
        for (int i = 0; i < imageList.Count; i++)
        {
            imageList[i].GetComponent<Button>().interactable = true;
        }
    }

    //현재 단계에 필요한 기본 상태 실행
    public void NowStepUI()
    {
        if (isDraw)
        {
            return;
        }

        MapSceneManager.instance.pointManager.StartStatus();
        MapSceneManager.instance.zoneManager.StartStatus();

        MapSceneManager.instance.cctvManager.StartStatus();
        MapSceneManager.instance.uwbManager.StartStatus();

        MapSceneManager.instance.markerManager.StartStatus();
        MapSceneManager.instance.boardManager.StartStatus();
        MapSceneManager.instance.fireexitManager.StartStatus();
        Panel2D_InteractiveOn();
        OffSelect_NoFilePanel();
    }

    //이미지 이동 상태
    public void StartMoveImage()
    {
        InitStatus();
        Panel2D_InteractiveOff();

        MapSceneManager.instance.setFormImage.StartStatus();
    }

    //점 그리기 상태
    public void StartDrawCorner()
    {
        InitStatus();
        MapSceneManager.instance.pointManager.StartDrawStatus();
        Panel2D_InteractiveOff();
        isDraw = true;
    }

    //점 기본 상태
    public void StartPointStatus()
    {
        InitStatus();
        MapSceneManager.instance.pointManager.StartStatus();
    }

    //존 그리기 상태
    public void StartDrawZone()
    {
        InitStatus();
        MapSceneManager.instance.zoneManager.StartDrawStatus();
        isDraw = true;
        Panel2D_InteractiveOff();
    }

    //크기 변환 상태
    public void StartScaleConversion()
    {
        InitStatus();
        Panel2D_InteractiveOff();
        MapSceneManager.instance.scaleConversion.StartStatus();
    }

    //cctv 그리기 상태
    public void StartDrawCCTVStatus()
    {
        InitStatus();
        Panel2D_InteractiveOff();
        MapSceneManager.instance.cctvManager.StartDrawStatus();
        isDraw = true;
    }

    //uwb 그리기 상태
    public void StartDrawUWBStatus()
    {
        InitStatus();
        Panel2D_InteractiveOff();
        MapSceneManager.instance.uwbManager.StartDrawStatus();
        isDraw = true;
    }

    //marker 그리기 상태
    public void StartDrawMarkerStatus()
    {
        InitStatus();
        Panel2D_InteractiveOff();
        MapSceneManager.instance.markerManager.StartDrawStatus();
        isDraw = true;
    }

    //board 그리기 상태
    public void StartDrawBoardStatus()
    {
        InitStatus();
        Panel2D_InteractiveOff();
        MapSceneManager.instance.boardManager.StartDrawStatus();
        isDraw = true;
    }

    public void StartDrawFireExitStatus()
    {
        InitStatus();
        Panel2D_InteractiveOff();
        MapSceneManager.instance.fireexitManager.StartDrawStatus();
        isDraw = true;
    }

    //3d 모드 상태
    public void Start3DMode()
    {
        InitStatus();
        MapSceneManager.instance.camera3d.gameObject.SetActive(true);
        MapSceneManager.instance.mainCamera.gameObject.SetActive(false);
        MapSceneManager.instance.makeModel.StartMakeModel();
        MapSceneManager.instance.camera3d.gameObject.GetComponent<Map_CameraMove3D>().PositionReset();

        leftPanel.SetActive(false);
        rightPanel.SetActive(false);

        panel3D.SetActive(true);
        panel2D.SetActive(false);

        OffSelect_All();
    }
    
    //2d 모드 상태
    public void Start2DMode()
    {
        InitStatus();
        MapSceneManager.instance.camera3d.gameObject.SetActive(false);
        MapSceneManager.instance.mainCamera.gameObject.SetActive(true);

        leftPanel.SetActive(true);
        rightPanel.SetActive(true);

        panel3D.SetActive(false);
        panel2D.SetActive(true);

        OffSelect_All();
    }

    //모든 상태 초기화
    public void InitStatus()
    {
        MapSceneManager.instance.setFormImage.EndStatus();

        MapSceneManager.instance.pointManager.EndStatus();
        MapSceneManager.instance.pointManager.EndDrawStatus();

        MapSceneManager.instance.scaleConversion.EndStatus();

        MapSceneManager.instance.cctvManager.EndStatus();
        MapSceneManager.instance.cctvManager.EndDrawStatus();

        MapSceneManager.instance.uwbManager.EndStatus();
        MapSceneManager.instance.uwbManager.EndDrawStatus();

        MapSceneManager.instance.zoneManager.EndStatus();
        MapSceneManager.instance.zoneManager.EndDrawStatus();

        MapSceneManager.instance.markerManager.EndStatus();
        MapSceneManager.instance.markerManager.EndDrawStatus();

        MapSceneManager.instance.boardManager.EndStatus();
        MapSceneManager.instance.boardManager.EndDrawStatus();

        MapSceneManager.instance.fireexitManager.EndStatus();
        MapSceneManager.instance.fireexitManager.EndDrawStatus();

        FilePanelOff();
    }

    //리셋 관리
    public void ResetUIOn()
    {
        BackPanelChangeOn();
        resetUI.SetActive(true);
        OffSelect_All();
        FilePanelOff();
    }

    public void ResetUIOff()
    {
        BackPanelChangeOff();
        resetUI.SetActive(false);
        OffSelect_All();
    }

    public void ResetProject()
    {
        ShareData.instance.InitData();
        drawingImage.texture = null;
        ResetUIOff();
        InitStatus();
        NowStepUI();
        MapSceneManager.instance.unDoManager.ResetUndo();
        OffSelect_All();
        InfomationUpdate();
        drawingImage.gameObject.GetComponent<SetFormImage>().SetSize(1, 1);
    }

    public void ErrorMessageOn(string messgae)
    {
        BackPanelChangeOn();
        errorMessage.transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = messgae;
        errorMessage.SetActive(true);
        OffSelect_All();
    }

    public void ErrorMessageOff()
    {
        BackPanelChangeOff();
        errorMessage.SetActive(false);
        OffSelect_All();
    }

    //이미지 사이즈 조절
    public void SetImageSize(int w, int h)
    {
        drawingImage.gameObject.GetComponent<SetFormImage>().SetSize(w, h);
        drawingImage.gameObject.SetActive(true);
    }
}
