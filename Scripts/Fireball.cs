using Godot;
using System;

public partial class Fireball : CharacterBody3D
{
	//public Attack attack;
	public Vector3 target;
	public float speed = 0.1f;
    public Vector3 direction;
	MeshInstance3D graphic;
    public Entity host;

    Timer timer;

	public void Setup(Vector3 _target, Attack _atttack){
		target = _target;
	}
	public override void _Ready()
	{
		timer = GetNode<Timer>("Timer");
        timer.Timeout += QueueFree;
		
	}

	public override void _Process(double delta)
	{	
		

		Velocity = direction * speed;

		MoveAndCollide(Velocity);

	}

	public void OnHurtboxBodyEntered(Node3D node3D){
        if (node3D != host && node3D is Entity){
            Attack attack = new Attack
            {
                damage = 4,
                attackType = AttackType.Ranged
            };
            Entity entity = node3D as Entity;
            entity.TakeDamage(attack);
            QueueFree();
        }
	}
}
