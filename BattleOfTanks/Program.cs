﻿using System;
using SplashKitSDK;

namespace BattleOfTanks
{
    public class Program
    {
        public static void LoadResources()
        {
            SplashKit.LoadBitmap("Tank", "assets/tank_green.png");
        }

        public static void Main()
        {
            LoadResources();
            Window window = new Window("Custom Program", 1000, 800);
            GameManager gameManager = new GameManager(window);

            gameManager.MainLoop();
        }
    }
}
