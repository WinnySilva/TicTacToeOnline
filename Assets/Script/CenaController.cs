using MLAPI;
using MLAPI.Configuration;
using MLAPI.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenaController : MonoBehaviour
{

    private void Awake()
    {


    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public static List<string> Cenas()
    {
        List<string> cenas;
        cenas = new List<string>();
        cenas.Add("SampleScene");
        cenas.Add("TitleScreen");
        return cenas;
    }

    public static void TrocarCenaServidor(string mySceneName)
    {
        NetworkSceneManager.SwitchScene(mySceneName);
    }

   
}
