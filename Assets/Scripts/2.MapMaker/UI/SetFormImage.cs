using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetFormImage : MonoBehaviour
{
    public float width;
    public float height;

    //��ũ��Ʈ ���� ��������
    public bool isStart;

    //���� �̵� ��������
    public bool isSelect;

    //���� �̹��� ù ��ġ
    public Vector3 firstPos;

    //���� �̹��� ȸ�� ��
    public float imageRot;

    public Button button;

    public Sprite buttonOn;
    public Sprite buttonOff;

    public void Update()
    {
        if (isStart)
        {
            if (Input.GetMouseButtonDown(2))
            {
                EndStatus();
            }

            //���� �̹��� ȸ��
            if (Input.GetMouseButtonDown(1))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform.tag == "FormImage")
                    {
                        imageRot += 90;
                        hit.transform.rotation = Quaternion.Euler(new Vector3(90, imageRot, 0));

                        ShareData.instance.jsonData.imageRot = hit.transform.rotation;
                    }

                }
            }

            //���� �̹��� �̵�
            if (Input.GetMouseButton(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform.tag != "FormImage")
                    {
                        return;
                    }
                    if (!isSelect)
                    {
                        firstPos = new Vector3(hit.transform.position.x, hit.point.y, hit.transform.position.z) - hit.point;
                        isSelect = true;
                    }
                    else
                    {
                        Vector3 pos = hit.point + firstPos;
                        pos = new Vector3(pos.x, 0.01f, pos.z);
                        transform.position = pos;
                    }

                }
            }
            else
                isSelect = false;
        }   
    }

    //���� ũ�� ����
    public void SetSize(int w, int h)
    {
        width = (float)w / 100f;
        height = (float)h / 100f;
        transform.localScale = new Vector3(width, height, 0.001f);

        if (GetComponent<BoxCollider>())
        {
            Destroy(GetComponent<BoxCollider>());
        }
        gameObject.AddComponent<BoxCollider>();

        Vector3 jsonImagePos = ShareData.instance.jsonData.imagePos;

        gameObject.transform.position = new Vector3(jsonImagePos.x, 0.01f, jsonImagePos.z);

        gameObject.transform.rotation = ShareData.instance.jsonData.imageRot;
        imageRot = gameObject.transform.rotation.y;
    }

    //�⺻ ���� ����
    public void StartStatus()
    {
        isStart = true;
        gameObject.AddComponent<BoxCollider>();
    }

    public void EndStatus()
    {
        isStart = false;
        ShareData.instance.jsonData.imagePos = gameObject.transform.position;
        if (GetComponent<BoxCollider>())
        {
            Destroy(GetComponent<BoxCollider>());
        }
        MapSceneManager.instance.uiManager.NowStepUI();
    }
}
