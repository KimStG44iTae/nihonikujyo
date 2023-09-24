using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineManager : MonoBehaviour
{
    public static LineManager instance;

    //선 프리펍
    public GameObject linePrefeb;

    //모든 선 리스트
    public List<GameObject> lineList;

    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    //모든 선 지우고 새로 그리기
    public void LineUpdate()
    {
        for (int i = lineList.Count-1; i >= 0; i--)
        {
            Destroy(lineList[i]);
        }

        lineList.Clear();

        for (int i = 0; i < ShareData.instance.jsonData.lines.Count; i++)
        {
            int startPointId = ShareData.instance.jsonData.lines[i].startPointId;
            int endPointId = ShareData.instance.jsonData.lines[i].endPointId;

            JsonPoint j1 = ShareData.instance.jsonData.points.Find(x => x.id == startPointId);
            JsonPoint j2 = ShareData.instance.jsonData.points.Find(x => x.id == endPointId);

            MakeLine2(j1, j2, ShareData.instance.jsonData.lines[i]);
        }
    }

    //선 그리기1
    public void MakeLine(JsonPoint v1, JsonPoint v2)
    {
        GameObject line = Instantiate(linePrefeb);
        line.GetComponent<LineRenderer>().SetPosition(0, v1.position);
        line.GetComponent<LineRenderer>().SetPosition(1, v2.position);

        lineList.Add(line);

        line.GetComponent<LineInfo>().id = ShareData.instance.jsonData.lastID;
        line.GetComponent<LineInfo>().startId = v1.id;
        line.GetComponent<LineInfo>().endId = v2.id;


        JsonLine jLine = new JsonLine();
        jLine.id = ShareData.instance.jsonData.lastID;
        jLine.startPointId = v1.id;
        jLine.endPointId = v2.id;

        MapSceneManager.instance.shareData.AddJsonLine(jLine);

        ShareData.instance.jsonData.lastID++;

    }

    //선 그리기2
    public void MakeLine2(JsonPoint p1, JsonPoint p2, JsonLine L1)
    {
        GameObject line = Instantiate(linePrefeb);
        lineList.Add(line);

        line.GetComponent<LineRenderer>().SetPosition(0, p1.position);
        line.GetComponent<LineRenderer>().SetPosition(1, p2.position);

        line.GetComponent<LineInfo>().id = L1.id;
        line.GetComponent<LineInfo>().startId = L1.startPointId;
        line.GetComponent<LineInfo>().endId = L1.endPointId;

    }

    //특정 선 삭제
    public void DeleteLine(JsonPoint point)
    {
        int pointId = point.id;

        List<int> lineIdList = new List<int>();

        for (int i = ShareData.instance.jsonData.lines.Count - 1; i >= 0; i--)
        {
            if ((ShareData.instance.jsonData.lines[i].startPointId == pointId) ||(ShareData.instance.jsonData.lines[i].endPointId == pointId))
            {
                lineIdList.Add(ShareData.instance.jsonData.lines[i].id);
                ShareData.instance.jsonData.lines.RemoveAt(i);
            }
        }

        for (int i = lineList.Count - 1; i >= 0; i--)
        {
            for (int j = 0; j < lineIdList.Count; j++)
            {
                if (lineList[i].GetComponent<LineInfo>().id == lineIdList[j])
                {
                    Destroy(lineList[i]);
                    lineList.RemoveAt(i);
                    break;
                }
            }
        }

    }

    //모든 선 삭제
    public void DestroyAllLine()
    {
        for (int i = lineList.Count - 1; i >= 0; i--) 
        {
            Destroy(lineList[i]);
        }
        lineList.Clear();
    }

    //충돌 체크를 위해 선에 콜라이더 생성
    public void MakeCollider()
    {
        for (int i = 0; i < lineList.Count; i++)
        {
            if (lineList[i].GetComponent<MeshCollider>())
            {
                Destroy(lineList[i].GetComponent<MeshCollider>());
            }

            LineRenderer lineRenderer = lineList[i].GetComponent<LineRenderer>();
            MeshCollider meshCollider = lineList[i].AddComponent<MeshCollider>();

            Mesh mesh = new Mesh();
            lineRenderer.BakeMesh(mesh, true);
            meshCollider.sharedMesh = mesh;
        }
    }
}
