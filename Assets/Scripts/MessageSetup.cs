using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageSetup : MonoBehaviour {
    public MessageDescription Message;
    public Text LabelComponent;
    public SpriteRenderer BackgroundSpriteRenderer;

	// Use this for initialization
	void Start ()
    {
        var color = Color.gray;
        switch(Message.Type)
        {
            case MessageType.Good:
                color = Color.green;
                break;
            case MessageType.Bad:
                color = Color.red;
                break;
        }

        LabelComponent.text = Message.Message;
        LabelComponent.color = BackgroundSpriteRenderer.color = color;
        var width = LabelComponent.preferredWidth * 0.05f + 0.5f;
        BackgroundSpriteRenderer.size = new Vector2(width, 0.5f);
        transform.localPosition += new Vector3(width / 2f, 0);
	}
	
	// Update is called once per frame
	void Update () {
		if(transform.parent.position.x < -50)
        {
            Destroy(transform.parent.gameObject);
        }
	}
}
