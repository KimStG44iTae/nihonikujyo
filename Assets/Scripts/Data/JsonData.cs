using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

[Serializable]
public class JsonData
{
    public string mapName;

    public List<JsonPoint> points;
    public List<JsonLine> lines;
    public List<JsonFace> faces;
    public List<JsonCCTV> cctvs;
    public List<JsonUWB> uwbs;
    public List<JsonZone> zones;
    public List<JsonFireExit> fireexits;
    
    public List<JsonMarker> markers;
    public List<JsonBoard> boards;

    public int id; // 고유 인덱스를 위한 ID

    public Vector3 imagePos;
    public Quaternion imageRot;
    public float ratio;
    public int lastID;

    //public Vector3 vuforiaOriginPos;

    public JsonData()
    {
        mapName = "";
        points = new List<JsonPoint>();
        lines = new List<JsonLine>();
        faces = new List<JsonFace>();
        cctvs = new List<JsonCCTV>();
        uwbs = new List<JsonUWB>();
        zones = new List<JsonZone>();
        markers = new List<JsonMarker>();
        boards = new List<JsonBoard>();
        fireexits = new List<JsonFireExit>();

        imageRot = Quaternion.Euler(90, 0, 0);
        ratio = 1;
        lastID = 0;
    }

    public JsonData DeepCopy()
    {
        JsonData jsonData = new JsonData();

        jsonData.id = this.id;
        jsonData.imagePos = this.imagePos;
        jsonData.imageRot = this.imageRot;
        jsonData.ratio = this.ratio;
        jsonData.lastID = this.lastID;
      //  jsonData.vuforiaOriginPos = this.vuforiaOriginPos;

        List<JsonPoint> pointList = new List<JsonPoint>();
        List<JsonLine> linesList = new List<JsonLine>();
        List<JsonFace> facesList = new List<JsonFace>();
        List<JsonCCTV> cctvsList = new List<JsonCCTV>();
        List<JsonUWB> uwbsList = new List<JsonUWB>();
        List<JsonZone> zonesList = new List<JsonZone>();
        List<JsonMarker> markersList = new List<JsonMarker>();
        List<JsonBoard> boardsList = new List<JsonBoard>();
        List<JsonFireExit> fireexitsList = new List<JsonFireExit>();

        for (int i = 0; i < points.Count; i++)
        {
            JsonPoint tempPoint = new JsonPoint();
            tempPoint = points[i].CopyPoint();

            pointList.Add(tempPoint);
        }
        jsonData.points = pointList;

        for (int i = 0; i < lines.Count; i++)
        {
            JsonLine tempLine = new JsonLine();
            tempLine = lines[i].CopyLine();

            linesList.Add(tempLine);
        }
        jsonData.lines = linesList;

        for (int i = 0; i < faces.Count; i++)
        {
            JsonFace tempFace = new JsonFace();
            tempFace = faces[i].CopyFace();

            facesList.Add(tempFace);
        }
        jsonData.faces = facesList;

        for (int i = 0; i < cctvs.Count; i++)
        {
            JsonCCTV tempCCTV = new JsonCCTV();
            tempCCTV = cctvs[i].CopyCCTV();

            cctvsList.Add(tempCCTV);
        }
        jsonData.cctvs = cctvsList;


        for (int i = 0; i < uwbs.Count; i++)
        {
            JsonUWB tempUWB = new JsonUWB();
            tempUWB = uwbs[i].CopyUWB();

            uwbsList.Add(tempUWB);
        }
        jsonData.uwbs = uwbsList;

        for (int i = 0; i < zones.Count; i++)
        {
            JsonZone tempZone = new JsonZone();
            tempZone = zones[i].CopyZone();

            zonesList.Add(tempZone);
        }
        jsonData.zones = zonesList;

        for (int i = 0; i < markers.Count; i++)
        {
            JsonMarker tempMarker = new JsonMarker();
            tempMarker = markers[i].CopyMarker();

            markersList.Add(tempMarker);
        }
        jsonData.markers = markersList;

        for (int i = 0; i < boards.Count; i++)
        {
            JsonBoard tempBoard = new JsonBoard();
            tempBoard = boards[i].CopyBoard();

            boardsList.Add(tempBoard);
        }
        jsonData.boards = boardsList;

        for (int i = 0; i < fireexits.Count; i++)
        {
            JsonFireExit tempFireExit = new JsonFireExit();
            tempFireExit = fireexits[i].CopyFireExit();

            fireexitsList.Add(tempFireExit);
        }
        jsonData.fireexits = fireexitsList;

        return jsonData;
    }

    public Vector3 GetPosition(int id)
    {
        Vector3 position = points.Find(x => x.id == id).position;
        return position;
    }
}

[Serializable]
public class JsonPoint
{
    public int id;
    public Vector3 position;

    public JsonPoint()
    {
        position = new Vector3();
    }
    public JsonPoint(JsonPoint jp)
    {
        this.id = jp.id;
        this.position = jp.position;
    }

    public JsonPoint CopyPoint()
    {
        JsonPoint tempPoint = new JsonPoint();
        tempPoint.position = position;
        tempPoint.id = id;

        return tempPoint;
    }
}

[Serializable]
public class JsonLine
{
    public int id;
    public int startPointId;
    public int endPointId;
    public float height;
    public Vector3 controlPoint;

    public bool isStandard;

    public JsonLine()
    {
        controlPoint = new Vector3();
        height = 2000;
    }
    public JsonLine(JsonLine jl)
    {
        id = jl.id;
        startPointId = jl.startPointId;
        endPointId = jl.endPointId;
        controlPoint = jl.controlPoint;
        height = jl.height;
    }

    public JsonLine CopyLine()
    {
        JsonLine tempLine = new JsonLine();
        tempLine.id = id;
        tempLine.startPointId = startPointId;
        tempLine.endPointId = endPointId;
        tempLine.height = height;
        tempLine.controlPoint = controlPoint;
        tempLine.isStandard = isStandard;

        return tempLine;
    }
}


[Serializable]
public class JsonCCTV
{
    public int id;
    public string ip;
    public string zone;
    public Vector3 position;
    public Quaternion rotation;

    public bool isMatchBoard;

    public JsonCCTV()
    {
        position = new Vector3();
        ip = "127.0.0.1";
        zone = "";
        isMatchBoard = false;
    }
    public JsonCCTV(JsonCCTV jc)
    {
        id = jc.id;
        ip = jc.ip;
        zone = jc.zone;
        position = jc.position;
        rotation = jc.rotation;
        isMatchBoard = jc.isMatchBoard;
    }

    public JsonCCTV CopyCCTV()
    {
        JsonCCTV tempCCTV = new JsonCCTV();
        tempCCTV.id = id;
        tempCCTV.ip = ip;
        tempCCTV.zone = zone;
        tempCCTV.position = position;
        tempCCTV.rotation = rotation;
        tempCCTV.isMatchBoard = isMatchBoard;

        return tempCCTV;
    }
}

[Serializable]
public class JsonUWB
{
    public int id;
    public string ip;
    public Vector3 position;

    public JsonUWB()
    {
        position = new Vector3();
        ip = "127.0.0.1";
    }
    public JsonUWB(JsonUWB jb)
    {
        id = jb.id;
        ip = jb.ip;
        position = jb.position;
    }

    public JsonUWB CopyUWB()
    {
        JsonUWB tempUWB = new JsonUWB();
        tempUWB.id = id;
        tempUWB.ip = ip;
        tempUWB.position = position;

        return tempUWB;
    }
}

[Serializable]
public class JsonFace
{
    public List<int> face;
    public int id;
    public string color;
    public double area;

    public JsonFace(List<int> face, double area, int id)
    {
        color = "#FFFFFF";
        this.face = new List<int>();
        this.face = face;
        this.area = area;
        this.id = id;
    }
    public JsonFace(JsonFace jf)
    {
        face = jf.face.ConvertAll(o => o);
        color = jf.color;
        area = jf.area;
    }

    public JsonFace()
    {
        face = new List<int>();
    }

    public JsonFace CopyFace()
    {
        JsonFace tempFace = new JsonFace();
        tempFace.id = id;
        tempFace.color = color;
        tempFace.area = area;

        List<int> tempFaceInt = new List<int>();

        for (int i = 0; i < face.Count; i++)
        {
            int tempInt = face[i];
            tempFaceInt.Add(tempInt);
        }

        tempFace.face = tempFaceInt;

        return tempFace;
    }
}


[Serializable]
public class JsonZone
{
    public int id;
    public string name;
    public string color;
    public List<Vector3> positionList;
    public List<int> zonePointID;

    public JsonZone()
    {
        positionList = new List<Vector3>();
        zonePointID = new List<int>();
    }
    public JsonZone(JsonZone jz)
    {
        this.id = jz.id;
        this.name = jz.name;
        this.positionList = jz.positionList;
    }

    public JsonZone CopyZone()
    {
        JsonZone tempZone = new JsonZone();
        tempZone.id = id;
        tempZone.name = name;
        tempZone.color = color;

        List<Vector3> tempPositionList = new List<Vector3>();

        for (int i = 0; i < positionList.Count; i++)
        {
            Vector3 tempPosition = positionList[i];
            tempPositionList.Add(tempPosition);
        }

        tempZone.positionList = tempPositionList;

        List<int> tempZonePointIDList = new List<int>();

        for (int i = 0; i < zonePointID.Count; i++)
        {
            int tempPosition = zonePointID[i];
            tempZonePointIDList.Add(tempPosition);
        }

        tempZone.zonePointID = tempZonePointIDList;

        return tempZone;
    }
}

[Serializable]
public class JsonMarker
{
    public int id;
    public string name;
    public Vector3 originPos;
    public Vector3 yAxis;
    public Vector3 zAxis;

    public Vector3 rotation;
    public Vector3 position;

    public JsonMarker()
    {
        id = 0;
        name = "";
        originPos = Vector3.zero;
        yAxis = Vector3.zero;
        zAxis = Vector3.zero;
    }

    public JsonMarker CopyMarker()
    {
        JsonMarker tempMarker = new JsonMarker();
        tempMarker.id = id;
        tempMarker.name = name;
        tempMarker.originPos = originPos;
        tempMarker.yAxis = yAxis;
        tempMarker.zAxis = zAxis;

        tempMarker.rotation = rotation;
        tempMarker.position = position;


        return tempMarker;
    }
}


[Serializable]
public class JsonBoard
{
    public int id;

    public Vector3 position;

    public float rotation;

    public string matchCam;

    public Vector3 pos1;
    public Vector3 pos2;

    //public Vector3 directionHeight;
    //public Vector3 directionWidth;

    public JsonBoard()
    {
        id = 0;
    }

    public JsonBoard CopyBoard()
    {
        JsonBoard tempBoard = new JsonBoard();
        tempBoard.id = id;
        tempBoard.position = position;
        tempBoard.matchCam = matchCam;
        tempBoard.pos1 = pos1;
        tempBoard.pos2 = pos2;
        //tempBoard.directionHeight = directionHeight;
        //tempBoard.directionWidth = directionWidth;

        return tempBoard;
    }
}

[Serializable]
public class JsonFireExit
{
    public int id;
    public string ip;
    public string zone;
    public Vector3 position;
    public Quaternion rotation;

    public bool isMatchBoard;

    public JsonFireExit()
    {
        position = new Vector3();
        ip = "127.0.0.1";
        zone = "";
        isMatchBoard = false;
    }
    public JsonFireExit(JsonFireExit jfe)
    {
        id = jfe.id;
        ip = jfe.ip;
        zone = jfe.zone;
        position = jfe.position;
        rotation = jfe.rotation;
        isMatchBoard = jfe.isMatchBoard;
    }

    public JsonFireExit CopyFireExit()
    {
        JsonFireExit tempFireExit = new JsonFireExit();
        tempFireExit.id = id;
        tempFireExit.ip = ip;
        tempFireExit.zone = zone;
        tempFireExit.position = position;
        tempFireExit.rotation = rotation;
        tempFireExit.isMatchBoard = isMatchBoard;

        return tempFireExit;
    }
}