using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public TextMeshProUGUI sceneName;
    public Image background;

    public float time = 0;
    
    //튜토리얼 메세지 시작
    public void StartShow(string message, int t)
    {
        StopAllCoroutines();

        background.gameObject.SetActive(true);
        time = t;
        sceneName.text = message;
        StartCoroutine(ShowTutorialMessage());
    }

    //페이드인 시작
    public IEnumerator ShowTutorialMessage()
    {
        //초기 컬러값(Min)
        sceneName.color = new Color(sceneName.color.r, sceneName.color.g, sceneName.color.b, 0);
        background.color = new Color(background.color.r, background.color.g, background.color.b, 0);

        //알파값 늘려감
        while (sceneName.color.a < 1.0f)
        {
            sceneName.color = new Color(sceneName.color.r, sceneName.color.g, sceneName.color.b, sceneName.color.a + (Time.deltaTime / 2.0f));

            background.color = new Color(background.color.r, background.color.g, background.color.b, background.color.a + (Time.deltaTime / 3.0f));

            yield return null;
        }

        yield return new WaitForSeconds(time);

        yield return HideTutorialMessage();
    }

    //페이드 아웃 시작
    IEnumerator HideTutorialMessage()
    {
        //초기 컬러값(Max)
        sceneName.color = new Color(sceneName.color.r, sceneName.color.g, sceneName.color.b, 1);
        background.color = new Color(background.color.r, background.color.g, background.color.b, 0.75f);

        //알파값 줄여감
        while (sceneName.color.a > 0.0f)
        {
            sceneName.color = new Color(sceneName.color.r, sceneName.color.g, sceneName.color.b, sceneName.color.a - (Time.deltaTime / 2.0f));

            background.color = new Color(background.color.r, background.color.g, background.color.b, background.color.a - (Time.deltaTime / 3.0f));
            yield return null;
        }

        background.gameObject.SetActive(false);

        yield return null;
    }
}
