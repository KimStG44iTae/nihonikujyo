using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardManager : MonoBehaviour
{
    public GameObject boardPrefeb;

    public GameObject tempBoard;

    public GameObject selectBoard;

    public List<GameObject> boardList;

    public BoardProperty boardProperty;

    RaycastHit hit;
    Ray ray;

    public bool isStart;
    public bool isDraw;
    public bool isDrawEnd;

    public Button button;

    public Sprite buttonOn;
    public Sprite buttonOff;

    void Update()
    {
        if (isStart)
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (hit.transform.tag == "Board")
                    {
                        MapSceneManager.instance.uiManager.InitStatus();
                        MapSceneManager.instance.uiManager.NowStepUI();
                        MapSceneManager.instance.uiManager.OffSelect_All();

                        selectBoard = hit.transform.gameObject;

                        boardProperty.gameObject.SetActive(true);
                        boardProperty.SetBoard(selectBoard);

                        MapSceneManager.instance.markerManager.EndStatus();
                    }
                }

                if (Input.GetMouseButtonUp(0))
                {
                    if (selectBoard != null)
                    {
                        selectBoard = null;
                    }

                    MapSceneManager.instance.uiManager.NowStepUI();
                }
            }
        }

        if (isDraw)
        {
            if (isDrawEnd)
            {
                if (Input.GetMouseButtonUp(0))
                {
                    EndDrawStatus();
                    StartStatus();
                    MapSceneManager.instance.uiManager.NowStepUI();
                    MapSceneManager.instance.uiManager.InfomationUpdate();
                    isDrawEnd = false;
                }
            }

            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                Vector3 temp = new Vector3(hit.point.x, 0.03f, hit.point.z);

                temp.x = (float)Math.Round(temp.x, 2);
                temp.y = (float)Math.Round(temp.y, 2);
                temp.z = (float)Math.Round(temp.z, 2);

                tempBoard.transform.position = temp;

                if (Input.GetMouseButtonDown(0) && !MapSceneManager.instance.isMouseInUi)
                {
                    GameObject nowBoard = Instantiate(boardPrefeb, temp, Quaternion.identity);

                    nowBoard.GetComponent<BoardInfo>().id = ShareData.instance.jsonData.lastID;
                    ShareData.instance.jsonData.lastID += 1;

                    JsonBoard jsonBoard = new JsonBoard();
                    jsonBoard.id = nowBoard.GetComponent<BoardInfo>().id;

                    jsonBoard.pos1 = temp - Vector3.forward * boardPrefeb.transform.localScale.z * 0.5f - Vector3.right * boardPrefeb.transform.localScale.x * 0.5f;

                    jsonBoard.pos2 = temp - Vector3.forward * boardPrefeb.transform.localScale.z * 0.5f + Vector3.right * boardPrefeb.transform.localScale.x * 0.5f;

                    jsonBoard.position = new Vector3(jsonBoard.pos1.x, jsonBoard.pos1.z, 0);

                    MapSceneManager.instance.shareData.AddJsonBoard(jsonBoard);

                    boardList.Add(nowBoard);

                    MapSceneManager.instance.unDoManager.AddUndo();
                    MapSceneManager.instance.uiManager.NowStepUI();

                    isDrawEnd = true;
                }
            }
            if (Input.GetMouseButtonUp(1))
            {
                EndDrawStatus();
                StartStatus();
                MapSceneManager.instance.uiManager.NowStepUI();
            }


        }
    }


    public void DestroyBoard()
    {
        for (int i = boardList.Count - 1; i >= 0; i--)
        {
            Destroy(boardList[i]);
        }

        boardList.Clear();
    }


    public void UpdateBoard()
    {
        DestroyBoard();

        if (ShareData.instance.jsonData.boards.Count > 0)
        {
            for (int i = 0; i < ShareData.instance.jsonData.boards.Count; i++)
            {
                JsonBoard tempJsonBoard = ShareData.instance.jsonData.boards[i];

                GameObject nowBoard = Instantiate(boardPrefeb, tempJsonBoard.position, Quaternion.identity);

                Vector3 v1 = new Vector3(tempJsonBoard.pos1.x, tempJsonBoard.pos1.y, tempJsonBoard.pos1.z);
                Vector3 v2 = new Vector3(tempJsonBoard.pos2.x, tempJsonBoard.pos2.y, tempJsonBoard.pos2.z);

                Vector3 xAxis = (v2 - v1).normalized;

                Vector3 forwardVector = Vector3.Cross(xAxis, Vector3.up).normalized;

                nowBoard.transform.rotation = Quaternion.LookRotation(forwardVector, Vector3.up);

                Vector3 boardPos = tempJsonBoard.pos1 + nowBoard.transform.forward * nowBoard.transform.localScale.z * 0.5f + nowBoard.transform.right * nowBoard.transform.localScale.x * 0.5f;

                nowBoard.transform.position = new Vector3(boardPos.x, 0.03f, boardPos.z);

                //Vector3 boardPos = tempJsonBoard.pos1 + nowBoard.transform.forward * nowBoard.transform.localScale.z * 0.5f + nowBoard.transform.right * nowBoard.transform.localScale.x * 0.5f;

                //nowBoard.transform.position = boardPos;

                nowBoard.GetComponent<BoardInfo>().id = tempJsonBoard.id;

                boardList.Add(nowBoard);
            }
        }

        if (boardProperty.gameObject.activeSelf)
        {
            for (int i = 0; i < boardList.Count; i++)
            {
                if (boardList[i].GetComponent<BoardInfo>().id == boardProperty.boardId)
                {
                    boardProperty.SetBoard(boardList[i]);
                }
            }
        }

    }


    //기본상태 변경
    public void StartStatus()
    {
        isStart = true;
    }

    public void EndStatus()
    {
        isStart = false;
        boardProperty.gameObject.SetActive(false);
    }

    //그리기 상태 변경
    public void StartDrawStatus()
    {
        tempBoard.SetActive(true);
        isDraw = true;
    }

    public void EndDrawStatus()
    {
        isDraw = false;
        tempBoard.SetActive(false);
        MapSceneManager.instance.uiManager.EndIsDraw();
    }
}
