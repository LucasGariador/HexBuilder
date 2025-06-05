using UnityEngine;

[CreateAssetMenu(fileName = "NewAttackAction", menuName = "Action/Attack")]
public class AttackAction : Action
{
    public int damage;

    public override void Execute(ShipBehaiviour user, ShipBehaiviour target)
    {
        if (target != null)
        {
            target.TakeDamage(damage);
            Debug.Log($"{user.ShipName} attacks {target.ShipName} for {damage} damage! It has {target.Health} health left");
        }
    }
}
