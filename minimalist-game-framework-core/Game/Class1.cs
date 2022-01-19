using System;
using System.Collections.Generic;

class Class1
{
    public static readonly string Title = "Minimalist Game Framework";
    public static readonly Vector2 Resolution = new Vector2(640, 480);


    readonly Font font = Engine.LoadFont("OpenSans-Regular.ttf", 10);
    readonly Texture prizeSkin = Engine.LoadTexture("eastern_orthodox_cross.png");






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
    readonly Texture iceSpace = Engine.LoadTexture("iceSpace.png");

    readonly Texture player = Engine.LoadTexture("player.png");


    int biome = 1;

    private int screen = 1;
    private float scrollValue = 0;

    private float spaceBack1Y = -3840;
    private float spaceBack2Y = -4320;


    int spriteX = 320;
    int spriteY = 280;

    int spriteSizeX = 20;
    int spriteSizeY = 25;

    //readonly Texture _block = Engine.LoadTexture("square.png");

    List<int> blocksX = new List<int>();
    List<int> blocksY = new List<int>();
    //amount of time block needs to be hit before breaking
    List<int> blockHitCount = new List<int>();


    //bonus trinkets
    readonly Texture trinketSkin = Engine.LoadTexture("eastern_orthodox_cross.png");
    List<int> trinketX = new List<int>();
    List<int> trinketY = new List<int>();
    int trinketSizeX = 20;
    int trinketSizeY = 30;

    int totalPoints = 0;

    float underSandY = -960;
    float iceSpaceY = -2880;

    public Class1()

    {
        addInitialLayers();
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

        Engine.DrawTexture(underBack, new Vector2(0, 0 + scrollValue));
        Engine.DrawTexture(underSand, new Vector2(0, underSandY));
        Engine.DrawTexture(sandBack, new Vector2(0, -1440 + scrollValue));
        Engine.DrawTexture(iceBack, new Vector2(0, -1920 + scrollValue));
        Engine.DrawTexture(iceSpace, new Vector2(0, iceSpaceY));
        Engine.DrawTexture(spaceBack, new Vector2(0, -3360 + scrollValue));
        Engine.DrawTexture(spaceBlack1, new Vector2(0, spaceBack1Y));
        Engine.DrawTexture(spaceBlack2, new Vector2(0, spaceBack2Y));

        if (spaceBack1Y >= 480)
        {
            spaceBack1Y = -480;
        }
        if (spaceBack2Y >= 480)
        {
            spaceBack2Y = -480;
        }

        if (Engine.GetKeyHeld(Key.Up))
        {
            scrollValue += 1f;
            spaceBack1Y += 1f;
            spaceBack2Y += 1f;
            underSandY += 1f;
            iceSpaceY += 1f;

        }


        //else if (Engine.GetKeyHeld(Key.Down))
        //{
        //scrollValue -= 1f;
        //spaceBack1Y -= 1f;
        //spaceBack2Y -= 1f;
        //}




        /*
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
        */



        if (playerHitsBorders())
        {
            Engine.DrawString("HITTING BORDERS", new Vector2(10, 440), Color.Red, font);
        }



        //draw levels
        for (int i = 0; i < blocksX.Count; i++)
        {

            Vector2 vec = new Vector2(blocksX[i], blocksY[i]);




            Engine.DrawTexture(checkBiome(), vec, null, new Vector2(spriteSizeX, spriteSizeY));



        }

        //if statement inside addLayer to see if a layer sould be added
        addLayer();

        Engine.DrawTexture(player, new Vector2(spriteX, spriteY), null, new Vector2(20, 25));

        if (Engine.GetKeyDown(Key.Left, true))
        {
            spriteX -= 10;
        }
        if (Engine.GetKeyDown(Key.Right, true))
        {
            spriteX += 10;
        }
        if (Engine.GetKeyDown(Key.Up, true))
        {
            spriteY -= 10;
        }
        /*
        if (Engine.GetKeyDown(Key.Down, true))
        {
            spriteY += 10;
        }
        */
        if (spriteY == Resolution.Y / 2)
        {
            for (int i = 0; i < blocksY.Count; i++)
            {
                blocksY[i] = blocksY[i] + 10;
            }

            //fixing the trinkets
            for (int i = 0; i < trinketY.Count; i++)
            {
                trinketY[i] = trinketY[i] + 10;
            }

            spriteY += 20;
        }

        if (playerIsOverlapping())
        {
            Engine.DrawString("OVERLAPPING", new Vector2(10, 440), Color.Red, font);
        }

        //displaying the number of points
        Engine.DrawString(totalPoints.ToString(), new Vector2(440, 440), Color.Red, font);


        //trinket code

        //draw all the trinkets
        for (int i = 0; i < trinketX.Count; i++)
        {
            Vector2 vec = new Vector2(trinketX[i], trinketY[i]);
            Engine.DrawTexture(trinketSkin, vec, null, new Vector2(trinketSizeX, trinketSizeY));

        }

        //collect trinkets
        for (int i = 0; i < trinketX.Count; i++)
        {
            Bounds2 trinketBounds = new Bounds2(trinketX[i], trinketY[i], trinketSizeX, trinketSizeY);

            Bounds2 playerBounds = new Bounds2(spriteX, spriteY, spriteSizeX, spriteSizeY);

            if (playerBounds.Overlaps(trinketBounds))
            {
                trinketX.RemoveAt(i);
                trinketY.RemoveAt(i);
                totalPoints += 50;
            }
        }

        Engine.DrawString("L: " + (underSandY), new Vector2(200, 200), Color.AliceBlue, font);
        Engine.DrawString("L: " + (iceSpaceY), new Vector2(200, 250), Color.AliceBlue, font);
        Engine.DrawString("L: " + (spriteY), new Vector2(200, 300), Color.AliceBlue, font);

        Engine.DrawString("L: " + (biome), new Vector2(200, 350), Color.AliceBlue, font);
    }


    public Texture checkBiome()
    {
        if (biome == 1)
        {

            if (underSandY <= spriteY - 100)
            {
                return floorUnder;
            }
            if (spriteY - 100 <= underSandY)
            {
                biome = 2;
                return floorSand;
            }
        }
        if (biome == 2)
        {
            if (spriteY - 100 <= iceSpaceY)
            {
                biome = 3;
                return floorSpace;
            }
            else
            {
                return floorSand;
            }
        }
        else
        {
            return floorSpace;
        }

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
            blockHitCount.Add(4);

            blocksX.Add(620);
            blocksY.Add(0);
            blockHitCount.Add(4);

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
                blockHitCount.Add(4);
            }

            for (int i = bound + 60; i <= Resolution.X; i += 20)
            {
                blocksX.Add(i);
                blocksY.Add(0);
                blockHitCount.Add(4);
            }

            totalPoints++;

            createTrinket();
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
                    blockHitCount[i] = blockHitCount[i] - 1;
                }
                else if (spriteY > blocksY[i])
                {
                    spriteY += 10;
                    blockHitCount[i] = blockHitCount[i] - 1;
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

    public bool playerHitsBorders()
    {
        //borders
        if (spriteX < 0)
        {
            spriteX += 10;
            return true;
        }
        if (spriteX + spriteSizeX > (int)Resolution.X)
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

    //trinket method
    public void createTrinket()
    {
        Random rand = new Random();
        int bound = rand.Next(0, (int)Resolution.X);

        trinketX.Add(bound);
        trinketY.Add(blocksY[blocksY.Count - 1] - 30);
    }
}

