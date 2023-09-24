using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OperationUIManager : MonoBehaviour
{
    public GameObject insertMember;
    public GameObject updateMember;

    public GameObject errorMessgae;
    public TMP_Text errorMessgaeText;

    public enum Status
    {
        Register, Update
    }

    public Status nowStatus;

    public void ErrorMessgaeOn(string message)
    {
        errorMessgaeText.text = message;
        errorMessgae.SetActive(true);
    }

    public void ErrorMessageOff()
    {
        errorMessgae.SetActive(false);
    }

    //µî·ÏÃ¢ ÆÇ³Ú
    public void InsertOn()
    {
        nowStatus = Status.Register;
        insertMember.SetActive(true);
        updateMember.SetActive(false);
    }

    //Á¤º¸Ã¢ ÆÇ³Ú
    public void UpdateOn()
    {
        nowStatus = Status.Update;
        insertMember.SetActive(false);
        updateMember.SetActive(true);
    }
}
