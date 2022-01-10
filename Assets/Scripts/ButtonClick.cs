using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonClick : MonoBehaviour
{
	public Button play;
	public Button info;
	public Button setting;

	void Start()
	{
		Button p = play.GetComponent<Button>();
		Button i = info.GetComponent<Button>();
		Button s = setting.GetComponent<Button>();
		p.onClick.AddListener(TaskOnClick_p);
		i.onClick.AddListener(TaskOnClick_i);
		s.onClick.AddListener(TaskOnClick_s);
	}

	void TaskOnClick_p()
	{
		// Debug.Log("You have clicked the button!");
		play.GetComponent<AudioSource>().Play();
	}
	void TaskOnClick_i()
	{
		info.GetComponent<AudioSource>().Play();
	}
	void TaskOnClick_s()
	{
		setting.GetComponent<AudioSource>().Play();
	}
}
