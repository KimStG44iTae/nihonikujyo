using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PointProperty : MonoBehaviour
{
    //���� ���õ� �� ������Ʈ
    public GameObject selectPoint;

    //���� ���õ� �� json ������
    public JsonPoint selectJPoint;

    public int pointId;

    //x�� �Է�
    public TMP_InputField xPos;
    //y�� �Է�
    public TMP_InputField yPos;

    public bool isStart;
    public bool isEdit;

    float tempXPos;
    float tempYPos;

    //x,y inputfield �� ����
    public void Update()
    {
        if (isStart)
        {
            if (xPos.isFocused || yPos.isFocused)
            {
                isEdit = true;
            }
            else if (!isEdit)
            {
                xPos.text = selectJPoint.position.x.ToString("F2");
                yPos.text = selectJPoint.position.z.ToString("F2");
            }

        }
    }

    //��ġ ����
    public void ChangePosition()
    {
        if (isStart)
        {
            try
            {
                tempXPos = float.Parse(xPos.text);
                tempYPos = float.Parse(yPos.text);
            }
            catch (System.Exception e)
            {
                Debug.Log(e.Message);
            }
        }
    }

    //��ġ ���� inputfield
    public void EditPosition()
    {
        float x = tempXPos;
        float y = tempYPos;

        selectJPoint.position = new Vector3(x, selectJPoint.position.y, y);

        MapSceneManager.instance.pointManager.PointUpdate();
        MapSceneManager.instance.lineManager.LineUpdate();

        MapSceneManager.instance.unDoManager.AddUndo();

        isEdit = false;
    }

    //�� �ʱ�ȭ
    public void SetPoint(GameObject point)
    {
        int selectId = point.GetComponent<PointInfo>().id;
        pointId = selectId;
        selectJPoint = ShareData.instance.jsonData.points.Find(x => x.id == selectId);
        xPos.text = selectJPoint.position.x.ToString("F2");
        yPos.text = selectJPoint.position.z.ToString("F2");
        isStart = true;
    }

    //�� ����
    public void DeletePoint()
    {
        MapSceneManager.instance.uiManager.OffSelect_All();

        MapSceneManager.instance.pointManager.DeletePoint(selectPoint, selectJPoint);
        MapSceneManager.instance.lineManager.DeleteLine(selectJPoint);

        isStart = false;
        CheckPoint_Delete();
        MapSceneManager.instance.unDoManager.AddUndo();

        gameObject.SetActive(false);
    }

    //�� ������Ʈ ���� �� �� ���� (���� or ����)
    public void CheckPoint_Delete()
    {
        for (int i = ShareData.instance.jsonData.points.Count - 1; i >= 0; i--) 
        {
            bool isInclude = false;
            JsonPoint nowJPoint = ShareData.instance.jsonData.points[i];

            for (int j = 0; j < ShareData.instance.jsonData.lines.Count; j++)
            {
                JsonLine nowJLine = ShareData.instance.jsonData.lines[j];

                if ((nowJLine.startPointId == nowJPoint.id) ||(nowJLine.endPointId == nowJPoint.id))
                {
                    isInclude = true;
                    break;
                }
            }

            if (!isInclude)
            {
                for (int j = 0; j < MapSceneManager.instance.pointManager.pointList.Count; j++)
                {
                    GameObject nowPoint = MapSceneManager.instance.pointManager.pointList[j];

                    if (nowPoint.GetComponent<PointInfo>().id == nowJPoint.id)
                    {
                        MapSceneManager.instance.pointManager.DeletePoint(nowPoint, nowJPoint);
                    }
                }
            }
        }
    }

    //�� ���� �Ϸ�
    public void UpdatePoint()
    {
        float x = float.Parse(xPos.text);
        float y = float.Parse(yPos.text);

        selectJPoint.position = new Vector3(x, selectJPoint.position.y, y);

        MapSceneManager.instance.pointManager.PointUpdate();
        MapSceneManager.instance.lineManager.LineUpdate();

        gameObject.SetActive(false);
        MapSceneManager.instance.uiManager.NowStepUI();

        isStart = false;
    }
}
