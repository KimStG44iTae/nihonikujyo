using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrashUser : MonoBehaviour
{
    //사용자 충돌 시작
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Crash")
        {
            MonitoringSceneManager.instance.userCrashManager.AddCrashInfo(gameObject.transform.parent.gameObject, other.transform.parent.gameObject);
        }
    }

    //사용자 충돌 유지(느낌표)
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Crash")
        {
            other.transform.parent.GetChild(0).gameObject.SetActive(true);
            gameObject.transform.parent.GetChild(0).gameObject.SetActive(true);
        }
    }

    //사용자 충돌 종료
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Crash")
        {
            other.transform.parent.GetChild(0).gameObject.SetActive(false);
            gameObject.transform.parent.GetChild(0).gameObject.SetActive(false);

            MonitoringSceneManager.instance.userCrashManager.DeleteCrashInfo(gameObject.transform.parent.gameObject, other.transform.parent.gameObject);
        }
    }
}
