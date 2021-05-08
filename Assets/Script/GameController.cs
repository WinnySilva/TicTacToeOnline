using MLAPI;
using MLAPI.NetworkVariable;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : NetworkBehaviour
{
    public enum EtapaJogo { INICIO_JOGO, AGUARDA_JOGADOR_1, AGUARDA_JOGADOR_2, FINAL_JOGO, ATUALIZA_TABULEIRO }

    public NetworkVariable<ulong> id_jogador_atual;


    [SerializeField]
    private EtapaJogo etapaAtual;
    private EtapaJogo ultimoJogador;
    [SerializeField]
    private EtapaJogo EtapaAtual { get { return etapaAtual; } }
    public TabuleiroController tabuleiro;
    public ConexaoController conn;

    private void Awake()
    {
        if (!NetworkManager.Singleton.IsHost)
        {
            Destroy(this);
        }
        
    }

    // Start is called before the first frame update
    void Start()
    {
        etapaAtual = EtapaJogo.INICIO_JOGO;
        id_jogador_atual.OnValueChanged += OnChangeJogador;
    }

    // Update is called once per frame
    void Update()
    {
        switch (etapaAtual)
        {
            case EtapaJogo.INICIO_JOGO:
                Debug.Log("INICIO");
                InicioJogo();
                break;
            case EtapaJogo.AGUARDA_JOGADOR_1:
                this.id_jogador_atual = new NetworkVariable<ulong>(conn.Id_jogador_um);

                Debug.Log("AGUARDA_JOGADOR_1");
                AguardaJogadorUm();
                break;
            case EtapaJogo.AGUARDA_JOGADOR_2:
                this.id_jogador_atual = new NetworkVariable<ulong>(conn.Id_jogador_dois);

                Debug.Log("AGUARDA_JOGADOR_2");
                AguardaJogadorDois();
                break;
            case EtapaJogo.ATUALIZA_TABULEIRO:
                Debug.Log("ATUALIZA_TABULEIRO");
                AtualizaTabuleiro();
                break;
            case EtapaJogo.FINAL_JOGO:
                Debug.Log("FINAL_JOGO");
                break;
            default:
                break;
        }
    }
    
    private void InicioJogo()
    {
        //atualiza etapa
        etapaAtual = EtapaJogo.ATUALIZA_TABULEIRO;
    }

    private void AguardaJogadorUm()
    {

        //atualiza etapa
        etapaAtual = EtapaJogo.ATUALIZA_TABULEIRO;
        ultimoJogador = EtapaJogo.AGUARDA_JOGADOR_1;
    }
    private void AguardaJogadorDois()
    {

        //atualiza etapa
        etapaAtual = EtapaJogo.ATUALIZA_TABULEIRO;
        ultimoJogador = EtapaJogo.AGUARDA_JOGADOR_2;
    }

    private void AtualizaTabuleiro()
    {
        //atualiza etapa
        if (tabuleiro.ehFinalJogo())
        {
            etapaAtual = EtapaJogo.FINAL_JOGO;
            return;
        }
        // envia atualização do tabuleiro

        //atualiza etapa
        //proximo jogador
        etapaAtual = (ultimoJogador == EtapaJogo.AGUARDA_JOGADOR_1) ? EtapaJogo.AGUARDA_JOGADOR_2 : EtapaJogo.AGUARDA_JOGADOR_1;
    }

    private void FinalJogo()
    {

    }

    void  OnChangeJogador(ulong anterior, ulong novo)
    {
        Debug.Log("OnChangeJogador");
        
    }

}
