using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spewnity;

public class Tracker : MonoBehaviour 
{
	public Image[] marks;
	public Image pointer;
	public int lastMark = 0;
	public float speed = 1.0f;

	private Image planet;
	private Image spacejunk;
	private float range;
	private float startX, endX;
	private ActionQueue aq;
	private int maxMark;
	private float current;

	void Awake()
	{
		planet = marks[0];
		maxMark = marks.Length - 1;
		spacejunk = marks[maxMark];
		startX = planet.transform.localPosition.x;
		endX = spacejunk.transform.localPosition.x;
		range = endX - startX;
		aq = gameObject.AddComponent<ActionQueue>();

		reset();
	}

	public void reset()
	{
		current = 0;
		lastMark = 0;

		pointer.transform.localPosition = planet.transform.localPosition;

		foreach(Image mark in marks)
			mark.enabled = false;

		planet.enabled = true;
		
		setPosition(1.0f);
	}

	// Gradually sets position of pointer
	public void setPosition(float pos)
	{
		pos = Mathf.Min(pos, 1f);
		Debug.Assert(pos > 0f);
		StartCoroutine(current.LerpFloat(pos, speed, (f) => setPositionNow(f)));
	}

	// Immediately sets position of pointer
	// Checks if the NEXT mark has been reached, and highlights it
	public void setPositionNow(float pos)
	{
		Vector3 v = pointer.transform.localPosition;
		v.x = startX + pos * range;
		pointer.transform.localPosition = v;
		current = pos;
		if(lastMark != maxMark && (v.x >= marks[lastMark + 1].transform.localPosition.x || pos >= 1.0f))
		{
			lastMark++;
			marks[lastMark].enabled = true;
			SoundManager.instance.Play("ding");
		}
	}
}
