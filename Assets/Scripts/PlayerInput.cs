﻿using UnityEngine;

//* PlayerInput
//* -> PCActions
//* -> Move, Attack, etc.
public class PlayerInput : MonoBehaviour
{
    private bool down;
    private bool downLeft;
    private bool downRight;
    private bool left;
    private bool right;
    private bool up;
    private bool upLeft;
    private bool upRight;

    public enum Command
    {
        Left, Right, Up, Down,
        UpLeft, UpRight, DownLeft, DownRight,
        Wait,

        // Debug commands:
        EndTurn, Initialize, RenderAll, PrintEnergy,

        Confirm, Cancel, Invalid
    };

    public Command GameCommand()
    {
        left = Input.GetKeyDown(KeyCode.LeftArrow)
            || Input.GetKeyDown(KeyCode.H)
            || Input.GetKeyDown(KeyCode.Keypad4);

        down = Input.GetKeyDown(KeyCode.DownArrow)
            || Input.GetKeyDown(KeyCode.J)
            || Input.GetKeyDown(KeyCode.Keypad2);

        up = Input.GetKeyDown(KeyCode.UpArrow)
            || Input.GetKeyDown(KeyCode.K)
            || Input.GetKeyDown(KeyCode.Keypad8);

        right = Input.GetKeyDown(KeyCode.RightArrow)
            || Input.GetKeyDown(KeyCode.L)
            || Input.GetKeyDown(KeyCode.Keypad6);

        upLeft = Input.GetKeyDown(KeyCode.Y)
            || Input.GetKeyDown(KeyCode.Keypad7);

        upRight = Input.GetKeyDown(KeyCode.U)
            || Input.GetKeyDown(KeyCode.Keypad9);

        downLeft = Input.GetKeyDown(KeyCode.B)
            || Input.GetKeyDown(KeyCode.Keypad1);

        downRight = Input.GetKeyDown(KeyCode.N)
            || Input.GetKeyDown(KeyCode.Keypad3);

        if (left)
        {
            return Command.Left;
        }
        else if (down)
        {
            return Command.Down;
        }
        else if (up)
        {
            return Command.Up;
        }
        else if (right)
        {
            return Command.Right;
        }
        else if (upLeft)
        {
            return Command.UpLeft;
        }
        else if (upRight)
        {
            return Command.UpRight;
        }
        else if (downLeft)
        {
            return Command.DownLeft;
        }
        else if (downRight)
        {
            return Command.DownRight;
        }
        // Test key combinations.
        else if (Input.GetKey(KeyCode.LeftControl)
            && Input.GetKeyDown(KeyCode.F))
        {
            return Command.Up;
        }
        else if (Input.GetKeyDown(KeyCode.End))
        {
            return Command.EndTurn;
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            return Command.Initialize;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            return Command.RenderAll;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            return Command.PrintEnergy;
        }

        return Command.Invalid;
    }

    public bool IsMovementCommand()
    {
        switch (GameCommand())
        {
            case Command.Left:
            case Command.Right:
            case Command.Up:
            case Command.Down:
            case Command.UpLeft:
            case Command.UpRight:
            case Command.DownLeft:
            case Command.DownRight:
            case Command.Wait:
                return true;
        }

        return false;
    }
}
