using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UWBMovement : MonoBehaviour
{
    public bool isStart;
    public ButtonInteraction buttonInteraction;

    //동선 남기기
    public void StartTrail()
    {
        isStart = !isStart;

        if (isStart)
        {
            buttonInteraction.OnSelect();
        }
        else
        {
            buttonInteraction.OnDeSelect();
        }

        List<GameObject> userList = MonitoringSceneManager.instance.userManager.userList;

        for (int i = 0; i < userList.Count; i++)
        {
            userList[i].transform.GetChild(2).GetComponent<TrailRenderer>().enabled = isStart;
        }
    }
}
