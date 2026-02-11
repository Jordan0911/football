namespace football;

internal class Program
{
    // Assigns random 'power' levels (30-100) to each player in the array
    static void Squad_power(int[] a)
    {
        for (int i = 0; i < a.Length; i++)
        {
            a[i] = rand_roll.Next(30, 100);
        }
    }

    // Calculates the win probability for squad_1 based on the relative power of both teams
    static double Probability_founder(int[] squad_1, int[] squad_2)
    {
        double squad_chance = 0;
        double squad_1_power = 0;
        double squad_2_power = 0;
        
        // Sums the power of the first 11 players (the active lineup)
        for (int i = 0; i < 11; i++)
        {
            squad_1_power += squad_1[i];
            squad_2_power += squad_2[i];
        }

        // Standard probability formula: (Team A / (Team A + Team B)) * 100
        squad_chance = squad_1_power / (squad_1_power + squad_2_power) * 100;
        return squad_chance;
    }

    // Global Random instance to ensure consistent randomness throughout the execution
    static Random rand_roll = new Random();

    static void Main(string[] args)
    {
        // --- VARIABLE INITIALIZATION ---
        int manchester_goals = 0,
            arsenal_goals = 0,
            event_decider = 0,      // Determines what happens in a specific minute
            manchester_yellows = 0,
            manchester_reds = 0,
            arsenal_yellows = 0,
            arsenal_reds = 0,
            penalty_decider = 0,    // Used to pick a random player for cards/injuries
            smallest_main = 100,    // Tracker for weakest player on pitch
            biggest_reserve = 0,    // Tracker for strongest player on bench
            weakestIndex = 100,
            switcher = 0,           // Temporary variable for swapping players
            manchester_subs = 0,
            arsenal_subs = 0,
            strongestReserveIndex = 12,
            time = 90;

        // Arrays for players: 0-10 are starters, 11-15 are reserves
        int[] manchester_main = new int[16];
        bool[] manchester_yellow = new bool[16]; // Tracks if player already has 1 yellow
        int[] arsenal_main = new int[16];
        bool[] arsenal_yellow = new bool[16];

        double manchester_chance = 0, arsenal_chance = 0;

        // --- PRE-MATCH SETUP ---
        Squad_power(manchester_main);
        Squad_power(arsenal_main);
        
        // Adds 1-5 minutes of "overtime"
        time += rand_roll.Next(1, 6);

        // --- MATCH SIMULATION LOOP (Minute by Minute) ---
        for (int i = 0; i < time; i++)
        {
            // Fatigue Logic: Every 15 minutes, all starters lose 2 power points
            if (i == 15 || i == 30 || i == 45 || i == 60 || i == 75 || i == 90)
            {
                for (int t = 0; t < 11; t++)
                {
                    manchester_main[t] -= 2;
                    arsenal_main[t] -= 2;
                }
            }

            // Determine if a major event occurs this minute (0-7 range)
            event_decider = rand_roll.Next(0, 8);
            {
                // EVENT 1: A goal is scored
                if (event_decider == 1)
                {
                    manchester_chance = Probability_founder(manchester_main, arsenal_main);
                    
                    // Roll against the calculated probability to see who scored
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
                // EVENT 2: Yellow Card logic
                else if (event_decider == 2)
                {
                    penalty_decider = rand_roll.Next(0, 11);

                    // 50/50 chance for which team gets the card
                    if (rand_roll.Next(0, 100) <= 50)
                    {
                        // Check if player already had a yellow (Second Yellow = Red)
                        if (manchester_yellow[penalty_decider])
                        {
                            if (manchester_main[penalty_decider] > 0)
                            {
                                manchester_main[penalty_decider] = 0; // Remove player from power sum
                                manchester_reds++;
                                Console.WriteLine($"[RED] Manchester player number {penalty_decider}gets a second yellow at minute {i}");
                            }
                        }
                        else if (manchester_main[penalty_decider] > 0)
                        {
                            manchester_yellow[penalty_decider] = true;
                            manchester_main[penalty_decider] -= 10; // Penalize player power for yellow
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
                // EVENT 3: Straight Red Card logic
                else if (event_decider == 3)
                {
                    penalty_decider = rand_roll.Next(1, 12);

                    if (rand_roll.Next(0, 100) <= 50)
                    {
                        if (manchester_main[penalty_decider] > 0)
                        {
                            manchester_main[penalty_decider] = 0;
                            manchester_reds++;
                            Console.WriteLine($"Manchester player {penalty_decider} has been sent off (Straight Red) at minute {i}.");
                        }
                    }
                    else
                    {
                        if (arsenal_main[penalty_decider] > 0)
                        {
                            arsenal_main[penalty_decider] = 0;
                            arsenal_reds++;
                            Console.WriteLine($"Arsenal player {penalty_decider} has been sent off (Straight Red) at minute {i}.");
                        }
                    }
                }
                // EVENT 4: Substitution Logic
                else if (event_decider == 4)
                {
                    // Manchester Sub
                    if (rand_roll.Next(0, 100) <= 50)
                    {
                        if (manchester_subs < 5) // FIFA/IFAB rule: Max 5 subs
                        {
                            smallest_main = 100;
                            biggest_reserve = 0;
                            weakestIndex = 1;
                            strongestReserveIndex = 12;

                            // Find the weakest active player on the field
                            for (int t = 1; t <= 11; t++)
                                if (manchester_main[t] < smallest_main && manchester_main[t] > 0)
                                {
                                    smallest_main = manchester_main[t];
                                    weakestIndex = t;
                                }

                            // Find the strongest player on the bench
                            for (int t = 12; t < 16; t++)
                                if (manchester_main[t] > biggest_reserve)
                                {
                                    biggest_reserve = manchester_main[t];
                                    strongestReserveIndex = t;
                                }

                            // Swap them if the reserve is better than the starter
                            if (biggest_reserve > smallest_main)
                            {
                                switcher = manchester_main[weakestIndex];
                                manchester_main[weakestIndex] = manchester_main[strongestReserveIndex];
                                manchester_main[strongestReserveIndex] = switcher;
                                manchester_subs++;
                                Console.WriteLine($"Manchester: Player {strongestReserveIndex} replaces Player {weakestIndex} at minute {i}.");
                            }
                        }
                    }
                    // Arsenal Sub
                    else
                    {
                        if (arsenal_subs < 5)
                        {
                            smallest_main = 101;
                            biggest_reserve = 0;
                            weakestIndex = 1;
                            strongestReserveIndex = 12;

                            for (int t = 1; t <= 11; t++)
                                if (arsenal_main[t] < smallest_main && arsenal_main[t] > 0)
                                {
                                    smallest_main = arsenal_main[t];
                                    weakestIndex = t;
                                }

                            for (int t = 12; t < 16; t++)
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
                                Console.WriteLine($"Arsenal: Player {strongestReserveIndex} replaces Player {weakestIndex} at minute {i}.");
                            }
                        }
                    }
                }
                // EVENT 5: Manchester Injury
                else if (event_decider == 5)
                {
                    if (rand_roll.Next(0, 101) <= 50)
                    {
                        penalty_decider = rand_roll.Next(0, 12);
                        // Injuries reduce power by 20; cannot go below 1
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
                // EVENT 6: Arsenal Injury
                else if (event_decider == 6)
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
                // EVENT 7: Minor Fatigue
                else if (event_decider == 7)
                {
                    // Everyone on the pitch loses 1 power point
                    for (int t = 0; t < 11; t++)
                    {
                        manchester_main[t]--;
                        arsenal_main[t]--;
                    }
                }
            }
        }

        // --- FINAL RESULTS OUTPUT ---
        Console.WriteLine("========================================");
        Console.WriteLine("            FINAL WHISTLE");
        Console.WriteLine("========================================");

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
        Console.WriteLine("========================================");
    }
}
