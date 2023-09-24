using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UWBManager : MonoBehaviour
{
    //UWB 프리펍
    public GameObject UWBPrefeb;

    //마우스를 따르다니는 임시 UWB 오브젝트
    public GameObject tempUWB;

    //현재 그리는 UWB 오브젝트
    public GameObject nowUWB;

    //수정하기 위한 현재 선택된 UWB 오브젝트
    public GameObject selectUWB;

    //현재 있는 모든 UWB 리시트
    public List<GameObject> UWBList;

    public bool isDraw;
    public bool isStart;
    public bool isDrawEnd;

    Vector3 firstVector;

    RaycastHit hit;
    Ray ray;

    //UWB 속성창
    public UWBProperty uwbProperty;

    public Button button;

    public Sprite buttonOn;
    public Sprite buttonOff;

    void Update()
    {
        //현재 기본 상태 - 클릭하면 UWB 수정 상태
        if (isStart)
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                Vector3 temp = new Vector3(hit.point.x, 0.03f, hit.point.z);

                if (Input.GetMouseButtonDown(0))
                {
                    if (hit.transform.tag == "UWB")
                    {
                        MapSceneManager.instance.uiManager.InitStatus();
                        MapSceneManager.instance.uiManager.NowStepUI();
                        MapSceneManager.instance.uiManager.OffSelect_All();

                        selectUWB = hit.transform.gameObject;
                        uwbProperty.gameObject.SetActive(true);
                        firstVector = new Vector3(hit.point.x, 0.03f, hit.point.z);
                        uwbProperty.SetUWBPosition(selectUWB);

                        //CCTV 기본 상태를 정지
                        MapSceneManager.instance.cctvManager.EndStatus();
                    }
                }
                if (Input.GetMouseButton(0))
                {
                    //선택된 UWB 위치를 이동하면서 속성창에 반영
                    if (selectUWB)
                    {
                        if (Vector3.Distance(firstVector, temp) > 0.01f)
                        {
                            selectUWB.transform.position = temp;
                            int id = selectUWB.GetComponent<UWBInfo>().id;

                            Vector3 tempVector = new Vector3(temp.x, ShareData.instance.jsonData.uwbs.Find(x => x.id == id).position.y, temp.z);

                            ShareData.instance.jsonData.uwbs.Find(x => x.id == id).position = tempVector;

                            uwbProperty.SetUWBPosition(selectUWB);
                        }
                    }
                }
                if (Input.GetMouseButtonUp(0))
                {
                    if (selectUWB != null)
                    {
                        selectUWB = null;
                        MapSceneManager.instance.unDoManager.AddUndo();

                    }
                    MapSceneManager.instance.uiManager.NowStepUI();
                }
            }

        }

        //UWB 설치
        if(isDraw)
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (isDrawEnd)
            {
                if (Input.GetMouseButtonUp(0))
                {
                    EndDrawStatus();
                    StartStatus();
                    isDrawEnd = false;
                    MapSceneManager.instance.uiManager.NowStepUI();
                    MapSceneManager.instance.uiManager.InfomationUpdate();
                }
            }

            if (Physics.Raycast(ray, out hit))
            {
                Vector3 temp = new Vector3(hit.point.x, 0.03f, hit.point.z);

                tempUWB.transform.position = temp;

                if (Input.GetMouseButtonDown(0) && !MapSceneManager.instance.isMouseInUi)
                {
                    nowUWB = Instantiate(UWBPrefeb, temp, Quaternion.identity);

                    nowUWB.GetComponent<UWBInfo>().id = ShareData.instance.jsonData.lastID;

                    UWBList.Add(nowUWB);

                    JsonUWB jsonUWB = new JsonUWB();
                    jsonUWB.position = temp;
                    jsonUWB.id = ShareData.instance.jsonData.lastID;

                    MapSceneManager.instance.shareData.AddJsonUWB(jsonUWB);

                    isDrawEnd = true;
                    uwbProperty.gameObject.SetActive(true);
                    uwbProperty.SetUWBPosition(nowUWB);

                    ShareData.instance.jsonData.lastID++;

                    tempUWB.SetActive(false);

                    nowUWB = null;
                    MapSceneManager.instance.unDoManager.AddUndo();
                    MapSceneManager.instance.uiManager.NowStepUI();

                }
            }
            if (Input.GetMouseButtonUp(1))
            {
                EndDrawStatus();
                StartStatus();
                MapSceneManager.instance.uiManager.NowStepUI();

            }
        }
    }

    //모든 UWB 지우고 새로 다시 그림
    public void UpdateUWB()
    {
        DestroyAllUWB();

        for (int i = 0; i < ShareData.instance.jsonData.uwbs.Count; i++)
        {
            GameObject uwbTemp = Instantiate(UWBPrefeb, ShareData.instance.jsonData.uwbs[i].position, Quaternion.identity);

            uwbTemp.GetComponent<UWBInfo>().id = ShareData.instance.jsonData.uwbs[i].id;

            UWBList.Add(uwbTemp);
        }


        if (uwbProperty.gameObject.activeSelf)
        {
            for (int i = 0; i < UWBList.Count; i++)
            {
                if (UWBList[i].GetComponent<UWBInfo>().id == uwbProperty.uwbId)
                {
                    uwbProperty.SetUWBPosition(UWBList[i]);
                }
            }
        }
    }

    //모든 UWB 리스트 삭제
    public void DestroyAllUWB()
    {
        for (int i = UWBList.Count - 1; i >= 0; i--)
        {
            Destroy(UWBList[i]);
        }
        UWBList.Clear();
    }

    //기본상태 변경
    public void StartStatus()
    {
        isStart = true;
    }

    public void EndStatus()
    {
        isStart = false;
        uwbProperty.gameObject.SetActive(false);
    }

    //그리기 상태 변경
    public void StartDrawStatus()
    {
        tempUWB.SetActive(true);
        isDraw = true;
    }

    public void EndDrawStatus()
    {
        isDraw = false;
        tempUWB.SetActive(false);
        MapSceneManager.instance.uiManager.EndIsDraw();
    }
}
