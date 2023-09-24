using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrashUser : MonoBehaviour
{
    //����� �浹 ����
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Crash")
        {
            MonitoringSceneManager.instance.userCrashManager.AddCrashInfo(gameObject.transform.parent.gameObject, other.transform.parent.gameObject);
        }
    }

    //����� �浹 ����(����ǥ)
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Crash")
        {
            other.transform.parent.GetChild(0).gameObject.SetActive(true);
            gameObject.transform.parent.GetChild(0).gameObject.SetActive(true);
        }
    }

    //����� �浹 ����
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
