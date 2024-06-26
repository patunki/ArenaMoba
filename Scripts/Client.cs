using Godot;
using System;

public partial class Client : Node3D
{
    public string name;
    public int id;
    public Player player;
    public void SetUpPlayer(string _name){
        name = _name;
	}

    public override void _Ready()
    {
        player = GetNode<Player>("Player");
        GetNode<MultiplayerSynchronizer>("MultiplayerSynchronizer").SetMultiplayerAuthority(int.Parse(Name));
        if(GetNode<MultiplayerSynchronizer>("MultiplayerSynchronizer").GetMultiplayerAuthority() == Multiplayer.GetUniqueId()){
            GetNode<Label>("Interface/Label").Text = name;
            player.Auth = true;
            player.camera.Current = true;
        } else {
            player.camera.Current = false;
            player.Auth = false;
        }
    }

    public override void _Process(double delta)
    {
        if(player.Auth == true){
            player.FrameCall(delta);
        }
        
    }
}
