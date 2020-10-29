using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace GameServer
{
    class Server
    {
        public static int MaxPlayer {get; private set;}
        public static int Port {get; private set;}
        private static TcpListener tcpListener;
        public static Dictionary<int,Client> clients = new Dictionary<int, Client>();

        public static void Start(int _maxplayer, int _port){
            MaxPlayer = _maxplayer;
            Port = _port;
            Console.WriteLine("Starting Server...");
            InitializeServerData();
            tcpListener = new TcpListener (IPAddress.Any, Port);
            tcpListener.Start();
            tcpListener.BeginAcceptTcpClient(new AsyncCallback(TCPConnectCallback),null);
            Console.WriteLine($"Server Started on {Port}");

        }
        private static void TCPConnectCallback(IAsyncResult _result){
            TcpClient _client = tcpListener.EndAcceptTcpClient(_result);
            tcpListener.BeginAcceptTcpClient(new AsyncCallback(TCPConnectCallback),null);
            Console.WriteLine($"incoming connection from {_client.Client.RemoteEndPoint}...");
            for (int i =1; i<=MaxPlayer;i++){
                if(clients[1].tCP.socket == null){
                    clients[1].tCP.Connect(_client);
                    return;
                }
            }
            Console.WriteLine($"{_client.Client.RemoteEndPoint} falied to connect : server full!");
        }
        private static void InitializeServerData(){
            for (int i =1; i<=MaxPlayer;i++){
                clients.Add(i,new Client(1));
            }
        }
    }
}