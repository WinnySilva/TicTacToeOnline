using MLAPI;
using MLAPI.SceneManagement;
using MLAPI.Spawning;
using MLAPI.Transports.UNET;
using UnityEngine;

    public class ConexaoController : NetworkBehaviour
    {

        public TMPro.TMP_InputField serverAddress;
        public GameObject menuConexao;
        public GameObject aguarde;
        public GameObject desconectar;

        private ulong id_jogador_um, id_jogador_dois;

    public ulong Id_jogador_um { get => id_jogador_um;  }
    public ulong Id_jogador_dois { get => id_jogador_dois; }

    public void Awake()
        {
            
        }

        public void Start()
        {
            NetworkManager.Singleton.NetworkConfig.EnableSceneManagement = true;
            
            NetworkManager.Singleton.NetworkConfig.RegisteredScenes = CenaController.Cenas();
        }

        public void IniciarHost()
        {
            NetworkManager.Singleton.OnServerStarted += OnServerStarted;
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnectedCallback;
            NetworkManager.Singleton.StartHost();        
        }

        public void IniciarClient()
        {
            var network = NetworkManager.Singleton;
            network.GetComponent<UNetTransport>().ConnectAddress = serverAddress.text;

            network.NetworkConfig.ConnectionData = System.Text.Encoding.ASCII.GetBytes("room password");         
            NetworkManager.Singleton.StartClient();
            menuConexao.SetActive(false);
            desconectar.SetActive(true);

            Debug.Log(NetworkManager.Singleton.LocalClientId);
        }

        public void ApprovalCheck(byte[] connectionData, ulong clientId, MLAPI.NetworkManager.ConnectionApprovedDelegate callback)
        {
            //Your logic here
            bool approve = true;
            bool createPlayerObject = true;

            // The prefab hash. Use null to use the default player prefab
            // If using this hash, replace "MyPrefabHashGenerator" with the name of a prefab added to the NetworkPrefabs field of your NetworkManager object in the scene
            ulong? prefabHash = NetworkSpawnManager.GetPrefabHashFromGenerator("player");

            //If approve is true, the connection gets added. If it's false. The client gets disconnected
            callback(createPlayerObject, prefabHash, approve, new Vector3(5,6,8), Quaternion.identity);

            Debug.Log("ApprovalCheck");
        }

        public void OnClientConnectedCallback(ulong clientId)
        {
            id_jogador_dois = clientId;
            Debug.Log("host player: "+ NetworkManager.Singleton.LocalClientId + " cliente player: "+clientId);
            CenaController.TrocarCenaServidor("SampleScene");

        }

        public void OnServerStarted()
        {
            menuConexao.SetActive(false);
            aguarde.SetActive(true);
            desconectar.SetActive(true);

            id_jogador_um = NetworkManager.Singleton.LocalClientId;
            Debug.Log(NetworkManager.Singleton.LocalClientId);
            Debug.Log("OnServerStarted");
        }

        public void Disconnect()
        {
            if (NetworkManager.Singleton.IsHost)
            {
                NetworkManager.Singleton.StopHost();
            }
            else if (NetworkManager.Singleton.IsClient)
            {
                NetworkManager.Singleton.StopClient();
            }
           
            UnityEngine.SceneManagement.SceneManager.LoadScene("TitleScreen");
        }

        public void EfetivarMovimento()
        {

        }

    }