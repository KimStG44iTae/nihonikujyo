using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIClick : MonoBehaviour
{
    private GraphicRaycaster gr;

    private void Awake()
    {
        gr = GetComponent<GraphicRaycaster>();
    }

    //���� �ǳ����� ���콺 Ŀ���� ���� �� ������ �ȵǰ� ���� ����
    private void Update()
    {
        var ped = new PointerEventData(null);
        ped.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        gr.Raycast(ped, results);

        if (results.Count > 0)
        {
            MapSceneManager.instance.isMouseInUi = true;
        }
        else
        {
            MapSceneManager.instance.isMouseInUi = false;
        }
    }
}
