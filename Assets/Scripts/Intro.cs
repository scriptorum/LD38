using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Spewnity;

public class Intro : MonoBehaviour 
{
	private float fadeTime = 1.5f;	
	public string nextScene;
	public SpriteRenderer fadeCurtain;
	public UnityEngine.UI.Button button;      


	public void startGame()
	{
		button.interactable = false;
		SoundManager.instance.Play("click");

		// WTF, Color.Lerp is backwards? Durp!
		StartCoroutine(Color.clear.LerpColor(Color.white, fadeTime, (c) => {
				fadeCurtain.color = c;
			}
		));

		ActionQueue aq = gameObject.AddComponent<ActionQueue>();
		aq.Delay(fadeTime);
		aq.Add(() => {
			SceneManager.LoadScene(nextScene);
		});
		aq.Run();
	}
}
