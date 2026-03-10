using UnityEngine;
using NativeWebSocket;
using System;
using System.Threading.Tasks;
using SoundLab.Core;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SoundLab.Tangible
{
    public struct TangibleMessage
    {
        public string type;
        public string action;
    }

    public class TangibleController : MonoBehaviour
    {
        public static TangibleController Instance;

        private WebSocket websocket;

        [Header("Connection")]
        public string serverIP = "10.204.0.49";
        public int serverPort = 8081;
        public bool autoReconnect = true;
        public float reconnectDelay = 3f;

        [Range(0, 255)]
        public int ledIntensity = 0;

        public bool IsConnected => websocket != null && websocket.State == WebSocketState.Open;

        public event Action OnConnected;
        public event Action OnDisconnected;
        public event Action<TangibleMessage> OnMessageReceived;

        private bool isConnecting = false;

        #region Unity Lifecycle

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        async void Start()
        {
            await Connect();
        }

        void Update()
        {
    #if !UNITY_WEBGL || UNITY_EDITOR
            websocket?.DispatchMessageQueue();
    #endif
        }

        async void OnDestroy()
        {
            if (websocket != null)
                await websocket.Close();
        }

        #endregion

        #region Connection

        public async Task Connect()
        {
            if (isConnecting) return;

            isConnecting = true;

            websocket = new WebSocket($"ws://{serverIP}:{serverPort}");

            websocket.OnOpen += async () =>
            {
                Debug.Log("WebSocket Connected");
                isConnecting = false;

                OnConnected?.Invoke();

                string UUID = SystemInfo.deviceUniqueIdentifier;
                await websocket.SendText("Device (Unity): " + SystemInfo.deviceName +
                                        " | UUID: " + UUID);
            };

            websocket.OnMessage += (bytes) =>
            {
                string message = System.Text.Encoding.UTF8.GetString(bytes);
                Debug.Log("Received: " + message);
                IncomingMessageParser(message);
            };

            websocket.OnClose += (code) =>
            {
                Debug.Log("WebSocket Closed");
                OnDisconnected?.Invoke();

                if (autoReconnect)
                    Invoke(nameof(Reconnect), reconnectDelay);
            };

            websocket.OnError += (err) =>
            {
                Debug.LogError("WebSocket Error: " + err);
            };

            await websocket.Connect();
        }

        private async void Reconnect()
        {
            Debug.Log("Reconnecting...");
            await Connect();
        }

        #endregion

        #region Send Methods

        private async Task SafeSend(string message)
        {
            if (IsConnected)
            {
                await websocket.SendText(message);
                Debug.Log("Sent: " + message);
            }
            else
            {
                Debug.LogWarning("WebSocket not connected. Message not sent: " + message);
            }
        }

        public async void SendHello()
        {
            await SafeSend("Hello from Unity");
        }

        public async void SendLedON()
        {
            await SafeSend("button:1");
        }

        public async void SendLedOFF()
        {
            await SafeSend("button:0");
        }

        public async void SendLedIntensity()
        {
            await SafeSend("LED_INTENSITY:" + ledIntensity);
        }

        #endregion

        #region Incoming Messages

        public void IncomingMessageParser(string msg)
        {
            if (!msg.Contains(":")) return;

            string key         = msg.Substring(0, msg.IndexOf(":")).Trim();
            string valueParsed = msg.Substring(msg.IndexOf(":") + 1).Trim();

            if (key == "force_plate1")
            {
                float receivedValue = float.Parse(valueParsed);
                forcePlateMessage(receivedValue);
            }
            else if (key == "sunrise")
            {
                OnMessageReceived?.Invoke(new TangibleMessage { type = "sunrise", action = valueParsed });

                if (valueParsed == "rise") TriggerSunrise();
                else if (valueParsed == "set") TriggerSunset();
                else if (valueParsed == "reset") ResetSunrise();
            }
        }

        public void forcePlateMessage(float value)
        {
            if (GameController.Instance.Instrument != null)
            GameController.Instance.Instrument.BendNote(value);
        }

        public void TriggerSunrise()
        {
            OnMessageReceived?.Invoke(new TangibleMessage { type = "sunrise", action = "start" });
        }

        public void TriggerSunset()
        {
            OnMessageReceived?.Invoke(new TangibleMessage { type = "sunrise", action = "sunset" });
        }

        public void ResetSunrise()
        {
            OnMessageReceived?.Invoke(new TangibleMessage { type = "sunrise", action = "reset" });
        }

        #endregion
    }
}
