using System;
using System.Collections.Generic;
using System.Text;


class SandEnemy : Enemy
{
    public String moveDirection;
    public int yDifference; 
    public SandEnemy(){
        moveDirection = "right";
        enemyTexture = Engine.LoadResizableTexture("12.png", 0, 0, 0, 0);
        yDifference = 0;
    }

    public override void drawEnemy()
    {
        Engine.DrawResizableTexture(enemyTexture, new Bounds2(enemyX, enemyY, 30, 30));
    }
    public override void enemyAct()
    {
        if (moveDirection.Equals("right"))
        {
            enemyX = enemyX + 1;
            if(enemyX > initialX + 50)
            {
                moveDirection = "up";
            }
        } 
        else if (moveDirection.Equals("up"))
        {
            yDifference -= 1;
            enemyY -= 1;
            if (yDifference <= -50)
            {
                moveDirection = "left";
            }
        }
        else if (moveDirection.Equals("left"))
        {
            enemyX -= 1;
            if (enemyX <= initialX)
            {
                moveDirection = "down";
            }
        }
        else if (moveDirection.Equals("down"))
        {
            yDifference += 1;
            enemyY += 1;
            if (yDifference >= 0)
            {
                moveDirection = "right";
            }
        }
    }

    
}

