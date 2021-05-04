using MLAPI;
using UnityEngine;

namespace HelloWorld
{
    public class ConexaoController : MonoBehaviour
    {

        public TMPro.TMP_InputField serverAddress;

        public void IniciarHost()
        {
            NetworkManager.Singleton.StartHost();
        }

        public void IniciarClient()
        {
            var network = NetworkManager.Singleton;
           
            NetworkManager.Singleton.StartClient();
        }

    }
}