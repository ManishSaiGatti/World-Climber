using System;
using System.Collections.Generic;

class Game
{
    public static readonly string Title = "IceClimber";
    public static readonly Vector2 Resolution = new Vector2(640, 480);

    readonly Texture _background = Engine.LoadTexture("Background.jpg");

    readonly Texture _sprite = Engine.LoadTexture("player.png");

    readonly Font font = Engine.LoadFont("OpenSans-Regular.ttf", 10);

    readonly Texture prizeSkin = Engine.LoadTexture("eastern_orthodox_cross.png");


    Player player = new Player();
    float playerVelocity = 0f;
    float maxVelocity = 5f;
    float origVelocity = 0f;

    Enemy enemy1 = new Enemy();
    Boolean enemy1OnScreen = false;
    Boolean enemy1MoveLeft = true;

    int blockSizeX = 20;
    int blockSizeY = 25;


    readonly Texture _block = Engine.LoadTexture("square.png");

    List<Block> blocks = new List<Block>();


    //bonus trinkets
    readonly Texture trinketSkin = Engine.LoadTexture("eastern_orthodox_cross.png");
    List<int> trinketX = new List<int>();
    List<int> trinketY = new List<int>();
    int trinketSizeX = 20;
    int trinketSizeY = 30;

    int totalPoints = 0;

    Boolean gameOver = false;

    bool generate = false;


    public Game()
    {
        addInitialLayers();
    }

    public void Update()
    {
        Engine.DrawTexture(_background, Vector2.Zero);
        Engine.DrawTexture(player.getTexture(), player.getVectorPos());

        //draw levels
        for (int i = 0; i < blocks.Count; i++)
        {
            blocks[i].drawBlock();
        }

        //if statement inside addLayer to see if a layer sould be added
        addLayer();

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
        for (int i = 0; i < blocks.Count; i++)
        {
            if (blocks[i].getY() < player.yPos + 33f && blocks[i].getY() > player.yPos + 27f)
            {
                isInRange = true;
                player.yPos = blocks[i].getY() - 30f;
                //origVelocity = 0;
                //break;
            }

            Block currentBlock = blocks[i];
            Bounds2 blockBounds = new Bounds2(currentBlock.getLocation(), new Vector2(20, 25));
            if(playerBound.Overlaps(blockBounds))
            {
                if(player.xPos >= currentBlock.getX() + 20)
                {
                    canMoveLeft = false;
                    isInRange = false;
                    player.xPos = currentBlock.getX() + 22;
                    i = blocks.Count + 1;
                }
                if(player.xPos + 13 <= currentBlock.getX())
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
            playerVelocity = maxVelocity;
            origVelocity = maxVelocity;
            player.up(playerVelocity);
            generate = true;
        }

        if (!stopMoving)
        {
            player.up(playerVelocity);
            playerVelocity -= .1f;
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

            if (enemy1OnScreen)
            {
                enemy1.setEnemyY(enemy1.getEnemyY() + 30);

            }
        }

        if (enemy1OnScreen)
        {

            enemy1.drawEnemy();
            if (!enemy1MoveLeft)
            {
                enemy1.setEnemyX(enemy1.getEnemyX() + 1);
                if (enemy1.getEnemyX() == enemy1.getInitialX())
                {
                    enemy1MoveLeft = true;
                }
            }
            else if (enemy1MoveLeft)
            {
                enemy1.setEnemyX(enemy1.getEnemyX() - 1);
                if (enemy1.getEnemyX() < enemy1.getInitialX() - 50)
                {
                    enemy1MoveLeft = false;
                }
            }

        }

        if (playerIsOverlapping())
        {
            Engine.DrawString("OVERLAPPING", new Vector2(10, 440), Color.Red, font);
        }

        //displaying the number of points
        Engine.DrawString(totalPoints.ToString(), new Vector2(Resolution.X - 10, 10), Color.Red, font);


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
                trinketX.RemoveAt(i);
                trinketY.RemoveAt(i);
                totalPoints += 50;
            }
        }

    }

    public void addInitialLayers()
    {
        for (int y = 0; y < (int)Resolution.Y; y += 100)
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

            totalPoints++;
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

            if (!enemy1OnScreen && rand.Next(1, 5) == 3)
            {
                enemy1 = new Enemy();
                enemy1MoveLeft = true;
                enemy1OnScreen = true;
            }
            if (enemy1.getEnemyY() > 640)
            {
                enemy1OnScreen = false;
            }




            createTrinket();

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
                    player.yPos += 10;
                    blocks[i].blockHit();
                }

                if (blocks[i].getBlockHp() == 0)
                {
                    blocks.RemoveAt(i);

                    //lose points for breaking a block rather than going through the hole.
                    totalPoints -= 4;
                }
                return true;
            }
        }
        Bounds2 enemyBounds = new Bounds2(new Vector2(enemy1.getEnemyX(), enemy1.getEnemyY())
            , new Vector2(29, 29));
        if (spritePosition.Overlaps(enemyBounds))
        {            
            gameOver = true;
            totalPoints = -1;
        }


        return false;
    }

    public bool GameOverByGoingOutsideOfBorders()
    {
        gameOver = true;
        if (player.yPos + blockSizeY > (int)Resolution.Y)
        {
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





}