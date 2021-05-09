using MLAPI;
using MLAPI.SceneManagement;
using MLAPI.Spawning;
using MLAPI.Transports.PhotonRealtime;
using MLAPI.Transports.UNET;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ConexaoController : NetworkBehaviour
{

    public static ConexaoController Instance { get; private set; }
    public static Action<ulong> ClientConected;
    public static Action<ulong> ClientDisconected;
    public static Action ServerStarted;
    

    public UNetTransport uNetTransport;
    public PhotonRealtimeTransport photonTransport;
    public bool EhLocal { get; private set; }

    private ulong id_jogador_um, id_jogador_dois;
    private NetPlayer localPlayer;
    public ulong Id_jogador_um { get => id_jogador_um; }
    public ulong Id_jogador_dois { get => id_jogador_dois; }

    public void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
        
        NetworkManager.Singleton.NetworkConfig.EnableSceneManagement = true;
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnectCallback;
    }

    public void IniciarHost(string connectAddress)
    {
        NetworkManager.Singleton.OnServerStarted += OnServerStarted;
        NetworkManager.Singleton.StartHost();
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnectedCallback;
        photonTransport.RoomName = connectAddress;
    }

    public void IniciarClient(string connectAddress)
    {
        var network = NetworkManager.Singleton;

        uNetTransport.ConnectAddress = connectAddress;
        photonTransport.RoomName = connectAddress;

        NetworkManager.Singleton.StartClient();

    }

    public void OnClientConnectedCallback(ulong clientId)
    {
        id_jogador_dois = clientId;
        ClientConected?.Invoke(clientId);
    }

    public void OnClientDisconnectCallback(ulong clientId)
    {
        id_jogador_dois = 99999;
        ClientDisconected?.Invoke(clientId);
    }

    public void OnServerStarted()
    {
        id_jogador_um = NetworkManager.Singleton.LocalClientId;
        ServerStarted?.Invoke();
    }

    public void Desconectar()
    {
        if (NetworkManager.Singleton.IsHost)
        {
            NetworkManager.Singleton.DisconnectClient(id_jogador_dois);
            NetworkManager.Singleton.StopHost();
        }
        else if (NetworkManager.Singleton.IsClient)
        {
            NetworkManager.Singleton.StopClient();
        }
    }

    public void SwitchScene(string mySceneName)
    {
        NetworkSceneManager.SwitchScene(mySceneName);
    }

    public int ClientesConectados()
    {
        return NetworkManager.Singleton.ConnectedClientsList.Count;
    }

    public void ControleTransporte(bool ehLocal)
    {
        EhLocal = ehLocal;

        if (EhLocal)
        {
            NetworkManager.Singleton.NetworkConfig.NetworkTransport = this.uNetTransport;
        }
        else
        {
            NetworkManager.Singleton.NetworkConfig.NetworkTransport = this.photonTransport;
        }

    }
 

}