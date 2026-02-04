
namespace football;

 internal class Program
 {
    static void Squad_power(int[] a)
    {
        for (int i = 0; i < a.Length; i++)
        {
         a[i] = rand_roll.Next(30, 100);    
        } 
    }

    static double Probability_founder(int[] squad_1, int[] squad_2)
    {
        double squad_chance = 0;
        double squad_1_power = 0;
        double squad_2_power = 0;
        for (int i = 0; i < 11; i++)
        {
            squad_1_power += squad_1[i];
            squad_2_power += squad_2[i];
        }

        squad_chance = squad_1_power / (squad_1_power + squad_2_power) * 100;
        return squad_chance;
    }

    Random rand_roll = new();

     static void Main(string[] args)
    {
        int manchester_goals = 0,
            arsenal_goals = 0,
            event_decider = 0,
            manchester_yellows = 0,
            manchester_reds = 0,
            arsenal_yellows=0,
            arsenal_reds=0,
            penalty_decider = 0,
            smallest_main = 100,
            biggest_reserve = 0,
            weakestIndex = 100,
            switcher = 0,
            manchester_subs = 0,
            arsenal_subs = 0,
            strongestReserveIndex = 12,
            time=90;
        int[] manchester_main = new int[16];
        bool[] manchester_yellow = new bool[16];
        int[] arsenal_main = new int[16];
        bool[] arsenal_yellow = new bool[16];
        double manchester_chance = 0, arsenal_chance = 0;
        Squad_power(manchester_main);
        Squad_power(arsenal_main);
        time += rand_roll.Next(1, 6);
        for (int i = 0; i < time; i++)
        {
            if (i == 15 || i == 30 || i == 45 || i == 60 || i == 75 || i == 90)
            {
                for (int t = 0; t < 11; t++)
                {
                    manchester_main[t]-=2;
                    arsenal_main[t]-=2;
                }
            }
            event_decider = rand_roll.Next(1, 8);
            {
                if (event_decider == 1)
                {
                    manchester_chance = Probability_founder(manchester_main, arsenal_main);
                    arsenal_chance = Probability_founder(arsenal_main, manchester_main);

                    if (rand_roll.Next(1, 100) < manchester_chance)
                    {
                        manchester_goals++;
                        Console.WriteLine("manchester has scored");
                    }
                    else
                    {
                        arsenal_goals++;
                        Console.WriteLine("arsenal has scored");
                    }
                }
                else if (event_decider == 2)
                {
                   
                    penalty_decider = rand_roll.Next(0, 11); 

                    
                    if (rand_roll.Next(0, 100) <= 50)
                    {
                        if (manchester_yellow[penalty_decider])
                        {
                            if (manchester_main[penalty_decider] > 0)
                            {
                                manchester_main[penalty_decider] = 0;
                                manchester_reds++;
                                Console.WriteLine($"[RED] Manchester #{penalty_decider} second yellow at minute {i}");
                            }
                        }
                        else if (manchester_main[penalty_decider] > 0)
                        {
                            manchester_yellow[penalty_decider] = true;
                            manchester_main[penalty_decider] -= 10;
                            manchester_yellows++;
                            Console.WriteLine($"[YELLOW] Manchester #{penalty_decider} at minute {i}");
                        }
                    }
                    else 
                    {
                        if (arsenal_yellow[penalty_decider])
                        {
                            if (arsenal_main[penalty_decider] > 0)
                            {
                                arsenal_main[penalty_decider] = 0;
                                arsenal_reds++;
                                Console.WriteLine($"[RED] Arsenal #{penalty_decider} second yellow at minute {i}");
                            }
                        }
                        else if (arsenal_main[penalty_decider] > 0)
                        {
                            arsenal_yellow[penalty_decider] = true;
                            arsenal_main[penalty_decider] -= 10;
                            arsenal_yellows++;
                            Console.WriteLine($"[YELLOW] Arsenal #{penalty_decider} at minute {i}");
                        }
                    }
                }
                else if (event_decider == 3)
                {
                    penalty_decider = rand_roll.Next(1, 12);
    
                   
                    if (rand_roll.Next(0, 100) <= 50)
                    {
            
                        if (manchester_main[penalty_decider] > 0)
                        {
                            manchester_main[penalty_decider] = 0;
                            manchester_reds++;
                            Console.WriteLine($"Manchester player {penalty_decider} sent off (Straight Red) at minute {i}.");
                        }
                    }
                    else
                    {
                       
                        if (arsenal_main[penalty_decider] > 0)
                        {
                            arsenal_main[penalty_decider] = 0;
                            arsenal_reds++;
                            Console.WriteLine($"Arsenal player {penalty_decider} sent off (Straight Red) at minute {i}.");
                        }
                    }
                }
                else if (event_decider == 4)
                {
                    if (rand_roll.Next(0, 101) <= 50)
                    {
                        if (manchester_subs < 5)
                        {
                            smallest_main = 101;
                            biggest_reserve = 0;
                            weakestIndex = 1;
                            strongestReserveIndex = 12;


                            for (var t = 1; t <= 11; t++)
                                if (manchester_main[t] < smallest_main && manchester_main[t] > 0)
                                {
                                    smallest_main = manchester_main[t];
                                    weakestIndex = t;
                                }


                            for (var t = 12; t < 16; t++)
                                if (manchester_main[t] > biggest_reserve)
                                {
                                    biggest_reserve = manchester_main[t];
                                    strongestReserveIndex = t;
                                }

                            if (biggest_reserve > smallest_main)
                            {
                                switcher = manchester_main[weakestIndex];
                                manchester_main[weakestIndex] = manchester_main[strongestReserveIndex];
                                manchester_main[strongestReserveIndex] = switcher;
                                manchester_subs++;
                                Console.WriteLine(
                                    $"[SUB] Manchester: Player {strongestReserveIndex} replaces Player {weakestIndex} at minute {i}.");
                            }
                        }
                    }
                    else
                    {
                        if (arsenal_subs < 5)
                        {
                            smallest_main = 101;
                            biggest_reserve = 0;
                            weakestIndex = 1;
                            strongestReserveIndex = 12;


                            for (var t = 1; t <= 11; t++)
                                if (arsenal_main[t] < smallest_main && arsenal_main[t] > 0)
                                {
                                    smallest_main = arsenal_main[t];
                                    weakestIndex = t;
                                }


                            for (int = 12; t < 16; t++)
                                if (arsenal_main[t] > biggest_reserve)
                                {
                                    biggest_reserve = arsenal_main[t];
                                    strongestReserveIndex = t;
                                }


                            if (biggest_reserve > smallest_main)
                            {
                                switcher = arsenal_main[weakestIndex];
                                arsenal_main[weakestIndex] = arsenal_main[strongestReserveIndex];
                                arsenal_main[strongestReserveIndex] = switcher;
                                arsenal_subs++;
                                Console.WriteLine(
                                    $"[SUB] Arsenal: Player {strongestReserveIndex} replaces Player {weakestIndex} at minute {i}.");
                            }
                        }
                    }
                }
                else if (event_decider == 5)
                {
                    if (rand_roll.Next(0, 101) <= 50)
                    {
                        penalty_decider = rand_roll.Next(0, 12);
                        if (manchester_main[penalty_decider] - 20 <= 0)
                        {
                            manchester_main[penalty_decider] = 1;
                            Console.WriteLine($"Manchester player {penalty_decider} has contracted an injury at minute {i}.");
                        }
                        else
                        {
                            manchester_main[penalty_decider] -= 20;
                            Console.WriteLine($"manchester player {penalty_decider} has contracted an injury at minute {i}.");
                        }
                        
                    }
                    

                }
                else if(event_decider==6)
                {
                    penalty_decider = rand_roll.Next(0, 12);
                    if (arsenal_main[penalty_decider] > 0)
                    {
                        if (arsenal_main[penalty_decider] - 20 <= 0)
                        {
                            arsenal_main[penalty_decider] = 1;
                            Console.WriteLine($"Arsenal player {penalty_decider} has contracted an injury at minute {i}.");
                        }
                        else
                        {
                            arsenal_main[penalty_decider] -= 20;
                        }
                        Console.WriteLine($"Arsenal player {penalty_decider} has contracted an injury at minute {i}.");
                    }
                }
                else if (event_decider == 7)
                {
                    for (int t = 0; t < 11; t++)
                    {
                        manchester_main[t]--;
                        arsenal_main[t]--;
                    }
                }
            }
        }
        Console.WriteLine("========================================");
        Console.WriteLine("           FINAL WHISTLE");
        Console.WriteLine(========================================);
        
 
        Console.WriteLine($"MANCHESTER {manchester_goals} - {arsenal_goals} ARSENAL");
     Console.WriteLine("----------------------------------------");

        
        if (manchester_goals > arsenal_goals)
        {
            Console.WriteLine("RESULT: Manchester United takes the win!");
        }
        else if (arsenal_goals > manchester_goals)
        {
            Console.WriteLine("RESULT: Arsenal secures the victory!");
        }
        else
        {
            Console.WriteLine("RESULT: The points are shared in a draw!");
        }

   
        Console.WriteLine("----------------------------------------");
        Console.WriteLine($"Match Duration: {time} minutes");
        Console.WriteLine($"Subs Used: MAN {manchester_subs} | ARS {arsenal_subs}");
        Console.WriteLine($"yellow cards given: MAN {manchester_yellows} | ARS {arsenal_yellows}");
        Console.WriteLine($"red cards given: MAN {manchester_reds} | ARS {arsenal_reds}");
        Console.WriteLine(========================================);
    }
     
