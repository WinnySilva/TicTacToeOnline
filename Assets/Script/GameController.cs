using MLAPI;
using MLAPI.NetworkVariable;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PecaController;

public class GameController : MonoBehaviour

{

    public static GameController Instance { get; private set; }

    private EtapaJogo etapaAtual;
    private EtapaJogo ultimoJogador;
    private bool liberado;
    public EnumPeca[] Pecas { get; private set; }
    public enum EtapaJogo
    {
        INICIO_JOGO, AGUARDA_JOGADOR_1, AGUARDA_JOGADOR_2, FINAL_JOGO, ATUALIZA_TABULEIRO, NONE
    }
    public NetworkVariable<ulong> id_jogador_atual;
    public TabuleiroController tabuleiro;
    public ConexaoController conn;
    public EtapaJogo EtapaAtual { get { return etapaAtual; } }
    public static Action<ulong> OnFinalJogo;
    public static Action<ulong> OnNovoJogo;
    public static Action<ulong> MudancaJogador;

    private void Awake()
    {
        if (!NetworkManager.Singleton.IsHost)
        {
            Destroy(this);
            return;
        }


        Instance = this;
        conn = ConexaoController.Instance;

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

        switch (etapaAtual)
        {
            case EtapaJogo.INICIO_JOGO:
                Debug.Log("INICIO");
                InicioJogo();
                break;
            case EtapaJogo.AGUARDA_JOGADOR_1:
                //   Debug.Log("AGUARDA_JOGADOR_1");
                if (liberado)
                {
                    AguardaJogadorUm();
                }
                break;
            case EtapaJogo.AGUARDA_JOGADOR_2:
                //   Debug.Log("AGUARDA_JOGADOR_2");
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
            case EtapaJogo.NONE:
                
                break;
            default:
                break;
        }
    }

    private void InicioJogo()
    {

        Pecas = new EnumPeca[] {
             EnumPeca.NONE, EnumPeca.NONE, EnumPeca.NONE ,
             EnumPeca.NONE, EnumPeca.NONE, EnumPeca.NONE ,
             EnumPeca.NONE, EnumPeca.NONE, EnumPeca.NONE
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

        bool ehFinal = ehFinalJogo();

        //atualiza etapa
        if (ehFinal)
        {
            etapaAtual = EtapaJogo.FINAL_JOGO;
            
        }
        
        if (!ehFinal && ultimoJogador == EtapaJogo.AGUARDA_JOGADOR_1)
        {
            this.id_jogador_atual = new NetworkVariable<ulong>(conn.Id_jogador_dois);
            etapaAtual = EtapaJogo.AGUARDA_JOGADOR_2;
        }
        else if(!ehFinal)
        {
            this.id_jogador_atual = new NetworkVariable<ulong>(conn.Id_jogador_um);
            etapaAtual = EtapaJogo.AGUARDA_JOGADOR_1;
        }

        //atualiza etapa
        TabuleiroController.Instance.UpdateTabuleiroLocal(Pecas);
        MudancaJogador?.Invoke(this.id_jogador_atual.Value);
    }

    private void FinalJogo()
    {
        liberado = false;
        OnFinalJogo?.Invoke(id_jogador_atual.Value);
        etapaAtual = EtapaJogo.NONE;
    }

    void OnChangeJogador(ulong anterior, ulong novo)
    {
        Debug.Log("OnChangeJogador");

    }


    private bool ehFinalJogo()
    {
      
        int L0 = (int)Pecas[0] + (int)Pecas[1] + (int)Pecas[2];
        int L1 = (int)Pecas[3] + (int)Pecas[4] + (int)Pecas[5];
        int L2 = (int)Pecas[6] + (int)Pecas[7] + (int)Pecas[8];

        int C0 = (int)Pecas[0] + (int)Pecas[3] + (int)Pecas[6];
        int C1 = (int)Pecas[1] + (int)Pecas[4] + (int)Pecas[7];
        int C2 = (int)Pecas[2] + (int)Pecas[5] + (int)Pecas[8];

        int D0 = (int)Pecas[0] + (int)Pecas[4] + (int)Pecas[8];
        int D1 = (int)Pecas[2] + (int)Pecas[4] + (int)Pecas[6];
      
        List<int> val = new List<int>();

        val.Add(L0);
        val.Add(L1);
        val.Add(L2);
        val.Add(C0);
        val.Add(C1);
        val.Add(C2);
        val.Add(D0);
        val.Add(D1);

        List<EnumPeca> pc = new List<EnumPeca>(Pecas);
    
        return val.Contains(-3) || val.Contains(3) || !pc.Contains(EnumPeca.NONE);
    }

    public void JogadaEfetuada(int x, int y, ulong playerId)
    {
        Debug.Log(playerId + " X " + this.id_jogador_atual.Value);

        if (playerId != this.id_jogador_atual.Value)
        {
            return;
        }

        if (etapaAtual != EtapaJogo.AGUARDA_JOGADOR_1 && etapaAtual != EtapaJogo.AGUARDA_JOGADOR_2)
        {
            return;
        }
        int pos = x * 3 + y;
        if (etapaAtual == EtapaJogo.AGUARDA_JOGADOR_1)
        {
            Pecas[pos] = EnumPeca.X;

        }
        else if (etapaAtual == EtapaJogo.AGUARDA_JOGADOR_2)
        {
            Pecas[pos] = EnumPeca.O;
        }

        liberado = true;

    }

    public void NovoJogo(ulong clientId)
    {
        this.etapaAtual = EtapaJogo.INICIO_JOGO;
        OnNovoJogo?.Invoke(clientId);
    }


}
