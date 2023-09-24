using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Runtime.InteropServices;

public class toUnity : MonoBehaviour
{
#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport ("__Internal")]
    private static extern void ShowMessage(string message);
#endif
    public TMP_InputField TextInput;
    public TMP_Text DisplayText;
    // Start is called before the first frame update
    void Start()
    {
        /*#if !UNITY_EDITOR && UNITY_WEBGL
        WebGLInput.captureAllkeyboardInput = false;
        #endif*/
    }

    // Update is called once per frame
    public void SendToJS()
    {
        string MessageToSend = TextInput.text;
        Debug.Log("Sending message to JS: " + MessageToSend);
        #if UNITY_WEBGL && !UNITY_EDITOR
        ShowMessage(MessageToSend); 
        #endif //플러그인 안에 저장된 자바스크립트 기능을 사용하겠다는 코드
    }
    public void SendToUnity(string message)
    {
        DisplayText.text = message;
    }

    public void BottonNextScen()
    {
        SceneManager.LoadScene("MapClient");
    }
    public void BottonScen()
    {
        SceneManager.LoadScene("SampleScene");
    }
    public void NextScen(string map)
    {
        if (map == "MapClient") 
        { 
        SceneManager.LoadScene("MapClient");
        }
    }
}
