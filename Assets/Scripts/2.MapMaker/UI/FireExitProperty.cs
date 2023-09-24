using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FireExitProperty : MonoBehaviour
{
    //���� ���õ� CCTV ������Ʈ
    public GameObject nowFireExit;

    //���� ���õ� CCTV json ������
    public JsonFireExit jsonFireExit;

    public int fireexitId;

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
        nowFireExit.transform.GetChild(0).gameObject.SetActive(false);
    }

    //cctv ��ġ ����
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

    //id�� ����
    public void EditID()
    {
        jsonFireExit.id = int.Parse(idInputField.text);
        MapSceneManager.instance.unDoManager.AddUndo();
    }

    //ip�� ����
    public void EditIP()
    {
        jsonFireExit.ip = ipInputField.text;
        MapSceneManager.instance.unDoManager.AddUndo();
    }

    //zone ����
    public void EditZone()
    {
        jsonFireExit.zone = zoneInputField.text;
    }

    //x�� ����
    public void EditXPos()
    {
        jsonFireExit.position.x = float.Parse(xInputField.text);
        UpdatePosition();
        MapSceneManager.instance.unDoManager.AddUndo();
    }

    //y�� ����
    public void EditYPos()
    {
        jsonFireExit.position.z = float.Parse(yInputField.text);
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
        jsonFireExit.position.y = float.Parse(zInputField.text);
        UpdatePosition();
        MapSceneManager.instance.unDoManager.AddUndo();
    }

    //cctv ��ġ �̵�
    public void UpdatePosition()
    {
        nowFireExit.transform.position = new Vector3(jsonFireExit.position.x, 0.03f, jsonFireExit.position.z);
    }

    //���� ���� �����̴�
    public void UpdateRotation()
    {
        float value = rotSlider.value;

        value = Mathf.Round(value);

        nowFireExit.transform.rotation = Quaternion.Euler(new Vector3(0, value, 0));
        jsonFireExit.rotation = nowFireExit.transform.rotation;

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

        nowFireExit.transform.rotation = Quaternion.Euler(new Vector3(0, value, 0));
        jsonFireExit.rotation = nowFireExit.transform.rotation;

        rotSlider.value = value;
    }

    //cctv ���� ����
    public void UpdateEnd()
    {
        MapSceneManager.instance.uiManager.NowStepUI();
        gameObject.SetActive(false);
    }

    //cctv ������Ʈ ����
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
