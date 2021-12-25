using System;
using System.Collections.Generic;

class Game
{
    public static readonly string Title = "IceClimber";
    public static readonly Vector2 Resolution = new Vector2(640, 480);

    readonly Texture _background = Engine.LoadTexture("Background.jpg");

    readonly Texture _sprite = Engine.LoadTexture("player.png");
    int spriteX = 320;
    int spriteY = 240;

    public Game()
    {
        
    }

    public void Update()
    {
        Engine.DrawTexture(_background, Vector2.Zero);

        Engine.DrawTexture(_sprite, new Vector2(spriteX, spriteY), null, new Vector2(25, 30));

        if (Engine.GetKeyDown(Key.Left))
        {
            spriteX -= 10;
        }
        else if (Engine.GetKeyDown(Key.Right))
        {
            spriteX += 10;
        }
        else if (Engine.GetKeyDown(Key.Up))
        {
            spriteY -= 10;
        }
        else if (Engine.GetKeyDown(Key.Down)) 
        {
            spriteY += 10;
        }
    }
}
