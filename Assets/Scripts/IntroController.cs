using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroController : MonoBehaviour
{
    public GameObject Intro;
    public GameObject Canvas;
    public GameObject Terrain;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("Begin", 7);
    }

    // Update is called once per frame
    void Begin()
    {
        Intro.SetActive(false);
        Canvas.SetActive(true);
        Terrain.SetActive(true);
    }
}
