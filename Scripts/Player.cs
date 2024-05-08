using Godot;
using System;

public partial class Player : Entity
{
    Plane dropPlane;
    MeshInstance3D indicator;
    MeshInstance3D model;
    Camera3D camera;
    NavigationAgent3D navigator;
    Vector3 cursorPos;
    Entity target;
    PackedScene projectile;
    Node3D game;

    public EntityState entityState = EntityState.Idle;

    public override void _Ready()
    {
        dropPlane = new Plane(new Vector3(0,1,0));
        indicator = GetNode<MeshInstance3D>("UI/Cursor");
        model = GetNode<MeshInstance3D>("Model");
        camera = GetNode<Camera3D>("Camera");
        navigator = GetNode<NavigationAgent3D>("Navigator");
        projectile = GD.Load<PackedScene>("res://Scenes/AttackProjectile.tscn");
        game = GetNode<Node3D>("/root/Map");
        indicator.Hide();
    }

    public override void _Process(double delta)
    {
        if (entityState == EntityState.Idle){
            MoveLoop();
        }
        if (entityState == EntityState.Attacking && target != null){
            AttackMoveLoop();
        }


        
    }

    public override void _Input(InputEvent @event)
    {
        if (Input.IsActionJustPressed("RightClick")){
            navigator.PathDesiredDistance = 0.2f;
            GetCursorPos();
            indicator.GlobalPosition = cursorPos;
            navigator.TargetPosition = cursorPos;
            indicator.Show();
        }
    }

    void MoveLoop(){

        if (navigator.IsNavigationFinished()){
            indicator.Hide();
            return;
        }
        Vector3 targetPos = navigator.GetNextPathPosition();
        model.LookAt(targetPos);
        Vector3 direction = GlobalPosition.DirectionTo(targetPos);
        Velocity = direction * speed;
        MoveAndSlide();
    }

    void AttackMoveLoop(){

        if (navigator.IsNavigationFinished()){
            indicator.Hide();
            return;
        }
        Vector3 targetPos = navigator.GetNextPathPosition();
        Vector3 direction = GlobalPosition.DirectionTo(targetPos);
        Velocity = direction * speed;
        MoveAndSlide();
        float distance = GlobalPosition.DistanceTo(target.GlobalPosition);
        if (distance <= attackRange){
            Attack(target);
        }  
        
    }

    Vector3 GetCursorPos(){
        Vector2 mousePos = GetViewport().GetMousePosition();
        Vector3 ray = camera.ProjectRayNormal(mousePos);
        cursorPos = (Vector3)dropPlane.IntersectsRay(camera.GlobalPosition,ray);
        return cursorPos;
    }

    public void BasicAttack(Entity _target){
        target = _target;
        entityState = EntityState.Attacking;
        float distance = GlobalPosition.DistanceTo(_target.GlobalPosition);
        Vector3 dir = GlobalPosition.DirectionTo(_target.GlobalPosition);
        navigator.PathDesiredDistance = attackRange;
        if (distance <= attackRange){
            Attack(_target);
        }   
    }

    public void TakeDamage(){
        return;
    }

    public void Attack(Entity _target){
        Attack attack = new Attack
        {
            damage = attackDamage,
            attackType = AttackType.Ranged
        };
        AttackProjectile instance = (AttackProjectile)projectile.Instantiate();
        game.AddChild(instance);
        instance.target = target;
        instance.attack = attack;
        instance.GlobalPosition = GlobalPosition + new Vector3(0,1,0);
        model.LookAt(target.GlobalPosition);
        entityState = EntityState.Idle;
    }


}
