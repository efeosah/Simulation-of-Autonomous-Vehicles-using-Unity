using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void onGenerated()
    {
        SceneManager.LoadScene("GeneratedRoad");
    }

    public void onTrack()
    {
        SceneManager.LoadScene("Map1");
    }

    public void onTrack2()
    {
        SceneManager.LoadScene("Map2");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
