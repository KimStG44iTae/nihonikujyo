using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSceneManager : MonoBehaviour
{
    public string pubkey;

    public static TitleSceneManager instance;

    public enum NextScene
    {
        MapManager, Monitoring, Operation, InsertOperation
    }

    public NextScene nextscene;

    public string mapManager = "MapClient";
    public string monitoring = "Monitoring";
    public string operation = "Operation";
    public string insertOperation = "InsertOperation";

    public string url = "https://192.168.0.100:5000";

    public TitleFileLoad titleFileLoad;
    public TitleUIManager titleUIManager;

    public void Start()
    {
        if (instance == null)
        {
            instance = this;
        }

        //ÀÎÁõ¼­ Å° »ý¼º
        //TextAsset tx = Resources.Load<TextAsset>("day");
        //byte[] by = Encoding.UTF8.GetBytes(tx.ToString());

        //X509Certificate2 certificate = new X509Certificate2(by);
        //pubkey = certificate.GetPublicKeyString();

        pubkey = NetworkManager.instance.pubkey;
        url = NetworkManager.instance.url;
    }

    public void LoadScene(int index) //¹öÆ° Å¬¸¯À¸·Î ¾À º¯°æ
    {
        switch (index)
        {
            case 0:
                nextscene = NextScene.MapManager; //mapclient ¾À
                SceneManager.LoadScene(mapManager);
                break;

            case 1:
                nextscene = NextScene.Monitoring; //monitoring ¾À
                SceneManager.LoadScene(monitoring);
                break;

            case 2:
                nextscene = NextScene.Operation; //Operation ¾À
                //SceneManager.LoadScene(operation);
                break;

            case 3:
                nextscene = NextScene.InsertOperation; //insert Operation ¾À
            //    SceneManager.LoadScene(insertOperation);
                break;

            default:
                break;
        }
    }

    public string projectName;

    public void FileLoad_SceneLoad(GameObject projectName) 
    {
        nextscene = NextScene.MapManager;
        SceneManager.LoadScene(mapManager);

        GameObject test = Instantiate( projectName);
        test.name = "projectName";


        DontDestroyOnLoad(test);
    }

}
