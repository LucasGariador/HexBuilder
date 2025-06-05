[System.Serializable]
public class ShipResources
{
    public int fuel = 10;
    public int food = 5;

    public bool ConsumeFuel(int amount)
    {
        if (fuel < amount) return false;
        fuel -= amount;
        return true;
    }

    public bool ConsumeFood(int amount)
    {
        if (food < amount) return false;
        food -= amount;
        return true;
    }

    public void AddFuel(int amount) => fuel += amount;
    public void AddFood(int amount) => food += amount;
}
