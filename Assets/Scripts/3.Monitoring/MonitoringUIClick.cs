using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MonitoringUIClick : MonoBehaviour
{
    private GraphicRaycaster gr;

    public UserSelect userSelect;

    private void Awake()
    {
        gr = GetComponent<GraphicRaycaster>();
    }

    //현재 판넬위에 마우스 커서가 있을 때 선택이 안되게 상태 변경
    private void Update()
    {
        var ped = new PointerEventData(null);
        ped.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        gr.Raycast(ped, results);

        if (results.Count > 0)
        {
            userSelect.isMouseInUi = true;
        }
        else
        {
            userSelect.isMouseInUi = false;
        }
    }
}
