using MLAPI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PecaController : NetworkBehaviour
{
    [SerializeField]
    private EnumPeca peca;
    public enum EnumPeca { NONE, X, O }


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

    }

    // Update is called once per frame
    void Update()
    {

    }


    private void OnMouseDown()
    {

        Debug.Log("x:" + posicao.x + " y:" + posicao.y);
        Debug.Log("Player " + NetworkManager.Singleton.LocalClientId);
        if (NetworkManager.Singleton.IsHost)
        {
            this.Peca = EnumPeca.X;
        }
        else
        {
            this.Peca = EnumPeca.O;
        }

    }

}
