using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ShareData : MonoBehaviour
{
    public JsonData jsonData;

    public static ShareData instance;

    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

        jsonData = new JsonData();
    }

    public void InitData()
    {
        jsonData = new JsonData();

        MapSceneManager.instance.pointManager.DestroyAllPoint();
        MapSceneManager.instance.lineManager.DestroyAllLine();
        MapSceneManager.instance.uwbManager.DestroyAllUWB();
        MapSceneManager.instance.cctvManager.DestroyAllCCTV();
        MapSceneManager.instance.zoneManager.DestroyAllZone();
        MapSceneManager.instance.boardManager.DestroyBoard();
        MapSceneManager.instance.markerManager.DestroyMarker();
        MapSceneManager.instance.fireexitManager.DestroyAllFireExit();
    }

    public void AddJsonBoard(JsonBoard jsonBoard)
    {
        jsonData.boards.Add(jsonBoard);
    }

    public void AddJsonMarker(JsonMarker jsonMarker)
    {
        jsonData.markers.Add(jsonMarker);
    }

    public void AddJsonZone(JsonZone jsonZone)
    {
        jsonData.zones.Add(jsonZone);
    }

    public void AddJsonPoint(JsonPoint jsonPoint)
    {
        jsonData.points.Add(jsonPoint);
    }

    public void AddJsonCCTV(JsonCCTV jsonCCTV)
    {
        jsonData.cctvs.Add(jsonCCTV);
    }

    public void AddJsonUWB(JsonUWB jsonUWB)
    {
        jsonData.uwbs.Add(jsonUWB);
    }

    public void AddJsonLine(JsonLine jsonLine)
    {
        jsonData.lines.Add(jsonLine);
    }

    public void AddJsonFace(JsonFace jsonFace)
    {
        jsonData.faces.Add(jsonFace);
    }

    public void AddJsonFireExit(JsonFireExit jsonFireExit)
    {
        jsonData.fireexits.Add(jsonFireExit);
    }
}
