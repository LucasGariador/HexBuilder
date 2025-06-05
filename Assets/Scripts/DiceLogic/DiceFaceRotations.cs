using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "DiceRotations", menuName = "Dice/D20 Rotations")]
public class DiceFaceRotations : ScriptableObject
{
    public List<Quaternion> faceRotations = new List<Quaternion>(20);

    public Quaternion GetRotationForValue(int value)
    {
        return faceRotations[Mathf.Clamp(value - 1, 0, 19)];
    }
}
