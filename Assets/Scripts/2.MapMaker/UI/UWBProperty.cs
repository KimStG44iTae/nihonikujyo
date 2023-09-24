using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UWBProperty : MonoBehaviour
{
    //���� ���õ� uwb ������Ʈ
    public GameObject nowUWB;

    //���� ���õ� uwb json ������
    public JsonUWB jsonUWB;

    public int uwbId;

    //id �Է�
    public TMP_InputField idInputField;
    //ip �Է�
    public TMP_InputField ipInputField;

    //x�� �Է�
    public TMP_InputField xInputField;
    //y�� �Է�
    public TMP_InputField yInputField;
    //z�� �Է�
    public TMP_InputField zInputField;

    //uwb ��ġ ����
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

    //id ����
    public void EditID()
    {
        jsonUWB.id = int.Parse(idInputField.text);
        MapSceneManager.instance.unDoManager.AddUndo();
    }

    //ip ����
    public void EditIP()
    {
        jsonUWB.ip = ipInputField.text;
        MapSceneManager.instance.unDoManager.AddUndo();
    }

    //x�� ����
    public void EditXPos()
    {
        jsonUWB.position.x = float.Parse(xInputField.text);
        UpdatePosition();
        MapSceneManager.instance.unDoManager.AddUndo();
    }

    //y�� ����
    public void EditYPos()
    {
        jsonUWB.position.z = float.Parse(yInputField.text);
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
        jsonUWB.position.y = float.Parse(zInputField.text);
        UpdatePosition();
        MapSceneManager.instance.unDoManager.AddUndo();
    }

    //uwb ��ġ �̵�
    public void UpdatePosition()
    {
        nowUWB.transform.position = new Vector3(jsonUWB.position.x, 0.03f, jsonUWB.position.z);
    }

    //���� ����
    public void UpdateEnd()
    {
        MapSceneManager.instance.uiManager.NowStepUI();
        gameObject.SetActive(false);
    }

    //UWB ������Ʈ ����
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
