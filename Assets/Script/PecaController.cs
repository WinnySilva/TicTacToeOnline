using MLAPI;
using MLAPI.Messaging;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PecaController : NetworkBehaviour
{
    [SerializeField]
    private EnumPeca peca;
    private ulong clientId;
    public enum EnumPeca { NONE = 0, X = -1, O = 1 }


    public Vector2Int posicao;

    public GameObject none;
    public GameObject x;
    public GameObject o;


    public EnumPeca Peca
    {
        get { return peca; }
        set
        {
            peca = value;
            none.SetActive(false);
            x.SetActive(false);
            o.SetActive(false);
            switch (peca)
            {
                case EnumPeca.NONE:
                    none.SetActive(true);
                    break;
                case EnumPeca.O:
                    o.SetActive(true);
                    break;
                case EnumPeca.X:
                    x.SetActive(true);
                    break;
                default:
                    none.SetActive(true);
                    break;
            }
        }
    }

 
    // Start is called before the first frame update
    void Start()
    {
        clientId = NetworkManager.Singleton.LocalClientId;
    }

    // Update is called once per frame
    void Update()
    {

    }


    private void OnMouseDown()
    {
        JogadaEfetuadaClientRpc();
    }

    [ClientRpc]
    public void JogadaEfetuadaClientRpc()
    {
        Debug.Log("FazerMovimento");

        GameController.Instance.JogadaEfetuada(this, clientId);

    }


}
