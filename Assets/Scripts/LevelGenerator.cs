using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MessageType
{
    Neutral,
    Good,
    Bad
}

[Serializable]
public struct MessageDescription
{
    public string Message;
    public MessageType Type;
}

public class LevelGenerator : MonoBehaviour {
    public float Speed = 3f;
    public GameObject DialogPrefab;

    public List<MessageDescription> Messages;

    public GameObject LastBox;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        var delta = Speed * Time.smoothDeltaTime;
        foreach (Transform child in transform)
        {
            child.position -= new Vector3(delta, 0);
        }
        if(LastBox == null || LastBox.transform.position.x <= 20)
        {
            var x = (LastBox == null) ? -3 : LastBox.transform.position.x + LastBox.GetComponentInChildren<SpriteRenderer>().bounds.size.x + 2f;
            var y = (LastBox == null) ? 0 : LastBox.transform.position.y + UnityEngine.Random.Range(-1f, 1f);
            y = Mathf.Clamp(y, -3, 3);
            var newBox = Instantiate(DialogPrefab, transform);
            var desc = (LastBox == null) ? new MessageDescription { Message = "I know things are not great, but you'll make it", Type = MessageType.Neutral } :
                Messages[UnityEngine.Random.Range(0, Messages.Count)];
            var message = newBox.GetComponentInChildren<MessageSetup>();
            message.Message = desc;
            newBox.transform.position = new Vector3(x, y);
            LastBox = newBox;
        }
	}
}
