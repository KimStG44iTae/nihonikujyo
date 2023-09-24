using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarkerManager : MonoBehaviour
{
    public GameObject markerPrefeb;

    public List<GameObject> markerList = new List<GameObject>();

    public GameObject tempMarker;

    public GameObject selectMarker;

    public bool isStart;
    public bool isDraw;
    public bool isDrawEnd;

    Vector3 firstVector;

    RaycastHit hit;
    Ray ray;

    public MarkerProperty markerProperty;

    public Button button;

    public Sprite buttonOn;
    public Sprite buttonOff;

    public void Update()
    {
        if (isStart)
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition); //카메라기준 마우스의 위치를 ray에 저장
            
            if (Physics.Raycast(ray, out hit)) //레이의 값을 히트에 반환
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (hit.transform.tag == "Marker")//태그가 마커가 맞다면 실행
                    {
                        MapSceneManager.instance.uiManager.InitStatus(); //모든 설정을 초기화
                        MapSceneManager.instance.uiManager.NowStepUI(); // 실행에 필요한 기본적인 설정
                        MapSceneManager.instance.uiManager.OffSelect_All();

                        selectMarker = hit.transform.gameObject;//히트의 게임오브젝트 값을 저장
                        firstVector = selectMarker.transform.position; //그 오브젝트의 x y z 값을 저장

                        markerProperty.gameObject.SetActive(true);//markerProperty를 활성화

                        markerProperty.SetMarker(selectMarker);//markerProperty를 selectMarker와 같은 오브젝트로 세팅
                        MapSceneManager.instance.boardManager.EndStatus();
                    }
                }

                if (Input.GetMouseButtonUp(0))
                {
                    if (selectMarker != null)
                    {
                        selectMarker = null;
                    }
                    MapSceneManager.instance.uiManager.NowStepUI();
                }

            }
        }

        if (isDraw)
        {
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

            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                Vector3 temp = new Vector3(hit.point.x, 0.03f, hit.point.z);

                temp.x = (float)Math.Round(temp.x,2);
                temp.y = (float)Math.Round(temp.y,2);
                temp.z = (float)Math.Round(temp.z,2);

                tempMarker.transform.position = temp;

                if (Input.GetMouseButtonDown(0) && !MapSceneManager.instance.isMouseInUi)
                {
                    GameObject nowMarker = Instantiate(markerPrefeb, temp, Quaternion.identity);

                    nowMarker.GetComponent<MarkerInfo>().id = ShareData.instance.jsonData.lastID;
                    ShareData.instance.jsonData.lastID += 1;

                    JsonMarker jsonMarker = new JsonMarker();
                    jsonMarker.id = nowMarker.GetComponent<MarkerInfo>().id;
                    jsonMarker.position = temp;

                    MapSceneManager.instance.shareData.AddJsonMarker(jsonMarker);

                    nowMarker.GetComponent<MarkerInfo>().jsonMarker = jsonMarker;
                    nowMarker.GetComponent<MarkerInfo>().SetOriginPos();

                    markerList.Add(nowMarker);

                    MapSceneManager.instance.unDoManager.AddUndo();
                    MapSceneManager.instance.uiManager.NowStepUI();

                    isDrawEnd = true;
                }

                if (Input.GetMouseButtonUp(1))
                {
                    EndDrawStatus();
                    StartStatus();
                    MapSceneManager.instance.uiManager.NowStepUI();
                }
            }
        }
    }

    public void DestroyMarker()
    {
        for (int i = markerList.Count - 1; i >= 0; i--) 
        {
            Destroy(markerList[i]);
        }

        markerList.Clear();
    }


    public void UpdateMarker()
    {
        DestroyMarker();

        for (int i = 0; i < ShareData.instance.jsonData.markers.Count; i++)
        {
            JsonMarker tempJsonMarker = ShareData.instance.jsonData.markers[i];

            GameObject nowMarker = Instantiate(markerPrefeb, tempJsonMarker.position, Quaternion.identity);

            nowMarker.GetComponent<MarkerInfo>().id = tempJsonMarker.id;
            nowMarker.GetComponent<MarkerInfo>().jsonMarker = tempJsonMarker;

            nowMarker.transform.GetChild(0).rotation = Quaternion.Euler(tempJsonMarker.rotation);
            nowMarker.GetComponent<MarkerInfo>().SetLocalRotation();
            nowMarker.GetComponent<MarkerInfo>().SetOriginPos();

            markerList.Add(nowMarker);
        }


        if (markerProperty.gameObject.activeSelf)
        {
            for (int i = 0; i < markerList.Count; i++)
            {
                if (markerList[i].GetComponent<MarkerInfo>().id == markerProperty.markerId)
                {
                    markerProperty.SetMarker(markerList[i]);
                }
            }
        }
    }



    //기본상태 변경
    public void StartStatus()
    {
        isStart = true;
    }

    public void EndStatus()
    {
        isStart = false;
        markerProperty.gameObject.SetActive(false);
    }

    //그리기 상태 변경
    public void StartDrawStatus()
    {
        tempMarker.SetActive(true);
        isDraw = true;
    }

    public void EndDrawStatus()
    {
        isDraw = false;
        tempMarker.SetActive(false);
        MapSceneManager.instance.uiManager.EndIsDraw();
    }
}
