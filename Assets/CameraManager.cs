using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> Cam = new List<GameObject>();
    


    public void ChanegeCam(string CamName)
    {
        if (CamName.Equals("Cube"))
        {
            for (int i = 0; i < Cam.Count; i++)
            {
                Cam[i].gameObject.SetActive(false);
            }
            Cam[0].gameObject.SetActive(true);
            Debug.Log("Cube!");
        }
        else if (CamName.Equals("Sphere"))
        {
            for (int i = 0; i < Cam.Count; i++)
            {
                Cam[i].gameObject.SetActive(false);
            }
            Cam[1].gameObject.SetActive(true);
            Debug.Log("Sphere!");
        }
        
        
        /*switch (CamName)
        {
            case "Cube":
                for (int i = 0; i < Cam.Count; i++)
                {
                    Cam[i].gameObject.SetActive(false);
                }
                Cam[0].gameObject.SetActive(true);
                break;
            case "Sphere":
                for (int i = 0; i < Cam.Count; i++)
                {
                    Cam[i].gameObject.SetActive(false);
                }
                Cam[1].gameObject.SetActive(true);
                break;

        }*/
    }
}
