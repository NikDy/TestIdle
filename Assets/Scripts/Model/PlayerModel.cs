using System;
using UnityEngine;
using System.Collections.Generic;



public class PlayerModel
{

    public Vector3 position { get; set; }
    public float normalizedSpeed { get; set; }
    public int countOfStacks { get; set; }
    public int maxAmountOfStacks { get; set; }
    public Stack<GameObject> stacks = new Stack<GameObject>();
    public int countOfCoins { get; set; }

    public PlayerModel(Vector3 position, float normalizedSpeed, int maxAmountOfStacks)
    {
        this.position = position;
        this.normalizedSpeed = normalizedSpeed;
        this.countOfStacks = 0;
        this.maxAmountOfStacks = maxAmountOfStacks;
        this.countOfCoins = 0;
    }
}
