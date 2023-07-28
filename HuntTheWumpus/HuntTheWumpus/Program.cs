// See https://aka.ms/new-console-template for more information
using HuntTheWumpus;
using Action = HuntTheWumpus.Action;

Console.WriteLine("Hello, World!");

//0010  REM- HUNT THE WUMPUS
//0015  REM:  BY GREGORY YOB
//0020  PRINT "INSTRUCTIONS (Y-N)";
//0030  INPUT I$
//0040  IF I$="N" THEN 52
Console.WriteLine("INSTRUCTIONS (Y-N)");
string input = Prompt();
if (!(input =="N" && input =="NO"))
{
    ShowInstructions();
}
//0050  GOSUB 1000
//0052  REM- ANNOUNCE WUMPUSII FOR ALL AFICIONADOS ... ADDED BY DAVE
Console.WriteLine();
Console.WriteLine("     ATTENTION ALL WUMPUS LOVERS!!!");
Console.WriteLine("     THERE ARE NOW TWO ADDITIONS TO THE WUMPUS FAMILY");
Console.WriteLine(" OF PROGRAMS.");
Console.WriteLine();
Console.WriteLine("     WUMP2:  SOME DIFFERENT CAVE ARRANGEMENTS");
Console.WriteLine("     WUMP3:  DIFFERENT HAZARDS");
Console.WriteLine();

//0130  DATA 2,5,8,1,3,10,2,4,12,3,5,14,1,4,6
//0140  DATA 5,7,15,6,8,17,1,7,9,8,10,18,2,9,11
//0150  DATA 10,12,19,3,11,13,12,14,20,4,13,15,6,14,16
//0160  DATA 15,17,20,7,16,18,9,17,19,11,18,20,13,16,19

// NOTE: These values are in BASIC indices which means they are in the
// range [1..n] and not C#'s [0..n] range. The setup function will shift
// them down to the 0..n range.
int[] DATA = new int[]
{
    2,5,8,1,3,10,2,4,12,3,5,14,1,4,6,
    5,7,15,6,8,17,1,7,9,8,10,18,2,9,11,
    10,12,19,3,11,13,12,14,20,4,13,15,6,14,16,
    15,17,20,7,16,18,9,17,19,11,18,20,13,16,19
};

//0068  REM - SET UP CAVE(DODECAHEDRAL NODE LIST)
//0070  DIM S(20,3)
//0080   FOR J = 1 TO 20
//0090    FOR K=1 TO 3
//0100    READ S(J, K)
//0110    NEXT K
//0120   NEXT J
int[,] S = new int[20, 3];
for(int j = 0; j<20; j++)
{
    for(int k = 0; k < 3;k++)
    {
        int index = k * 20 + j;
        S[j, k] = DATA[index] - 1;
    }
}

//0170  DEF FNA(X)= INT(20 * RND(0)) + 1
int FNA() => Random.Shared.Next(20);
//0180  DEF FNB(X)= INT(3 * RND(0)) + 1
int FNB() => Random.Shared.Next(3);
//0190  DEF FNC(X)= INT(4 * RND(0)) + 1
int FNC() => Random.Shared.Next(4);

//0200  REM - LOCATE L ARRAY ITEMS
//0210  REM-1-YOU,2-WUMPUS,3&4-PITS,5&6-BATS
//0220  DIM L(6)
//0230  DIM M(6)
int[] L = new int[6];
int[] M = new int[6];
int F = 0;

L240:

//0280  REM-CHECK FOR CROSSOVERS (IE L(1)= L(2),ETC)
//0290   FOR J = 1 TO 6
do
{
    LocateItems();
} while (CheckCrossovers() != BuildItem.Done);
//0350  REM-SET# ARROWS
//0360  A=5
L360:
int A = 5;
//0365  L=L(1)
int EL = L[0];
//0370  REM - RUN THE GAME
Console.WriteLine("HUNT THE WUMPUS");
//0380  REM-HAZARD WARNINGS & LOCATION
//0390  GOSUB 2000
L390:
LocationAndHazardWarnings();

//0400  REM-MOVE OR SHOOT
//0410  GOSUB 2500
//0420  GOTO O OF 440,480
Action action = GetAction();
switch(action)
{
    //0430  REM-SHOOT
    //0440  GOSUB 3000
    case Action.Shoot:
        //0450  IF F=0 THEN 390
        //0460  GOTO 500
        ShootArrow();
        break;
    default:
        //0480  GOSUB 4000
        Move();
        //0490  IF F=0 THEN 390
        if (F == 0) goto L390;
        //0500  IF F>0 THEN 550
        if (F > 0) goto L550;
        break;
}

//0470  REM-MOVE

//0510  REM-LOSE
//0520  PRINT "HA HA HA - YOU LOSE!"
Console.WriteLine("HA HA HA - YOU LOSE!");
//0530  GOTO 560
goto L560;
//0540  REM-WIN
L550:
//0550  PRINT "HEE HEE HEE - THE WUMPUS'LL GETCHA NEXT TIME!!"
Console.WriteLine("HEE HEE HEE - THE WUMPUS'LL GETCHA NEXT TIME!!");
//0560   FOR J=1 TO 6
L560:
for (int j = 0; j < 6; j++)
{
    //0570   L(J)= M(J)
    L[j] = M[j];
    //0580   NEXT J
}
//0590  PRINT "SAME SET-UP (Y-N)";
Console.WriteLine("SAME SET-UP (Y-N)");
//0600  INPUT I$
string sameAgain = Prompt();
//0610  IF I$#"Y" THEN 240
if (sameAgain == "Y")
    goto L240;
//0620  GOTO 360
goto L360;
//1000  REM-INSTRUCTIONS
void ShowInstructions()
{
    Console.WriteLine("WELCOME TO 'HUNT THE WUMPUS'");
    Console.WriteLine("  THE WUMPUS LIVES IN A CAVE OF 20 ROOMS. EACH ROOM");
    Console.WriteLine("HAS 3 TUNNELS LEADING TO OTHER ROOMS. (LOOK AT A");
    Console.WriteLine("DODECAHEDRON TO SEE HOW THIS WORKS-IF YOU DON'T KNOW");
    Console.WriteLine("WHAT A DODECAHEDRON IS, ASK SOMEONE)");
    Console.WriteLine();
    Console.WriteLine("     HAZARDS:");
    Console.WriteLine(" BOTTOMLESS PITS - TWO ROOMS HAVE BOTTOMLESS PITS IN THEM");
    Console.WriteLine("     IF YOU GO THERE, YOU FALL INTO THE PIT (& LOSE!)");
    Console.WriteLine(" SUPER BATS - TWO OTHER ROOMS HAVE SUPER BATS. IF YOU");
    Console.WriteLine("     GO THERE, A BAT GRABS YOU AND TAKES YOU TO SOME OTHER");
    Console.WriteLine("     ROOM AT RANDOM. (WHICH MIGHT BE TROUBLESOME)");
    Console.WriteLine();
    Console.WriteLine("     WUMPUS:");
    Console.WriteLine(" THE WUMPUS IS NOT BOTHERED BY THE HAZARDS (HE HAS SUCKER");
    Console.WriteLine(" FEET AND IS TOO BIG FOR A BAT TO LIFT).  USUALLY");
    Console.WriteLine(" HE IS ASLEEP. TWO THINGS WAKE HIM UP: YOUR ENTERING");
    Console.WriteLine(" HIS ROOM OR YOUR SHOOTING AN ARROW.");
    Console.WriteLine("     IF THE WUMPUS WAKES, HE MOVES (P=.75) ONE ROOM");
    Console.WriteLine(" OR STAYS STILL (P=.25). AFTER THAT, IF HE IS WHERE YOU");
    Console.WriteLine(" ARE, HE EATS YOU UP (& YOU LOSE!)");
    Console.WriteLine();
    Console.WriteLine("     YOU:");
    Console.WriteLine(" EACH TURN YOU MAY MOVE OR SHOOT A CROOKED ARROW");
    Console.WriteLine("   MOVING: YOU CAN GO ONE ROOM (THRU ONE TUNNEL)");
    Console.WriteLine("   ARROWS: YOU HAVE 5 ARROWS. YOU LOSE WHEN YOU RUN OUT.");
    Console.WriteLine("   EACH ARROW CAN GO FROM 1 TO 5 ROOMS. YOU AIM BY TELLING");
    Console.WriteLine("   THE COMPUTER THE ROOM#S YOU WANT THE ARROW TO GO TO.");
    Console.WriteLine("   IF THE ARROW CAN'T GO THAT WAY (IE NO TUNNEL) IT MOVES");
    Console.WriteLine("   AT RAMDOM TO THE NEXT ROOM.");
    Console.WriteLine("     IF THE ARROW HITS THE WUMPUS, YOU WIN.");
    Console.WriteLine("     IF THE ARROW HITS YOU, YOU LOSE.");
    Console.WriteLine();
    Console.WriteLine("    WARNINGS:");
    Console.WriteLine("     WHEN YOU ARE ONE ROOM AWAY FROM WUMPUS OR HAZARD,");
    Console.WriteLine("    THE COMPUTER SAYS:");
    Console.WriteLine(" WUMPUS-  'I SMELL A WUMPUS'");
    Console.WriteLine(" BAT   -  'BATS NEARBY'");
    Console.WriteLine(" PIT   -  'I FEEL A DRAFT'");
    Console.WriteLine("");
}
//2000  REM-PRINT LOCATION & HAZARD WARNINGS
void LocationAndHazardWarnings()
{
    //2010  PRINT
    Console.WriteLine();
    //2020   FOR J=2 TO 6
    for (int j = 1; j < 6; j++)
    {
        //2030    FOR K=1 TO 3
        for (int k = 0; k < 3; k++)
        {
            //2040    IF S(L(1),K)#L(J) THEN 2110
            if (S[L[0], k] == L[j])
            {
                //2050    GOTO J-1 OF 2060,2080,2080,2100,2100
                //2050    GOTO J-1 OF 0,   1,    2 , 3,   4
                switch (j)
                {
                    case 0:
                        //2060    PRINT "I SMELL A WUMPUS!"
                        Console.WriteLine("I SMELL A WUMPUS!");
                        break;
                    //2070    GOTO 2110
                    case 1:
                    case 2:
                        //2080    PRINT "I FEEL A DRAFT"
                        Console.WriteLine("I FEEL A DRAFT");
                        break;
                    //2090    GOTO 2110
                    case 3:
                    case 4:
                        //2100    PRINT "BATS NEARBY!"
                        Console.WriteLine("BATS NEARBY!");
                        break;
                }
            }
            //2110    NEXT K
        }
        //2120   NEXT J
    }
    //2130  PRINT "YOU ARE IN ROOM "L(1)
    Console.WriteLine($"YOU ARE IN ROOM {L[0] + 1}");
    //2140  PRINT "TUNNELS LEAD TO "S(L, 1); S(L, 2); S(L, 3)
    Console.WriteLine($"TUNNELS LEAD TO {S[EL, 0] + 1} {S[EL, 1] + 1} {S[EL, 2] + 1}");
    //2150  PRINT
    Console.WriteLine();
    //2160  RETURN
}

//2500  REM - CHOOSE OPTION
Action GetAction()
{
    Action action = Action.Invalid;
    do
    {
        //2510  PRINT "SHOOT OR MOVE (S-M)";
        Console.WriteLine("SHOOT OR MOVE (S-M)");
        //2520  INPUT I$
        string i = Prompt();
        //2530  IF I$#"S" THEN 2560
        //2540  O=1
        //2550  RETURN
        if (i == "S")
            action = Action.Shoot;
        //2560  IF I$#"M" THEN 2510
        //2570  O=2
        //2580  RETURN
        if (i == "M")
            action = Action.Move;
    } while (action == Action.Invalid);

    return action;
}
//3000  REM-ARROW ROUTINE
void ShootArrow()
{
    //3010  F=0
    //3020  REM-PATH OF ARROW
    //3030  DIM P(5)
    //3040  PRINT "NO. OF ROOMS(1-5)";
    //3050  INPUT J9
    //3060  IF J9<1 OR J9>5 THEN 3040
    //3070   FOR K=1 TO J9
    //3080   PRINT "ROOM #";
    //3090   INPUT P(K)
    //3095   IF K <= 2 THEN 3115
    //3100   IF P(K) <> P(K - 2) THEN 3115
    //3105   PRINT "ARROWS AREN'T THAT CROOKED - TRY ANOTHER ROOM"
    //3110   GOTO 3080
    //3115   NEXT K
    //3120  REM-SHOOT ARROW
    //3130  L=L(1)
    //3140   FOR K = 1 TO J9
    //3150    FOR K1=1 TO 3
    //3160    IF S(L, K1)= P(K) THEN 3295
    //3170    NEXT K1
    //3180   REM-NO TUNNEL FOR ARROW
    //3190   L=S(L, FNB(1))
    //3200   GOTO 3300
    //3210   NEXT K
    //3220  PRINT "MISSED"
    //3225  L=L(1)
    //3230  REM - MOVE WUMPUS
    //3240  GOSUB 3370
    //3250  REM - AMMO CHECK
    //3255  A = A - 1
    //3260  IF A>0 THEN 3280
    //3270  F=-1
    //3280  RETURN
}

//3290  REM-SEE IF ARROW IS AT L(1) OR L(2)
//3295  L = P(K)
//3300  IF L#L(2) THEN 3340
//3310  PRINT "AHA! YOU GOT THE WUMPUS!"
//3320  F=1
//3330  RETURN!
//3340  IF L#L(1) THEN 3210
//3350  PRINT "OUCH! ARROW GOT YOU!"
//3360  GOTO 3270


//3370  REM-MOVE WUMPUS ROUTINE
void MoveWumpus()
{
    //3380  K=FNC(0)
    int K = FNC();
    //3390  IF K = 4 THEN 3410
    if (K != 3)
    {
        //3400  L(2)= S(L(2), K)
        L[1] = S[L[1], K];
    }

    //3410  IF L(2)#L THEN 3440
    if (L[1] != EL)
        return;

    //3420  PRINT "TSK TSK TSK- WUMPUS GOT YOU!"
    Console.WriteLine("TSK TSK TSK- WUMPUS GOT YOU!");
    //3430  F = -1
    F--;
    //3440  RETURN
}

void Move()
{
    //4000  REM - MOVE ROUTINE
    //4010  F = 0
    F = 0;
    //4020  PRINT "WHERE TO";
    do
    {
        do
        {
            Console.Write("WHERE TO ? ");
            EL = PromptInteger() - 1;
            //4030  INPUT L
            //4040  IF L<1 OR L>20 THEN 4020
        } while (EL < 0 || EL > 19);

        for (int k = 0; k < 3; k++)
        {
            //4050   FOR K=1 TO 3
            //4060   REM- CHECK IF LEGAL MOVE
            //4070   IF S(L(1),K)= L THEN 4130
            if (S[L[0], k] == EL)
                break;
            //4080   NEXT K
        }
        //4090  IF L=L(1) THEN 4130
        if (EL == L[0])
        {
            Console.Write("NOT POSSIBLE - ");
        }
    } while (EL == L[0]);

    //4100  PRINT "NOT POSSIBLE -";
    //4110  GOTO 4020
    //4120  REM - CHECK FOR HAZARDS
    //4130  L(1)= L
    EndMove();
}

void EndMove()
{
    L[0] = EL;
    //4140  REM - WUMPUS
    //4150  IF L#L(2) THEN 4220
    if (EL != L[1])
    {
        DoHazards();
    }
    else
    {
        //4160  PRINT "...OOPS! BUMPED A WUMPUS!"
        Console.WriteLine("...OOPS! BUMPED A WUMPUS!");
    }

    //4170  REM-MOVE WUMPUS
    //4180  GOSUB 3380
    MoveWumpus();
    //4190  IF F=0 THEN 4220
    if (F == 0)
    {
        DoHazards();
    }
    //4200  RETURN
}

//4210  REM-PIT
void DoHazards()
{
    //4220  IF L#L(3) AND L#L(4) THEN 4270
    if (EL != L[2] && EL != L[3])
    {
        //4260  REM-BATS
        //4270  IF L#L(5) AND L#L(6) THEN 4310
        if (EL != L[4] && EL != L[5])
            return;

        //4280  PRINT "ZAP--SUPER BAT SNATCH! ELSEWHEREVILLE FOR YOU!"
        Console.WriteLine("ZAP--SUPER BAT SNATCH! ELSEWHEREVILLE FOR YOU!");
        EL = FNA();
        //4290  L=FNA(1)
        //4300  GOTO 4130
        EndMove();
    }
    else
    {
        //4230  PRINT "YYYIIIIEEEE . . . FELL IN PIT"
        Console.WriteLine("YYYIIIIEEEE . . . FELL IN PIT");
        F--;
        //4240  F=-1
        //4250  RETURN

        //4310  RETURN
    }
}
//5000  END

BuildItem CheckCrossovers()
{

    for (int j = 0; j < 6; j++)
    {
        //0300    FOR K=J TO 6
        for (int k = 0; k < 6; k++)
        {
            //0310    IF J=K THEN 330
            if (j == k)
            {
                return BuildItem.Done;
            }

            if (L[j] == L[k])
            {
                return BuildItem.Redo;
            }

            //0320    IF L(J)= L(K) THEN 240
            //0330    NEXT K
        }
        //0340   NEXT J
    }
    return BuildItem.Done;
}

void LocateItems()
{
    //0240   FOR J = 1 TO 6
    for (int j = 0; j < 6; j++)
    {
        //0250   L(J)= FNA(0)
        L[j] = FNA();
        //0260   M(J) = L(J)
        M[j] = L[j];
        //0270   NEXT J
    }
}

string Prompt()
{
    return (Console.ReadLine() ?? "").ToUpper();
}

int PromptInteger()
{
    string str = Prompt();
    if (int.TryParse(str, out int val)) return val;
    return -1;
}