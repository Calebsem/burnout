using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleAnimation : MonoBehaviour {
    public Text Label;

	// Use this for initialization
	void Start () {
        StartCoroutine(ChangeBackground());
	}

    IEnumerator ChangeBackground()
    {
        for(;;)
        {
            var trueColor = new Color(
                Random.Range(0.8f,1f),
                Random.Range(0.8f,1f),
                Random.Range(0.8f,1f), 1);
            var firstColor = Camera.main.backgroundColor;
            for (float i = 0; i < 1; i += Time.smoothDeltaTime)
            {
                Camera.main.backgroundColor = Color.Lerp(firstColor, trueColor, i);
                yield return null;
            }
            yield return new WaitForSeconds(2f);
        }
    }
	
	// Update is called once per frame
	void Update () {
        Label.rectTransform.anchoredPosition3D = new Vector3(0,75) + Random.insideUnitSphere * 5f;
        var tmpColor = Random.insideUnitSphere;
        var trueColor = new Color(tmpColor.x, tmpColor.y, tmpColor.z, 1);
        Label.color = trueColor;

    }
}
