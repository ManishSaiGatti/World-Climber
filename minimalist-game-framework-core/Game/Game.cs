using System;
using System.Collections.Generic;

class Game
{
    public static readonly string Title = "IceClimber";
    public static readonly Vector2 Resolution = new Vector2(640, 480);

    readonly Texture _background = Engine.LoadTexture("Background.jpg");

    readonly Texture _sprite = Engine.LoadTexture("player.png");

    readonly Font font = Engine.LoadFont("OpenSans-Regular.ttf", 10);

    int spriteX = 320;
    int spriteY = 280;

    int spriteSizeX = 20;
    int spriteSizeY = 25;

    readonly Texture _block = Engine.LoadTexture("square.png");

    List<int> blocksX = new List<int>();
    List<int> blocksY = new List<int>();

    int totalPoints = 0;
    public Game()
    {
        
    }

    public void Update()
    {
        Engine.DrawTexture(_background, Vector2.Zero);

        if (playerHitsBorders()) 
        {
            Engine.DrawString("HITTING BORDERS", new Vector2(10, 440), Color.Red, font);
        }

        //draw levels
        for (int i = 0; i < blocksX.Count; i++)
        {
            Vector2 vec = new Vector2(blocksX[i], blocksY[i]);
            Engine.DrawTexture(_block, vec, null, new Vector2(spriteSizeX, spriteSizeY));
        }

        //if statement inside addLayer to see if a layer sould be added
        addLayer();

        Engine.DrawTexture(_sprite, new Vector2(spriteX, spriteY), null, new Vector2(20, 25));

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

        if (playerIsOverlapping()) 
        {
            Engine.DrawString("OVERLAPPING", new Vector2(10, 440), Color.Red, font);
        }

        //displaying the number of points
        Engine.DrawString(totalPoints.ToString(), new Vector2(440, 440), Color.Red, font);
       
    }

    public void addLayer() 
    {
        if (blocksY.Count == 0)
        {
            blocksX.Add(0);
            blocksY.Add(0);

            blocksX.Add(620);
            blocksY.Add(0);

            totalPoints++;
        }
        else if (blocksY[blocksY.Count - 2] > 100 && blocksY[blocksY.Count - 1] > 100) 
        {
            Random rand = new Random();            
            int bound = rand.Next(0, (int)Resolution.X);

            while (bound % 20 != 0) 
            {
                bound = rand.Next(0, (int)Resolution.X - 60); 
            }

            for (int i = 0; i < bound; i += 20)
            {
                blocksX.Add(i);
                blocksY.Add(0);
            }

            for (int i = bound + 60; i <= Resolution.X; i += 20) 
            {
                blocksX.Add(i);
                blocksY.Add(0);
            }

            totalPoints++;
        }
        
    }
    
    //check if the player overlaps with any blocks
    public bool playerIsOverlapping() 
    {

        Bounds2 spritePosition = new Bounds2(new Vector2(spriteX, spriteY), new Vector2(20, 25));

        for (int i = 0; i < blocksX.Count; i++)
        {
            Bounds2 floorBounds = new Bounds2(new Vector2(blocksX[i], blocksY[i]), new Vector2(20, 20));
            if (spritePosition.Overlaps(floorBounds))
            {
                //correct the error
                if (spriteY < blocksY[i])
                {
                    spriteY -= 10;
                }
                else if (spriteY > blocksY[i]) 
                {
                    spriteY += 10;
                }


                return true;
            }
        }

        


        return false;
    }

    public bool playerHitsBorders() 
    {
        //borders
        if (spriteX < 0)
        {
            spriteX += 10;
            return true;
        }
        if (spriteX + spriteSizeX> (int)Resolution.X)
        {
            spriteX -= 10;
            return true;
        }
        if (spriteY < 0)
        {
            spriteY += 10;
            return true;
        }
        if (spriteY + spriteSizeY > (int)Resolution.Y)
        {
            spriteY -= 10;
            return true;
        }

        return false;       

    }
}
