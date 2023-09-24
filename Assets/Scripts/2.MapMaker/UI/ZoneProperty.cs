using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class ZoneProperty : MonoBehaviour
{
    //현재 선택된 zone
    public GameObject nowZone;

    //현재 선택된 zone json 데이터
    public JsonZone jsonZone;

    public int zoneId;

    //zone 이름 입력
    public TMP_InputField nameInputField;

    //zone 색 표시 이미지
    public Image nowColorImage;

    //현재 선택된 zone point
    public GameObject nowPoint;
    public int nowPointID;

    //현재 zone의 point 오브젝트 리스트
    List<GameObject> jsonObjList = new List<GameObject>();

    Vector3 firstVector;

    //zone 설정
    public void SetZone(GameObject zone)
    {
        nowZone = zone;

        int id = zone.GetComponent<ZoneInfo>().id;
        
        zoneId = id;

        jsonZone = ShareData.instance.jsonData.zones.Find(x => x.id == id);

        nameInputField.text = jsonZone.name;

        Color tempColor = new Color();
        ColorUtility.TryParseHtmlString("#" + jsonZone.color, out tempColor);

        nowColorImage.color = tempColor;
    }

    //컬러 이미지 및 데이터 변경
    public void SetColor(Button button)
    {
        Color selectColor = button.colors.normalColor;
        string colorName = ColorUtility.ToHtmlStringRGB(selectColor);

        jsonZone.color = colorName;

        nowColorImage.color = selectColor;
        nowZone.GetComponent<MeshRenderer>().material.color =selectColor;
        MapSceneManager.instance.unDoManager.AddUndo();
    }

    //이름 변경
    public void EditName()
    {
        jsonZone.name = nameInputField.text;
        nowZone.GetComponent<ZoneInfo>().zoneName = nameInputField.text;
        MapSceneManager.instance.unDoManager.AddUndo();
    }

    //zone 삭제
    public void DeleteZone()
    {

        for (int i = ShareData.instance.jsonData.zones.Count - 1; i >= 0; i--)
        {
            if (ShareData.instance.jsonData.zones[i] == jsonZone)
            {
                ShareData.instance.jsonData.zones.RemoveAt(i);
                break;
            }
        }

        Destroy(nowZone.transform.parent.gameObject);

        MapSceneManager.instance.uiManager.NowStepUI();
        MapSceneManager.instance.uiManager.OffSelect_All();
        MapSceneManager.instance.unDoManager.AddUndo();

        gameObject.SetActive(false);
    }

    //선택된 zone point 위치 수정
    public void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            UpdateEnd();
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Vector3 temp = new Vector3(hit.point.x, 0.03f, hit.point.z);

            if (Input.GetMouseButtonDown(0))
            {
                if (hit.transform.tag == "ZonePoint")
                {
                    MapSceneManager.instance.uiManager.OffSelect_All();

                    firstVector = new Vector3(hit.point.x, 0.03f, hit.point.z);

                    if (hit.transform.parent.GetChild(hit.transform.parent.childCount-1).gameObject == nowZone)
                    {
                        nowPoint = hit.transform.gameObject;

                        int childCount = nowPoint.transform.parent.childCount;

                        for (int i = 0; i < childCount - 1; i++)
                        {
                            jsonObjList.Add(nowPoint.transform.parent.GetChild(i).gameObject);
                        }
                    }
                }
            }
            if (Input.GetMouseButton(0))
            {
                if (nowPoint != null)
                {
                    if (Vector3.Distance(firstVector, temp) > 0.01f)
                    {
                        nowPoint.transform.position = temp;
                        nowPointID = nowPoint.transform.GetComponent<ZonePointInfo>().id;

                        for (int i = 0; i < jsonZone.zonePointID.Count; i++)
                        {
                            if (jsonZone.zonePointID[i] == nowPointID)
                            {
                                jsonZone.positionList[i] = nowPoint.transform.position;

                                if (i == 0)
                                {
                                    jsonZone.positionList[jsonZone.zonePointID.Count - 1] = nowPoint.transform.position;
                                }
                                break;
                            }
                        }

                        int childCount = nowPoint.transform.parent.childCount;

                        Destroy(nowPoint.transform.parent.GetChild(childCount - 1).gameObject);

                        MapSceneManager.instance.zoneManager.UpdateZoneLine(jsonObjList);
                        nowZone = MapSceneManager.instance.zoneManager.UpdateZoneFloor(nowPoint.transform.parent.gameObject, jsonZone, jsonZone.positionList);
                    }
                }
            }
            if (Input.GetMouseButtonUp(0))
            {
                if (nowPoint != null)
                {
                    MapSceneManager.instance.unDoManager.AddUndo();
                    nowPoint = null;
                    jsonObjList.Clear();

                }
            }
        }
    }

    //속성 수정 완료
    public void UpdateEnd()
    {
        MapSceneManager.instance.uiManager.NowStepUI();
        gameObject.SetActive(false);
    }
}
