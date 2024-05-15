using Godot;
using System;

public partial class EntityTester : Entity
{
    bool dires = true;
    Vector3 suunta;
    int count = 0;
    MeshInstance3D outline;
    MultiplayerSynchronizer sync;
    public override void _Ready()
    {
        sync = GetNode<MultiplayerSynchronizer>("MultiplayerSynchronizer");
        suunta = new Vector3(0,0,1);
        outline = GetNode<MeshInstance3D>("Model/Outline");
        outline.Hide();
        entityIndex = GetIndex();
    }

    public override void MpDie()
    {
        sync.QueueFree();
        Hide();
        Dispose();
    }

    void Switcher(){
        dires = !dires;
        if (dires){
            suunta = new Vector3(0,0,1);
        }else{
            suunta = new Vector3(0,0,-1);
        }
        count = 0;
    }

    public override void _Process(double delta)
    {
        count++;
        if (count >= 400){
            Switcher();
        }
        Velocity = suunta * speed;
        MoveAndSlide();
    }

    void OnAreaMouseEntered(){
        outline.Show();
    } 
    
    void OnAreaMouseExited(){
        outline.Hide();
    }
}
