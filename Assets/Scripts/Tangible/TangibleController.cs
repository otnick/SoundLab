using UnityEngine;
using NativeWebSocket;

namespace SoundLab.Tangible
{
    public class TangibleController : MonoBehaviour
    {
        [SerializeField] private string _url = "ws://XXXX:XXXX";  // set this to your ip
        
        private WebSocket _socket;

        public async void Connect()
        {
            _socket = new WebSocket(_url);
            
            _socket.OnOpen += () => Debug.Log("[Tangible] Connected");
            _socket.OnClose += _ => Debug.Log("[Tangible] Disconnected");
            _socket.OnError += e => Debug.LogWarning($"[Tangible] Error: {e}");
            _socket.OnMessage += bytes => Debug.Log($"[Tangible] Message: {System.Text.Encoding.UTF8.GetString(bytes)}");
            
            await _socket.Connect();
        }

        public async void Disconnect()
        {
            if (_socket != null) await _socket.Close();
        }

        private void Update()
        {
            _socket?.DispatchMessageQueue();
        }

        private void OnDestroy()
        {
            Disconnect();
        }
    }
}
