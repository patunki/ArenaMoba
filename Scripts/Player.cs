using Godot;
using System;

public partial class Player : Entity
{
    Plane dropPlane;
    MeshInstance3D cursor;
    Camera3D camera;
    NavigationAgent3D navigator;
    Vector3 cursorPos;

    public override void _Ready()
    {
        dropPlane = new Plane(new Vector3(0,1,0));
        cursor = GetNode<MeshInstance3D>("UI/Cursor");
        camera = GetNode<Camera3D>("Camera");
        navigator = GetNode<NavigationAgent3D>("Navigator");
    }

    public override void _Process(double delta)
    {
        MoveLoop();
    }

    public override void _Input(InputEvent @event)
    {
        if (Input.IsActionJustPressed("RightClick")){
            navigator.PathDesiredDistance = 0.2f;
            GetCursorPos();
            cursor.GlobalPosition = cursorPos;
            navigator.TargetPosition = cursorPos;
        }
    }

    void MoveLoop(){

        if (navigator.IsNavigationFinished()){
            return;
        }
        Vector3 targetPos = navigator.GetNextPathPosition();
        Vector3 direction = GlobalPosition.DirectionTo(targetPos);
        Velocity = direction * speed;
        MoveAndSlide();
    }

    Vector3 GetCursorPos(){
        Vector2 mousePos = GetViewport().GetMousePosition();
        Vector3 ray = camera.ProjectRayNormal(mousePos);
        cursorPos = (Vector3)dropPlane.IntersectsRay(camera.GlobalPosition,ray);
        return cursorPos;
    }

    public void BasicAttack(Entity target){
        float distance = GlobalPosition.DistanceTo(target.GlobalPosition);
        Vector3 dir = GlobalPosition.DirectionTo(target.GlobalPosition);
        navigator.PathDesiredDistance = attackRange;
        if (distance <= attackRange){
            Attack attack = new Attack();
            attack.damage = 2;
            target.TakeDamage(attack);
        }   
    }


}
