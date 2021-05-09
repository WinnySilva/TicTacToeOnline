using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIGameModeController : MonoBehaviour
{

    public GameObject FinalDeJogoMsg;
    private NetPlayer localPlayer;
    public static UIGameModeController Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        ConexaoController.ClientDisconected += OnClientDisconected;
        GameController.OnNovoJogo += OnNovoJogo;
        FinalDeJogoMsg.SetActive(false);
    }

    private void OnDestroy()
    {
        ConexaoController.ClientDisconected -= OnClientDisconected;
        GameController.OnNovoJogo -= OnNovoJogo;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Desconectar()
    {
        ConexaoController.Instance.Desconectar();
        SceneManager.LoadScene("TitleScreen");

    }

    public void NovoJogo()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject obj in players)
        {
            obj.GetComponent<NetPlayer>().NovoJogo();
        }

        FinalDeJogoMsg.SetActive(false);
    }

    void OnNovoJogo(ulong clientId)
    {
        FinalDeJogoMsg.SetActive(false);
    }

    void OnClientDisconected(ulong clientId)
    {
        Desconectar();
    }

    public void OnFinalJogo(ulong clientId)
    {
        FinalDeJogoMsg.SetActive(true);
    }

}
