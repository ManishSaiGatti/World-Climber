using System;
using System.Collections.Generic;

class Class3
{
    public static readonly string Title = "IceClimber";
    public static readonly Vector2 Resolution = new Vector2(640, 480);

    readonly Texture _background = Engine.LoadTexture("Background.jpg");

    Player player = new Player();
    float playerVelocity = 0f;
    float maxVelocity = 5f;

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

    int blockY = 350;
    int subtract = 34;
    List<float> yLevels = new List<float>();

    bool generate = false;

    public Class3()
    {
        addInitialLayers();
        yLevels.Add(0);
        yLevels.Add(100);
        yLevels.Add(200);
        yLevels.Add(300);
        yLevels.Add(400);
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

        if (Engine.GetKeyHeld(Key.Left))
        {
            player.left();
        }

        if (Engine.GetKeyHeld(Key.Right))
        {
            player.right();
        }

        if (Engine.GetKeyDown(Key.X))
        {
            player.yPos = 350;
        }

        if (player.xPos < -13)
        {
            player.xPos = 640;
        }
        if (player.xPos > 640)
        {
            player.xPos = -13;
        }

        bool isInRange = false;
        for (int i = 0; i < blocksY.Count; i++)
        {
            if (blocksY[i] < player.yPos + 33f && blocksY[i] > player.yPos + 27f)
            {
                isInRange = true;
                player.yPos = blocksY[i] - 30f;
                break;
            }
        }

        bool stopMoving = false;
        if (isInRange && playerVelocity <= 0 && playerIsOverlapping())
        {
            stopMoving = true;
        }

        if (Engine.GetKeyDown(Key.Up) && isInRange && stopMoving)
        {
            playerVelocity = maxVelocity;
            player.up(playerVelocity);
            generate = true;
        }

        if (!stopMoving)
        {
            player.up(playerVelocity);
            playerVelocity -= 0.1f;
        }

        if (player.yPos > Resolution.Y / 2 - 3 && player.yPos < Resolution.Y / 2 + 3 && generate)
        {
            for (int i = 0; i < blocksY.Count; i++)
            {
                blocksY[i] = blocksY[i] + 30;
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
                blockHitCount.Add(1);
            }
        }
    }

    public void addLayer()
    {
        if (blocksY.Count == 0)
        {
            blocksX.Add(0);
            blocksY.Add(0);
            blockHitCount.Add(1);

            blocksX.Add(620);
            blocksY.Add(0);
            blockHitCount.Add(1);

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
                blockHitCount.Add(1);
            }

            for (int i = bound + 60; i <= Resolution.X; i += 20)
            {
                blocksX.Add(i);
                blocksY.Add(0);
                blockHitCount.Add(1);
            }

            totalPoints++;
        }

    }

    //check if the player overlaps with any blocks
    public bool playerIsOverlapping()
    {

        Bounds2 spritePosition = new Bounds2(new Vector2(player.xPos, player.yPos), new Vector2(13, 30));

        for (int i = 0; i < blocksX.Count; i++)
        {
            Bounds2 floorBounds = new Bounds2(new Vector2(blocksX[i], blocksY[i]), new Vector2(20, 20));
            if (spritePosition.Overlaps(floorBounds))
            {
                playerVelocity = 0;
                //correct the error
                if (player.yPos + 12 <= blocksY[i] - 30)
                {
                    return false;
                }
                else if (player.yPos > blocksY[i])
                {
                    player.yPos += 10;
                    blockHitCount[i] = 0;
                }

                if (blockHitCount[i] == 0)
                {
                    blocksX.RemoveAt(i);
                    blocksY.RemoveAt(i);
                    blockHitCount.RemoveAt(i);

                    //lose points for breaking a block rather than going through the hole.
                    totalPoints -= 4;
                }
                return true;
            }
        }
        return false;
    }
}