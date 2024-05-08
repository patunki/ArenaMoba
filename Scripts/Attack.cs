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
}