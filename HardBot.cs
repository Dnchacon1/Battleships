using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships
{
    internal class HardBot
    {
        //build fields for the hardbot to reference
        private static int[,] MyField = new int[8, 8];
        private static int[,] EnemyField = new int[8, 8];
        //create flags for the advanced attack method. X and Y are indexes of last shot location
        private static int LastHitX = 0;
        private static int LastHitY = 0;
        private static bool Hit = false;
        private static (int, int) HitLoaction = (0, 0);
        private static bool ShipFound = false;



        public static void StartUp()
        {
            MyField.Initialize();
            EnemyField.Initialize();
            PlaceShip(MyField);
        }
        public static int[,] PlaceShip(int[,] field)
        {
            Random random = new Random();
            //place a ship randomly in the array, without reaching out of bounds.
            int length = field.GetLength(0);
            int randomIndexX = random.Next(0, length - 2);
            int randomIndexY = randomIndexX;
            for (int i = 0; i < 3; i++)
            {
                field[randomIndexX, randomIndexY] = 1;
                randomIndexY++;
            }
            return field;
        }

        public static int[,] GetMyField()
        {
            return MyField;
        }

        public static int[,] GetEnemyField()
        {
            return EnemyField;
        }

        public static void UpdateHitFlag(bool x, (int, int) loc)
        {
           Hit = x;
           HitLoaction = loc;
           ShipFound = true;
        }

        public static (int, int) NewAttack()
        {
            //attack the enemy field in a horizontal pattern, skipping every 2 spaces since the ship will be 3 unit long
            //create copy of lasthitx and lasthity to return without adjusting class level vars yet
            int NewX = LastHitX;
            int NewY = LastHitY;
            if(Hit == false || ShipFound == false)
            {
                if(LastHitY < 7)
                {
                    //check to see if shots are at the horizontal end of the board
                    if(LastHitY != 6) { LastHitY += 2; }
                    else { LastHitY++; }
                    return (NewX, NewY);
                }
                else if (LastHitX < 7)
                {
                    //if shots are at the horizontal end of the board, and NOT at the vertical end, move to the next row
                    LastHitY = 2; NewY = 0;
                    LastHitX++;
                    return (LastHitX, NewY);
                }
                //if the bot has reached the end of the board, shoot randomly
                return Attack();
            }
            else
            {
                if (HitLoaction.Item2 - 1 >= 0)
                {
                    return (HitLoaction.Item1, HitLoaction.Item2 - 1);
                }
                else if (HitLoaction.Item2 + 2 < 8)
                {
                    return (HitLoaction.Item1, HitLoaction.Item2 + 2);
                }
                else if(ShipFound)
                {
                    return (HitLoaction.Item1, HitLoaction.Item2 - 1);
                }
                //return (HitLoaction.Item1 +1 , HitLoaction.Item2 + 1);
            }
            //if the last attack was a hit, the bot will search around the hit
            return (NewX, NewY);
        }




        public static (int, int) Attack()
        {
            Random random = new Random();
            int pos1 = random.Next(0, 8);
            int pos2 = random.Next(0, 8);
            (int, int) shots = (pos1, pos2);
            //AttackEnemy(shots);
            return shots;
        }
    }
}
