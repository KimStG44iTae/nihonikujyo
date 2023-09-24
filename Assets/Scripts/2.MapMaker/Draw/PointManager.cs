using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointManager : MonoBehaviour
{
    //�� ������
    public GameObject pointPrefeb;

    //�ν��Ͻ�ȭ
    public static PointManager instance;

    //�� �Ӽ�â
    public PointProperty pointProperty;

    //���� �׸��� ���� �ӽ� ������ ���� ����Ʈ
    public List<JsonPoint> pointPosition;

    //���� �ִ� ��� �� ����Ʈ
    public List<GameObject> pointList;

    public bool isDraw;
    public bool isStart;
    public bool isDrawEnd;

    RaycastHit hit;
    Ray ray;

    //���� ���õ� ��
    public GameObject selectPoint;

    Vector3 firstVector;
    
    public Button button;

    public Sprite buttonOn;
    public Sprite buttonOff;

    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        pointPosition = new List<JsonPoint>();
    }

    void Update()
    {
        if (!isStart )
        {
        }
        //���� �ǳ��� �ƴ� ���� ������ ������ �ִ��� 
        else if (!MapSceneManager.instance.isMouseInUi)
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
           
            if (Physics.Raycast(ray, out hit))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (hit.transform.tag == "Point")
                    {
                        MapSceneManager.instance.uiManager.OffSelect_All();

                        MapSceneManager.instance.uiManager.InitStatus();
                        MapSceneManager.instance.uiManager.NowStepUI();

                        selectPoint = hit.transform.gameObject;
                        pointProperty.gameObject.SetActive(true);
                        pointProperty.GetComponent<PointProperty>().SetPoint(selectPoint);
                        firstVector = new Vector3(hit.point.x, 0.03f, hit.point.z);
                    }
                    //�����Ѱ� zone�̸� �� �Ӽ�â off
                    else if (hit.transform.tag == "Zone")
                    {
                        pointProperty.gameObject.SetActive(false);
                    }
                }
                else if (Input.GetMouseButton(0))
                {
                    //���� ���õ� �� ��ġ �ű�鼭 �Ӽ�â�� �ݿ�, �� ������Ʈ
                    if (selectPoint !=null)
                    {
                        Vector3 hitPoint = new Vector3(hit.point.x, 0.03f, hit.point.z);

                        if (Vector3.Distance(firstVector, hitPoint) > 0.01f)
                        {
                            selectPoint.transform.position = hitPoint;

                            ShareData.instance.jsonData.points.Find(x => x.id == selectPoint.GetComponent<PointInfo>().id).position = selectPoint.transform.position;

                            MapSceneManager.instance.lineManager.LineUpdate();
                        }
                    }
                
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    if (selectPoint !=null)
                    {
                        MapSceneManager.instance.unDoManager.AddUndo();
                        selectPoint = null;
                    }
                }

                if (Input.GetMouseButtonDown(1))
                {
                    pointProperty.gameObject.SetActive(false);
                }

            }
        }

        if (!isDraw || MapSceneManager.instance.isMouseInUi)
        {
        }
        // ���� �׸��� �����̰� ������ �� �ִ� ������ ��� ����
        else
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            //�׸��� ���°� ������ �ʱ�ȭ
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
                if (Input.GetMouseButtonDown(0))
                {
                    Vector3 temp = new Vector3(hit.point.x, 0.02f, hit.point.z);

                    // �̹� �׷��� ���� Ŭ���ϸ� �� �׸��⸦ ��ħ
                    if (hit.transform.tag == "Point" && (pointPosition.Count != 0))
                    {
                        int tempid = hit.transform.GetComponent<PointInfo>().id;

                        JsonPoint tempjPoint = new JsonPoint();
                        tempjPoint = ShareData.instance.jsonData.points.Find(x => x.id == tempid);

                        if (pointPosition.Count == 1)
                        {
                            LineManager.instance.MakeLine(pointPosition[0], tempjPoint);
                        }
                        else
                        {
                            LineManager.instance.MakeLine(pointPosition[1], tempjPoint);
                        }

                        isDrawEnd = true;

                        MapSceneManager.instance.unDoManager.AddUndo();

                        return;
                    }
                    else if (hit.transform.tag == "Point" && (pointPosition.Count == 0))
                    {
                        JsonPoint jPoint = new JsonPoint();
                        jPoint.position = hit.transform.position;
                        jPoint.id = hit.transform.GetComponent<PointInfo>().id;

                        pointPosition.Add(jPoint);
                        return;
                    }
                    else
                    {
                        //�� �׸���
                        GameObject pointTemp = Instantiate(pointPrefeb, temp, Quaternion.identity);

                        pointTemp.GetComponent<PointInfo>().id = ShareData.instance.jsonData.lastID;

                        pointList.Add(pointTemp);

                        JsonPoint jPoint = new JsonPoint();
                        jPoint.position = temp;
                        jPoint.id = ShareData.instance.jsonData.lastID;

                        pointPosition.Add(jPoint);

                        MapSceneManager.instance.shareData.AddJsonPoint(jPoint);

                        ShareData.instance.jsonData.lastID++;
                    }


                    if (pointPosition.Count > 2)
                    {
                        pointPosition[0] = pointPosition[1];
                        pointPosition[1] = pointPosition[2];
                        pointPosition.RemoveAt(2);

                        LineManager.instance.MakeLine(pointPosition[0], pointPosition[1]);
                    }
                    else if (pointPosition.Count == 2)
                    {
                        LineManager.instance.MakeLine(pointPosition[0], pointPosition[1]);

                    }

                    MapSceneManager.instance.uiManager.InfomationUpdate();
                    MapSceneManager.instance.unDoManager.AddUndo();

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

    //��� �� �����ϰ� �ٽ� �׸�
    public void PointUpdate()
    {
        DestroyAllPoint();

        for (int i = 0; i < ShareData.instance.jsonData.points.Count; i++)
        {
            bool isIn = false;

            JsonPoint tempPoint = ShareData.instance.jsonData.points[i];

            for (int j = 0; j < ShareData.instance.jsonData.lines.Count; j++)
            {
                JsonLine tempLine = ShareData.instance.jsonData.lines[j];

                if ((tempLine.startPointId == tempPoint.id) || (tempLine.endPointId == tempPoint.id))
                {
                    isIn = true;
                }
            }

            if (!isIn)
            {
                continue;
            }

            GameObject pointTemp = Instantiate(pointPrefeb, tempPoint.position, Quaternion.identity);

            pointTemp.GetComponent<PointInfo>().id = tempPoint.id;

            pointList.Add(pointTemp);
        }

        if (pointProperty.gameObject.activeSelf)
        {
            for (int i = 0; i < pointList.Count; i++)
            {
                if (pointList[i].GetComponent<PointInfo>().id == pointProperty.pointId)
                {
                    pointProperty.SetPoint(pointList[i]);
                }
            }
        }
    }

    public int NoMatcingPoint()
    {
        int noCount = 0;

        for (int i = 0; i < ShareData.instance.jsonData.points.Count; i++)
        {
            JsonPoint tempPoint = ShareData.instance.jsonData.points[i];

            bool isIn = false;

            for (int j = 0; j < ShareData.instance.jsonData.lines.Count; j++)
            {
                JsonLine tempLine = ShareData.instance.jsonData.lines[j];

                if ((tempLine.startPointId == tempPoint.id) || (tempLine.endPointId == tempPoint.id))
                {
                    isIn = true;
                }

            }

            if (!isIn)
            {
                ShareData.instance.jsonData.points.RemoveAt(i);
                noCount++;
            }
        }

        return noCount;
    }

    // Ư�� �� ����
    public void DeletePoint(GameObject point, JsonPoint jPoint)
    {
        Destroy(point);

        for (int i = 0; i < ShareData.instance.jsonData.points.Count; i++)
        {
            if (ShareData.instance.jsonData.points[i] == jPoint)
            {
                ShareData.instance.jsonData.points.RemoveAt(i);
            }
        }

        PointUpdate();
    }

    //��� �� ����
    public void DestroyAllPoint()
    {
        for (int i = pointList.Count - 1; i >= 0; i--)
        {
            Destroy(pointList[i]);
        }
        pointList.Clear();
    }

    //�⺻ ���� ����
    public void StartStatus()
    {
        isStart = true;
    }

    public void EndStatus()
    {
        isStart = false;
        pointProperty.gameObject.SetActive(false);
    }

    //�׸��� ���� ����
    public void StartDrawStatus()
    {
        isDraw = true;
    }

    public void EndDrawStatus()
    {
        isDraw = false;
        pointPosition.Clear();
        MapSceneManager.instance.uiManager.EndIsDraw();
    }
}
