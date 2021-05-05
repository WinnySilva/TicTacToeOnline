using MLAPI;
using MLAPI.Transports.UNET;
using UnityEngine;

namespace HelloWorld
{
    public class ConexaoController : MonoBehaviour
    {

        public TMPro.TMP_InputField serverAddress;
        public GameObject menuConexao;

        public void IniciarHost()
        {
            NetworkManager.Singleton.StartHost();
            menuConexao.SetActive(false);
        }

        public void IniciarClient()
        {
            var network = NetworkManager.Singleton;
            network.GetComponent<UNetTransport>().ConnectAddress = serverAddress.text;

            NetworkManager.Singleton.StartClient();
            menuConexao.SetActive(false);
        }

    }
}