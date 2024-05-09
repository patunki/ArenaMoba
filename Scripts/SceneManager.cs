using Godot;
using System;
using System.Data.Common;


public partial class SceneManager : Node3D
{
	[Export]
	public PackedScene playerScene;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        playerScene = GD.Load<PackedScene>("res://Scenes/Client.tscn");
		int index = 0;
		foreach (var item in GameManager.Players)
		{
			Client currentPlayer = playerScene.Instantiate<Client>();
			AddChild(currentPlayer);
			foreach (Node3D spawnPoint in GetTree().GetNodesInGroup("PlayerSpawnPoints"))
			{
				if(int.Parse(spawnPoint.Name) == index){
					currentPlayer.GlobalPosition = spawnPoint.GlobalPosition;
				}
			}
			index ++;
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}