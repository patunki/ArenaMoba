using Godot;
using System;

public partial class Client : Node3D
{
    public Button upButton;

    public override void _Ready()
    {
        upButton = GetNode<Button>("Interface/UpButton");
    }
}
