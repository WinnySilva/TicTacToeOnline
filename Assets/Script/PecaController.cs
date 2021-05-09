using MLAPI;
using MLAPI.Messaging;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PecaController : MonoBehaviour
{
    [SerializeField]
    private EnumPeca peca;
    private GameObject[] players;
    private BoxCollider boxColl;

    public enum EnumPeca { NONE = 0, X = -1, O = 1 }


    public Vector2Int posicao;

    public GameObject none;
    public GameObject x;
    public GameObject o;


    public EnumPeca Peca
    {
        get { return peca; }
        
    }

    public void setPeca(EnumPeca peca)
    {
        this.peca = peca;
        none.SetActive(false);
        x.SetActive(false);
        o.SetActive(false);
       
        switch (peca)
        {
            case EnumPeca.NONE:
                none.SetActive(true);
                boxColl.enabled = true;
                break;
            case EnumPeca.O:
                o.SetActive(true);
                boxColl.enabled = false;
                break;
            case EnumPeca.X:
                x.SetActive(true);
                boxColl.enabled = false;
                break;
            default:
                none.SetActive(true);
                break;
        }
    }

    private void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        boxColl = GetComponent<BoxCollider>();
    }

    // Start is called before the first frame update

    private void OnMouseDown()
    {
        foreach (GameObject obj in players)
        {
            obj.GetComponent<NetPlayer>().JogadaEfetuada(posicao.x, posicao.y);
        }
    }

}
