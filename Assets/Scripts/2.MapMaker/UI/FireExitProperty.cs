using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FireExitProperty : MonoBehaviour
{
    //현재 선택된 CCTV 오브젝트
    public GameObject nowFireExit;

    //현재 선택된 CCTV json 데이터
    public JsonFireExit jsonFireExit;

    public int fireexitId;

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
        nowFireExit.transform.GetChild(0).gameObject.SetActive(false);
    }

    //cctv 위치 설정
    public void SetFireExitPosition(GameObject fireexit)
    {
        if (nowFireExit)
        {
            nowFireExit.transform.GetChild(0).gameObject.SetActive(false);
        }
        int id = fireexit.GetComponent<FireExitInfo>().id;

        fireexitId = id;

        nowFireExit = fireexit;
        nowFireExit.transform.GetChild(0).gameObject.SetActive(true);

        jsonFireExit = ShareData.instance.jsonData.fireexits.Find(x => x.id == id);

        idInputField.text = jsonFireExit.id.ToString();
        ipInputField.text = jsonFireExit.ip.ToString();
        zoneInputField.text = jsonFireExit.zone.ToString();

        xInputField.text = jsonFireExit.position.x.ToString("F2");
        yInputField.text = jsonFireExit.position.z.ToString("F2");
        zInputField.text = jsonFireExit.position.y.ToString("F2");

        Vector3 rot = jsonFireExit.rotation.eulerAngles;

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
        jsonFireExit.id = int.Parse(idInputField.text);
        MapSceneManager.instance.unDoManager.AddUndo();
    }

    //ip값 변경
    public void EditIP()
    {
        jsonFireExit.ip = ipInputField.text;
        MapSceneManager.instance.unDoManager.AddUndo();
    }

    //zone 변경
    public void EditZone()
    {
        jsonFireExit.zone = zoneInputField.text;
    }

    //x값 설정
    public void EditXPos()
    {
        jsonFireExit.position.x = float.Parse(xInputField.text);
        UpdatePosition();
        MapSceneManager.instance.unDoManager.AddUndo();
    }

    //y값 설정
    public void EditYPos()
    {
        jsonFireExit.position.z = float.Parse(yInputField.text);
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
        jsonFireExit.position.y = float.Parse(zInputField.text);
        UpdatePosition();
        MapSceneManager.instance.unDoManager.AddUndo();
    }

    //cctv 위치 이동
    public void UpdatePosition()
    {
        nowFireExit.transform.position = new Vector3(jsonFireExit.position.x, 0.03f, jsonFireExit.position.z);
    }

    //각도 수정 슬라이더
    public void UpdateRotation()
    {
        float value = rotSlider.value;

        value = Mathf.Round(value);

        nowFireExit.transform.rotation = Quaternion.Euler(new Vector3(0, value, 0));
        jsonFireExit.rotation = nowFireExit.transform.rotation;

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

        nowFireExit.transform.rotation = Quaternion.Euler(new Vector3(0, value, 0));
        jsonFireExit.rotation = nowFireExit.transform.rotation;

        rotSlider.value = value;
    }

    //cctv 수정 종료
    public void UpdateEnd()
    {
        MapSceneManager.instance.uiManager.NowStepUI();
        gameObject.SetActive(false);
    }

    //cctv 오브젝트 삭제
    public void DeleteFireExit()
    {
        for (int i = ShareData.instance.jsonData.fireexits.Count - 1; i >= 0; i--)
        {
            if (ShareData.instance.jsonData.fireexits[i] == jsonFireExit)
            {
                ShareData.instance.jsonData.fireexits.RemoveAt(i);
                break;
            }
        }

        Destroy(nowFireExit);

        MapSceneManager.instance.fireexitManager.UpdateFireExit();

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
