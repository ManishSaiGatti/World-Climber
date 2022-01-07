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


    public Game()
    {

    }

    public void Update()
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
}
