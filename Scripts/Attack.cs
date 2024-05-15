using Godot;
using System;

public enum AttackType {
    Ranged,
    Melee,
    Ability
}
public partial class Attack : Node
{
    public float damage;
    public AttackType attackType;
    public PackedScene attackProjectile;
    public Entity host;
    public Entity target;

}
