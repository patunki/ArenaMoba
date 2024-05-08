using Godot;
using System;

public partial class EntityTester : Entity
{
    bool dires = true;
    Vector3 suunta;
    int count = 0;
    public override void _Ready()
    {
        suunta = new Vector3(0,0,1);
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
        if (count >= 200){
            Switcher();
        }
        Velocity = suunta * speed;
        MoveAndSlide();
    }
}
