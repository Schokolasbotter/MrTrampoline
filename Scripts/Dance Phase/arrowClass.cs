using System;
using System.Collections;
using System.Collections.Generic;

public class Arrow
{
    #region
    public enum Direction
    {
        up,
        down,
        left,
        right,
        upLeft,
        upRight,
        downLeft,
        downRight
    };

    public Direction _direction;
    public float _rotationAngle;
    public int test;

    private static Random rnd = new Random();
    private Direction[] allDirections = { Direction.up, Direction.down, Direction.left, Direction.right, Direction.upLeft, Direction.upRight, Direction.downLeft, Direction.downRight };

    #endregion

    //Create arrow with difficulty Level
    public Arrow(int difficultyLevel)
    {
        //Create a direction randomly depending on the difficulty level
        _direction = difficultyLevel switch
        {
            0 => allDirections[rnd.Next(2)],
            1 => allDirections[rnd.Next(2)],
            2 => allDirections[rnd.Next(3)],
            3 => allDirections[rnd.Next(4)],
            4 => allDirections[rnd.Next(4)],
            5 => allDirections[rnd.Next(5)],
            6 => allDirections[rnd.Next(6)],
            7 => allDirections[rnd.Next(7)],
            8 => allDirections[rnd.Next(8)],
            _ => allDirections[rnd.Next(2)]
        };

        //Set the according angle for Rotation
        _rotationAngle = _direction switch
        {
            Direction.up => 0f,
            Direction.down => -180f,
            Direction.left => 90f,
            Direction.right => -90f,
            Direction.upRight => -45f,
            Direction.upLeft => 45f,
            Direction.downRight => -135f,
            Direction.downLeft => 135f,
            _ => 0f,
        };
    }

    //Create Arrow with an angle
    public Arrow(float degAngle)
    {
        _rotationAngle = degAngle;
        //Set the according angle for Rotation

        switch (_rotationAngle)
        {
            default:
                _direction = Direction.right;
                break;
            case float x when x <= 22.5f && x >= -22.5f:
                _direction = Direction.right;
                break;
            case float x when x <= 67.5f && x > 22.5f:
                _direction = Direction.upRight;
                break;
            case float x when x <= 112.5f && x > 67.5f:
                _direction = Direction.up;
                break;
            case float x when x <= 157.5f && x > 112.5f:
                _direction = Direction.upLeft;
                break;
            case float x when x > 157.5f || x < -157.5f:
                _direction = Direction.left;
                break;
            case float x when x >= -67.5f && x < -22.5f:
                _direction = Direction.downRight;
                break;
            case float x when x >= -112.5f && x < -67.5f:
                _direction = Direction.down;
                break;
            case float x when x >= -157.5f && x < -112.5f:
                _direction = Direction.downLeft;
                break;
        }
    }
}
