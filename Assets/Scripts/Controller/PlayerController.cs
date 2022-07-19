using System;
using UnityEngine;


public class PlayerController
{
    private PlayerModel playerModel;
    private PlayerUpdate playerUpdate;
    private InputController inputController;

    private float lastStackSellTime;

    public PlayerController(PlayerModel playerModel, PlayerUpdate playerUpdate, InputController inputController)
    {
        this.playerModel = playerModel;
        this.playerUpdate = playerUpdate;
        this.playerUpdate.SetPosition(playerModel.position);
        this.inputController = inputController;

        this.inputController.OnInput += Move;
        StackUpdate.HitEvent += CollectStack;
        TraidpointUpdate.HitEvent += SellStacks;
    }


    private void CollectStack(GameObject stack)
    {
        if (playerModel.countOfStacks < playerModel.maxAmountOfStacks)
        {
            playerModel.countOfStacks++;
            playerUpdate.CollectStack(stack, playerModel.countOfStacks);
            playerModel.stacks.Push(stack);
        }
    }


    private void SellStacks()
    {
        if (playerModel.countOfStacks > 0 && Time.time - lastStackSellTime > 0.05)
        {
            playerModel.countOfCoins += 15; //from settings
            playerUpdate.SellStack(playerModel.stacks.Pop());
            playerModel.countOfStacks--;
            lastStackSellTime = Time.time;
        }
    }


    private void Move(Vector2 direction)
    {
        if (direction != Vector2.zero)
        {
            playerUpdate.StartRun();
            playerUpdate.Move(direction, 1 / playerModel.normalizedSpeed);
            playerModel.position = playerUpdate.gameObject.transform.position;
        }
        else
        {
            playerUpdate.StopRun();
        }
    }
}

