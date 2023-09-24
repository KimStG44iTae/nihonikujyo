using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZoneManager : MonoBehaviour
{
    //zone 점, 바닥 프리펍
    public GameObject pointPrefeb;
    public GameObject floorPrefeb;

    //인스턴스화
    public static ZoneManager instance;

    //zone json 데이터 클래스
    public JsonZone zone;

    //점 벡터를 임시로 담아놓는 리스트
    public List<GameObject> lineRenderPoint;

    //현재 그려져 있는 모든 zone list
    public List<GameObject> zoneList;

    //현재 그리고 있는 임시 zone
    GameObject tempZone;

    //현재 그리는 상태인지
    public bool isDraw;

    //현재 기본 상태인지
    public bool isStart;

    //그리는 것을 끝낸 상태인지
    public bool isDrawEnd;

    RaycastHit hit;
    Ray ray;

    //바닥 material
    public Material zoneMt;

    //zone 속성창
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
        //기본 상태 - zone 선택
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

        //zone을 그리는 상태
        if (isDraw && !MapSceneManager.instance.isMouseInUi)
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            //그리는 상태가 끝나면 ui 상태 초기화 및 스크립트 초기화
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

            //zone 그리기
            if (Physics.Raycast(ray, out hit))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Vector3 temp = new Vector3(hit.point.x, 0.02f, hit.point.z);

                    // 이미 그려진 zone point를 클릭하면 zone 그리기를 마침
                    if (hit.transform.tag == "ZonePoint")
                    {
                        //점과 점을 이어 선을 그림
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

                    //zone point를 그림
                    GameObject pointTemp = Instantiate(pointPrefeb, temp, Quaternion.identity);

                    pointTemp.GetComponent<ZonePointInfo>().id = ShareData.instance.jsonData.lastID;

                    zone.zonePointID.Add(ShareData.instance.jsonData.lastID);

                    lineRenderPoint.Add(pointTemp);
                    zone.positionList.Add(temp);

                    pointTemp.transform.SetParent(tempZone.transform);

                    ShareData.instance.jsonData.lastID++;

                    //임시 zone 포인트 개수가 2개보다 클 경우
                    if (zone.positionList.Count > 2)
                    {
                        lineRenderPoint[0] = lineRenderPoint[1];
                        lineRenderPoint[1] = lineRenderPoint[2];
                        lineRenderPoint.RemoveAt(2);

                        MakeLine(lineRenderPoint[0].transform.position, lineRenderPoint[1].transform.position, lineRenderPoint[0]);
                    }
                    //임시 zone 포인트 개수가 2개일 경우
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

    //현재 있는 모든 zone 새로 그림
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

    //모든 zone 제거
    public void DestroyAllZone()
    {
        for (int i = zoneList.Count - 1; i >= 0; i--)
        {
            Destroy(zoneList[i]);
        }
        zoneList.Clear();
    }

    //현재 있는 모든 zone "바닥" 다시 그림
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

    //현재 있는 모든 zone "선" 다시 그림
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

    //점과 점을 이어 선을 그림
    public void MakeLine(Vector3 v1, Vector3 v2, GameObject point)
    {
        point.GetComponent<LineRenderer>().SetPosition(0, v1);
        point.GetComponent<LineRenderer>().SetPosition(1, v2);
    }

    // 기본 상태 변경
    public void StartStatus()
    {
        isStart = true;
    }

    public void EndStatus()
    {
        isStart = false;
        zoneProperty.gameObject.SetActive(false);
    }

    // 그리기 상태 변경
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
