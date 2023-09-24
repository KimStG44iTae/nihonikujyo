using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BoardProperty : MonoBehaviour
{
    public TMP_InputField matchCam;

    public TMP_InputField x1PosText;
    public TMP_InputField y1PosText;
    public TMP_InputField z1PosText;

    public TMP_InputField x2PosText;
    public TMP_InputField y2PosText;
    public TMP_InputField z2PosText;

    public GameObject selectBoard;
    public JsonBoard jsonBoard;

    public int boardId;

    private void OnDisable()
    {
        ResetProperty();
    }

    public void ResetProperty()
    {
        x1PosText.text = "";
        y1PosText.text = "";
        z1PosText.text = "";
        x2PosText.text = "";
        y2PosText.text = "";
        z2PosText.text = "";

        if (selectBoard)
        {
            selectBoard.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    public void SetBoard(GameObject board)
    {
        ResetProperty();

        selectBoard = board;

        selectBoard.transform.GetChild(0).gameObject.SetActive(true);
        
        int id = selectBoard.GetComponent<BoardInfo>().id;

        boardId = id;

        jsonBoard = MapSceneManager.instance.shareData.jsonData.boards.Find(x => x.id == id);

        x1PosText.text = jsonBoard.pos1.x.ToString();
        y1PosText.text = jsonBoard.pos1.z.ToString();
        z1PosText.text = jsonBoard.pos1.y.ToString();

        x2PosText.text = jsonBoard.pos2.x.ToString();
        y2PosText.text = jsonBoard.pos2.z.ToString();
        z2PosText.text = jsonBoard.pos2.y.ToString();

        matchCam.text = jsonBoard.matchCam;

        //SetDropDownCamera();

        //for (int i = 0; i < matchCam.options.Count; i++)
        //{
        //    if (jsonBoard.matchCCTV == matchCam.options[i].text)
        //    {
        //        matchCam.value = i;
        //    }
        //}

        CalcPosition();
    }

    public void CalcPosition()
    {
        float x1 = float.Parse(x1PosText.text);
        float y1 = float.Parse(y1PosText.text);
        float z1 = float.Parse(z1PosText.text);

        float x2 = float.Parse(x2PosText.text);
        float y2 = float.Parse(y2PosText.text);
        float z2 = float.Parse(z2PosText.text);

        jsonBoard.pos1 = new Vector3(x1, z1, y1);
        jsonBoard.pos2 = new Vector3(x2, z2, y2);

        CalculateRotation();

        Vector3 boardPos = jsonBoard.pos1 + selectBoard.transform.forward * selectBoard.transform.localScale.z * 0.5f + selectBoard.transform.right * selectBoard.transform.localScale.x * 0.5f;

        selectBoard.transform.position = new Vector3(boardPos.x, 0.03f, boardPos.z);

        Vector3 position = new Vector3(x1, y1, 0);

        jsonBoard.position = position;

        if (selectBoard.transform.rotation.eulerAngles.y > 180)
        {
            jsonBoard.rotation = 360 - selectBoard.transform.rotation.eulerAngles.y;
        }
        else
        {
            jsonBoard.rotation = -selectBoard.transform.rotation.eulerAngles.y;
        }
    }

    public void PositionChange()
    {
        CalcPosition();
        MapSceneManager.instance.unDoManager.AddUndo();
    }

    public void CalculateRotation()
    {
        Vector3 v1 = new Vector3(float.Parse(x1PosText.text), float.Parse(z1PosText.text), float.Parse(y1PosText.text));
        Vector3 v2 = new Vector3(float.Parse(x2PosText.text), float.Parse(z2PosText.text), float.Parse(y2PosText.text));

        Vector3 xAxis = (v2 - v1).normalized;

        Vector3 forwardVector = Vector3.Cross(xAxis, Vector3.up).normalized;

        selectBoard.transform.rotation = Quaternion.LookRotation(forwardVector,Vector3.up);
    }


    public void DeleteBoard()
    {
        MapSceneManager.instance.unDoManager.AddUndo();

        for (int i = 0; i < ShareData.instance.jsonData.boards.Count; i++)
        {
            if (ShareData.instance.jsonData.boards[i].id == jsonBoard.id)
            {
                ShareData.instance.jsonData.boards.RemoveAt(i);
            }
        }

        MapSceneManager.instance.boardManager.UpdateBoard();
        MapSceneManager.instance.uiManager.OffSelect_All();

        gameObject.SetActive(false);
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            ResetProperty();
            gameObject.SetActive(false);
        }
    }

    //public void SetDropDownCamera()
    //{
    //    matchCam.ClearOptions();

    //    List<string> option = new List<string>();

    //    for (int i = 0; i < MapSceneManager.instance.shareData.jsonData.cctvs.Count; i++)
    //    {
    //        option.Add(MapSceneManager.instance.shareData.jsonData.cctvs[i].ip);
    //    }

    //    matchCam.AddOptions(option);
    //}

    public void SelectCamera()
    {
        MapSceneManager.instance.unDoManager.AddUndo();

        jsonBoard.matchCam = matchCam.text;

        //if (matchCam.options.Count > 0)
        //{
        //    jsonBoard.matchCCTV = matchCam.options[matchCam.value].text;
        //}
    }
}
