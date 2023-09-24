using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UWBProperty : MonoBehaviour
{
    //현재 선택된 uwb 오브젝트
    public GameObject nowUWB;

    //현재 선택된 uwb json 데이터
    public JsonUWB jsonUWB;

    public int uwbId;

    //id 입력
    public TMP_InputField idInputField;
    //ip 입력
    public TMP_InputField ipInputField;

    //x값 입력
    public TMP_InputField xInputField;
    //y값 입력
    public TMP_InputField yInputField;
    //z값 입력
    public TMP_InputField zInputField;

    //uwb 위치 설정
    public void SetUWBPosition(GameObject uwb)
    {
        int id = uwb.GetComponent<UWBInfo>().id;

        uwbId = id;

        nowUWB = uwb;

        jsonUWB = ShareData.instance.jsonData.uwbs.Find(x => x.id == id);

        idInputField.text = jsonUWB.id.ToString();
        ipInputField.text = jsonUWB.ip.ToString();
        xInputField.text = jsonUWB.position.x.ToString("F2");
        yInputField.text = jsonUWB.position.z.ToString("F2");
        zInputField.text = jsonUWB.position.y.ToString("F2");

    }

    //id 설정
    public void EditID()
    {
        jsonUWB.id = int.Parse(idInputField.text);
        MapSceneManager.instance.unDoManager.AddUndo();
    }

    //ip 설정
    public void EditIP()
    {
        jsonUWB.ip = ipInputField.text;
        MapSceneManager.instance.unDoManager.AddUndo();
    }

    //x값 설정
    public void EditXPos()
    {
        jsonUWB.position.x = float.Parse(xInputField.text);
        UpdatePosition();
        MapSceneManager.instance.unDoManager.AddUndo();
    }

    //y값 설정
    public void EditYPos()
    {
        jsonUWB.position.z = float.Parse(yInputField.text);
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
        jsonUWB.position.y = float.Parse(zInputField.text);
        UpdatePosition();
        MapSceneManager.instance.unDoManager.AddUndo();
    }

    //uwb 위치 이동
    public void UpdatePosition()
    {
        nowUWB.transform.position = new Vector3(jsonUWB.position.x, 0.03f, jsonUWB.position.z);
    }

    //수정 종료
    public void UpdateEnd()
    {
        MapSceneManager.instance.uiManager.NowStepUI();
        gameObject.SetActive(false);
    }

    //UWB 오브젝트 삭제
    public void DeleteUWB()
    {

        for (int i = ShareData.instance.jsonData.uwbs.Count - 1; i >= 0; i--)
        {
            if (ShareData.instance.jsonData.uwbs[i] == jsonUWB)
            {
                ShareData.instance.jsonData.uwbs.RemoveAt(i);
                break;
            }
        }

        Destroy(nowUWB);

        MapSceneManager.instance.uwbManager.UpdateUWB();

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
