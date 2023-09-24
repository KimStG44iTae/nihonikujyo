using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagListInfo : MonoBehaviour
{
    public string tagid;

    public void SelectTag()
    {
        OperationSceneManager.instance.searchTagID.SetTagID(tagid);
    }
}
