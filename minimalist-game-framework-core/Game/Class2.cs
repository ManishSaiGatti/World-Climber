using System;
using System.Collections.Generic;

class Class2
{
    public static readonly string Title = "IceClimber";
    public static readonly Vector2 Resolution = new Vector2(640, 480);

    readonly Texture _background = Engine.LoadTexture("Background.jpg");

    Player player = new Player();
    float playerVelocity = 0f;

    readonly Texture _sprite = Engine.LoadTexture("pickaxeSprite2.png");

    readonly Font font = Engine.LoadFont("OpenSans-Regular.ttf", 10);

    int spriteX = 320;
    int spriteY = 280;

    readonly Texture _block = Engine.LoadTexture("square.png");

    List<int> blocksX = new List<int>();
    List<int> blocksY = new List<int>();

    List<int> blockHitCount = new List<int>();

    int totalPoints = 0;

    readonly Texture trinketSkin = Engine.LoadTexture("eastern_orthodox_cross.png");
    List<int> trinketX = new List<int>();
    List<int> trinketY = new List<int>();
    int trinketSizeX = 20;
    int trinketSizeY = 30;

    public Class2()
    {
        addInitialLayers();
    }

    public void Update()
    {
        Engine.DrawTexture(_background, Vector2.Zero);
        Engine.DrawTexture(player.getTexture(), player.getVectorPos());

        for (int i = 0; i < blocksX.Count; i++)
        {
            Vector2 vec = new Vector2(blocksX[i], blocksY[i]);
            Engine.DrawTexture(_block, vec, null, new Vector2(20, 20));
        }

        addLayer();

        Engine.DrawTexture(_sprite, new Vector2(spriteX, spriteY), null, new Vector2(20, 25));

        //if (Engine.GetKeyDown(Key.Left))
        //{
        //    spriteX -= 10;
        //}
        //else if (Engine.GetKeyDown(Key.Right, true))
        //{
        //    spriteX += 10;
        //}

        if (Engine.GetKeyHeld(Key.Left))
        {
            player.left();
        }

        if (Engine.GetKeyHeld(Key.Right))
        {
            player.right();
        }


        //if (Engine.GetKeyDown(Key.Up, true))
        //{
        //    spriteY -= 10;
        //}
        //else if (Engine.GetKeyDown(Key.Down, true))
        //{
        //    spriteY += 10;
        //}

        if (Engine.GetKeyDown(Key.X))
        {
            player.yPos = 105;
        }

        if (Engine.GetKeyDown(Key.Up) && player.yPos >= blocksY[0] - 99)
        {
            playerVelocity = 5.5f;
            player.up(playerVelocity);
        }
        if (player.yPos < blocksY[0] - 99)
        {
            player.up(playerVelocity);
            if (player.yPos <= blocksY[blocksY.Count - 1] + 21)
            {
                playerVelocity = 0;
            }
            else
            {
                playerVelocity -= 0.1f;
            }
        }

        if (player.yPos == Resolution.Y / 2)
        {
            for (int i = 0; i < blocksY.Count; i++)
            {
                blocksY[i] = blocksY[i] + 10;
            }

            player.yPos += 20;
        }

        if (playerIsOverlapping())
        {
            Engine.DrawString("OVERLAPPING", new Vector2(10, 440), Color.Red, font);
        }

        //displaying the number of points
        Engine.DrawString(totalPoints.ToString(), new Vector2(440, 440), Color.Red, font);

    }

    public void addInitialLayers()
    {
        for (int y = 0; y < (int)Resolution.Y; y += 100)
        {
            for (int i = 0; i <= Resolution.X; i += 20)
            {
                blocksX.Add(i);
                blocksY.Add(y);
                blockHitCount.Add(4);
            }
        }
    }

    public void addLayer()
    {
        if (blocksY.Count == 0)
        {
            for (int i = 0; i <= 620; i += 20)
            {
                blocksX.Add(i);
                blocksY.Add(220);
            }

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

            //Console.WriteLine(blocksY.ToArray());
            //Console.WriteLine(String.Join(",", blocksY));

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
            //Console.WriteLine("List: ");
            //Console.WriteLine(String.Join(",", blocksY));
            //Console.WriteLine(blocksY[0] + 1);
        }

    }

    //check if the player overlaps with any blocks
    public bool playerIsOverlapping()
    {
        Bounds2 spritePosition = new Bounds2(player.getVectorPos(), new Vector2(20, 25));

        for (int i = 0; i < blocksX.Count; i++)
        {
            Bounds2 floorBounds = new Bounds2(new Vector2(blocksX[i], blocksY[i]), new Vector2(20, 20));
            if (spritePosition.Overlaps(floorBounds))
            {
                //correct the error
                if (player.yPos < blocksY[i])
                {
                    player.yPos -= 10;
                }
                else if (player.yPos > blocksY[i])
                {
                    player.yPos += 10;
                }


                return true;
            }
        }




        return false;
    }
}