using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class HeroJump : MonoBehaviour {

    public Rigidbody2D Body;
    private float fatiguePixelMaxLevel = 970;
    public LevelGenerator Level;
    public Timeline Timeline;
    public Image FatigueBar;
    public Image FatigueBg;
    public Text FatigueLabel;
    public ParticleSystem LoveParticles;
    public GameObject SighPrefab;
    public float HoursOfFatigueAvailable = 8;
    private float hoursOfFatigueLeft;
    private float unitsOfHourPerSecond;

    public LineRenderer LineRenderer;
    private List<Vector3> linePoints = new List<Vector3>();
    private float linePointAddDelta = 0;
    private int maxPoints = 20;

    public float ConsumptionMultiplier = 1f;

    private float maxTime;

    private bool ended;

    public AudioSource JumpAudio;
    public AudioSource LandAudio;
    public AudioSource EndAudio;

    public Image FadeOutRenderer;

    // Use this for initialization
    void Start () {
        ChangeLoveEmission(0);
        HoursOfFatigueAvailable = SystemState.Instance.FatigueLeft;
        unitsOfHourPerSecond = Level.Speed / Timeline.HourUnitSize;
        hoursOfFatigueLeft = HoursOfFatigueAvailable;
        maxTime = Timeline.HourUnitSize * (8 + HoursOfFatigueAvailable) + 5;
        Timeline.SetEndLimit(maxTime);
    }

    private void ChangeLoveEmission(int count)
    {
        var emission = LoveParticles.emission;
        emission.rateOverTime = count;
    }
	
	// Update is called once per frame
	void Update () {
        if (ended)
            return;
		if(Input.GetButtonDown("Jump"))
        {
            Body.AddForce(-Physics2D.gravity, ForceMode2D.Impulse);
            JumpAudio.Play();
        }

        hoursOfFatigueLeft -= unitsOfHourPerSecond * Time.smoothDeltaTime * ConsumptionMultiplier;
        var pixelSize = fatiguePixelMaxLevel * hoursOfFatigueLeft / HoursOfFatigueAvailable;
        FatigueBar.rectTransform.sizeDelta = new Vector2(pixelSize, -30);
        FatigueBar.color = FatigueLabel.color = Color.Lerp(Color.red, Color.green, pixelSize / 970f);
        FatigueLabel.text = "Fatigue " + hoursOfFatigueLeft.ToString("0h"); 

        if (ConsumptionMultiplier != 1)
        {
            if (ConsumptionMultiplier == 0)
                maxTime += unitsOfHourPerSecond * Time.smoothDeltaTime;
            else
                maxTime -= unitsOfHourPerSecond * Time.smoothDeltaTime;
            Timeline.SetEndLimit(maxTime);
        }

        linePointAddDelta += Time.smoothDeltaTime * 10;
        if(linePointAddDelta >= 1)
        {
            linePointAddDelta = 0;
            linePoints.Add(transform.position + new Vector3(0, 0.5f, 0));
        }
        for(var i = 0; i < linePoints.Count; i++)
        {
            linePoints[i] -= new Vector3(Level.Speed * Time.smoothDeltaTime, 0);
        }
        if (linePoints.Count > maxPoints)
        {
            linePoints = linePoints.Skip(linePoints.Count - maxPoints).Take(maxPoints).ToList();
        }
        var tmpLinePoints = new List<Vector3>(linePoints);
        tmpLinePoints.Add(transform.position + new Vector3(0, 0.5f, 0));
        LineRenderer.positionCount = tmpLinePoints.Count;
        LineRenderer.SetPositions(tmpLinePoints.ToArray());

        if(transform.position.y < - 6)
        {
            EndAudio.Play();
            StartCoroutine(EndTransition(HoursOfFatigueAvailable - hoursOfFatigueLeft));
            return;
        }

        if(hoursOfFatigueLeft <= 0)
        {
            EndAudio.Play();
            StartCoroutine(EndTransition(HoursOfFatigueAvailable - 0.25f));
        }
    }

    IEnumerator EndTransition(float FatigueLeft)
    {
        ended = true;
        var color = FadeOutRenderer.color;
        var finalColor = color;
        finalColor.a = 1;
        for (var i = 0f; i < 1; i += Time.smoothDeltaTime)
        {
            FadeOutRenderer.color = Color.Lerp(color, finalColor, i);
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        SystemState.Instance.Day++;
        SystemState.Instance.FatigueLeft = FatigueLeft;
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.transform.parent.tag == "Box")
        {
            LandAudio.Play();
            var type = coll.gameObject.GetComponent<MessageSetup>().Message.Type;
            switch(type)
            {
                default:
                    ChangeLoveEmission(0);
                    ConsumptionMultiplier = 1f;
                    FatigueBg.color = Color.grey;
                    Camera.main.backgroundColor = Color.white;
                    break;
                case MessageType.Good:
                    ChangeLoveEmission(10);
                    ConsumptionMultiplier = 0f;
                    FatigueBg.color = Color.green;
                    Camera.main.backgroundColor = new Color(0.9f, 1, 0.9f, 1);
                    break;
                case MessageType.Bad:
                    ChangeLoveEmission(0);
                    ConsumptionMultiplier = 2f;
                    FatigueBg.color = Color.red;
                    Camera.main.backgroundColor = new Color(1, 0.9f, 0.9f, 1);
                    var sigh = Instantiate(SighPrefab, Level.gameObject.transform);
                    sigh.transform.position = transform.position + new Vector3(0, 0.5f);
                    break;
            }
        }
    }

    void OnCollisionExit2D(Collision2D coll)
    {
        if (coll.gameObject.transform.parent.tag == "Box")
        {
            ChangeLoveEmission(0);
            ConsumptionMultiplier = 1f;
            FatigueBg.color = Color.gray;
            Camera.main.backgroundColor = Color.white;
        }
    }

}
