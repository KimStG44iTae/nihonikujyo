using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeModel : MonoBehaviour
{
    //3d 생성에 필요한 벽, 바닥, cctv, uwb 프리펍
    public GameObject wallPrefb;
    public GameObject floorPrefb;
    public GameObject cctvPrefb;
    public GameObject uwbPrefb;
    public GameObject fireexitPrefb;

    //현재 생성되는 외벽 바닥
    public GameObject nowFloor;

    //생성된 모든 오브젝트를 담는 부모 오브젝트
    public GameObject Model;

    //바닥 메테리얼
    public Material floorMt;
    //zone 메테리얼
    public Material zoneMt;

    public void StartMakeModel()
    {
        if (Model != null)
        {
            Destroy(Model);
            Model = null;
        }

        List<Vector3> facePoints = new List<Vector3>();

        JsonData jData = ShareData.instance.jsonData;

        Model = Instantiate(new GameObject());
        Model.name = "Model";

        //바닥 생성
        nowFloor = Instantiate(floorPrefb);
        MakeFace.GetFaces(jData);

        //외벽으로 인해 생성되는 첫번째 바닥만 생성
        if (jData.faces.Count>0)
        {
            for (int i = 0; i < jData.faces[0].face.Count; i++)
            {
                int pointId = jData.faces[0].face[i];
                Vector3 pointPos = jData.points.Find(x => x.id == pointId).position;

                facePoints.Add(pointPos);
            }

            facePoints.Add(facePoints[0]);
            GetComponent<MakeMesh>().Make(facePoints, nowFloor,floorMt);
            nowFloor.transform.SetParent(Model.transform);
        }

        //zone 생성
        for (int i = 0; i < jData.zones.Count; i++)
        {
            nowFloor = Instantiate(floorPrefb);
            GetComponent<MakeMesh>().Make(jData.zones[i].positionList, nowFloor, zoneMt);

            Color tempColor = new Color();
            ColorUtility.TryParseHtmlString("#" + jData.zones[i].color, out tempColor);

            tempColor.a = 0.5f;

            nowFloor.GetComponent<MeshRenderer>().material.color = tempColor;

            nowFloor.name = jData.zones[i].name;
            nowFloor.transform.position += Vector3.up * 0.001f;
            nowFloor.tag = "Zone";
            nowFloor.transform.SetParent(Model.transform);
        }

        //벽생성
        for (int i = 0; i < jData.lines.Count; i++)
        {
            if (jData.lines[i].isStandard)
            {
                continue;
            }

            Vector3 startPos = jData.points.Find(x => x.id == jData.lines[i].startPointId).position;
            Vector3 endPos = jData.points.Find(x => x.id == jData.lines[i].endPointId).position;

            Vector3 center = (startPos + endPos) / 2f;
            center += Vector3.up;

            float distance = Vector3.Distance(startPos, endPos);
            
            float angle = Mathf.Atan2(startPos.x - endPos.x, startPos.z - endPos.z) * Mathf.Rad2Deg;

            Quaternion rotation = Quaternion.Euler(new Vector3(0, angle+90f, 0));

            GameObject tempWall = Instantiate(wallPrefb, center, rotation);

            tempWall.transform.localScale = new Vector3(distance, 2, 0.1f);

            tempWall.transform.SetParent(Model.transform);

            //크기에 따른 벽 타일링
            Vector2 tiling = new Vector2(tempWall.transform.localScale.x, 2);
            tempWall.GetComponent<MeshRenderer>().material.mainTextureScale = tiling;
        }

        //CCTV 설치
        for (int i = 0; i < jData.cctvs.Count; i++)
        {
            Vector3 cctvPos = jData.cctvs[i].position; 
            Quaternion cctvRot = jData.cctvs[i].rotation;

            GameObject tempCCtv = Instantiate(cctvPrefb, cctvPos, cctvRot);

            tempCCtv.transform.SetParent(Model.transform);
            tempCCtv.name = jData.cctvs[i].ip;
            tempCCtv.tag = "CCTV";
        }

        //UWB 설치
        for (int i = 0; i < jData.uwbs.Count; i++)
        {
            Vector3 uwbPos = jData.uwbs[i].position;

            GameObject tempUWB = Instantiate(uwbPrefb, uwbPos, Quaternion.identity);

            tempUWB.transform.SetParent(Model.transform);
            tempUWB.tag = "UWB";
        }

        for (int i = 0; i < jData.fireexits.Count; i++)
        {
            Vector3 fireexitPos = jData.fireexits[i].position;
            Quaternion fireexitRot = jData.fireexits[i].rotation;

            GameObject tempFireExit = Instantiate(fireexitPrefb, fireexitPos, fireexitRot);

            tempFireExit.transform.SetParent(Model.transform);
            tempFireExit.name = jData.fireexits[i].ip;
            tempFireExit.tag = "FireExit";
        }

        Model.transform.position = new Vector3(1000, 1000, 1000);

    }

}
