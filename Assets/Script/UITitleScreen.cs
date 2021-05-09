using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITitleScreen : MonoBehaviour
{

    public GameObject menuConexao;
    public GameObject aguarde;
    public GameObject iniciandoHost;
    public GameObject desconectarBtn;
    public TMPro.TMP_InputField serverAddressTxt;       

    // Start is called before the first frame update
    void Start()
    {
        ConexaoController.ClientConected += ClientConected;
        ConexaoController.ServerStarted += ServerIniciado;
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDestroy()
    {
        ConexaoController.ClientConected -= ClientConected;
        ConexaoController.ServerStarted -= ServerIniciado;
    }

    public void IniciarHost()
    {
        ConexaoController.Instance.IniciarHost();
        //      menuConexao.SetActive(false);
        //       iniciandoHost.SetActive(true);
        //aguarde.SetActive(false);
        //      desconectarBtn.SetActive(false);
    }
    public void IniciarCliente()
    {
        ConexaoController.Instance.IniciarClient(serverAddressTxt.text);
        menuConexao.SetActive(false);
        iniciandoHost.SetActive(false);
        aguarde.SetActive(true);
        desconectarBtn.SetActive(true);
    }



    public void Desconectar()
    {
        ConexaoController.Instance.Desconectar();
        menuConexao.SetActive(true);
        iniciandoHost.SetActive(false);
        aguarde.SetActive(false);
        desconectarBtn.SetActive(false);
    }


    private void ClientConected(ulong clientId)
    {
        if (ConexaoController.Instance.ClientesConectados() == 2)
        {
            ConexaoController.Instance.SwitchScene("SampleScene");
        }

    }
    private void ServerIniciado()
    {
        menuConexao.SetActive(false);
        iniciandoHost.SetActive(false);
        aguarde.SetActive(true);
        desconectarBtn.SetActive(true);

    }

}
