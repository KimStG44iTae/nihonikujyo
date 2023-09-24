using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapSceneManager : MonoBehaviour
{
    public static MapSceneManager instance;
    //��ũ��Ʈ��
    public UIManager uiManager;
    public SetFormImage setFormImage;
    public PointManager pointManager;
    public LineManager lineManager;
    public ShareData shareData;
    public ScaleConversion scaleConversion;
    public CCTVManager cctvManager;
    public UWBManager uwbManager;
    public MakeModel makeModel;
    public FileInOutput fileInOutput;
    public ZoneManager zoneManager;
    public MakeMesh makeMesh;
    public UnDoManager unDoManager;
    public MarkerManager markerManager;
    public BoardManager boardManager;
    public FireExitManager fireexitManager;
    public FileLoadList fileLoadList;

    public bool isMouseInUi; // ���콺�� ui�ȿ� ���� �ִ°�

    public GameObject mainCamera; // ����ķ
    public GameObject camera3d; // 3d ķ

    public string pubkey; // ��й�ȣ? �����ҵ�

    public void Start()
    {
        if (instance == null)
        {
            instance = this;
        }

        TextAsset tx = Resources.Load<TextAsset>("day");
        byte[] by = Encoding.UTF8.GetBytes(tx.ToString());

        X509Certificate2 certificate = new X509Certificate2(by);
        pubkey = certificate.GetPublicKeyString();

        GameObject project = GameObject.Find("projectName");

        if (project != null)
        {
            string projectName = project.GetComponent<ProjectInfo>().projectName;
            fileInOutput.StartReceive(projectName);
            Destroy(project);
        }

        uiManager.NowStepUI();
    }

    public void SceneChange(int index)
    {
        switch (index)
        {
            case 0:
                SceneManager.LoadScene("Title");
                break;

            case  1 :
                SceneManager.LoadScene("Monitoring");
                GameObject dontDestroy = GameObject.Find("Model");
                DontDestroyOnLoad(dontDestroy);
                break;

            default:
                break;
        }
    }

}
