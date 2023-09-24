using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CCTVProperty : MonoBehaviour
{
    //���� ���õ� CCTV ������Ʈ
    public GameObject nowCCTV;

    //���� ���õ� CCTV json ������
    public JsonCCTV jsonCCTV;

    public int cctvId;

    //id �Է�
    public TMP_InputField idInputField;
    
    //ip �Է�
    public TMP_InputField ipInputField;

    //zone �Է�
    public TMP_InputField zoneInputField;

    //x�� �Է�
    public TMP_InputField xInputField;
   
    //y�� �Է�
    public TMP_InputField yInputField;
    
    //z�� �Է�
    public TMP_InputField zInputField;

    //������ �Է�
    public TMP_InputField rotInputField;

    //���� �����̴�
    public Slider rotSlider;

    private void OnDisable()
    {
        nowCCTV.transform.GetChild(0).gameObject.SetActive(false);
    }

    //cctv ��ġ ����
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

    //id�� ����
    public void EditID()
    {
        jsonCCTV.id = int.Parse(idInputField.text);
        MapSceneManager.instance.unDoManager.AddUndo();
    }

    //ip�� ����
    public void EditIP()
    {
        jsonCCTV.ip = ipInputField.text;
        MapSceneManager.instance.unDoManager.AddUndo();
    }

    //zone ����
    public void EditZone()
    {
        jsonCCTV.zone = zoneInputField.text;
    }

    //x�� ����
    public void EditXPos()
    {
        jsonCCTV.position.x = float.Parse(xInputField.text);
        UpdatePosition();
        MapSceneManager.instance.unDoManager.AddUndo();
    }

    //y�� ����
    public void EditYPos()
    {
        jsonCCTV.position.z = float.Parse(yInputField.text);
        UpdatePosition();
        MapSceneManager.instance.unDoManager.AddUndo();
    }

    //z�� ����
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

    //cctv ��ġ �̵�
    public void UpdatePosition()
    {
        nowCCTV.transform.position = new Vector3(jsonCCTV.position.x, 0.03f, jsonCCTV.position.z);
    }

    //���� ���� �����̴�
    public void UpdateRotation()
    {
        float value = rotSlider.value;

        value = Mathf.Round(value);

        nowCCTV.transform.rotation = Quaternion.Euler(new Vector3(0, value, 0));
        jsonCCTV.rotation = nowCCTV.transform.rotation;

        rotInputField.text = value.ToString();
    }

    //���� ���� inputField
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

    //cctv ���� ����
    public void UpdateEnd()
    {
        MapSceneManager.instance.uiManager.NowStepUI();
        gameObject.SetActive(false);
    }

    //cctv ������Ʈ ����
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
