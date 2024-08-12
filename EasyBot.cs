using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships
{
    internal class EasyBot
    {
        private static int[,] MyField = new int[8, 8];
        private static int[,] EnemyField = new int[8, 8];
        public static void StartUp() 
        {
            MyField.Initialize();
            EnemyField.Initialize();
            PlaceShip(MyField);
        }
        public static int[,] PlaceShip(int[,] field)
        {
            Random random = new Random();
            //place a ship randomly in the array, without reaching out of bounds. Places randomly Vertically or Horizontally
            int direction = random.Next(0, 2); 
            int HLength = field.GetLength(0);
            int VLength = field.GetLength(1);
            if (direction == 0)
            {
                //if random direction is 0, place ship randomly horizontally, within field
                int randomIndexX = random.Next(0, 7);
                int randomIndexY = random.Next(0,HLength - 2);
                for (int i = 0; i < 3; i++)
                {
                    field[randomIndexX, randomIndexY] = 1;
                    randomIndexY++;
                }
            }
            if(direction == 1)
            {
                //if random direction is 1, place ship randomly Vertically, within field
                int randomIndexX = random.Next(0, VLength - 2);
                int randomIndexY = random.Next(0,7);
                for (int i = 0; i < 3;i++)
                {
                    field[randomIndexX, randomIndexY] = 1;
                    randomIndexX++;
                }
            }

                return field;
        }
        
        public static int[,] GetMyField()
        {
            return MyField;
        }
        /*
         * determined to be unused/useless
        public static int[,] GetEnemyField()
        {
            return EnemyField;
        }

        private static void AttackEnemy((int, int) cord)
        {
            EnemyField[cord.Item1, cord.Item2] = 2;
        }
        */
        public static (int, int) Attack()
        {
            Random random = new Random();
            int pos1 = random.Next(0,8);
            int pos2 = random.Next(0,8);
            (int, int) shots = (pos1, pos2);
            //AttackEnemy(shots);
            return shots;
        }
    }
}
