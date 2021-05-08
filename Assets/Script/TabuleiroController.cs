using MLAPI.Messaging;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PecaController;

public class TabuleiroController : MonoBehaviour
{

    public static TabuleiroController Instance { get; private set; }
    public GameObject[] pecas;
    public EnumPeca[,] peca;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool ehFinalJogo()
    {
        return false;
    }

    [ClientRpc]
    public void UpdateTabuleiroClientRpc(EnumPeca[,] tabuleiro)
    {
        Debug.Log("UpdateTabuleiro");
    }


}
