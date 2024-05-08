using Godot;
using System;

public partial class AttackProjectile : CharacterBody3D
{
	public Attack attack;
	public Entity target;
	public float speed = 0.1f;
	MeshInstance3D graphic;

	public void Setup(Entity _target, Attack _atttack){
		target = _target;
		attack = _atttack;
	}
	public override void _Ready()
	{
		graphic = GetNode<MeshInstance3D>("Graphic");
		
	}

	public override void _Process(double delta)
	{	
		if (!IsInstanceValid(target)){
			QueueFree();
		} else{

			LookAt(target.GlobalPosition);

			Vector3 direction = (target.GlobalPosition - GlobalPosition).Normalized();

			Velocity = direction * speed;

			MoveAndCollide(Velocity);
		}

	}

	public void OnHurtboxBodyEntered(Node3D node3D){
		if (node3D == target){
			target.TakeDamage(attack);
			QueueFree();
		}
	}
	
}
