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
    
    //Ʃ�丮�� �޼��� ����
    public void StartShow(string message, int t)
    {
        StopAllCoroutines();

        background.gameObject.SetActive(true);
        time = t;
        sceneName.text = message;
        StartCoroutine(ShowTutorialMessage());
    }

    //���̵��� ����
    public IEnumerator ShowTutorialMessage()
    {
        //�ʱ� �÷���(Min)
        sceneName.color = new Color(sceneName.color.r, sceneName.color.g, sceneName.color.b, 0);
        background.color = new Color(background.color.r, background.color.g, background.color.b, 0);

        //���İ� �÷���
        while (sceneName.color.a < 1.0f)
        {
            sceneName.color = new Color(sceneName.color.r, sceneName.color.g, sceneName.color.b, sceneName.color.a + (Time.deltaTime / 2.0f));

            background.color = new Color(background.color.r, background.color.g, background.color.b, background.color.a + (Time.deltaTime / 3.0f));

            yield return null;
        }

        yield return new WaitForSeconds(time);

        yield return HideTutorialMessage();
    }

    //���̵� �ƿ� ����
    IEnumerator HideTutorialMessage()
    {
        //�ʱ� �÷���(Max)
        sceneName.color = new Color(sceneName.color.r, sceneName.color.g, sceneName.color.b, 1);
        background.color = new Color(background.color.r, background.color.g, background.color.b, 0.75f);

        //���İ� �ٿ���
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
