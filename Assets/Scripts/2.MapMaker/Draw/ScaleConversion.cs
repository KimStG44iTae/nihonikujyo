using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScaleConversion : MonoBehaviour
{
    public bool isStart;

    //���� ���õ� ��
    public GameObject selectLine;

    //ũ�⺯ȯ�� �Ӽ�â
    public GameObject linePropertyUI;

    //ũ�� �Է�â
    public TMP_InputField inputField;

    //������ ó�� ũ��
    public float firstLength;

    //start point���� end point���� ����
    public Toggle point1Toggle;
    public Toggle point2Toggle;

    //����������
    public Toggle isStarndardToggle;

    //���� ������Ʈ
    public GameObject formImage;

    //���� ���õ� ���� JSON ������
    public JsonLine nowLine;

    public Button button;

    public Sprite buttonOn;
    public Sprite buttonOff;

    void Update()
    {
        //�� Ŭ�� ���� ����
        if (isStart)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            if (Input.GetMouseButtonDown(1))
            {
                EndStatus();

                MapSceneManager.instance.uiManager.NowStepUI();
            }

            if (Physics.Raycast(ray, out hit))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (hit.transform.tag == "Line")
                    {
                        if (selectLine != null)
                        {
                            selectLine.transform.GetComponent<LineRenderer>().material.color = Color.black;
                        }
                        selectLine = hit.transform.gameObject;

                        selectLine.transform.GetComponent<LineRenderer>().material.color = Color.red;

                        Vector3 firstPos = selectLine.GetComponent<LineRenderer>().GetPosition(0);
                        Vector3 secondPos = selectLine.GetComponent<LineRenderer>().GetPosition(1);

                        linePropertyUI.SetActive(true);

                        inputField.text = (Vector3.Distance(firstPos, secondPos)).ToString("F2");
                        firstLength = (Vector3.Distance(firstPos, secondPos));

                        int selectLineId = selectLine.GetComponent<LineInfo>().id;
                        nowLine = ShareData.instance.jsonData.lines.Find(x => x.id == selectLineId);

                        isStarndardToggle.isOn = nowLine.isStandard;
                        ChangeStandardPoint();
                    }
                }
            }
        }
    }

    //���õ� �ʷϻ��� ���������� �����ϸ鼭 ���� ����
    public void OriginColor()
    {
        for (int i = 0; i < MapSceneManager.instance.pointManager.pointList.Count; i++)
        {
            GameObject tempObject = MapSceneManager.instance.pointManager.pointList[i];
            tempObject.GetComponent<MeshRenderer>().material.color = Color.black;
        }

    }

    //������ ����
    public void ChangeStandardPoint()
    {
        OriginColor();

        for (int i = 0; i < MapSceneManager.instance.pointManager.pointList.Count; i++)
        {
            GameObject tempObject = MapSceneManager.instance.pointManager.pointList[i];

            int tempId = tempObject.GetComponent<PointInfo>().id;

            int standartId = 0;

            if (point1Toggle.isOn)
            {
                standartId = nowLine.startPointId;
            }
            else
            {
                standartId = nowLine.endPointId;
            }
            if (tempId == standartId)
            {
                tempObject.GetComponent<MeshRenderer>().material.color = Color.green;
            }
        }
        MapSceneManager.instance.unDoManager.AddUndo();

    }

    //ũ�� ���� ���(��ư Ŭ��)
    public void Calculation()
    {
        Vector3 minusVector = Vector3.zero;
        Vector3 compareVector = Vector3.zero;

        if (point1Toggle.isOn)
        {
            compareVector = selectLine.GetComponent<LineRenderer>().GetPosition(0);
        }
        else if (point2Toggle.isOn)
        {
            compareVector = selectLine.GetComponent<LineRenderer>().GetPosition(1);
        }

        print(compareVector);

        minusVector = -compareVector + Vector3.up * 0.02f;

        float realLength = float.Parse(inputField.text);

        float ratio = realLength / firstLength;

        for (int i = 0; i < ShareData.instance.jsonData.points.Count; i++)
        {
            Vector3 calcPosition = ShareData.instance.jsonData.points[i].position;

            calcPosition += minusVector;

            if (calcPosition != Vector3.zero)
            {
                calcPosition *= ratio;
                calcPosition = new Vector3(calcPosition.x, 0.02f, calcPosition.z);

            }
            ShareData.instance.jsonData.points[i].position = calcPosition;
        }

        for (int i = 0; i < ShareData.instance.jsonData.cctvs.Count; i++)
        {
            Vector3 calcPosition = ShareData.instance.jsonData.cctvs[i].position;

            calcPosition += minusVector;

            calcPosition *= ratio;
            calcPosition = new Vector3(calcPosition.x, 0.03f, calcPosition.z);

            ShareData.instance.jsonData.cctvs[i].position = calcPosition;
        }


        for (int i = 0; i < ShareData.instance.jsonData.uwbs.Count; i++)
        {
            Vector3 calcPosition = ShareData.instance.jsonData.uwbs[i].position;

            calcPosition += minusVector;

            calcPosition *= ratio;
            calcPosition = new Vector3(calcPosition.x, 0.03f, calcPosition.z);

            ShareData.instance.jsonData.uwbs[i].position = calcPosition;
        }

        for (int i = 0; i < ShareData.instance.jsonData.zones.Count; i++)
        {
            JsonZone tempZone = ShareData.instance.jsonData.zones[i];

            for (int j = 0; j < tempZone.positionList.Count; j++)
            {
                Vector3 calcPosition = tempZone.positionList[j];

                calcPosition += minusVector;

                calcPosition *= ratio;
                calcPosition = new Vector3(calcPosition.x, 0.02f, calcPosition.z);

                ShareData.instance.jsonData.zones[i].positionList[j] = calcPosition;
            }
        }

        for (int i = 0; i < ShareData.instance.jsonData.markers.Count; i++)
        {
            Vector3 calcPosition = ShareData.instance.jsonData.markers[i].position;

            calcPosition += minusVector;

            calcPosition *= ratio;
            calcPosition = new Vector3(calcPosition.x, 0.03f, calcPosition.z);

            ShareData.instance.jsonData.markers[i].position = calcPosition;
        }

        for (int i = 0; i < ShareData.instance.jsonData.boards.Count; i++)
        {
            Vector3 calcPosition = ShareData.instance.jsonData.boards[i].position;

            calcPosition += minusVector;

            calcPosition *= ratio;
            calcPosition = new Vector3(calcPosition.x, 0.03f, calcPosition.z);

            ShareData.instance.jsonData.boards[i].position = calcPosition;
        }

        MapSceneManager.instance.cctvManager.UpdateCCTV();
        MapSceneManager.instance.uwbManager.UpdateUWB();
        MapSceneManager.instance.lineManager.LineUpdate();
        MapSceneManager.instance.pointManager.PointUpdate();
        MapSceneManager.instance.zoneManager.UpdateZone();
        MapSceneManager.instance.boardManager.UpdateBoard();
        MapSceneManager.instance.markerManager.UpdateMarker();

        formImage.transform.localScale *= ratio;

        formImage.transform.position += minusVector;
        formImage.transform.position *= ratio;

        formImage.transform.position = new Vector3(formImage.transform.position.x, 0.01f, formImage.transform.position.z);


        ShareData.instance.jsonData.ratio *= ratio;

        nowLine.isStandard = isStarndardToggle.isOn;

        linePropertyUI.SetActive(false);

        MapSceneManager.instance.unDoManager.AddUndo();

        EndStatus();

        MapSceneManager.instance.uiManager.NowStepUI();

    }

    //���� ���� ����
    public void StartStatus()
    {
        // button.image.sprite = buttonOn;

        isStart = true;
        MapSceneManager.instance.lineManager.MakeCollider();
    }

    public void EndStatus()
    {
        //  button.image.sprite = buttonOff;

        ShareData.instance.jsonData.imagePos = formImage.transform.position;
        OriginColor();
        isStart = false;
        linePropertyUI.SetActive(false);
        if (selectLine != null)
        {
            selectLine.transform.GetComponent<LineRenderer>().material.color = Color.black;
        }
    }

}
