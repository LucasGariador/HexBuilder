using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShipSettings", menuName = "Ships", order = 0)]
public class ShipSetting : ScriptableObject
{
    public enum ShipType { 
        Bob,
        Challenger,
        Dispatcher,
        Executioner,
        Imperial,
        Insurgent,
        Omen,
        Pancake,
        Spitfire,
        Striker,
        Zenith
    }

    public Ship Bob;
    public Ship Challenger;
    public Ship Dispatcher;
    public Ship Executioner;
    public Ship Imperial;
    public Ship Insurgent;
    public Ship Omen;
    public Ship Pancake;
    public Ship SpitFire;
    public Ship Striker;
    public Ship Zenith;

    public Ship GetShip(ShipType ship)
    {
        return ship switch
        {
            ShipType.Bob => Bob,
            ShipType.Challenger => Challenger,
            ShipType.Dispatcher => Dispatcher,
            ShipType.Executioner => Executioner,
            ShipType.Imperial => Imperial,
            ShipType.Insurgent => Insurgent,
            ShipType.Omen => Omen,
            ShipType.Pancake => Pancake,
            ShipType.Spitfire => SpitFire,
            ShipType.Striker => Striker,
            ShipType.Zenith => Zenith,
            _ => null,
        };
    }

}
