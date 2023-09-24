using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnDoManager : MonoBehaviour
{
    public List<JsonData> undoJsonList;
    public List<JsonData> redoJsonList;

    public void Start()
    {
        JsonData tempJson = new JsonData();
        undoJsonList.Add(tempJson);
    }

    public void AddUndo()
    {
        if (undoJsonList.Count > 30)
        {
            undoJsonList.RemoveAt(1);
        }

        JsonData tempJson = new JsonData();
        tempJson = ShareData.instance.jsonData.DeepCopy();
        undoJsonList.Add(tempJson);
    }

    public void ExcuteUndo()
    {
        if (undoJsonList.Count < 2)
        {
            return;
        }

        if (undoJsonList.Count == 2)
        {
            ShareData.instance.jsonData = undoJsonList[0];

            JsonData tempJson = new JsonData();
            tempJson = ShareData.instance.jsonData.DeepCopy();

            ShareData.instance.jsonData = tempJson;

            undoJsonList.RemoveAt(undoJsonList.Count - 1);

            MapSceneManager.instance.uiManager.InitStatus();
            MapSceneManager.instance.uiManager.NowStepUI();
            MapSceneManager.instance.uiManager.OffSelect_All();
        }
        else if (undoJsonList.Count > 1)
        {
            undoJsonList.RemoveAt(undoJsonList.Count - 1);

            ShareData.instance.jsonData = undoJsonList[undoJsonList.Count - 1];

            int noMatchingPointCount = MapSceneManager.instance.pointManager.NoMatcingPoint();

            for (int i = 0; i < noMatchingPointCount; i++)
            {
                undoJsonList.RemoveAt(undoJsonList.Count - 1);
            }

            if (undoJsonList.Count != 0)
            {
                ShareData.instance.jsonData = undoJsonList[undoJsonList.Count - 1];
            }

            JsonData tempJson = new JsonData();
            tempJson = ShareData.instance.jsonData.DeepCopy();

            ShareData.instance.jsonData = tempJson;
        }

        MapSceneManager.instance.cctvManager.UpdateCCTV();
        MapSceneManager.instance.uwbManager.UpdateUWB();
        MapSceneManager.instance.lineManager.LineUpdate();
        MapSceneManager.instance.pointManager.PointUpdate();
        MapSceneManager.instance.zoneManager.UpdateZone();
        MapSceneManager.instance.markerManager.UpdateMarker();
        MapSceneManager.instance.boardManager.UpdateBoard();

    }

    public void ResetUndo()
    {
        undoJsonList.Clear();

        JsonData tempJson = new JsonData();
        undoJsonList.Add(tempJson);
    }
}
