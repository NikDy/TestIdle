using UnityEngine;


[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/PlayerValues", order = 1)]
public class PlayerValues : ScriptableObject
{
    public Vector3 position;
    public float normalizedSpeed;
}
