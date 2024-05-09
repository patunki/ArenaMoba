using Godot;
using System;

public partial class EntityTester : Entity
{
    bool dires = true;
    Vector3 suunta;
    int count = 0;
    MeshInstance3D outline;
    public override void _Ready()
    {
        suunta = new Vector3(0,0,1);
        outline = GetNode<MeshInstance3D>("Model/Outline");
        GD.Print(outline);
        outline.Hide();
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
