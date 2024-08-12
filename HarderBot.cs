using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Battleships
{
    internal class HarderBot
    {
        //build fields for the hardbot to reference
        private static int[,] MyField = new int[8, 8];
        //create flags for the advanced attack method. X and Y are indexes of last shot location
        private static int LastHitX = 0;
        private static int LastHitY = 0;
        private static bool Hit = false;
        private static (int, int) HitLocation = (0, 0);  //HIT LOCATION UPDATED EVERY HIT
        private static bool ShipFound = false;
        private static (int, int) ShipLocation = (0, 0);
        //create flags for ship direction
        private static bool North;
        private static bool South;
        private static bool East;
        private static bool West;
        //create flags for testing ship direction
        private static bool PNorth;
        private static bool PSouth;
        private static bool PEast;
        private static bool PWest;

        public static void StartUp()
        {
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
                int randomIndexY = random.Next(0, HLength - 2);
                for (int i = 0; i < 3; i++)
                {
                    field[randomIndexX, randomIndexY] = 1;
                    randomIndexY++;
                }
            }
            if (direction == 1)
            {
                //if random direction is 1, place ship randomly Vertically, within field
                int randomIndexX = random.Next(0, VLength - 2);
                int randomIndexY = random.Next(0, 7);
                for (int i = 0; i < 3; i++)
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

        public static void UpdateHitFlag(bool x, (int, int) loc)
        {
            Hit = x;
            HitLocation = loc;
            if (Hit && ShipFound == false)
            {
                //update ShipLocation to where the ship was first found
                ShipLocation = loc;
            }
            if (Hit)
            {
                ShipFound = true;
            }
        }

        public static (int, int) Attack()
        {
            int NewY = LastHitY;
            int NewX = LastHitX;
            if (!ShipFound)
            {
                if (LastHitY < 8)
                {
                    LastHitY += 3; //update next move ahead 3 spaces
                    return (LastHitX, NewY);
                }
                else if (LastHitX < 7)
                {
                    LastHitX++; //drop down a row 
                    NewY = LastHitY - MyField.GetLength(0); //for return coordinates, take last hit and subtract row length
                    LastHitY -= 5; //Sets up future shot to be 3 positions past this current shot. Assumes return to if statement above
                    return (LastHitX, NewY); //return coordinates on next line down and reduced 
                }
                else
                {
                    //if search through array finds no boat, attack randomly
                    return (EasyBot.Attack());
                }
            }
            else
            {
                //Code works when: Hitting left part of horiz ship; Middle part of ship; Right part of ship
                if (ShipLocation.Item2 + 1 < 8 && !PEast && Hit &&!PNorth)
                {
                    //fire to the Right if possible, and not already tried
                    PEast = true;
                    return (ShipLocation.Item1, ShipLocation.Item2 + 1);
                }
                if (ShipLocation.Item2 + 1 == 8 &&!PNorth)
                {
                    PWest = true;
                    if (!West)
                    {
                        West = true;
                        return (ShipLocation.Item1, ShipLocation.Item2 - 1);
                    }
                    else
                    {
                        return (ShipLocation.Item1, ShipLocation.Item2 - 2);
                    }
                }
                else if (PEast && Hit && !PWest && !PNorth)
                {
                    //if we tested East, and landed a hit, the ship is possibly completely East
                    //fire 2 spaces right of shiplocation. Comes back if we landed that shot to send another shot 3 spaces to the right
                    if (ShipLocation.Item2 + 2 < 8 && !East)
                    {
                        East = true;
                        return (ShipLocation.Item1, ShipLocation.Item2 + 2);
                    }
                    else if (ShipLocation.Item2 + 3 < 8 && East)
                    {
                        return (ShipLocation.Item1, ShipLocation.Item2 + 3);
                    }
                }
                else if (PEast && !Hit && !PNorth)
                {
                    //if shooting to the right lands nothing, start checking West
                    if (ShipLocation.Item2 - 1 >= 0 && !PWest)
                    {
                        PWest = true;
                        return (ShipLocation.Item1, ShipLocation.Item2 - 1);
                    }
                }
                else if (PWest && Hit && !PNorth)
                {
                    //repeat steps from above if shots to the left land
                    if (ShipLocation.Item2 - 2 >= 0 && !West)
                    {
                        West = true;
                        return (ShipLocation.Item1, ShipLocation.Item2 - 2);
                    }
                    else if (ShipLocation.Item2 - 3 >= 0 && West)
                    {
                        return (ShipLocation.Item1, ShipLocation.Item2 - 3);
                    }
                }

                //These  following checks are for vertical direction
                //these checks currently work on bottom ship pos, middle ship pos, top ship pos
                if(ShipLocation.Item1 - 1 < 0)
                {
                    //if ship is at the top of the map, then shoot the positions below it
                    if(!South)
                    {
                        South = true;
                        return (ShipLocation.Item1 + 1, ShipLocation.Item2);
                    }
                    else
                    {
                        return (ShipLocation.Item1 + 2, ShipLocation.Item2);
                    }
                }
                else if(ShipLocation.Item1 + 1 == 8)
                {
                    //if the ship is at the bottom of the map, shoot the positions above it
                    if(!North)
                    {
                        North = true;
                        return (ShipLocation.Item1 - 1, ShipLocation.Item2);
                    }
                    else
                    {
                        return (ShipLocation.Item1 - 2, ShipLocation.Item2);
                    }
                }
                if(!PNorth && ShipLocation.Item1 - 1 >= 0)
                {
                    //check the position above the ship's location
                    PNorth = true;
                    return (ShipLocation.Item1 - 1, ShipLocation.Item2);
                }
                else if (PNorth && Hit)
                {
                    //if hits above the ship, try the next 2 positions above it
                    if (ShipLocation.Item1 - 2 >= 0 && !North)
                    {
                        North = true;
                        return (ShipLocation.Item1 - 2, ShipLocation.Item2);
                    }
                    else if (ShipLocation.Item1 - 3 >= 0 && North)
                    {
                        return (ShipLocation.Item1 - 3, ShipLocation.Item2);
                    }
                }
                else
                {
                    //this code shoots at the positions below ShipLocation
                    if(ShipLocation.Item1 + 1 < 8 && !PSouth)
                    {
                        PSouth = true;
                        return (ShipLocation.Item1 + 1,  ShipLocation.Item2);
                    }
                    else if (ShipLocation.Item1 + 2 < 8 && PSouth)
                    {
                        South = true;
                        return (ShipLocation.Item1 + 2, ShipLocation.Item2);
                    }
                    else if (ShipLocation.Item1 + 3 < 8 && South)
                    {
                        return (ShipLocation.Item1 + 3, ShipLocation.Item2);
                    }
                }
                return EasyBot.Attack();
            }
            
        }
    }
}
