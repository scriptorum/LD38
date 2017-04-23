using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spewnity;

public class MessageBar : MonoBehaviour 
{
	public Text text;
	public RectTransform groupRT;
	public float speed = 1f;
	public float pause = 3f;
	public AnimationCurve curve;

	private ActionQueue aq;
	private float outsideY = -50;
	private float insideY = 50;

	void Awake()
	{
		aq = gameObject.AddComponent<ActionQueue>();
		moveGroup(outsideY);
		// showGroup(false);
	}

	private void moveGroup(float pos)
	{
		groupRT.anchoredPosition = new Vector2(0, pos);
	}

	public void reset()
	{
		aq.Cancel();
		aq.Clear();
	}

	// If you supply hold = true, call reset() to clear the message, or just call this again
	public MessageBar showMessage(string msg, bool hold = false)
	{
		reset();
		queueMessage(msg, hold);
		aq.Run();
		return this;
	}

	// After clearing the last message, you can then queue up another message
	public MessageBar queueMessage(string msg, bool hold = false)
	{
		aq.Add(() => setMessage(msg));
		aq.AddCoroutine(outsideY.LerpFloat(insideY, speed, (f)=>moveGroup(f), curve));
		if(!hold)
		{
			aq.Delay(speed);
			aq.Delay(pause);
			aq.AddCoroutine(insideY.LerpFloat(outsideY, speed, (f)=>moveGroup(f), curve));
			aq.Delay(speed);
		}
		
		return this;
	}

	private void setMessage(string msg)
	{
		text.text = msg;
	}
}
