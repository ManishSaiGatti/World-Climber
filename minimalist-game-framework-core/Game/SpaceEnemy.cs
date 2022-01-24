using System;
using System.Collections.Generic;
using System.Text;


class SpaceEnemy : Enemy
{
    Player player;
    public SpaceEnemy(Player player)
    {
        enemyTexture = enemyTexture = Engine.LoadResizableTexture("13.svg", 0, 0, 0, 0);
        this.player = player;
    }
    public override void enemyAct()
    {
        Vector2 playerLocation = player.getVectorPos();
        if (playerLocation.X > enemyX)
        {
            enemyX += 1;
        }
        else if (playerLocation.X < enemyX)
        {
            enemyX -= 1;
        }
        else if (playerLocation.Y < enemyY)
        {
            enemyY -= 1;
        }
        else if (playerLocation.Y > enemyY)
        {
            enemyY += 1;
        }
    }
}
