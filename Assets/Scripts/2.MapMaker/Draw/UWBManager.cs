using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UWBManager : MonoBehaviour
{
    //UWB ������
    public GameObject UWBPrefeb;

    //���콺�� �����ٴϴ� �ӽ� UWB ������Ʈ
    public GameObject tempUWB;

    //���� �׸��� UWB ������Ʈ
    public GameObject nowUWB;

    //�����ϱ� ���� ���� ���õ� UWB ������Ʈ
    public GameObject selectUWB;

    //���� �ִ� ��� UWB ����Ʈ
    public List<GameObject> UWBList;

    public bool isDraw;
    public bool isStart;
    public bool isDrawEnd;

    Vector3 firstVector;

    RaycastHit hit;
    Ray ray;

    //UWB �Ӽ�â
    public UWBProperty uwbProperty;

    public Button button;

    public Sprite buttonOn;
    public Sprite buttonOff;

    void Update()
    {
        //���� �⺻ ���� - Ŭ���ϸ� UWB ���� ����
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

                        //CCTV �⺻ ���¸� ����
                        MapSceneManager.instance.cctvManager.EndStatus();
                    }
                }
                if (Input.GetMouseButton(0))
                {
                    //���õ� UWB ��ġ�� �̵��ϸ鼭 �Ӽ�â�� �ݿ�
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

        //UWB ��ġ
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

    //��� UWB ����� ���� �ٽ� �׸�
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

    //��� UWB ����Ʈ ����
    public void DestroyAllUWB()
    {
        for (int i = UWBList.Count - 1; i >= 0; i--)
        {
            Destroy(UWBList[i]);
        }
        UWBList.Clear();
    }

    //�⺻���� ����
    public void StartStatus()
    {
        isStart = true;
    }

    public void EndStatus()
    {
        isStart = false;
        uwbProperty.gameObject.SetActive(false);
    }

    //�׸��� ���� ����
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
