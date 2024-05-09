using Godot;
using System;

public partial class Player : Entity
{
    Plane dropPlane;
    MeshInstance3D indicator;
    MeshInstance3D model;
    Camera3D camera;
    NavigationAgent3D navigator;
    Entity target;
    PackedScene projectile;
    PackedScene fireball;
    Node3D game;
    Timer attackTimer;

    public EntityState entityState = EntityState.Idle;

    public override void _Ready()
    {
        dropPlane = new Plane(new Vector3(0,1,0));
        indicator = GetNode<MeshInstance3D>("UI/Cursor");
        model = GetNode<MeshInstance3D>("Model");
        camera = GetNode<Camera3D>("Camera");
        navigator = GetNode<NavigationAgent3D>("Navigator");
        projectile = GD.Load<PackedScene>("res://Scenes/AttackProjectile.tscn");
        fireball = GD.Load<PackedScene>("res://Scenes/Fireball.tscn");
        game = GetNode<Node3D>("/root/Map");
        indicator.Hide();
        attackTimer = new Timer();
        AddChild(attackTimer);

        attackTimer.Timeout += ResetAttackTimer;
    }

    public override void _Process(double delta)
    {
        MoveLoop();
    }

    public override void _Input(InputEvent @event)
    {
        if (Input.IsActionJustPressed("RightClick")){
            navigator.PathDesiredDistance = 0.2f;
            Vector3 clickPos = GetCursorPos();
            indicator.GlobalPosition = clickPos;
            navigator.TargetPosition = clickPos;
            indicator.Show();
        }

        if (Input.IsActionJustPressed("Q")){
            Vector3 dir = GlobalPosition.DirectionTo(GetCursorPos());
            Fireball instance = (Fireball)fireball.Instantiate();
            instance.host = this;
            game.AddChild(instance);
            instance.direction = GlobalPosition.DirectionTo(GetCursorPos());
            instance.GlobalPosition = GlobalPosition + new Vector3(0,1,0);
        }
    }

    void MoveLoop(){

        Vector3 direction = GlobalPosition.DirectionTo(navigator.GetNextPathPosition());

        if (entityState == EntityState.Attacking && IsInstanceValid(target)){
            
            float distance = GlobalPosition.DistanceTo(target.GlobalPosition);
            if (distance <= attackRange){
                indicator.Hide();
                Attack();
            } else {
                navigator.TargetPosition = target.GlobalPosition;
                navigator.PathDesiredDistance = attackRange;
                Velocity = direction * speed;
                MoveAndSlide();
            }
          
        } else {
            entityState = EntityState.Idle;
        }

        if (navigator.IsNavigationFinished()){
            indicator.Hide();
            return;
        }
        if (entityState == EntityState.Idle){
            Velocity = direction * speed;
            model.LookAt(new Vector3(indicator.GlobalPosition.X, 0.55f , indicator.GlobalPosition.Z));
            MoveAndSlide();
        } 

    }


    Vector3 GetCursorPos(){
        Vector3 ray = camera.ProjectRayNormal(GetViewport().GetMousePosition());
        Vector3 cursorPos = (Vector3)dropPlane.IntersectsRay(camera.GlobalPosition,ray);
        entityState = EntityState.Idle;
        return cursorPos;
    }

    public void BasicAttack(Entity _target){
        target = _target;
        entityState = EntityState.Attacking;
        navigator.PathDesiredDistance = attackRange; 
    }

    public void TakeDamage(){
        return;
    }

    public void Attack(){
        
        if (canAttack == true && GlobalPosition.DistanceTo(target.GlobalPosition) <= attackRange){
            canAttack = false;
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
            attackTimer.Start(attackSpeed);           
        }
        return;
    }

    void ResetAttackTimer(){
        canAttack = true;
    }

}

//float distance = GlobalPosition.DistanceTo(_target.GlobalPosition);
//Vector3 dir = GlobalPosition.DirectionTo(_target.GlobalPosition);
//Vector3 targetPos = navigator.GetNextPathPosition();