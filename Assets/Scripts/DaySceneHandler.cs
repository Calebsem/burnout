using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DaySceneHandler : MonoBehaviour
{
    public Text DayLabel;
    public Text FatigueLabel;
    public Text FatigueTitleLabel;

    private bool restart;

    // Use this for initialization
    void Start ()
    {
        if (SystemState.Instance.Day < 30 && SystemState.Instance.FatigueLeft >= 3)
        {
            DayLabel.text = "Day " + SystemState.Instance.Day.ToString();
            FatigueLabel.text = SystemState.Instance.FatigueLeft.ToString("0h");
            StartCoroutine(AnimateAndGoOn());
            StartCoroutine(ChangeBackground());
        }
        else if(SystemState.Instance.Day >= 30)
        {
            DayLabel.text = "You made it";
            FatigueLabel.text = "hurray\npress space to restart\npress ESC to quit";
            FatigueLabel.fontSize = 12;
            StartCoroutine(GameOver());
        }
        else if (SystemState.Instance.FatigueLeft < 3)
        {
            Camera.main.backgroundColor = new Color(0.15f, 0.15f, 0.15f);
            DayLabel.text = "burnout.";
            FatigueLabel.text = "hang in there, buddy\npress space to restart\npress ESC to quit";
            FatigueLabel.fontSize = 12;
            StartCoroutine(GameOver());
        }
    }

    IEnumerator ChangeBackground()
    {
        for (;;)
        {
            var trueColor = new Color(
                Random.Range(0.8f, 1f),
                Random.Range(0.8f, 1f),
                Random.Range(0.8f, 1f), 1);
            var firstColor = Camera.main.backgroundColor;
            for (float i = 0; i < 1; i += Time.smoothDeltaTime)
            {
                Camera.main.backgroundColor = Color.Lerp(firstColor, trueColor, i);
                yield return null;
            }
            yield return new WaitForSeconds(0.2f);
        }
    }

    IEnumerator AnimateAndGoOn()
    {
        var color1 = new Color(0, 213f / 255f, 1, 0);
        var color2 = new Color(0, 213f / 255f, 1, 1);
        var color3 = color1 / 2;
        var color4 = color2 / 2;
        color4.a = 1;
        for (float i = 0; i < 2; i += Time.smoothDeltaTime)
        {
            DayLabel.color = Color.Lerp(color1, color2, i / 2f);

            if (i < 1)
                yield return null;

            FatigueLabel.color = Color.Lerp(color3, color4, i - 1);
            if (SystemState.Instance.FatigueLeft < 5)
                FatigueLabel.rectTransform.anchoredPosition3D = new Vector3(10, -100) + Random.insideUnitSphere * 2f ;
            FatigueTitleLabel.color = Color.Lerp(color3, color4, i - 1);
            yield return null;
        }
        for (float i = 0; i < 2; i += Time.smoothDeltaTime)
        {
            if (SystemState.Instance.FatigueLeft < 5)
                FatigueLabel.rectTransform.anchoredPosition3D = new Vector3(10, -100) + Random.insideUnitSphere * 2f;
            yield return null;
        }
        SceneManager.LoadScene(2, LoadSceneMode.Single);
    }

    IEnumerator GameOver()
    {
        var color1 = new Color(0, 213f / 255f, 1, 0);
        var color2 = new Color(0, 213f / 255f, 1, 1);
        var color3 = color1 / 2;
        var color4 = color2 / 2;
        color4.a = 1;
        for (float i = 0; i < 2; i += Time.smoothDeltaTime)
        {
            DayLabel.color = Color.Lerp(color1, color2, i / 2f);
            DayLabel.rectTransform.anchoredPosition3D = Random.insideUnitSphere * 5f;

            if (i < 1)
                yield return null;

            restart = true;

            FatigueLabel.color = Color.Lerp(color3, color4, i - 1);
            yield return null;
        }
        for (;;)
        {
            DayLabel.rectTransform.anchoredPosition3D = Random.insideUnitSphere * 5f;
            yield return null;
        }
    }

    // Update is called once per frame
    void Update () {
		if(restart)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SystemState.Instance.Restart();
                SceneManager.LoadScene(1, LoadSceneMode.Single);
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene(0, LoadSceneMode.Single);
            }
        }
	}
}
