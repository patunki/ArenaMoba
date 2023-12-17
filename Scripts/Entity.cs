using Godot;
using System;
using System.Data.Common;

public enum EntityState {
    Idle,
    Attacking
}

public partial class Entity : CharacterBody3D
{
    [Export]
    public float speed = 10;
    [Export]
    public float maxHealth;
    [Export]
    public float health;
    [Export]
    public float attackSpeed;
    [Export]
    public float attackTime;
    [Export]
    public float attackRange;

    void OnAreaInputEvent(Node camera, InputEvent inputEvent, Vector3 position, Vector3 norman, int shapeIdx){
        if (Input.IsActionJustPressed("RightClick") && camera.GetParent() is Player){
            Player attacker = (Player)camera.GetParent();
            attacker.BasicAttack(this);
        }
    }

    public void TakeDamage(Attack attack){
        health -= attack.damage;
        GD.Print(health);
            if (health <= 0){
                GD.Print(attack.damage);
                GD.Print ("Dead");
                QueueFree();
            }else{
                GD.Print(attack.damage);
            }
    }

}
