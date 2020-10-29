using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace GameServer
{
    class Client
    {
        public static int dataBufferSize = 4096;
        public int id;
        public TCP tCP;

        public Client (int _clientId){
            id = _clientId;
            tCP = new TCP(id);

        }

        public class TCP
        {
            public TcpClient socket;

            private readonly int id;
            private NetworkStream stream;
            private byte[] receiveBuffer;

            public TCP (int _id){
                id = _id;
            }
            public void Connect(TcpClient _socket){
                socket = _socket;
                socket.ReceiveBufferSize = dataBufferSize;
                socket.SendBufferSize = dataBufferSize;

                stream = socket.GetStream();
                receiveBuffer = new byte [dataBufferSize];
                stream.BeginRead(receiveBuffer,0,dataBufferSize,ReceiveCallback, null);
                //send welcome packet
            }
            private void ReceiveCallback (IAsyncResult _result){
                try
                {
                    int _byteLength = stream.EndRead(_result);
                    if(_byteLength <=0){
                        //disconnect
                        return;
                    }
                    byte[] _data = new byte[_byteLength];
                    Array.Copy(receiveBuffer,_data,_byteLength);
                    //handle data
                    stream.BeginRead(receiveBuffer,0,dataBufferSize,ReceiveCallback, null);
                }
                catch (Exception _ex)
                {
                    Console.WriteLine($"Error Receiving TCP Data: {_ex}");
                    //disconnect
                }
            }
        }
    }
}