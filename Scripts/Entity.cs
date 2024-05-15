using Godot;
using System;
using System.Data.Common;

public enum EntityState {
    Idle,
    Attacking
}

public partial class Entity : CharacterBody3D
{
    public int entityIndex;
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

    bool alive = true;

    public bool canAttack = true;

    public override void _Ready()
    {
        Multiplayer.GetUniqueId();
        entityIndex = GetIndex();
        GD.Print("entity index: ", entityIndex);
    }

    [Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
    public virtual void MpDie(){

    }

    void OnAreaInputEvent(Node camera, InputEvent inputEvent, Vector3 position, Vector3 norman, int shapeIdx){
        if (Input.IsActionJustPressed("RightClick")){
            Player attacker = (Player)camera.GetParent();
            attacker.BasicAttack(this);
        }
    }

    public void TakeDamage(Attack attack){
        health -= attack.damage;
            if (health <= 0){
                alive = false;
                Rpc("MpDie");
            }else{

            }
    }

    public Entity GetEntityById(int id){
        foreach (Entity entity in GetTree().GetNodesInGroup("Entity")){
            if (entity.entityIndex == id){
                return entity;
            }
        }
        return null; 
    }


}
