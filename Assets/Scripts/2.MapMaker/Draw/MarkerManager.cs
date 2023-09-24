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
            ray = Camera.main.ScreenPointToRay(Input.mousePosition); //ī�޶���� ���콺�� ��ġ�� ray�� ����
            
            if (Physics.Raycast(ray, out hit)) //������ ���� ��Ʈ�� ��ȯ
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (hit.transform.tag == "Marker")//�±װ� ��Ŀ�� �´ٸ� ����
                    {
                        MapSceneManager.instance.uiManager.InitStatus(); //��� ������ �ʱ�ȭ
                        MapSceneManager.instance.uiManager.NowStepUI(); // ���࿡ �ʿ��� �⺻���� ����
                        MapSceneManager.instance.uiManager.OffSelect_All();

                        selectMarker = hit.transform.gameObject;//��Ʈ�� ���ӿ�����Ʈ ���� ����
                        firstVector = selectMarker.transform.position; //�� ������Ʈ�� x y z ���� ����

                        markerProperty.gameObject.SetActive(true);//markerProperty�� Ȱ��ȭ

                        markerProperty.SetMarker(selectMarker);//markerProperty�� selectMarker�� ���� ������Ʈ�� ����
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



    //�⺻���� ����
    public void StartStatus()
    {
        isStart = true;
    }

    public void EndStatus()
    {
        isStart = false;
        markerProperty.gameObject.SetActive(false);
    }

    //�׸��� ���� ����
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
