using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void SelectScene(int sceneNum){
        SceneManager.LoadScene(sceneNum, LoadSceneMode.Single);
    }
    public void TestClick(){
        Debug.Log("HELLO");
    }
}