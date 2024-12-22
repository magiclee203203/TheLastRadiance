using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Sirenix.OdinInspector;
using UnityEngine;

public class UDPReceiver : MonoBehaviour
{
    [SerializeField, Required] int _listenPort = 8888;
    [HideInInspector] public Action<string> UDPCallback;
    [HideInInspector] public bool UseSEMGControl;

    private UdpClient _udpClient;
    private IPEndPoint _remoteEndPoint;

    private void Start()
    {
        if (!UseSEMGControl) return;

        _udpClient = new UdpClient(_listenPort);
        _remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
    }

    private void Update()
    {
        if (!UseSEMGControl) return;

        if (_udpClient.Available <= 0) return;
        var receivedData = _udpClient.Receive(ref _remoteEndPoint);
        var receivedMessage = Encoding.UTF8.GetString(receivedData);

        // callback
        UDPCallback?.Invoke(receivedMessage);
    }
}