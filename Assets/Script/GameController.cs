using MLAPI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public enum EtapaJogo {INICIO_JOGO, AGUARDA_JOGADOR_1, AGUARDA_JOGADOR_2, FINAL_JOGO}

    [SerializeField]
    private EtapaJogo etapaAtual;

    // Start is called before the first frame update
    void Start()
    {
        etapaAtual = EtapaJogo.INICIO_JOGO;
    }

    // Update is called once per frame
    void Update()
    {
        switch (etapaAtual)
        {
            case EtapaJogo.INICIO_JOGO:
                Debug.Log("INICIO");
                break;
            case EtapaJogo.AGUARDA_JOGADOR_1:
                Debug.Log("AGUARDA_JOGADOR_1");
                break;
            case EtapaJogo.AGUARDA_JOGADOR_2:
                Debug.Log("AGUARDA_JOGADOR_2");
                break;
            case EtapaJogo.FINAL_JOGO:
                Debug.Log("FINAL_JOGO");
                break;
            default:
                break;
        }
    }

    private bool isHost()
    {
        return NetworkManager.Singleton.IsHost;
    }


}
