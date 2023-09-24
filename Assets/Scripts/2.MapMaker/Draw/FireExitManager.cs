using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireExitManager : MonoBehaviour
{
    //CCTV 프리펍
    public GameObject FireExitPrefeb;

    //마우스를 따르다니는 임시 CCTV 오브젝트
    public GameObject tempFireExit;

    //현재 그리는 CCTV 오브젝트
    public GameObject nowFireExit;

    //수정하기 위한 현재 선택된 CCTV 오브젝트
    public GameObject selectFireExit;

    //현재 있는 모든 CCTV 리시트
    public List<GameObject> fireexitList;

    public bool isDraw;
    public bool isStart;
    public bool isDrawEnd;

    public int countId;

    RaycastHit hit;
    Ray ray;

    Vector3 firstVector;

    //CCTV 속성창
    public FireExitProperty FireExitProperty;

    void Update()
    {
        //현재 기본 상태 - 클릭하면 CCTV 수정 상태
        if (isStart)
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                Vector3 temp = new Vector3(hit.point.x, 0.03f, hit.point.z);

                if (Input.GetMouseButtonDown(0))
                {
                    if (hit.transform.tag == "FireExit")
                    {
                        MapSceneManager.instance.uiManager.InitStatus();
                        MapSceneManager.instance.uiManager.NowStepUI();
                        MapSceneManager.instance.uiManager.OffSelect_All();

                        selectFireExit = hit.transform.gameObject;
                        FireExitProperty.gameObject.SetActive(true);
                        firstVector = new Vector3(hit.point.x, 0.03f, hit.point.z);

                        //UWB 기본 상태를 정지
                        MapSceneManager.instance.uwbManager.EndStatus();
                        FireExitProperty.SetFireExitPosition(selectFireExit);

                    }
                }
                if (Input.GetMouseButton(0))
                {
                    if (selectFireExit)
                    {
                        //선택된 CCTV 위치를 이동하면서 속성창에 반영
                        if (Vector3.Distance(firstVector, temp) > 0.01f)
                        {
                            selectFireExit.transform.position = temp;

                            int id = selectFireExit.GetComponent<FireExitInfo>().id;

                            Vector3 tempVector = new Vector3(temp.x, ShareData.instance.jsonData.fireexits.Find(x => x.id == id).position.y, temp.z);

                            ShareData.instance.jsonData.fireexits.Find(x => x.id == id).position = tempVector;

                            FireExitProperty.SetFireExitPosition(selectFireExit);
                        }
                    }
                }
                if (Input.GetMouseButtonUp(0))
                {
                    if (selectFireExit != null)
                    {
                        selectFireExit = null;
                        MapSceneManager.instance.unDoManager.AddUndo();
                    }

                    MapSceneManager.instance.uiManager.NowStepUI();

                }
            }

        }

        //CCTV 설치
        if (isDraw)
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (isDrawEnd)
            {
                if (Input.GetMouseButtonUp(0))
                {
                    EndDrawStatus();
                    StartStatus();
                    MapSceneManager.instance.uiManager.NowStepUI();
                    MapSceneManager.instance.uiManager.InfomationUpdate();
                    isDrawEnd = false;
                }
            }

            if (Physics.Raycast(ray, out hit))
            {
                Vector3 temp = new Vector3(hit.point.x, 0.03f, hit.point.z);

                tempFireExit.transform.position = temp;

                if (Input.GetMouseButtonDown(0) && !MapSceneManager.instance.isMouseInUi)
                {
                    nowFireExit = Instantiate(FireExitPrefeb, temp, Quaternion.identity);

                    nowFireExit.GetComponent<FireExitInfo>().id = ShareData.instance.jsonData.lastID;

                    fireexitList.Add(nowFireExit);

                    JsonFireExit jsonFireExit = new JsonFireExit();
                    jsonFireExit.position = temp;
                    jsonFireExit.id = ShareData.instance.jsonData.lastID;


                    MapSceneManager.instance.shareData.AddJsonFireExit(jsonFireExit);

                    FireExitProperty.gameObject.SetActive(true);
                    FireExitProperty.SetFireExitPosition(nowFireExit);

                    ShareData.instance.jsonData.lastID++;
                    isDrawEnd = true;
                    tempFireExit.SetActive(false);

                    nowFireExit = null;

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

    //모든 CCTV 지우고 새로 다시 그림
    public void UpdateFireExit()
    {
        for (int i = fireexitList.Count - 1; i >= 0; i--)
        {
            Destroy(fireexitList[i]);
        }

        fireexitList.Clear();

        for (int i = 0; i < ShareData.instance.jsonData.fireexits.Count; i++)
        {
            Vector3 fireexitPos = ShareData.instance.jsonData.fireexits[i].position;
            fireexitPos = new Vector3(fireexitPos.x, 0.03f, fireexitPos.z);

            GameObject FireExitTemp = Instantiate(FireExitPrefeb, fireexitPos, Quaternion.identity);

            FireExitTemp.transform.rotation = ShareData.instance.jsonData.fireexits[i].rotation;

            FireExitTemp.GetComponent<FireExitInfo>().id = ShareData.instance.jsonData.fireexits[i].id;

            fireexitList.Add(FireExitTemp);
        }


        if (FireExitProperty.gameObject.activeSelf)
        {
            for (int i = 0; i < fireexitList.Count; i++)
            {
                if (fireexitList[i].GetComponent<FireExitInfo>().id == FireExitProperty.fireexitId)
                {
                    FireExitProperty.SetFireExitPosition(fireexitList[i]);
                }
            }
        }
    }

    //모든 CCTV 리스트 삭제
    public void DeleteFireExit(GameObject fireexit, JsonFireExit jFireExit)
    {
        Destroy(fireexit);

        for (int i = 0; i < ShareData.instance.jsonData.fireexits.Count; i++)
        {
            if (ShareData.instance.jsonData.fireexits[i] == jFireExit)
            {
                ShareData.instance.jsonData.fireexits.RemoveAt(i);
            }
        }

        UpdateFireExit();
    }

    //모든 CCTV 리스트 삭제
    public void DestroyAllFireExit()
    {
        for (int i = fireexitList.Count - 1; i >= 0; i--)
        {
            Destroy(fireexitList[i]);
        }
        fireexitList.Clear();
    }

    //기본상태 변경
    public void StartStatus()
    {
        isStart = true;
    }

    public void EndStatus()
    {
        isStart = false;
        FireExitProperty.gameObject.SetActive(false);
    }

    //그리기 상태 변경
    public void StartDrawStatus()
    {
        tempFireExit.SetActive(true);
        isDraw = true;
    }

    public void EndDrawStatus()
    {
        isDraw = false;
        tempFireExit.SetActive(false);
        MapSceneManager.instance.uiManager.EndIsDraw();
    }


}