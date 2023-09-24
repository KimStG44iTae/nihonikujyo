using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireExitManager : MonoBehaviour
{
    //CCTV ������
    public GameObject FireExitPrefeb;

    //���콺�� �����ٴϴ� �ӽ� CCTV ������Ʈ
    public GameObject tempFireExit;

    //���� �׸��� CCTV ������Ʈ
    public GameObject nowFireExit;

    //�����ϱ� ���� ���� ���õ� CCTV ������Ʈ
    public GameObject selectFireExit;

    //���� �ִ� ��� CCTV ����Ʈ
    public List<GameObject> fireexitList;

    public bool isDraw;
    public bool isStart;
    public bool isDrawEnd;

    public int countId;

    RaycastHit hit;
    Ray ray;

    Vector3 firstVector;

    //CCTV �Ӽ�â
    public FireExitProperty FireExitProperty;

    void Update()
    {
        //���� �⺻ ���� - Ŭ���ϸ� CCTV ���� ����
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

                        //UWB �⺻ ���¸� ����
                        MapSceneManager.instance.uwbManager.EndStatus();
                        FireExitProperty.SetFireExitPosition(selectFireExit);

                    }
                }
                if (Input.GetMouseButton(0))
                {
                    if (selectFireExit)
                    {
                        //���õ� CCTV ��ġ�� �̵��ϸ鼭 �Ӽ�â�� �ݿ�
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

        //CCTV ��ġ
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

    //��� CCTV ����� ���� �ٽ� �׸�
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

    //��� CCTV ����Ʈ ����
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

    //��� CCTV ����Ʈ ����
    public void DestroyAllFireExit()
    {
        for (int i = fireexitList.Count - 1; i >= 0; i--)
        {
            Destroy(fireexitList[i]);
        }
        fireexitList.Clear();
    }

    //�⺻���� ����
    public void StartStatus()
    {
        isStart = true;
    }

    public void EndStatus()
    {
        isStart = false;
        FireExitProperty.gameObject.SetActive(false);
    }

    //�׸��� ���� ����
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