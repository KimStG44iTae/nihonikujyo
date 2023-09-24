using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WatchListInfo : MonoBehaviour
{
    public string watchid;

    public void SelectWatch()
    {
        OperationSceneManager.instance.searchWatchID.SetWatchID(watchid);
    }
}
