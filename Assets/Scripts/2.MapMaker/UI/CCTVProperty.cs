using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CCTVProperty : MonoBehaviour
{
    //현재 선택된 CCTV 오브젝트
    public GameObject nowCCTV;

    //현재 선택된 CCTV json 데이터
    public JsonCCTV jsonCCTV;

    public int cctvId;

    //id 입력
    public TMP_InputField idInputField;
    
    //ip 입력
    public TMP_InputField ipInputField;

    //zone 입력
    public TMP_InputField zoneInputField;

    //x값 입력
    public TMP_InputField xInputField;
   
    //y값 입력
    public TMP_InputField yInputField;
    
    //z값 입력
    public TMP_InputField zInputField;

    //각도값 입력
    public TMP_InputField rotInputField;

    //각도 슬라이더
    public Slider rotSlider;

    private void OnDisable()
    {
        nowCCTV.transform.GetChild(0).gameObject.SetActive(false);
    }

    //cctv 위치 설정
    public void SetCCTVPosition(GameObject cctv)
    {
        if (nowCCTV)
        {
            nowCCTV.transform.GetChild(0).gameObject.SetActive(false);
        }
        int id = cctv.GetComponent<CCTVInfo>().id;

        cctvId = id;

        nowCCTV = cctv;
        nowCCTV.transform.GetChild(0).gameObject.SetActive(true);

        jsonCCTV = ShareData.instance.jsonData.cctvs.Find(x => x.id == id);

        idInputField.text = jsonCCTV.id.ToString();
        ipInputField.text = jsonCCTV.ip.ToString();
        zoneInputField.text = jsonCCTV.zone.ToString();

        xInputField.text = jsonCCTV.position.x.ToString("F2");
        yInputField.text = jsonCCTV.position.z.ToString("F2");
        zInputField.text = jsonCCTV.position.y.ToString("F2");

        Vector3 rot = jsonCCTV.rotation.eulerAngles;

        if (rot.y > 180)
        {
            rotInputField.text = (-360 + rot.y).ToString();
            rotSlider.value = (-360 + rot.y);
        }
        else
        {
            rotInputField.text = rot.y.ToString();
            rotSlider.value = rot.y;
        }

    }

    //id값 변경
    public void EditID()
    {
        jsonCCTV.id = int.Parse(idInputField.text);
        MapSceneManager.instance.unDoManager.AddUndo();
    }

    //ip값 변경
    public void EditIP()
    {
        jsonCCTV.ip = ipInputField.text;
        MapSceneManager.instance.unDoManager.AddUndo();
    }

    //zone 변경
    public void EditZone()
    {
        jsonCCTV.zone = zoneInputField.text;
    }

    //x값 설정
    public void EditXPos()
    {
        jsonCCTV.position.x = float.Parse(xInputField.text);
        UpdatePosition();
        MapSceneManager.instance.unDoManager.AddUndo();
    }

    //y값 설정
    public void EditYPos()
    {
        jsonCCTV.position.z = float.Parse(yInputField.text);
        UpdatePosition();
        MapSceneManager.instance.unDoManager.AddUndo();
    }

    //z값 설정
    public void EditZPos()
    {
        if (float.Parse(zInputField.text) < 0.03f)
        {
            zInputField.text = (0.03).ToString();
        }
        jsonCCTV.position.y = float.Parse(zInputField.text);
        UpdatePosition();
        MapSceneManager.instance.unDoManager.AddUndo();
    }

    //cctv 위치 이동
    public void UpdatePosition()
    {
        nowCCTV.transform.position = new Vector3(jsonCCTV.position.x, 0.03f, jsonCCTV.position.z);
    }

    //각도 수정 슬라이더
    public void UpdateRotation()
    {
        float value = rotSlider.value;

        value = Mathf.Round(value);

        nowCCTV.transform.rotation = Quaternion.Euler(new Vector3(0, value, 0));
        jsonCCTV.rotation = nowCCTV.transform.rotation;

        rotInputField.text = value.ToString();
    }

    //각도 수정 inputField
    public void UpdateRotationField()
    {
        float value = float.Parse(rotInputField.text);

        if (value > 180)
        {
            rotInputField.text = 180.ToString();
            value = 180;
        }

        if (value < -180)
        {
            rotInputField.text = (-180).ToString();
            value = -180;
        }

        value = Mathf.Round(value);

        nowCCTV.transform.rotation = Quaternion.Euler(new Vector3(0, value, 0));
        jsonCCTV.rotation = nowCCTV.transform.rotation;

        rotSlider.value= value;
    }

    //cctv 수정 종료
    public void UpdateEnd()
    {
        MapSceneManager.instance.uiManager.NowStepUI();
        gameObject.SetActive(false);
    }

    //cctv 오브젝트 삭제
    public void DeleteCCTV()
    {
        for (int i = ShareData.instance.jsonData.cctvs.Count - 1; i >= 0; i--) 
        {
            if (ShareData.instance.jsonData.cctvs[i] == jsonCCTV)
            {
                ShareData.instance.jsonData.cctvs.RemoveAt(i);
                break;
            }
        }

        Destroy(nowCCTV);

        MapSceneManager.instance.cctvManager.UpdateCCTV();

        MapSceneManager.instance.uiManager.NowStepUI();
        MapSceneManager.instance.uiManager.OffSelect_All();
        MapSceneManager.instance.unDoManager.AddUndo();

        gameObject.SetActive(false);
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            gameObject.SetActive(false);
        }
    }
}
