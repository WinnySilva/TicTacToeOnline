using MLAPI;
using MLAPI.Messaging;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PecaController;

public class TabuleiroController : MonoBehaviour
{

    public static TabuleiroController Instance { get; private set; }
    public PecaController[] pecas;

    public static Action<EnumPeca[]> OnUpdateTabuleiro;


    private void Awake()
    {
        Instance = this;
    }

    public void UpdateTabuleiroLocal(EnumPeca[] tabuleiro)
    {
        UpdateTabuleiro(tabuleiro);
        OnUpdateTabuleiro?.Invoke(tabuleiro);
    }

    public void UpdateTabuleiro(EnumPeca[] tabuleiro)
    {
        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                int pos = x * 3 + y;
                pecas[pos].Peca = tabuleiro[pos];
            }
        }
       
    }

}
