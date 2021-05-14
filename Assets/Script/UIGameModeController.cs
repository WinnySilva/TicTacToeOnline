using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIGameModeController : MonoBehaviour
{

    public GameObject FinalDeJogoMsg;
    public TMPro.TMP_Text mensagemJogador;
    public TMPro.TMP_Text mensagemJogadorFinalJogo;
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

    public void OnNovoJogo(ulong clientId)
    {
        FinalDeJogoMsg.SetActive(false);
    }

    void OnClientDisconected(ulong clientId)
    {
        Desconectar();
    }

    public void OnFinalJogo(double vencedor, ulong clientId)
    {
        
        string msg;
        if (vencedor == clientId)
        {
            msg = "Você venceu";
        }
        else if( vencedor == -99)
        {
            msg = "Velhaaaa!!!";
        }
        else
        {
            msg = "Você perdeu";
        }
        mensagemJogadorFinalJogo.text = msg;
        FinalDeJogoMsg.SetActive(true);
    }

    public void MudancaDeTurno(bool minhaVez)
    {
        if (minhaVez)
        {
            this.mensagemJogador.text = "Sua vez de jogar";
            this.mensagemJogador.color = new Color(1,0,0);

        }
        else
        {
            this.mensagemJogador.text = "Aguarde sua vez de jogar";
            this.mensagemJogador.color = new Color(0,0,0);

        }
    }

}
