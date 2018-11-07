﻿using UnityEngine;

public class PCActions : MonoBehaviour
{
    private bool checkEnergy;
    private bool checkSchedule;
    private PlayerInput input;
    private Command moveDirection;
    private SchedulingSystem schedule;

    private void Start()
    {
        input = gameObject.GetComponent<PlayerInput>();
        schedule = FindObjects.GameLogic.GetComponent<SchedulingSystem>();
    }

    private void Update()
    {
        checkSchedule = schedule.IsCurrentActor(gameObject);
        checkEnergy = gameObject.GetComponent<Energy>().HasEnoughEnergy();

        if (!checkSchedule)
        {
            return;
        }

        if (!checkEnergy)
        {
            schedule.NextTurn();
            return;
        }

        if (input.GameCommand() != Command.Invalid)
        {
            FindObjects.GameLogic.GetComponent<UIModeline>().PrintText();
        }

        if (input.IsMovementCommand())
        {
            gameObject.GetComponent<Move>().MoveActor(input.GameCommand());
        }
        else
        {
            switch (input.GameCommand())
            {
                case Command.AutoExplore:
                    moveDirection = gameObject.GetComponent<AutoExplore>()
                        .ChooseNextStep();

                    if (moveDirection != Command.Invalid)
                    {
                        gameObject.GetComponent<Move>().MoveActor(moveDirection);
                    }
                    break;

                // Test commands.
                case Command.Initialize:
                    FindObjects.GameLogic.GetComponent<Initialize>()
                        .InitializeGame();
                    break;

                case Command.RenderAll:
                    FindObjects.GameLogic.GetComponent<Test>().RenderAll
                        = !FindObjects.GameLogic.GetComponent<Test>().RenderAll;
                    break;

                case Command.PrintEnergy:
                    schedule.CurrentActor.GetComponent<Energy>().PrintEnergy();
                    break;

                case Command.AddEnergy:
                    schedule.CurrentActor.GetComponent<Energy>()
                        .RestoreEnergy(2000, false);
                    break;

                    //case PlayerInput.Command.Confirm:
                    //    break;

                    //case PlayerInput.Command.Cancel:
                    //    break;

                    //case PlayerInput.Command.Invalid:
                    //    break;

                    //default:
                    //    FindObjects.GameLogic.GetComponent<TestMove>().
                    //        MoveAround(gameObject);
                    //    break;
            }
        }
    }
}
