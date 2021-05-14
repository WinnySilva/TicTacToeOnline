using MLAPI;
using MLAPI.Messaging;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetPlayer : NetworkBehaviour
{
    private NetworkObject netObj;
    public static Action<ulong> FinalJogoAction;

    private void Awake()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        netObj = GetComponent<NetworkObject>();
        if (IsServer)
        {
            TabuleiroController.OnUpdateTabuleiro += UpdateTabuleiroClientRpc;
           
        }

        GameController.OnFinalJogo +=FinalJogoClientRpc;
        GameController.OnNovoJogo += NovoJogoClientRpc;
        GameController.MudancaJogador += MudancaDeTurnoClientRpc;
    }

    private void OnDestroy()
    {
        TabuleiroController.OnUpdateTabuleiro -= UpdateTabuleiroClientRpc;
        GameController.OnFinalJogo -= FinalJogoClientRpc;
        GameController.OnNovoJogo -= NovoJogoClientRpc;
        GameController.MudancaJogador -= MudancaDeTurnoClientRpc;
    }

    [ClientRpc]
    public void UpdateTabuleiroClientRpc(PecaController.EnumPeca[] tabuleiro)
    {

        Debug.Log("UpdateTabuleiro");
        TabuleiroController.Instance.UpdateTabuleiro(tabuleiro);
    }

    [ClientRpc]
    public void FinalJogoClientRpc(double vencedor)
    {
        if (!IsOwner)
        {
            return;
        }

        UIGameModeController.Instance.OnFinalJogo(vencedor, this.OwnerClientId);

    }

    [ClientRpc]
    public void NovoJogoClientRpc(ulong clientId)
    {
        if (!IsOwner)
        {
            return;
        }

        UIGameModeController.Instance.OnNovoJogo(clientId);

    }

    [ClientRpc]
    public void MudancaDeTurnoClientRpc(ulong clientId)
    {
        if (!IsOwner)
        {
            return;
        }        
        UIGameModeController.Instance.MudancaDeTurno(clientId == this.OwnerClientId);
    }


    public void JogadaEfetuada(int x, int y)
    {
        if (!IsOwner)
        {
            return;
        }
        JogadaEfetuadaServerRpc(x, y);

    }

    public void NovoJogo()
    {
        if (!IsOwner)
        {
            return;
        }

        NovoJogoServerRpc();
    }


    [ServerRpc]
    private void NovoJogoServerRpc()
    {        
        GameController.Instance.NovoJogo(OwnerClientId);
    }

    [ServerRpc]
    private void JogadaEfetuadaServerRpc(int x, int y)
    {
        GameController.Instance.JogadaEfetuada(x, y, OwnerClientId);
    }

}
