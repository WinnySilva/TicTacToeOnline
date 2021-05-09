using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TMPro.TMP_Dropdown;

public class UITitleScreen : MonoBehaviour
{

    public GameObject menuConexao;
    public GameObject aguarde;
    public GameObject iniciandoHost;
    public GameObject desconectarBtn;
    public TMPro.TMP_InputField serverAddressTxt;
    public TMPro.TMP_InputField roomNameTxt;

    public TMPro.TMP_Dropdown modoTransportCmb;

    // Start is called before the first frame update
    void Start()
    {
        ConexaoController.ClientConected += ClientConected;
        ConexaoController.ServerStarted += ServerIniciado;
        ConexaoController.Instance.ControleTransporte(true);
        serverAddressTxt.gameObject.SetActive(true);
        this.roomNameTxt.gameObject.SetActive(false);
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
        ConexaoController.Instance.IniciarHost(serverAddressTxt.text);
        //      menuConexao.SetActive(false);
        //       iniciandoHost.SetActive(true);
        //aguarde.SetActive(false);
        //      desconectarBtn.SetActive(false);
    }
    public void IniciarCliente()
    {
        if (ConexaoController.Instance.EhLocal)
        {
            ConexaoController.Instance.IniciarClient(serverAddressTxt.text);
        }
        else
        {
            ConexaoController.Instance.IniciarClient(roomNameTxt.text);
        }

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

    public void OnModoTransportChange(Int32 val)
    {
        Debug.Log("ModoTransport " + modoTransportCmb.options[modoTransportCmb.value].text);
        if (modoTransportCmb.value == 0)
        {
            ConexaoController.Instance.ControleTransporte(true);
            serverAddressTxt.gameObject.SetActive(true);
            this.roomNameTxt.gameObject.SetActive(false);
        }
        else if (modoTransportCmb.value == 1)
        {
            serverAddressTxt.gameObject.SetActive(false);
            this.roomNameTxt.gameObject.SetActive(true);
            ConexaoController.Instance.ControleTransporte(false);
        }
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
