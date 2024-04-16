﻿using System;
using Microsoft.Xna.Framework;

namespace TestGame.Utils;

public struct TimedUpdate {

    private float Timer { get; set; }
    private CheckTime _checkTime { get; set; }

    public enum CheckTime {
        OneSecond,
        TwoSecond
    }

    public TimedUpdate(CheckTime checkTime) {
        Timer = 0;
        _checkTime = checkTime;
    }

    public bool UpdateTimer(GameTime gameTime) {
        Timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

        switch (_checkTime) {
            case CheckTime.OneSecond:
                if (Timer >= 1.0f) {
                    Timer = 0;
                    return true;
                }
                break;
            case CheckTime.TwoSecond:
                if (Timer >= 2.0f) {
                    Timer = 0;
                    return true;
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        return false;
    }
}