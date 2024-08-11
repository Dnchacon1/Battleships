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
        private static (int, int) HitLoaction = (0, 0);  //HIT LOCATION UPDATED EVERY HIT
        private static bool ShipFound = false;
        private static (int, int) ShipLocation = (0, 0);
        //create flags for ship direction
        private static bool North;
        private static bool South;
        private static bool East;
        private static bool West;
        //flags to show which direction is being tested
        private static bool PNorth;
        private static bool PSouth;
        private static bool PEast;
        private static bool PWest;



        public static void StartUp()
        {
            //MyField.Initialize();
            //EnemyField.Initialize();
            PlaceShip(MyField);
        }
        public static int[,] PlaceShip(int[,] field)
        {
            //requires update to place the ship vertically as well ******************************************************************8
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
            if (ShipFound == false && Hit)
            {
                //record position the ship was initially hit
                ShipLocation = loc;
            }
           Hit = x;
           HitLoaction = loc;
           ShipFound = true;
        }


        //this is currently a copy of NewAttack, working to build better quality attack pattern and implement direction finding
        public static (int, int) NewestAttack()
        {
            //scan the field every two spaces, inclusive of the last space (even if was a prior row)
            //this would look like X coordinate += 2, if x >= 8 then subtract 7

            int NewX = LastHitX;
            int NewY = LastHitY;
            if (Hit == false || ShipFound == false) //originally included hit as an || option
            {
                //check to see if shots are at the horizontal end of the board
                if (LastHitY <= 7)
                {
                    LastHitY += 3;
                    return (NewX, NewY);
                }
                else if (LastHitX < 7)
                {
                    //if shots are at the horizontal end of the board, and NOT at the vertical end, move to the next row, start at the beginning
                    LastHitY -= 8; NewY = LastHitY; LastHitY += 3;
                    LastHitX++;
                    return (LastHitX, NewY);
                }
                //if the bot has reached the end of the board, shoot randomly
                return Attack();
            }
            else if (ShipFound)
            {
                // shoot around the location the first hit occured, possibly track direction of the ship
                if (!PEast && !PWest && !PNorth && !PSouth)
                {
                    //if we don't know which direction, shoot laterally (if within bounds)
                    if (ShipLocation.Item2 + 1 < 8)
                    {
                        //check if East direction is within bounds
                        PEast = true;
                        return (ShipLocation.Item1, ShipLocation.Item2 + 1);
                    }
                    else
                    {
                        PWest = true;
                        return (ShipLocation.Item1, ShipLocation.Item2 - 1);
                    }
                }
                else if (Hit && PEast)
                {
                    //if there was a hit going to the right of last shot, ship is East at least one
                    if (ShipLocation.Item2 + 1 < 8)
                    {
                        //if the next 2 spaces ahead of initial ship hit is in bounds, fire there
                        return (ShipLocation.Item1, ShipLocation.Item2 +2);
                    }
                    else
                    {
                        //if not in bounds, throw stray shot to the West of ship's initial hit location
                        return (ShipLocation.Item1, ShipLocation.Item2 - 1);
                    }

                }
                else if( !Hit && PEast)
                {

                }
                else if (!Hit && !PNorth || !PSouth)
                {
                    //if we checked horizontally, and no hit, then check veritcally
                    if (HitLoaction.Item2 + 1 < 8)
                    {
                        PEast = false;
                        PWest = false;
                        PSouth = true;
                        return (HitLoaction.Item1 + 1, HitLoaction.Item2);
                    }
                    else
                    {
                        PEast = false;
                        PWest = false;
                        PNorth = true;
                        return (HitLoaction.Item1 - 1, HitLoaction.Item2);
                    }
                }
            }
            //if the last attack was a hit, the bot will search around the hit
            return NewAttack();
        }


        //newer attack that scanned the field horizonally every other space, then shot within one space of that position to sink the ship
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
                    if(LastHitY != 6) { LastHitY += 2; } //changed 2 to 3 here
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
            }
            //if the last attack was a hit, the bot will search around the hit
            return (NewX, NewY);
        }



        //old attack pattern which shot randomly, just like Easy Bot
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
