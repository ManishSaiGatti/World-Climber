using System;
using System.Collections.Generic;

class Game
{
    public static readonly string Title = "Minimalist Game Framework";
    public static readonly Vector2 Resolution = new Vector2(640, 480);

    readonly Texture floorUnder = Engine.LoadTexture("undergroundFloor.png");
    readonly Texture floorSand = Engine.LoadTexture("sandFloor.png");
    readonly Texture floorIce = Engine.LoadTexture("iceFloor.png");
    readonly Texture floorSpace = Engine.LoadTexture("spaceFloor.png");
    readonly Texture underBack = Engine.LoadTexture("UndergroundBack.png");
    readonly Texture sandBack = Engine.LoadTexture("SandBack.png");
    readonly Texture iceBack = Engine.LoadTexture("IceBack.png");
    readonly Texture spaceBack = Engine.LoadTexture("SpaceBack.png");
    readonly Texture spaceBlack1 = Engine.LoadTexture("spaceBlack.png");
    readonly Texture spaceBlack2 = Engine.LoadTexture("spaceBlack.png");
    readonly Texture underSand = Engine.LoadTexture("underSand.png");

    private int screen = 1;
    private float scrollValue = 0;

    private float spaceBack1Y = -2880;
    private float spaceBack2Y = -3560;

   

    public Game()

    {

    }

    public void Update()
    {
        
        if (Engine.GetKeyDown(Key.NumRow1))
        {
            screen = 1;
        }
        if (Engine.GetKeyDown(Key.NumRow2))
        {
            screen = 2;
            scrollValue = 0;
        }
        if (screen == 1)
        {
            if (Engine.GetKeyHeld(Key.U))
            {
                Engine.DrawTexture(underBack, new Vector2(0, 0));
                for (int i = 0; i < 640; i += 20)
                {
                    Engine.DrawTexture(floorUnder, new Vector2(i, 200));
                }
            }
            if (Engine.GetKeyHeld(Key.O))
            {
                Engine.DrawTexture(sandBack, new Vector2(0, 0));
                for (int i = 0; i < 640; i += 20)
                {
                    Engine.DrawTexture(floorSand, new Vector2(i, 200));
                }
            }
            if (Engine.GetKeyHeld(Key.I))
            {
                Engine.DrawTexture(iceBack, new Vector2(0, 0));
                for (int i = 0; i < 640; i += 20)
                {
                    Engine.DrawTexture(floorIce, new Vector2(i, 200));
                }
            }
            if (Engine.GetKeyHeld(Key.P))
            {
                Engine.DrawTexture(spaceBack, new Vector2(0, 0));
                for (int i = 0; i < 640; i += 20)
                {
                    Engine.DrawTexture(floorSpace, new Vector2(i, 200));
                }
            }

        }
        else if (screen == 2)
        {
            Engine.DrawTexture(underBack, new Vector2(0, 0 + scrollValue));
            Engine.DrawTexture(underSand, new Vector2(0, -960 + scrollValue));
            Engine.DrawTexture(sandBack, new Vector2(0, -1440 + scrollValue));
            Engine.DrawTexture(iceBack, new Vector2(0, -1920 + scrollValue));
            Engine.DrawTexture(spaceBack, new Vector2(0, -2400 + scrollValue));

            Engine.DrawTexture(spaceBlack1, new Vector2(0, spaceBack1Y));
            Engine.DrawTexture(spaceBlack2, new Vector2(0, spaceBack2Y));

            if(spaceBack1Y >= 480)
            {
                spaceBack1Y = -480;
            }
            if(spaceBack2Y >= 480)
            {
                spaceBack2Y = -480;
            }

            if (Engine.GetKeyHeld(Key.Down))
            {
                scrollValue += 10f;
                spaceBack1Y += 10f;
                spaceBack2Y += 10f;

            } else if (Engine.GetKeyHeld(Key.Up))
            {
                scrollValue -= 10f;
                spaceBack1Y -= 10f;
                spaceBack2Y -= 10f;
            }
        }
        
    }
}
