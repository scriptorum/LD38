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

	public void showMessage(string msg)
	{
		setMessage(msg);

		aq.Cancel();
		aq.Clear();
		
		aq.AddCoroutine(outsideY.LerpFloat(insideY, speed, (f)=>moveGroup(f), curve));
		aq.Delay(speed);
		aq.Delay(pause);
		aq.AddCoroutine(insideY.LerpFloat(outsideY, speed, (f)=>moveGroup(f), curve));
		aq.Delay(speed);
		aq.Run();
	}

	private void setMessage(string msg)
	{
		text.text = msg;
	}
}
