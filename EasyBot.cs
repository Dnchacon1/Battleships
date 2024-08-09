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
        /*
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
