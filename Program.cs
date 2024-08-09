using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Battleships;

class BuildField
{
    //each side has one ship length 3
    //most effective search method would be to check diagonlly, starting every two rows
    //main class mediates between two "bot" classes. the bots build their own "fields" and place a ship
    //the main class takes turn between the bots to ask them to make a shot placement
    //this continues until the main sees that one bot has sunk the other (3 "hits")

    //currently both bots are identical to test the Main build


    static void Main()
    {
        //initialize win conditions and bool for hit detection and hit counts
        bool EWin = false;
        bool Hwin = false;
        (bool, bool) check;
        int HardHits = 0;
        int EasyHits = 0;
        //initalize the two bots
        EasyBot Ebot = new EasyBot();
        HardBot HBot = new HardBot();
        EasyBot.StartUp();
        HardBot.StartUp();
        //build the fields for the Main class to reference
        int[,] EasyField = EasyBot.GetMyField();
        int[,] HardField = HardBot.GetMyField();
        Console.WriteLine("This is a game of Battleships where two bots attack each other!");
        Console.WriteLine("Press Enter to Continue:");
        Console.ReadLine();

        while( EWin == false && Hwin == false )
        {
            //EasyBot will go first since it's dumb. This is the Easy Bot Section
            //below is a tuple recieving coordinates from the Bots. Calling it's items is Attack.Item1/2
            (int, int) Attack = EasyBot.Attack();
            //this is specifcally checking if the attack hit anything
            check = HitDetect(Attack, HardField);
            //updates hit counts if there is a hit
            if(check.Item1 && check.Item2 == false) {EasyHits++; Console.WriteLine("Easy Bot makes a hit!"); }
            //updating hardfield based off the above attack
            HardField = UpdateField(Attack, HardField, check.Item1);

            //This is the HardBot Section
            Attack = HardBot.Attack();
            check = HitDetect(Attack, EasyField);
            if(check.Item1 && check.Item2 == false) {HardHits++; Console.WriteLine("Hard Bot makes a hit!"); }
            EasyField = UpdateField(Attack, EasyField, check.Item1);

            //displays both fields
            Console.WriteLine("Easy Bot's Waters VVVVV");
            Console.WriteLine(BuildString(EasyField));
            Console.WriteLine("");
            Console.WriteLine("Hard Bot's Waters VVVVV");
            Console.WriteLine(BuildString(HardField));
            Console.WriteLine("");
            Console.WriteLine("Press Enter to continue");
            Console.ReadLine();
            Console.WriteLine("\n\n\n\n"); //lines to separate previous displays from newest

            //checks win conditions
            if (HardHits == 3) { Hwin = true; }
            if(EasyHits == 3) { EWin = true; }
        }
        if(Hwin)
        {
            Console.WriteLine("Hard Bot has won the game and sunk the Easy Bot's Ship!");
        }
        else
        {
            Console.WriteLine("Easy Bot has won the game and sunk the Hard Bot's Ship!");
        }
       
        
    }

    public static string BuildString(int[,] field)
    {
        //builds string to display on the console where the boat is and hits/misses
        //0 = water; 1 = boat; 2 = miss; 3 ship hit
        string output = "";
        for (int i = 0; i < field.GetLength(0); i++)
        {
            for (int x = 0; x < field.GetLength(1); x++)
            {
                if (field[i, x] == 1)
                {
                    output = output + "boat\t   ";
                }
                else if (field[i, x] == 2)
                {
                    output = output + "miss\t   ";
                }
                else if (field[i, x] == 3)
                {
                    output = output + "hit\t   ";
                }
                else
                {
                    output = output + "0\t   ";
                }
            }
            output = output + "\n"; //starts new row
        }
        return output;
    }

    static (bool, bool) HitDetect((int, int) pos, int[,] AField)
    {
        // 0 means water, 1 means ship, 2 means miss, 3 means hit
        //recieves attack coordinates, field that's being attacked
        //checks if attack hit a ship

        //returns two booleans to be used accordingly: Item1=hit?, Item2=Hit already hit ship?

        int CellValue = AField[pos.Item1, pos.Item2];
        if (CellValue == 1)
        {
            return (true, false);
        }
        else if(CellValue == 3)
        {
            return (true, true);
        }
        else if(CellValue == 0 || CellValue == 2)
        {
            return (false, false);
        }
        else { return (false, false);}
    }

    static int[,] UpdateField((int, int) pos, int[,] AField, bool hit)
    {
        //given coordinates, update recieved field based on boolean received
        if(hit)
        {
            AField[pos.Item1, pos.Item2] = 3;
        }
        else
        {
            AField[pos.Item1, pos.Item2] = 2;
        }
        return AField;

    }

    


}