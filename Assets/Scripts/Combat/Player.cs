﻿using UnityEngine;

public class Player : ShipBehaiviour
{
    public override void TakeTurn()
    {
        // Example: Always use the first action on the first enemy
        if (actions.Count > 0)
        {
            Action action = actions[0];
            ShipBehaiviour target = FindAnyObjectByType<Enemy>(); // Simplified target selection
            if (target != null && target.Health > 0)
            {
                action.Execute(this, target);
            }
        }

        Debug.Log(ShipName + " has completed their turn.");
    }
}
