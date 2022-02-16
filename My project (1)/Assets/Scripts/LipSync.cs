
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LipSync : MonoBehaviour
{
    private float[] freqData;
    private int nSamples = 256;
    private int fMax = 24000;
    private AudioSource audio;
    
    
    public float BandVol(float fLow, float fHigh) { 
        fLow= Mathf.Clamp(fLow, 20, fMax); //a lower limit for frequencies
        fHigh = Mathf.Clamp(fHigh, fLow, fMax); //a higher limit for frequencies
        audio.GetSpectrumData(freqData, 0, FFTWindow.BlackmanHarris);//This function analyze audio data and calculates instantaneous volume of a range of freqeuncies
         int n1 = (int)Mathf.Floor(fLow * nSamples / fMax);
         int n2 = (int)Mathf.Floor(fHigh * nSamples / fMax);
        float sum = 0;
        for(int i = n1; i <= n2; i++)
        {
            sum+=freqData[i];
            
        }
        Debug.Log(sum);
        return sum / (n2 - n1 + 1);

        
    

    }

    public GameObject mouth;
 
    int volume=40;
    int frqLow = 300;
    int frqHigh = 700;
    private float y0;

    void Start()
    {
        audio=GetComponent<AudioSource>();
        
        y0 =mouth.transform.position.y;
     
        freqData = new float[nSamples];
        audio.Play();
    }

    
    private AudioClip[] audioSources;
    void PlaySoundN(int N)
    {
        audio.clip = audioSources[N];
        audio.Play();
    }

    //Implementing moving average filter

    private int sizeFilter = 8;
    private float[] filter;
    private float filterSum;
    private int posFilter = 0;
    private int qSamples = 0;

    public float MovingAverage(float sample)
    {
        if (qSamples == 0)
        {
            filter = new float[sizeFilter];
            
        }
        filterSum += sample - filter[posFilter];
        filter[posFilter++] = sample;
        
        if (posFilter > qSamples)
        {
            qSamples=posFilter;
            
           
        }
        posFilter = posFilter % sizeFilter;
        return filterSum / qSamples;
    }
    void Update()
    {
        
        Vector3 p = mouth.transform.position;
       
        mouth.transform.position = new Vector3(p.x, (y0 - MovingAverage(BandVol(frqLow, frqHigh)) * volume/70), p.z);
        
    }
}
