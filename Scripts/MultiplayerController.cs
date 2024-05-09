using Godot;
using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

public partial class MultiplayerController : Control
{
    [Export]
    private int _port = 8910;
    [Export]
    private string _ip = "localhost";
    [Export]
    private int _maxPlayerCount = 2;
    private ENetMultiplayerPeer _peer;


    public override void _Ready()
    {
        Multiplayer.PeerConnected += PlayerConnected;
        Multiplayer.PeerDisconnected += PlayerDisconnected;
        Multiplayer.ConnectedToServer += ConnectedToServer;
        Multiplayer.ConnectionFailed += ConnectionFailed;
    }

    private void ConnectionFailed()
    {
        GD.Print("Connection FAILED.");
        GD.Print("Could not connect to server.");
    }
    
    private void ConnectedToServer()
    {
        GD.Print("Connection SUCCESSFULL.");

        int playerId = Multiplayer.GetUniqueId();
        RpcId(1, "SendPlayerInformation", GetNode<LineEdit>("LineEdit").Text, Multiplayer.GetUniqueId());
    }


    private void HostGame()
    {
        _peer = new ENetMultiplayerPeer();
        var status = _peer.CreateServer(_port, _maxPlayerCount);
        if (status != Error.Ok)
        {
            GD.Print("Server could not be created:");
            GD.Print($"Port: {_port}");
            return;
        }

        _peer.Host.Compress(ENetConnection.CompressionMode.RangeCoder);
        Multiplayer.MultiplayerPeer = _peer;
        GD.Print("Waiting for players to connect ...");
        SendPlayerInformation(GetNode<LineEdit>("LineEdit").Text, 1);
    }

    private void PlayerConnected(long id)
    {
        GD.Print($"HOST: Player <{id}> connected.");
        
    }

    private void PlayerDisconnected(long id)
    {
        GD.Print($"HOST: Player <{id}> disconected.");
    }

    public void OnHostButtonDown(){
        HostGame();
    }
    
    void OnStartButtonDown(){
        Rpc("StartGame");
    }
    public void ConnectToServer()
    {
        _peer = new ENetMultiplayerPeer();
        var status = _peer.CreateClient(_ip, _port);
        if (status != Error.Ok)
        {
            GD.Print("Creating client FAILED.");
            return;
        }

        _peer.Host.Compress(ENetConnection.CompressionMode.RangeCoder);
        Multiplayer.MultiplayerPeer = _peer;
    }

    public void OnJoinButtonDown()
    {
        ConnectToServer();
    }
    
    [Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
	private void StartGame(){
		var scene = ResourceLoader.Load<PackedScene>("res://Scenes/Map.tscn").Instantiate<Node3D>();
		GetTree().Root.AddChild(scene);
        this.Hide();
		
	}

    [Rpc(MultiplayerApi.RpcMode.AnyPeer)]
    public void SendPlayerInformation(string name, int id){
		PlayerInfo playerInfo = new PlayerInfo(){
			Name = name,
			Id = id
		};

        if(!GameManager.Players.Contains(playerInfo)){
			
			GameManager.Players.Add(playerInfo);
            GD.Print(playerInfo.Id, " ", playerInfo.Name, "  Sent and got");
			
		}

        if(Multiplayer.IsServer()){
			foreach (var item in GameManager.Players)
			{
				Rpc("SendPlayerInformation", item.Name, item.Id);
			}
		}

	}
    
}
