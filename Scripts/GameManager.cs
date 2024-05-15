using Godot;
using System;
using System.Collections.Generic;


public partial class GameManager : Node
{

	public static List<PlayerInfo> Players = new List<PlayerInfo>();
	Node3D game;
	public override void _Ready()
	{
		Multiplayer.GetUniqueId();
	}

	public void StartGame(){
		game = GetNode<Node3D>("/root/Map");
	}
	public override void _Process(double delta)
	{
	}

	/*public void AutoAttack(Attack _attack){
		Rpc("Execute", _attack);
	}

	[Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
	void Execute(Attack attack){
		AttackProjectile instance = (AttackProjectile)attack.attackProjectile.Instantiate();
        game.AddChild(instance);
        instance.target = attack.target;
        instance.attack = attack;
        instance.GlobalPosition = attack.host.GlobalPosition + new Vector3(0,1,0);
	}*/
}