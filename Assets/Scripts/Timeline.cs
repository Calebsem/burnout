using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timeline : MonoBehaviour {
    public GameObject TickPrefab;
    public GameObject TickLabelPrefab;
    public GameObject TickLimitPrefab;
    public GameObject HomePrefab;
    public float HourUnitSize = 6f;
    
    private GameObject endHome;

    // Use this for initialization
    void Start () {
        CreateTimeline();

    }

    private void CreateTimeline()
    {
        var startHome = Instantiate(HomePrefab, transform);
        startHome.transform.localPosition = new Vector3(HourUnitSize * 8, 0.5f);

        for (int i = 0; i < 24; i++)
        {
            var mainTick = Instantiate(TickPrefab, transform);
            mainTick.transform.localPosition = new Vector3(i * HourUnitSize, 0);
            var label = Instantiate(TickLabelPrefab, mainTick.transform);
            label.transform.localPosition = new Vector3(0,-0.825f);
            label.GetComponentInChildren<Text>().text = (i - 12 > 0) ? ((i - 12).ToString() + ":00pm") : (i.ToString() + ((i - 12 == 0) ? ":00pm": ":00am"));

            var sideTickSize = HourUnitSize / 6f;
            for(int y = 1; y < 6; y++)
            {
                var sideTick = Instantiate(TickPrefab, transform);
                sideTick.transform.localScale /= 2f;
                sideTick.transform.localPosition = new Vector3(i * HourUnitSize + y * sideTickSize, 0);
                sideTick.GetComponent<SpriteRenderer>().color = Color.gray;
            }
        }

        transform.position -= new Vector3(HourUnitSize * 8 + 3, 0);
    }

    public void SetEndLimit(float x)
    {
        if (endHome == null)
        {
            endHome = Instantiate(HomePrefab, transform);
        }
        endHome.transform.localPosition = new Vector3(x - 5,  0.5f);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
