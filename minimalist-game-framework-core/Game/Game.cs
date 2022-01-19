using System;
using System.Collections.Generic;
using System.IO;

class Game
{
    public static readonly string Title = "IceClimber";
    public static readonly Vector2 Resolution = new Vector2(640, 480);

    readonly Texture _background = Engine.LoadTexture("Background.jpg");

    readonly Texture _sprite = Engine.LoadTexture("player.png");

    readonly Font font = Engine.LoadFont("OpenSans-Regular.ttf", 10);

    readonly Texture prizeSkin = Engine.LoadTexture("eastern_orthodox_cross.png");

    int spriteX = 320;
    int spriteY = 280;

    int spriteSizeX = 20;
    int spriteSizeY = 25;

    readonly Texture _block = Engine.LoadTexture("square.png");

    //number to indicate what screen you are on menu screen or game screen
    int screenNumber = 0;

    List<int> blocksX = new List<int>();
    List<int> blocksY = new List<int>();
    //amount of time block needs to be hit before breaking
    List<int> blockHitCount = new List<int>();

    //Non-enemy obsticle
    Texture obsticleSkin1 = Engine.LoadTexture("Spike.png");
    List<int> obsticleX = new List<int>();
    List<int> obsticleY = new List<int>();
    List<Texture> obsticleImage = new List<Texture>();
    
    //bonus trinkets
    readonly Texture trinketSkin = Engine.LoadTexture("eastern_orthodox_cross.png");
    List<int> trinketX = new List<int>();
    List<int> trinketY = new List<int>();
    int trinketSizeX = 20;
    int trinketSizeY = 30;

    int totalPoints = 0;

    List<int> scores = new List<int>();
    public Game()
    {
        addInitialLayers();

        //get the highest scores
        string path = @"C:\Users\dandu\source\repos\recreate-a-classic-game-ice-climber\minimalist-game-framework-core\Assets\Leaderboards.txt";

        using (StreamReader sr = File.OpenText(path))
        {
            string s;
            while ((s = sr.ReadLine()) != null)
            {
                scores.Add(int.Parse(s));
            }
        }

        scores.Sort();
       
    }

    public void Update()
    {
        if (screenNumber == 0)
        {
            Engine.DrawTexture(_background, Vector2.Zero);

            Bounds2 buttonBounds = new Bounds2(Resolution.X / 2 - 100, Resolution.Y / 2 - 50, 100, 50);
            Engine.DrawRectEmpty(buttonBounds, Color.DarkRed);
            Engine.DrawString("Play Game", new Vector2(buttonBounds.Position.X + 40, buttonBounds.Position.Y + 15), Color.DarkRed, font);

            if(scores.Count > 0)
                Engine.DrawString("Highest Score of All: " + scores[scores.Count - 1].ToString(), new Vector2(buttonBounds.Position.X, buttonBounds.Position.Y - 25), Color.DarkRed, font);

            if (Engine.GetMouseButtonDown(MouseButton.Left))
            {
                screenNumber = 1;
            }
        }
        else if (screenNumber == 1)
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

            //draw obsticles
            for (int i = 0; i < obsticleX.Count; i++) 
            {
                Vector2 vec = new Vector2(obsticleX[i], obsticleY[i]);
                Engine.DrawTexture(obsticleImage[i], vec, null, new Vector2(obsticleImage[i].Size.X / 100, obsticleImage[i].Size.Y / 100));
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

            //end game and add person to leaderboards
            //Note: Person will only be added to leaderboards if game ends in this way.
            if (Engine.GetKeyDown(Key.M))
            {
                string path = @"C:\Users\dandu\source\repos\recreate-a-classic-game-ice-climber\minimalist-game-framework-core\Assets\Leaderboards.txt";

                if (File.Exists(path))
                {
                    //Console.WriteLine("What is your name");
                    //string name = Console.ReadLine();
                    // Create a file to write to.
                    using (StreamWriter sw = File.CreateText(path))
                    {
                        sw.WriteLine(totalPoints);
                    }
                }

                scores.Sort();
                screenNumber = 0;
            }
        }
    }

    public void addInitialLayers() 
    {
        for(int y = 0; y < (int)Resolution.Y; y += 100) 
        {
            for (int i = 0; i <= Resolution.X; i += 20)
            {
                blocksX.Add(i);
                blocksY.Add(y);
                blockHitCount.Add(4);
            }
        }
    }
    
    //returns an int array of two x-coordinates with the first representing the x-coordinate of the square
    //at the beginning of the gap and the second being the x-coordinate that represents the last
    //point of the square of the gap.
    //*You just have to run the program and you will know what I mean
    public int[] addLayer() 
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

            return null;
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

            int[] intervalOfGap = { bound, bound + 40 };

            return intervalOfGap;
        }

        return null;
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

    //trinket method
    public void createTrinket() 
    {
        Random rand = new Random();
        int bound = rand.Next(0, (int)Resolution.X);

        trinketX.Add(bound);
        trinketY.Add(blocksY[blocksY.Count - 1] - 30);        
    }


    public void createNonEnemyObsticles() 
    {
        Random rand = new Random();
        int bound = rand.Next(0, (int)Resolution.X);

        obsticleX.Add(bound);
        obsticleY.Add(blocksY[blocksY.Count - 1] - 30);
        obsticleImage.Add(obsticleSkin1);
    }




}
