using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using COVID_RUSH;

public class SpectrumHandle : MonoBehaviour
{
    public static float beatSpectrum =0; //beat spectrum
    public static float hitSpectrum =0;
    private float[] spectrumVal; //array to store spectrum data
    public int beatHz;
    public int hitHz;
    private EventStore m_eventStore = EventStore.instance;

    // Start is called before the first frame update
    void Start()
    {
        spectrumVal = new float [512];
        beatSpectrum = 0;
        hitSpectrum = 0;
    }

    // Update is called once per frame
    void Update()
    {
        AudioListener.GetSpectrumData(spectrumVal,0,FFTWindow.Hamming);
        if (spectrumVal!=null && spectrumVal.Length>0){
            beatSpectrum = spectrumVal[beatHz];
            hitSpectrum = spectrumVal[hitHz];
            // Debug.Log(beatSpectrum);
            // Debug.Log(hitSpectrum);
            m_eventStore.Notify("onUpdateSpectrum", this, spectrumVal);
        }
    }
}
