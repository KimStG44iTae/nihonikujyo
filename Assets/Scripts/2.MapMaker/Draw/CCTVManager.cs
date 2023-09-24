 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CCTVManager : MonoBehaviour
{
    //CCTV ������
    public GameObject CCTVPrefeb;
    
    //���콺�� �����ٴϴ� �ӽ� CCTV ������Ʈ
    public GameObject tempCCTV;
    
    //���� �׸��� CCTV ������Ʈ
    public GameObject nowCCTV;
   
    //�����ϱ� ���� ���� ���õ� CCTV ������Ʈ
    public GameObject selectCCTV;
    
    //���� �ִ� ��� CCTV ����Ʈ
    public List<GameObject> CCTVList;

    public bool isDraw;
    public bool isStart;
    public bool isDrawEnd;

    public int countId;

    RaycastHit hit;
    Ray ray;
    
    Vector3 firstVector;
   
    //CCTV �Ӽ�â
    public CCTVProperty cctvProperty;

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
                    if (hit.transform.tag== "CCTV")
                    {
                        MapSceneManager.instance.uiManager.InitStatus();
                        MapSceneManager.instance.uiManager.NowStepUI();
                        MapSceneManager.instance.uiManager.OffSelect_All();

                        selectCCTV = hit.transform.gameObject;
                        cctvProperty.gameObject.SetActive(true);
                        firstVector = new Vector3(hit.point.x, 0.03f, hit.point.z);

                        //UWB �⺻ ���¸� ����
                        MapSceneManager.instance.uwbManager.EndStatus();
                        cctvProperty.SetCCTVPosition(selectCCTV);

                    }
                }
                if (Input.GetMouseButton(0))
                {
                    if (selectCCTV)
                    {
                        //���õ� CCTV ��ġ�� �̵��ϸ鼭 �Ӽ�â�� �ݿ�
                        if (Vector3.Distance(firstVector, temp) > 0.01f)
                        {
                            selectCCTV.transform.position = temp;

                            int id = selectCCTV.GetComponent<CCTVInfo>().id;

                            Vector3 tempVector = new Vector3(temp.x, ShareData.instance.jsonData.cctvs.Find(x => x.id == id).position.y, temp.z);

                            ShareData.instance.jsonData.cctvs.Find(x => x.id == id).position = tempVector;

                            cctvProperty.SetCCTVPosition(selectCCTV);
                        }
                    }
                }
                if (Input.GetMouseButtonUp(0))
                {
                    if (selectCCTV != null)
                    {
                        selectCCTV = null;
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

                tempCCTV.transform.position = temp;

                if (Input.GetMouseButtonDown(0) && !MapSceneManager.instance.isMouseInUi)
                {
                    nowCCTV = Instantiate(CCTVPrefeb, temp, Quaternion.identity);

                    nowCCTV.GetComponent<CCTVInfo>().id = ShareData.instance.jsonData.lastID;

                    CCTVList.Add(nowCCTV);

                    JsonCCTV jsonCCTV = new JsonCCTV();
                    jsonCCTV.position = temp;
                    jsonCCTV.id = ShareData.instance.jsonData.lastID;


                    MapSceneManager.instance.shareData.AddJsonCCTV(jsonCCTV);

                    cctvProperty.gameObject.SetActive(true);
                    cctvProperty.SetCCTVPosition(nowCCTV);

                    ShareData.instance.jsonData.lastID++;
                    isDrawEnd = true;
                    tempCCTV.SetActive(false);

                    nowCCTV = null;

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
    public void UpdateCCTV()
    {
        for (int i = CCTVList.Count - 1; i >= 0; i--)
        {
            Destroy(CCTVList[i]);
        }

        CCTVList.Clear();

        for (int i = 0; i < ShareData.instance.jsonData.cctvs.Count; i++)
        {
            Vector3 cctvPos = ShareData.instance.jsonData.cctvs[i].position;
            cctvPos = new Vector3(cctvPos.x, 0.03f, cctvPos.z);

            GameObject cctvTemp = Instantiate(CCTVPrefeb, cctvPos, Quaternion.identity);

            cctvTemp.transform.rotation = ShareData.instance.jsonData.cctvs[i].rotation;

            cctvTemp.GetComponent<CCTVInfo>().id = ShareData.instance.jsonData.cctvs[i].id;

            CCTVList.Add(cctvTemp);
        }


        if (cctvProperty.gameObject.activeSelf)
        {
            for (int i = 0; i < CCTVList.Count; i++)
            {
                if (CCTVList[i].GetComponent<CCTVInfo>().id == cctvProperty.cctvId)
                {
                    cctvProperty.SetCCTVPosition(CCTVList[i]);
                }
            }
        }
    }

    //��� CCTV ����Ʈ ����
    public void DeleteCCTV(GameObject cctv, JsonCCTV jCCTV)
    {
        Destroy(cctv);

        for (int i = 0; i < ShareData.instance.jsonData.cctvs.Count; i++)
        {
            if (ShareData.instance.jsonData.cctvs[i] == jCCTV)
            {
                ShareData.instance.jsonData.cctvs.RemoveAt(i);
            }
        }

        UpdateCCTV();
    }

    //��� CCTV ����Ʈ ����
    public void DestroyAllCCTV()
    {
        for (int i = CCTVList.Count - 1; i >= 0; i--)
        {
            Destroy(CCTVList[i]);
        }
        CCTVList.Clear();
    }

    //�⺻���� ����
    public void StartStatus()
    {
        isStart = true;
    }

    public void EndStatus()
    {
        isStart = false;
        cctvProperty.gameObject.SetActive(false);
    }

    //�׸��� ���� ����
    public void StartDrawStatus()
    {
        tempCCTV.SetActive(true);
        isDraw = true;
    }

    public void EndDrawStatus()
    {
        isDraw = false;
        tempCCTV.SetActive(false);
        MapSceneManager.instance.uiManager.EndIsDraw();
    }


}
