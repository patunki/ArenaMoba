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
    [Export]
    public float attackDamage = 5;

    public bool canAttack = true;

    void OnAreaInputEvent(Node camera, InputEvent inputEvent, Vector3 position, Vector3 norman, int shapeIdx){
        if (Input.IsActionJustPressed("RightClick") && camera.GetParent() is Player){
            Player attacker = (Player)camera.GetParent();
            attacker.BasicAttack(this);
        }
    }

    public void TakeDamage(Attack attack){
        health -= attack.damage;
        GD.Print("health: ", health);
            if (health <= 0){
                GD.Print("damage: ", attack.damage);
                GD.Print (Name," Dead");
                QueueFree();
            }else{
                GD.Print("damage: ", attack.damage);
            }
    }

}
