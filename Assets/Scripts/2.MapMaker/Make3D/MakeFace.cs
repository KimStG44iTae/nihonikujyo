using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class MakeFace : MonoBehaviour
{
    private static void Start(JsonData jh)
    {
        Dictionary<int, int> idToIndex;
        Dictionary<int, int> indexToId;
        int[][] matrix;
        int pointCount = jh.points.Count;
        List<int> rankArray = new List<int>();
        for (int i = 0; i < pointCount; i++)
        {
            rankArray.Add(jh.points[i].id);

        }
        rankArray.Sort();

        idToIndex = new Dictionary<int, int>(); // [id,index] id : Point's unique ID
        indexToId = new Dictionary<int, int>(); // [index,id] index : ranked index

        for (var i = 0; i < pointCount; i++)
        {
            idToIndex.Add(rankArray[i], i);
            indexToId.Add(i, rankArray[i]);
        }

        matrix = new int[pointCount][];
        for (var i = 0; i < matrix.Length; i++)
        {
            matrix[i] = new int[pointCount];
        }
        for (int i = 0; i < jh.lines.Count; i++)
        {
            matrix[idToIndex[jh.lines[i].startPointId]][idToIndex[jh.lines[i].endPointId]] = 1;
            matrix[idToIndex[jh.lines[i].endPointId]][idToIndex[jh.lines[i].startPointId]] = 1;
        }
        List<List<int>> faces = new List<List<int>>();
        for (var i = 0; i < pointCount; i++)
        {
            LookPath(jh, idToIndex, indexToId, matrix, i, faces);
        }
        List<List<int>> tempFaces = new List<List<int>>();
        for (var i = 0; i < faces.Count; i++)
        {
            var minIndex = faces[i].IndexOf(faces[i].Min());
            for (int j = 0; j < minIndex; j++)
            {
                faces[i].Add(faces[i][j]);

            }
            faces[i].RemoveRange(0, minIndex);
            bool canAdd = true;
            for (int k = 0; k < tempFaces.Count; k++)
            {
                if (Compare(tempFaces[k], faces[i]))
                {
                    canAdd = false;
                    break;
                }
            }
            if (canAdd)
            {
                tempFaces.Add(faces[i]);
            }

        }
        for (var i = 0; i < tempFaces.Count; i++)
        {
            for (var j = tempFaces.Count-1; j > 0; j--)
            {
                if (i == j)
                {
                    continue;
                }
                var a = CopyArray(tempFaces[i]);
                var b = CopyArray(tempFaces[j]);
                a.Sort();
                b.Sort();
                if (Compare(a, b))
                {
                    tempFaces.RemoveAt(j);

                }
            }
        }
        for (int i = 0; i < tempFaces.Count; i++)
        {
            bool isAnyIn = false;
            var tempBigFaceVectors = new List<Vector3>();
            for (int j = 0; j < tempFaces[i].Count; j++)
            {
                tempBigFaceVectors.Add(jh.points.Find(o => o.id == indexToId[tempFaces[i][j]]).position);
            }
            for (int j = 0; j < tempFaces[i].Count; j++)
            {
                if (i == j)
                {
                    continue;
                }
                if (j >= tempFaces.Count)
                {
                    break;
                }
                var tempSmallFaceVectors = new List<Vector3>();
                for (int k = 0; k < tempFaces[j].Count; k++)
                {
                    tempSmallFaceVectors.Add(jh.points.Find(o => o.id == indexToId[tempFaces[j][k]]).position);

                }
                for (int k = 0; k < tempSmallFaceVectors.Count; k++)
                {
                    isAnyIn = ContainsPoint(tempBigFaceVectors.ToArray(), (tempSmallFaceVectors[k] + tempSmallFaceVectors[(k + 1) % tempSmallFaceVectors.Count]) / 2);
                    if (isAnyIn)
                    {
                        break;
                    }
                }
                if (isAnyIn)
                {
                    
                    bool anySamePoint = false;
                    foreach (var k in tempFaces[j])
                    {
                        foreach (var l in tempFaces[i])
                        {
                            if (k == l)
                            {
                                anySamePoint = true;
                                break;
                            }
                        }
                        if (anySamePoint)
                        {
                            break;
                        }
                    }
                    if (anySamePoint)
                    {
                        tempFaces.Remove(tempFaces[i]);
                        break;
                    }
                }
            }
        }

        faces = tempFaces.ConvertAll(o => new List<int>(o));
        jh.faces.Clear();
        int idCount = 0;
        for (int i = 0; i < faces.Count; i++)
        {
            var a = faces[i].ConvertAll(o => indexToId[o]);
            // var a = faces[i].ConvertAll(o => indexToId[o]);
            var b = PolygonArea(faces[i].ConvertAll(o => jh.points[o].position).ToArray());
            // var b = PolygonArea(faces[i].ConvertAll(o => jh.points[indexToId[o]].position).ToArray());
            jh.faces.Add(new JsonFace(a, b, idCount));
            idCount++;
        }
      //  print(jh.faces.Count);

    }
    public static bool IsCBetweenAB ( Vector3 A , Vector3 B , Vector3 C ) {
        return Mathf.Approximately(
        Vector3.Project( A-B , A-C ).magnitude ,
        (A-B).magnitude
        );
    }
    static double PolygonArea(Vector3[] polygon)
    {
        int i, j;
        double area = 0;

        for (i = 0; i < polygon.Length; i++)
        {
            j = (i + 1) % polygon.Length;

            area += polygon[i].x * polygon[j].y;
            area -= polygon[i].y * polygon[j].x;
        }

        area /= 2;
        return (area < 0 ? -area : area);
    }

    private static bool Compare(List<int> lList, List<int> rList)
    {
        bool isSame = true;
        if (lList.Count == rList.Count)
        {
            for (var i = 0; i < lList.Count; i++)
            {
                if (lList[i] != rList[i])
                {
                    isSame = false;
                    break;
                }
            }
        }
        else
        {
            isSame = false;
        }
        return isSame;
    }
    private static bool ContainsPoint(Vector3[] polyPoints, Vector3 p)
    {
        
        var j = polyPoints.Length - 1;
        var inside = false;
        for (int i = 0; i < polyPoints.Length; j = i++)
        {
            var pi = polyPoints[i];
            var pj = polyPoints[j];

            if(IsCBetweenAB(pi,pj,p))
            {
                inside = false;
                break;
            }
            if ( ((polyPoints[i].y <= p.y && p.y < polyPoints[j].y) || (polyPoints[j].y <= p.y && p.y < polyPoints[i].y)) && 
                (p.x < (polyPoints[j].x - polyPoints[i].x) * (p.y - polyPoints[i].y) / (polyPoints[j].y - polyPoints[i].y) + polyPoints[i].x)) 
                inside = !inside;
        }
        return inside;
    }
    private static double GetRightAngle(List<Vector3> points)
    {
        Vector3 oldPos = points[0] - points[1];
        Vector3 newPos = points[2] - points[1];
        double result = 0;
        float direction = oldPos.x * newPos.y - oldPos.y * newPos.x;
        if (direction > 0)
        {
            result += 180;
        }


        result += Mathf.Acos(Vector3.Dot(oldPos, newPos) / (Vector3.Magnitude(oldPos) * Vector3.Magnitude(newPos))) * (180.0f / Mathf.PI);
        return result;
    }
    private static List<List<int>> LookPath(JsonData jh, Dictionary<int, int> idToIndex, Dictionary<int, int> indexToId, int[][] matrix, int startPoint, List<List<int>> faces)
    {
        for (var i = 0; i < matrix.Count(); i++)
        {
            if (matrix[startPoint][i] == 1)
            {
                var tempMatrix = CopyArray(matrix);
                tempMatrix[startPoint][i] = 0;
                tempMatrix[i][startPoint] = 0;
                LookPathLoop(jh, idToIndex, indexToId, tempMatrix, new List<int>() { startPoint }, i, startPoint, faces);
            }
        }
        return faces;
    }
    private static void LookPathLoop(JsonData jh, Dictionary<int, int> idToIndex, Dictionary<int, int> indexToId, int[][] matrix, List<int> visitedPoint, int nowPoint, int targetPoint, List<List<int>> faces)
    {
        if (nowPoint == targetPoint)
        {
            faces.Add(visitedPoint);
            return;
        }
        // for (int i = 0; i < visitedPoint.Count; i++)
        // {
        //     if (visitedPoint[i] == nowPoint)
        //     {
        //         return;
        //     }
        // }
        visitedPoint.Add(nowPoint);
        List<double> forwards = new List<double>();
        bool canNext = false;
        for (var i = 0; i < matrix.Count(); i++)
        {

            if (matrix[nowPoint][i] == 1)
            {
                canNext = true;
                forwards.Add(GetRightAngle(new List<Vector3>(){
                        jh.points.Find(point => point.id == indexToId[visitedPoint[visitedPoint.Count-2]]).position,
                        jh.points.Find(point => point.id == indexToId[nowPoint]).position,
                        jh.points.Find(point => point.id == indexToId[i]).position
                    }));
            }
            else
            {
                forwards.Add(999);
            }
        }
        if (!canNext)
        {
            return;
        }
        int goIndex = forwards.IndexOf(forwards.Min());
        var tempMatrix = CopyArray(matrix);
        tempMatrix[nowPoint][goIndex] = 0;
        tempMatrix[goIndex][nowPoint] = 0;
        LookPathLoop(jh, idToIndex, indexToId, tempMatrix, visitedPoint, goIndex, targetPoint, faces);
    }
    private static List<int> CopyArray(List<int> originArray)
    {
        int pointCount = originArray.Count;
        List<int> result = new List<int>();

        for (var i = 0; i < pointCount; i++)
        {
            result.Add(originArray[i]);
        }
        return result;
    }
    private static int[][] CopyArray(int[][] originArray)
    {
        int pointCount = originArray.Length;
        int[][] result = new int[pointCount][];
        for (var i = 0; i < result.Length; i++)
        {
            result[i] = new int[pointCount];
        }

        for (var i = 0; i < pointCount; i++)
        {
            for (var j = 0; j < pointCount; j++)
            {
                result[i][j] = originArray[i][j];
            }
        }
        return result;
    }

    public static void GetFaces(JsonData jh)
    {
        Start(jh);
    }
}
