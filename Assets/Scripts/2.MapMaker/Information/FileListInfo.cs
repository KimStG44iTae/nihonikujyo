using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileListInfo : MonoBehaviour
{
    public string filename;

    public void SendFileName()
    {
        MapSceneManager.instance.fileInOutput.StartReceive(filename);
    }
}
