using System;
using System.Collections.Generic;
using System.Text;

class IceEnemy : Enemy
{
    Player player;
    public IceEnemy(Player player)
    {
        enemyTexture = Engine.LoadResizableTexture("11.svg", 0, 0, 0, 0);
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
    }
}
