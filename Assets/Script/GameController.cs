using MLAPI;
using MLAPI.NetworkVariable;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PecaController;

public class GameController : NetworkBehaviour

{

    public static GameController Instance { get; private set; }

    private EtapaJogo etapaAtual;
    private EtapaJogo ultimoJogador;
    private bool liberado;
    public EnumPeca[,] Pecas { get; private set; }
    public enum EtapaJogo
    {
        INICIO_JOGO, AGUARDA_JOGADOR_1, AGUARDA_JOGADOR_2, FINAL_JOGO, ATUALIZA_TABULEIRO
    }
    public NetworkVariable<ulong> id_jogador_atual;
    public TabuleiroController tabuleiro;
    public ConexaoController conn;
    public EtapaJogo EtapaAtual { get { return etapaAtual; } }

    private void Awake()
    {
        

        Instance = this;

        GameObject obj = GameObject.FindGameObjectWithTag("__controller");
        if (obj == null)
        {
            Destroy(this);
        }

        conn = obj.GetComponent<ConexaoController>();

        liberado = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        etapaAtual = EtapaJogo.INICIO_JOGO;
        id_jogador_atual.OnValueChanged += OnChangeJogador;
    }

    private void OnDestroy()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!NetworkManager.Singleton.IsHost)
        {
            return;
        }
        switch (etapaAtual)
        {
            case EtapaJogo.INICIO_JOGO:
                Debug.Log("INICIO");
                InicioJogo();
                break;
            case EtapaJogo.AGUARDA_JOGADOR_1:
                Debug.Log("AGUARDA_JOGADOR_1");
                if (liberado)
                {
                    AguardaJogadorUm();
                }
                break;
            case EtapaJogo.AGUARDA_JOGADOR_2:
                Debug.Log("AGUARDA_JOGADOR_2");
                if (liberado)
                {
                    AguardaJogadorDois();
                }
                break;
            case EtapaJogo.ATUALIZA_TABULEIRO:
                Debug.Log("ATUALIZA_TABULEIRO");
                AtualizaTabuleiro();
                break;
            case EtapaJogo.FINAL_JOGO:
                Debug.Log("FINAL_JOGO");
                FinalJogo();
                break;
            default:
                break;
        }
    }

    private void InicioJogo()
    {
        Pecas = new EnumPeca[,]
       {
            { EnumPeca.NONE, EnumPeca.NONE, EnumPeca.NONE },
            { EnumPeca.NONE, EnumPeca.NONE, EnumPeca.NONE },
            { EnumPeca.NONE, EnumPeca.NONE, EnumPeca.NONE }
       };

        //atualiza etapa
        etapaAtual = EtapaJogo.ATUALIZA_TABULEIRO;
    }

    private void AguardaJogadorUm()
    {
        liberado = false;
        //atualiza etapa
        etapaAtual = EtapaJogo.ATUALIZA_TABULEIRO;
        ultimoJogador = EtapaJogo.AGUARDA_JOGADOR_1;

    }
    private void AguardaJogadorDois()
    {
        liberado = false;
        //atualiza etapa
        etapaAtual = EtapaJogo.ATUALIZA_TABULEIRO;
        ultimoJogador = EtapaJogo.AGUARDA_JOGADOR_2;

    }

    private void AtualizaTabuleiro()
    {
        liberado = false;

        //atualiza etapa
        if (tabuleiro.ehFinalJogo())
        {
            etapaAtual = EtapaJogo.FINAL_JOGO;
            return;
        }
        
        //atualiza etapa
        //proximo jogador
        if (ultimoJogador == EtapaJogo.AGUARDA_JOGADOR_1)
        {
            this.id_jogador_atual = new NetworkVariable<ulong>(conn.Id_jogador_dois);
            etapaAtual = EtapaJogo.AGUARDA_JOGADOR_2;
        }
        else
        {
            this.id_jogador_atual = new NetworkVariable<ulong>(conn.Id_jogador_um);
            etapaAtual = EtapaJogo.AGUARDA_JOGADOR_1;
        }
        UpdateTabuleiro();

    }

    private void FinalJogo()
    {
        liberado = false;

    }

    void OnChangeJogador(ulong anterior, ulong novo)
    {
        Debug.Log("OnChangeJogador");

    }

    public void JogadaEfetuada(PecaController peca, ulong playerId)
    {
        Debug.Log("JogadaEfetuada "+ playerId+" jogador atual: "+this.id_jogador_atual);
       
        liberado = true;
    }

    public void UpdateTabuleiro()
    {
        TabuleiroController.Instance.UpdateTabuleiroClientRpc(Pecas);
    }

}
