using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScaleConversion : MonoBehaviour
{
    public bool isStart;

    //현재 선택된 선
    public GameObject selectLine;

    //크기변환할 속성창
    public GameObject linePropertyUI;

    //크기 입력창
    public TMP_InputField inputField;

    //변경할 처음 크기
    public float firstLength;

    //start point인지 end point인지 선택
    public Toggle point1Toggle;
    public Toggle point2Toggle;

    //기준점인지
    public Toggle isStarndardToggle;

    //도면 오브젝트
    public GameObject formImage;

    //현재 선택된 선의 JSON 데이터
    public JsonLine nowLine;

    public Button button;

    public Sprite buttonOn;
    public Sprite buttonOff;

    void Update()
    {
        //선 클릭 상태 시작
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

    //선택된 초록색을 검은색으로 변경하면서 선택 해제
    public void OriginColor()
    {
        for (int i = 0; i < MapSceneManager.instance.pointManager.pointList.Count; i++)
        {
            GameObject tempObject = MapSceneManager.instance.pointManager.pointList[i];
            tempObject.GetComponent<MeshRenderer>().material.color = Color.black;
        }

    }

    //기준점 변경
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

    //크기 비율 계산(버튼 클릭)
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

    //현재 상태 변경
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
