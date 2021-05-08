using MLAPI;
using MLAPI.Messaging;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetPlayer : NetworkBehaviour
{
    private NetworkObject netObj;

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

    }


    [ClientRpc]
    public void UpdateTabuleiroClientRpc(PecaController.EnumPeca[] tabuleiro)
    {
        Debug.Log("UpdateTabuleiro");
        TabuleiroController.Instance.UpdateTabuleiro(tabuleiro);
    }

    [ServerRpc]
    private void JogadaEfetuadaServerRpc(int x, int y)
    {
        GameController.Instance.JogadaEfetuada(x, y, OwnerClientId);
    }

    public void JogadaEfetuada(int x, int y)
    {
        if (!IsOwner)
        {
            return;
        } 
        JogadaEfetuadaServerRpc(x, y);

    }

}
