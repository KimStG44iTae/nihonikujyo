using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProjectInfo : MonoBehaviour
{
    public string projectName;

    public bool isSelect;

    public GameObject trashButton;

    public void MapClientSceneLoad()
    {
        if (!isSelect)
        {
            ProjectInfo[] projectInfoList = GameObject.FindObjectsOfType<ProjectInfo>();

            for (int i = 0; i < projectInfoList.Length; i++)
            {
                if (projectInfoList[i].isSelect)
                {
                    projectInfoList[i].isSelect = false;
                    projectInfoList[i].GetComponent<Image>().color = Color.white;
                    projectInfoList[i].trashButton.SetActive(false);

                    break;
                }
            }
            GetComponent<Image>().color = Color.red;
            isSelect = true;
            trashButton.SetActive(true);
        }
        else
        {
            TitleSceneManager.instance.FileLoad_SceneLoad(this.gameObject);
        }

    }

    public void DeleteProject()
    {
        TitleSceneManager.instance.titleFileLoad.StartDelete(projectName);
    }
}
