using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MarkerProperty : MonoBehaviour
{
    public TMP_InputField nameText;

    public TMP_InputField xAxisText;
    public TMP_InputField yAxisText;
    public TMP_InputField zAxisText;

    public TMP_InputField xPosText;
    public TMP_InputField yPosText;
    public TMP_InputField zPosText;

    public Image nowMarkerImage;

    public GameObject selectMarker;

    public JsonMarker jsonMarker;

    public GameObject purple;
    public GameObject unity;
    public GameObject vuforia;

    public int markerId;

    private void OnDisable()
    {
        ResetProperty();
    }

    public void ResetProperty()
    {
        nameText.text = "";
        xAxisText.text = "";
        yAxisText.text = "";
        zAxisText.text = "";
        xPosText.text = "";
        yPosText.text = "";
        zPosText.text = "";

        nowMarkerImage.sprite = null;

        if (selectMarker)
        {
            selectMarker.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    public void SetMarker(GameObject marker)
    {
        ResetProperty();

        selectMarker = marker;
        selectMarker.transform.GetChild(0).gameObject.SetActive(true);

        int id = selectMarker.GetComponent<MarkerInfo>().id;

        markerId = id;

        jsonMarker = ShareData.instance.jsonData.markers.Find(x => x.id == id);

        nameText.text = jsonMarker.name;

        xAxisText.text = jsonMarker.rotation.x.ToString();
        yAxisText.text = jsonMarker.rotation.z.ToString();
        zAxisText.text = jsonMarker.rotation.y.ToString();

        xPosText.text = jsonMarker.position.x.ToString();
        yPosText.text = jsonMarker.position.z.ToString();
        zPosText.text = jsonMarker.position.y.ToString();

        selectMarker.transform.GetChild(0).gameObject.transform.rotation = Quaternion.Euler(jsonMarker.rotation);

        Vector3 position = new Vector3(jsonMarker.position.x, jsonMarker.position.y, jsonMarker.position.z);

        position.y = 0.03f;

        selectMarker.transform.position = position;

        switch (nameText.text)
        {
            case "purple":
                ChangeNowMarkerImage(purple);
                break;
            case "unity":
                ChangeNowMarkerImage(unity);
                break;
            case "vuforia":
                ChangeNowMarkerImage(vuforia);
                break;
            default:
                break;
        }

    }

    public void ChangeNowMarkerImage(GameObject image)
    {
        nowMarkerImage.sprite = image.GetComponent<Image>().sprite;
        nameText.text = image.name;

        jsonMarker.name = nameText.text;
        MapSceneManager.instance.unDoManager.AddUndo();
    }

    public void AxisChange()
    {
        float x = float.Parse(xAxisText.text);
        float y = float.Parse(yAxisText.text);
        float z = float.Parse(zAxisText.text);

        jsonMarker.rotation = new Vector3(x, y, z);

        selectMarker.transform.GetChild(0).gameObject.transform.rotation = Quaternion.Euler(jsonMarker.rotation);

        selectMarker.GetComponent<MarkerInfo>().SetLocalRotation();
        MapSceneManager.instance.unDoManager.AddUndo();
    }

    public void PositionChange()
    {
        float x = float.Parse(xPosText.text);
        float y = float.Parse(yPosText.text);
        float z = float.Parse(zPosText.text);

        Vector3 position = new Vector3(x, z, y);

        jsonMarker.position = position;

        position = new Vector3(x, 0.03f, y);

        selectMarker.transform.position = position;

        selectMarker.GetComponent<MarkerInfo>().SetOriginPos();
        MapSceneManager.instance.unDoManager.AddUndo();
    }

    public void UpdateJsonMarker()
    {
        ResetProperty();
        gameObject.SetActive(false);
    }

    public void DeleteMarker()
    {

        for (int i = 0; i < ShareData.instance.jsonData.markers.Count; i++)
        {
            if (ShareData.instance.jsonData.markers[i].id == jsonMarker.id)
            {
                ShareData.instance.jsonData.markers.RemoveAt(i);
            }
        }

        MapSceneManager.instance.markerManager.UpdateMarker();
        ResetProperty();
        MapSceneManager.instance.uiManager.OffSelect_All();
        MapSceneManager.instance.unDoManager.AddUndo();

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
}
