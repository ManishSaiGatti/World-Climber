using System;
using System.Collections.Generic;

class Game
{
    public static readonly string Title = "IceClimber";
    public static readonly Vector2 Resolution = new Vector2(640, 480);

    readonly Texture _background = Engine.LoadTexture("Background.jpg");

    readonly Texture _sprite = Engine.LoadTexture("player.png");
    int spriteX = 320;
    int spriteY = 280;

    readonly Texture _block = Engine.LoadTexture("square.png");

    List<int> blocksX = new List<int>();
    List<int> blocksY = new List<int>();

    public Game()
    {
        
    }

    public void Update()
    {
        Engine.DrawTexture(_background, Vector2.Zero);

        for (int i = 0; i < blocksX.Count; i++)
        {
            Vector2 vec = new Vector2(blocksX[i], blocksY[i]);
            Engine.DrawTexture(_block, vec, null, new Vector2(20, 20));
        }

        addLayer();

        Engine.DrawTexture(_sprite, new Vector2(spriteX, spriteY), null, new Vector2(25, 30));

        if (Engine.GetKeyDown(Key.Left, true))
        {
            spriteX -= 10;
        }
        else if (Engine.GetKeyDown(Key.Right, true))
        {
            spriteX += 10;
        }
        else if (Engine.GetKeyDown(Key.Up, true))
        {
            spriteY -= 10;
        }
        else if (Engine.GetKeyDown(Key.Down, true)) 
        {
            spriteY += 10;
        }

        if (spriteY == Resolution.Y / 2) 
        {
            for (int i = 0; i < blocksY.Count; i++) 
            {
                blocksY[i] = blocksY[i] + 10;
            }

            spriteY += 20;
        }
    }

    public void addLayer() 
    {
        if (blocksY.Count == 0)
        {
            blocksX.Add(0);
            blocksY.Add(0);

            blocksX.Add(620);
            blocksY.Add(0);
        }
        else if (blocksY[blocksY.Count - 2] > 50 && blocksY[blocksY.Count - 1] > 50) 
        {
            blocksX.Add(0);
            blocksY.Add(0);

            blocksX.Add(620);
            blocksY.Add(0);
        }
    }

}
