using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpectrumBeat : MonoBehaviour
{
    public float bias;
	public float timeStep;
	public float timeToBeat;
	public float restSmoothTime;

	[SerializeField] public Material Group1;
	[SerializeField] public Material Group2;

	private float m_previousAudioValue;
	private float m_audioValue=0;
	private float m_timer;

	protected bool m_isBeat;
	private bool switchFalg = false;		//轉換用的flag

	private Color green = new Color(0f,1f,0f,1f);
	private Color red = new Color(1f,0f,0f,1f);
	
    // Start is called before the first frame update

	
    public virtual void OnBeat()
	{
		
		m_timer = 0;
		m_isBeat = true;
		switchFalg = !switchFalg;
		Debug.Log(switchFalg);
		if (switchFalg){
			Group1.SetFloat("_Metallic",0.25f);
			Group2.SetFloat("_Metallic",0.25f);
		}
		else{
			Group1.SetFloat("_Metallic",1f);
			Group2.SetFloat("_Metallic",1f);
		}
	}

    public virtual void OnUpdate()
	{ 
		// update audio value
		m_previousAudioValue = m_audioValue;
		m_audioValue = SpectrumHandle.beatSpectrum;

		// if audio value went below the bias during this frame
		if (m_previousAudioValue > bias &&
			m_audioValue <= bias)
		{
			// if minimum beat interval is reached
			if (m_timer > timeStep)
				OnBeat();
		}

		// if audio value went above the bias during this frame
		if (m_previousAudioValue <= bias &&
			m_audioValue > bias)
		{
			// if minimum beat interval is reached
			if (m_timer > timeStep)
				OnBeat();
		}

		m_timer += Time.deltaTime;
		if (m_isBeat) return;
	}
    void Start()
    {
        Group1.color = green;
		Group2.color = red;
    }

    // Update is called once per frame
    void Update()
    {
        OnUpdate();

		//Debug.Log(m_audioValue);
    }
}
