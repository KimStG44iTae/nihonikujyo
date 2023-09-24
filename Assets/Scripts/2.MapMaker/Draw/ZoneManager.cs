using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZoneManager : MonoBehaviour
{
    //zone ��, �ٴ� ������
    public GameObject pointPrefeb;
    public GameObject floorPrefeb;

    //�ν��Ͻ�ȭ
    public static ZoneManager instance;

    //zone json ������ Ŭ����
    public JsonZone zone;

    //�� ���͸� �ӽ÷� ��Ƴ��� ����Ʈ
    public List<GameObject> lineRenderPoint;

    //���� �׷��� �ִ� ��� zone list
    public List<GameObject> zoneList;

    //���� �׸��� �ִ� �ӽ� zone
    GameObject tempZone;

    //���� �׸��� ��������
    public bool isDraw;

    //���� �⺻ ��������
    public bool isStart;

    //�׸��� ���� ���� ��������
    public bool isDrawEnd;

    RaycastHit hit;
    Ray ray;

    //�ٴ� material
    public Material zoneMt;

    //zone �Ӽ�â
    public ZoneProperty zoneProperty;

    public Button button;

    public Sprite buttonOn;
    public Sprite buttonOff;

    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void Update()
    {
        //�⺻ ���� - zone ����
        if (isStart)
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                Vector3 temp = new Vector3(hit.point.x, 0.03f, hit.point.z);

                if (Input.GetMouseButtonDown(0))
                {
                    if (hit.transform.tag == "Zone")
                    {
                        MapSceneManager.instance.uiManager.OffSelect_All();
                        MapSceneManager.instance.uiManager.InitStatus();
                        MapSceneManager.instance.uiManager.NowStepUI();

                        zoneProperty.gameObject.SetActive(true);
                        zoneProperty.SetZone(hit.transform.gameObject);
                    }
                    else if (hit.transform.tag == "Point")
                    {
                        zoneProperty.gameObject.SetActive(false);
                    }
                }
            }
        }

        //zone�� �׸��� ����
        if (isDraw && !MapSceneManager.instance.isMouseInUi)
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            //�׸��� ���°� ������ ui ���� �ʱ�ȭ �� ��ũ��Ʈ �ʱ�ȭ
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

            //zone �׸���
            if (Physics.Raycast(ray, out hit))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Vector3 temp = new Vector3(hit.point.x, 0.02f, hit.point.z);

                    // �̹� �׷��� zone point�� Ŭ���ϸ� zone �׸��⸦ ��ħ
                    if (hit.transform.tag == "ZonePoint")
                    {
                        //���� ���� �̾� ���� �׸�
                        MakeLine(zone.positionList[zone.positionList.Count-1], hit.transform.position, lineRenderPoint[1]);

                        zoneList.Add(tempZone);
                        zone.positionList.Add(zone.positionList[0]);
                        zone.zonePointID.Add(zone.zonePointID[0]);

                        GameObject tempFloor = Instantiate(floorPrefeb);
                        tempFloor = MapSceneManager.instance.makeMesh.Make(zone.positionList, tempFloor, zoneMt);
                        tempFloor.transform.SetParent(tempZone.transform);
                        tempFloor.transform.position = new Vector3(0, 0.02f, 0);

                        tempFloor.AddComponent<ZoneInfo>();
                        tempFloor.GetComponent<ZoneInfo>().id = ShareData.instance.jsonData.lastID;
                        tempFloor.tag = "Zone";

                        zone.id = tempFloor.GetComponent<ZoneInfo>().id;

                        MapSceneManager.instance.shareData.AddJsonZone(zone);

                        ShareData.instance.jsonData.lastID++;
                        MapSceneManager.instance.unDoManager.AddUndo();

                        isDrawEnd = true;
                        return;
                    }

                    //zone point�� �׸�
                    GameObject pointTemp = Instantiate(pointPrefeb, temp, Quaternion.identity);

                    pointTemp.GetComponent<ZonePointInfo>().id = ShareData.instance.jsonData.lastID;

                    zone.zonePointID.Add(ShareData.instance.jsonData.lastID);

                    lineRenderPoint.Add(pointTemp);
                    zone.positionList.Add(temp);

                    pointTemp.transform.SetParent(tempZone.transform);

                    ShareData.instance.jsonData.lastID++;

                    //�ӽ� zone ����Ʈ ������ 2������ Ŭ ���
                    if (zone.positionList.Count > 2)
                    {
                        lineRenderPoint[0] = lineRenderPoint[1];
                        lineRenderPoint[1] = lineRenderPoint[2];
                        lineRenderPoint.RemoveAt(2);

                        MakeLine(lineRenderPoint[0].transform.position, lineRenderPoint[1].transform.position, lineRenderPoint[0]);
                    }
                    //�ӽ� zone ����Ʈ ������ 2���� ���
                    else if (zone.positionList.Count == 2)
                    {
                        MakeLine(lineRenderPoint[0].transform.position, lineRenderPoint[1].transform.position, lineRenderPoint[0]);
                    }


                }

                if (Input.GetMouseButtonUp(1))
                {
                    CancleDrawZone();

                }
            }

        }
    }

    //���� �ִ� ��� zone ���� �׸�
    public void UpdateZone()
    {
        for (int i = zoneList.Count - 1; i >= 0; i--)
        {
            Destroy(zoneList[i]);
        }

        zoneList.Clear();

        for (int i = 0; i < ShareData.instance.jsonData.zones.Count; i++)
        {
            tempZone = new GameObject();
            tempZone.name = "zone";

            zoneList.Add(tempZone);

            JsonZone tempJsonZone = ShareData.instance.jsonData.zones[i];

            List<GameObject> tempJsonPointList = new List<GameObject>();
            List<Vector3> tempJsonPositionList = new List<Vector3>();

            for (int j = 0; j < tempJsonZone.positionList.Count; j++)
            {
                if (j != tempJsonZone.positionList.Count - 1)
                {
                    GameObject pointTemp = Instantiate(pointPrefeb, tempJsonZone.positionList[j], Quaternion.identity);
                    pointTemp.GetComponent<ZonePointInfo>().id = tempJsonZone.zonePointID[j];

                    tempJsonPointList.Add(pointTemp);
                    tempJsonPositionList.Add(tempJsonZone.positionList[j]);
                    pointTemp.transform.SetParent(tempZone.transform);
                }
            }

            UpdateZoneLine(tempJsonPointList);

            tempJsonPositionList.Add(tempJsonPositionList[0]);

            UpdateZoneFloor(tempZone, tempJsonZone, tempJsonPositionList);
        }

        if (zoneProperty.gameObject.activeSelf)
        {
            for (int i = 0; i < zoneList.Count; i++)
            {
                if (zoneList[i].GetComponent<ZonePointInfo>().id == zoneProperty.zoneId)
                {
                    zoneProperty.SetZone(zoneList[i]);
                }
            }
        }
    }

    public void CancleDrawZone()
    {
        GameObject[] zonePointList = GameObject.FindGameObjectsWithTag("ZonePoint");

        for (int i = zonePointList.Length-1 ; i >= 0; i--)
        {
            for (int j = 0; j < zone.zonePointID.Count; j++)
            {
                if (zonePointList[i].GetComponent<ZonePointInfo>().id == zone.zonePointID[j])
                {
                    Destroy(zonePointList[i]);
                    break;
                }
            }
        }

        EndDrawStatus();
        StartStatus();
        MapSceneManager.instance.uiManager.NowStepUI();
    }

    //��� zone ����
    public void DestroyAllZone()
    {
        for (int i = zoneList.Count - 1; i >= 0; i--)
        {
            Destroy(zoneList[i]);
        }
        zoneList.Clear();
    }

    //���� �ִ� ��� zone "�ٴ�" �ٽ� �׸�
    public GameObject UpdateZoneFloor(GameObject tempZone,JsonZone tempJsonZone, List<Vector3> tempJsonPositionList)
    {

        GameObject tempFloor = Instantiate(floorPrefeb);
        tempFloor = MapSceneManager.instance.makeMesh.Make(tempJsonPositionList, tempFloor, zoneMt);
        tempFloor.transform.SetParent(tempZone.transform);
        tempFloor.transform.position = new Vector3(0, 0.02f, 0);

        tempFloor.AddComponent<ZoneInfo>();
        tempFloor.GetComponent<ZoneInfo>().id = tempJsonZone.id;
        tempFloor.tag = "Zone";

        Color tempColor = new Color();
        ColorUtility.TryParseHtmlString("#" + tempJsonZone.color, out tempColor);

        tempFloor.GetComponent<MeshRenderer>().material.color = tempColor;

        return tempFloor;
    }

    //���� �ִ� ��� zone "��" �ٽ� �׸�
    public void UpdateZoneLine(List<GameObject> tempJsonPointList)
    {
        for (int j = 0; j < tempJsonPointList.Count; j++)
        {
            if (j != tempJsonPointList.Count - 1)
            {
                MakeLine(tempJsonPointList[j].transform.position, tempJsonPointList[j + 1].transform.position, tempJsonPointList[j]);
            }
            else
            {
                MakeLine(tempJsonPointList[j].transform.position, tempJsonPointList[0].transform.position, tempJsonPointList[j]);
            }
        }
    }

    //���� ���� �̾� ���� �׸�
    public void MakeLine(Vector3 v1, Vector3 v2, GameObject point)
    {
        point.GetComponent<LineRenderer>().SetPosition(0, v1);
        point.GetComponent<LineRenderer>().SetPosition(1, v2);
    }

    // �⺻ ���� ����
    public void StartStatus()
    {
        isStart = true;
    }

    public void EndStatus()
    {
        isStart = false;
        zoneProperty.gameObject.SetActive(false);
    }

    // �׸��� ���� ����
    public void StartDrawStatus()
    {
        zone = new JsonZone();
        tempZone = new GameObject();
        tempZone.name = "zone";
        isDraw = true;
    }

    public void EndDrawStatus()
    {
        isDraw = false;
        zone = null;
        tempZone = null;
        lineRenderPoint.Clear();
        MapSceneManager.instance.uiManager.EndIsDraw();
    }
}
