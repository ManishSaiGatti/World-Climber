﻿
using System;
using System.Collections.Generic;
using System.IO;

class Game
{
    public static readonly string Title = "IceClimber";
    public static readonly Vector2 Resolution = new Vector2(640, 480);

    readonly Texture _background = Engine.LoadTexture("Background.jpg");
    readonly Texture _sprite = Engine.LoadTexture("player.png");
    readonly Font font = Engine.LoadFont("OpenSans-Regular.ttf", 20);
    readonly Texture whiteSpace = Engine.LoadTexture("whiteSpace.png");


    Player player = new Player();
    float playerVelocity = 0f;
    float maxVelocity = 5f;
    float origVelocity = 0f;

    List<Enemy> enemies = new List<Enemy>();
    List<SpaceEnemy> spaceEnemies = new List<SpaceEnemy>();
    Boolean enemyOnScreen = false;


    int blockSizeX = 20;
    int blockSizeY = 25;

    bool highScoreUpdate = false;

    readonly Texture _block = Engine.LoadTexture("square.png");

    List<Block> blocks = new List<Block>();


    //bonus trinkets
    readonly Texture trinketSkin = Engine.LoadTexture("star.png");
    List<int> trinketX = new List<int>();
    List<int> trinketY = new List<int>();
    int trinketSizeX = 30;
    int trinketSizeY = 30;

    int totalPoints = 0;
    int highScore = 0;

    Boolean gameOver = false;

    bool generate = false;

    readonly Texture intro = Engine.LoadTexture("n1.png");
    readonly Texture introHover = Engine.LoadTexture("n2.png");
    readonly Texture end = Engine.LoadTexture("n4.png");
    readonly Texture endHover = Engine.LoadTexture("n5.png");
    //mouse for starting game
    int mX = (int)Engine.MousePosition.X;
    int mY = (int)Engine.MousePosition.Y;
    // check if start
    Boolean start = true;
    Boolean play = false;
    Boolean endSc = false;

    private System.Timers.Timer timer;
    int timeLeft = 60;

    int biome = 0;
    // sand = -960
    // ice = -1420
    // space -3040
    float sandCheck = -960;
    float iceCheck = -1300;
    float spaceCheck = -2800;

    float underSandY = -960;
    float iceSpaceY = -2880;

    float scrollValue = 0;

    float spaceBack1Y = -3840;
    float spaceBack2Y = -4320;

    bool seeDebug = false;

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

    readonly Sound coinCollect = Engine.LoadSound("CollectCoinSound.wav");
    readonly Sound jump = Engine.LoadSound("jump.wav");
    readonly Sound blockBreak = Engine.LoadSound("blockBreak.wav");
    readonly Sound death = Engine.LoadSound("deathEffect.wav");

    readonly Music backMusic = Engine.LoadMusic("My Song 4.wav");


    public Game()
    {

        if (!File.Exists("HighScore.txt"))
        {
            // Create a file to write to.
            using (StreamWriter sw = File.CreateText("HighScore.txt")) {
                sw.WriteLine("0");
            }
        }
        using (StreamReader sr = File.OpenText("HighScore.txt"))
        {
            string s = "";
            while ((s = sr.ReadLine()) != null)
            {
                Console.WriteLine(s);
            }
        }
        highScore = Int32.Parse(File.ReadAllText("HighScore.txt"));
        Engine.PlayMusic(backMusic);
        addInitialLayers();
        player.yPos = 400;
    }
    public Texture biomeFloor()
    {
        if (biome == 0)
        {
            return floorUnder;
        }
        if (biome == 2)
        {
            return floorIce;
        }
        if (biome == 1)
        {
            return floorSand;
        }

        return floorSpace;
    }

    public void Update()
    {
        if (timeLeft == 0)
        {
            endGame();
        }
        if (sandCheck >= player.yPos)
        {
            if (biome == 0)
            {
                biome = 1;
                timeLeft += 20;
            }
        }
        if (iceCheck >= player.yPos)
        {
            if (biome == 1)
            {
                biome = 2;
                timeLeft += 20;
            }
        }
        if (spaceCheck >= player.yPos)
        {
            if (biome == 2)
            {
                biome = 3;
                timeLeft += 20;
            }
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

        mX = (int)Engine.MousePosition.X;
        //Engine.DrawString(Engine.MouseMotion.X.ToString(), Vector2.Zero, Color.Black, font);
        mY = (int)Engine.MousePosition.Y;
        if (start)
        {
            // checking if intro screen should be displayed
            //Console.WriteLine("start");
            Engine.DrawTexture(intro, Vector2.Zero);
        }
        if (mX > 95 && mX < 542 && mY > 306 && mY < 350 && start)
        {
            Engine.DrawTexture(introHover, Vector2.Zero);
            if (Engine.GetMouseButtonDown(MouseButton.Left))
            {
                //changing to green while hovering and starting game if clicked
                start = false;
                play = true;
                // set up timer
                timer = new System.Timers.Timer(1000);
                timer.Elapsed += OnTimedEvent;
                timer.AutoReset = true;
                timer.Enabled = true;
            }
        }
        if (mX > 0 && mX < 640 && mY > 0 && mY < 480 && endSc)
        {
            Engine.DrawTexture(endHover, Vector2.Zero);
        }
        if (play)
        {
            //Console.WriteLine("play");
            //Engine.DrawTexture(_background, Vector2.Zero);
            Engine.DrawTexture(player.getTexture(), player.getVectorPos());

            //draw levels
            for (int i = 0; i < blocks.Count; i++)
            {
                blocks[i].drawBlock(biomeFloor());
            }

            //if statement inside addLayer to see if a layer sould be added
            addLayer();

            // check if game is Over
            checkTouchingEnemy();

            //Engine.DrawTexture(_sprite, new Vector2(spriteX, spriteY), null, new Vector2(20, 25));

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
            bool canMoveRight = true;
            bool canMoveLeft = true;
            //Bounds2 playerRight = new Bounds2(player.getVectorPos(), new Vector2(13, 7));
            //Bounds2 playerLeft = new Bounds2(new Vector2(player.xPos - 7, player.yPos), new Vector2(13, 7));
            Bounds2 playerBound = new Bounds2(new Vector2(player.xPos - 2, player.yPos + 2), new Vector2(17, 26));
            Bounds2 playerBottomBound = new Bounds2(new Vector2(player.xPos, player.yPos + 25), new Vector2(13, 5));
            for (int i = 0; i < blocks.Count; i++)
            {
                Block currentBlock = blocks[i];
                Bounds2 blockBounds = new Bounds2(currentBlock.getLocation(), new Vector2(20, 25));
                if (playerBottomBound.Overlaps(blockBounds))
                {
                    isInRange = true;
                    player.yPos = blocks[i].getY() - 30f;
                }

                if (playerBound.Overlaps(blockBounds))
                {
                    if (player.xPos >= currentBlock.getX() + 20)
                    {
                        canMoveLeft = false;
                        isInRange = false;
                        player.xPos = currentBlock.getX() + 22;
                        i = blocks.Count + 1;
                    }

                    if (player.xPos + 13 <= currentBlock.getX())
                    {
                        canMoveRight = false;
                        isInRange = false;
                        player.xPos = currentBlock.getX() - 15;
                        i = blocks.Count + 1;
                    }
                }
            }

            bool stopMoving = false;
            if (isInRange && playerVelocity <= 0 && playerIsOverlapping())
            {
                stopMoving = true;
                origVelocity = 0;
            }

            if (Engine.GetKeyDown(Key.Up) && isInRange && stopMoving)
            {
                Engine.PlaySound(jump);
                playerVelocity = maxVelocity;
                origVelocity = maxVelocity;
                player.up(playerVelocity);
                generate = true;
            }

            if (!stopMoving)
            {
                player.up(playerVelocity);
                playerVelocity -= 0.1f;
                origVelocity = playerVelocity;
            }

            bool horizontalMove = origVelocity == 0 && !playerIsOverlapping();
            if (!horizontalMove)
            {
                if (Engine.GetKeyHeld(Key.Left) && canMoveLeft)
                {
                    player.left();
                }

                if (Engine.GetKeyHeld(Key.Right) && canMoveRight)
                {
                    player.right();
                }
            }

            if (player.yPos > Resolution.Y / 2 - 3 && player.yPos < Resolution.Y / 2 + 3 && generate)
            {
                for (int i = 0; i < blocks.Count; i++)
                {
                    blocks[i].changeY(blocks[i].getY() + 30);
                }

                //fixing the trinkets
                for (int i = 0; i < trinketY.Count; i++)
                {
                    trinketY[i] = trinketY[i] + 30;
                }

                player.yPos += 20;

                foreach (Enemy enemy in enemies)
                {
                    if (enemyOnScreen)
                    {
                        enemy.setEnemyY(enemy.getEnemyY() + 30);

                    }
                }

                scrollValue += 10f;
                spaceBack1Y += 10f;
                spaceBack2Y += 10f;
                underSandY += 10f;
                iceSpaceY += 10f;

                sandCheck += 10f;
                iceCheck += 10f;
                spaceCheck += 10f;
            }

            foreach (Enemy enemy in enemies)
            {
                if (enemyOnScreen)
                {

                    enemy.drawEnemy();
                    enemy.enemyAct();

                }
            }
            playerIsOverlapping();
            ghostBlockBreak();

            //displaying the number of points
            Engine.DrawTexture(whiteSpace, new Vector2(Resolution.X - 50, -5));

            Engine.DrawString(totalPoints.ToString(), new Vector2(Resolution.X - 50, 0), Color.Black, font);

            // displaying current time left
            Engine.DrawTexture(whiteSpace, new Vector2(-120, -5));
            Engine.DrawString("Time: " +timeLeft.ToString(), new Vector2(0, 0), Color.Black, font);


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

                Bounds2 playerBounds = new Bounds2(player.xPos, player.yPos, 13, 30);

                if (playerBounds.Overlaps(trinketBounds))
                {
                    Engine.PlaySound(coinCollect);
                    trinketX.RemoveAt(i);
                    trinketY.RemoveAt(i);
                    timeLeft += 5;
                }
            }
            if (Engine.GetKeyDown(Key.F3))
            {
                if (seeDebug)
                {
                    seeDebug = false;
                }
                else if (!seeDebug)
                {
                    seeDebug = true;
                }
            }
            if (seeDebug)
            {
                Engine.DrawString("yPos: " + player.yPos, new Vector2(50, 300), Color.AliceBlue, font);
                Engine.DrawString("sandCh: " + sandCheck, new Vector2(50, 100), Color.AliceBlue, font);
                Engine.DrawString("iceCh: " + iceCheck, new Vector2(50, 150), Color.AliceBlue, font);
                Engine.DrawString("spaceCh: " + spaceCheck, new Vector2(50, 200), Color.AliceBlue, font);
                Engine.DrawString("biome: " + biome, new Vector2(50, 250), Color.AliceBlue, font);
                Engine.DrawString("Debug: ", new Vector2(50, 50), Color.AliceBlue, font);
            }

        }
        if (gameOver == true)
        {
            endSc = true;
            Engine.DrawTexture(end, Vector2.Zero);
            if (!highScoreUpdate)
            {

                
                if (File.ReadAllText("HighScore.txt").Equals(""))
                {
                        highScore = totalPoints;
                        File.WriteAllTextAsync("HighScore.txt", totalPoints.ToString());
                }
                else if (highScore < totalPoints)
                {
                    //Console.WriteLine(highScore);
                    highScore = totalPoints;
                    File.WriteAllText("HighScore.txt", String.Empty);
                    File.WriteAllTextAsync("HighScore.txt", totalPoints.ToString());

                }
                highScoreUpdate = true;

            }
            Engine.DrawString("High Score: " + highScore.ToString(), Vector2.Zero, Color.White, font);
        }
        //  Engine.DrawString(endSc.ToString(), Vector2.Zero, Color.White, font);
        if (mX > 157 && mX < 488 && mY > 210 && mY < 241 && endSc)
        {
            Engine.DrawTexture(endHover, Vector2.Zero);
            Engine.DrawString("High Score: " + highScore.ToString(), Vector2.Zero, Color.White, font);
            if(Engine.GetMouseButtonDown(MouseButton.Left))
            {
                //Console.WriteLine("end screen click");
                gameOver = false;
                endSc = false;
                play = false;
                start = true;
                highScoreUpdate = false;

                blocks.Clear();
                trinketX.Clear();
                trinketY.Clear();
                enemies.Clear();

                highScore = Int32.Parse(File.ReadAllText("HighScore.txt"));

                addInitialLayers();
                player.yPos = 400;
                player.xPos = 240;

                timer.Dispose();
                timeLeft = 60;
                totalPoints = 0;

                biome = 0;
                sandCheck = -960;
                iceCheck = -1300;
                spaceCheck = -2800;
                underSandY = -960;
                iceSpaceY = -2880;
                scrollValue = 0;
                spaceBack1Y = -3840;
                spaceBack2Y = -4320;

                Engine.PlayMusic(backMusic);
            }
        }

    }

    public void addInitialLayers()
    {
        for (int y = 0; y < (int)Resolution.Y; y += 110)
        {
            for (int i = 0; i <= Resolution.X; i += 20)
            {

                blocks.Add(new Block(i, y, 1));
            }
        }
    }
    public void addLayer()
    {
        // create initial layer
        if (blocks.Count == 0)
        {
            blocks.Add(new Block(0, 0, 1));

            blocks.Add(new Block(620, 0, 1));

            
        }
        // create new layer
        else if (blocks[blocks.Count - 2].getY() > 100 && blocks[blocks.Count - 1].getY() > 100)
        {
            Random rand = new Random();
            int bound = rand.Next(0, (int)Resolution.X);

            while (bound % 20 != 0)
            {
                bound = rand.Next(0, (int)Resolution.X - 60);
            }

            for (int i = 0; i < bound; i += 20)
            {

                blocks.Add(new Block(i, 0, 1));
            }

            for (int i = bound + 60; i <= Resolution.X; i += 20)
            {
                blocks.Add(new Block(i, 0, 1));
            }

            totalPoints++;

            // Spawn Enemies!
            if (rand.Next(1, 5) == 3)
            {
                enemies.Add(new DirtEnemy());
                enemyOnScreen = true;
            }
            if(biome >= 1 && rand.Next(1,3) == 1)
            {
                enemies.Add(new SandEnemy());
                enemyOnScreen = true;
            }
            if (biome >= 2 && rand.Next(1, 6) == 1)
            {
                enemies.Add(new IceEnemy(player));
                enemyOnScreen = true;
            }
            if (biome >= 3 && rand.Next(1, 10) == 1)
            {
                SpaceEnemy ghostEnemy= new SpaceEnemy(player);
                enemies.Add(ghostEnemy);
                spaceEnemies.Add(ghostEnemy);
                enemyOnScreen = true;
                
            }
            List<Enemy> offScreen = new List<Enemy>();
            foreach (Enemy enemy in enemies)
            {
                if (enemy.getEnemyY() > 480)
                {
                    enemyOnScreen = false;
                    offScreen.Add(enemy);
                } else if(enemy.getEnemyY() < 480)
                {
                    enemyOnScreen = true;
                    break;
                }
            }
            foreach(Enemy enemy in offScreen)
            {
                enemies.Remove(enemy);
            }

            if (rand.Next(1, 4) == 1)
            {
                createTrinket();
            }

        }

    }

    //check if the player overlaps with any blocks
    public bool playerIsOverlapping()
    {
        Bounds2 spritePosition = new Bounds2(new Vector2(player.xPos, player.yPos), new Vector2(13, 30));

        for (int i = 0; i < blocks.Count; i++)
        {
            Bounds2 floorBounds = new Bounds2(new Vector2(blocks[i].getX(), blocks[i].getY()), new Vector2(20, 20));
            if (spritePosition.Overlaps(floorBounds))
            {
                playerVelocity = 0;
                //correct the y - error
                if (player.yPos + 12 <= blocks[i].getY() - 30)
                {
                    return false;
                }
                else if (player.yPos > blocks[i].getY())
                {
                    Engine.PlaySound(blockBreak);
                    player.yPos += 10;
                    blocks[i].blockHit();
                }

                if (blocks[i].getBlockHp() == 0)
                {
                    blocks.RemoveAt(i);

                    //lose points for breaking a block rather than going through the hole.
                    //totalPoints -= 4;
                }
                return true;
            }
        }
        
        


        return false;
    }

    public void ghostBlockBreak()
    {
        foreach(SpaceEnemy enemy in spaceEnemies)
        {
            for (int i = 0; i < blocks.Count; i++)
            {
                Bounds2 floorBounds = new Bounds2(new Vector2(blocks[i].getX(), blocks[i].getY()), new Vector2(20, 20));
                Bounds2 enemyBounds = new Bounds2(new Vector2(enemy.getEnemyX(), enemy.getEnemyY())
                , new Vector2(29, 29));
                if (enemyBounds.Overlaps(floorBounds))
                {
                    blocks.RemoveAt(i);
                    
                }
            }
        }
    }
    public bool checkTouchingEnemy()
    {
        Bounds2 spritePosition = new Bounds2(new Vector2(player.xPos, player.yPos), new Vector2(13, 30));
        foreach (Enemy enemy in enemies)
        {
            Bounds2 enemyBounds = new Bounds2(new Vector2(enemy.getEnemyX(), enemy.getEnemyY())
                , new Vector2(29, 29));
            if (spritePosition.Overlaps(enemyBounds) || player.yPos > 480)
            {
                endGame();

                return true;
            }
        }
        return false;
    }

    public void endGame()
    {
        Engine.StopMusic();
        Engine.PlaySound(death);
        gameOver = true;
        play = false;
        endSc = true;
    }
    public bool GameOverByGoingOutsideOfBorders()
    {
        gameOver = true;
        if (player.yPos + blockSizeY > (int)Resolution.Y)
        {
            Engine.StopMusic();
            Engine.PlaySound(death);
            endSc = true;
            play = false;
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
        trinketY.Add(blocks[blocks.Count - 1].getY() - 30);
    }

    public void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
    {
        timeLeft -= 1;
    }

    
}