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
    CanvasLayer canvasLayer;
    ItemList playerList;
    LineEdit nameBox;
    TextEdit warning;
    GameManager gameManager;


    public override void _Ready()
    {
        gameManager = GetNode<GameManager>("/root/GameManager");
        warning = GetNode<TextEdit>("CanvasLayer/WarningBox");
        nameBox = GetNode<LineEdit>("CanvasLayer/LineEdit");
        playerList = GetNode<ItemList>("CanvasLayer/PlayerList");
        canvasLayer = GetNode<CanvasLayer>("CanvasLayer");
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
        RpcId(1, "SendPlayerInformation", nameBox.Text, Multiplayer.GetUniqueId());
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
        SendPlayerInformation(nameBox.Text, 1);
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
        if (nameBox.Text.Length < 1){
            warning.Show();
        } else {
            HostGame();
        }
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
        if (nameBox.Text.Length < 1){
            warning.Show();
        } else {
            ConnectToServer();
        }

    }
    
    [Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
	private void StartGame(){
		var scene = ResourceLoader.Load<PackedScene>("res://Scenes/Map.tscn").Instantiate<Node3D>();
		GetTree().Root.AddChild(scene);
        canvasLayer.Hide();
        Hide();
		gameManager.StartGame();
	}

    [Rpc(MultiplayerApi.RpcMode.AnyPeer)]
    public void SendPlayerInformation(string name, int id){
		PlayerInfo playerInfo = new PlayerInfo(){
			Name = name,
			Id = id
		};

        playerList.AddItem(playerInfo.Name);

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
