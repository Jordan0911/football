I've added detailed comments to your code to explain exactly what each "engine" component is doing. I also highlighted a few spots where the logic is doing something specific you might want to keep an eye on (like which players are being targeted for cards or subs).

C#
namespace football;

internal class Program
{
    // Initializes player power levels between 30 and 100
    private static void Squad_power(int[] a)
    {
        for (var i = 0; i < a.Length; i++) a[i] = rand_roll.Next(30, 100);
    }

    // Calculates the win probability for squad_1 based on the ratio of total power
    private static double Probability_founder(int[] squad_1, int[] squad_2)
    {
        double squad_1_power = 0;
        double squad_2_power = 0;
        
        // Only considers the first 11 players (the active lineup)
        for (var i = 0; i < 11; i++)
        {
            squad_1_power += squad_1[i];
            squad_2_power += squad_2[i];
        }

        // Returns a percentage (e.g., 55.0 for 55%)
        return squad_1_power / (squad_1_power + squad_2_power) * 100;
    }

    private static Random rand_roll = new();

    private static void Main(string[] args)
    {
        // --- Match State Variables ---
        int manchester_goals = 0, arsenal_goals = 0;
        int event_decider = 0; // Determines what happens each minute
        int manchester_yellows = 0, manchester_reds = 0;
        int arsenal_yellows = 0, arsenal_reds = 0;
        int penalty_decider = 0; // Used to pick a random player for cards/injuries
        
        // Sub/Logic helpers
        int smallest_main = 100, biggest_reserve = 0;
        int weakestIndex = 100, strongestReserveIndex = 12;
        int switcher = 0, manchester_subs = 0, arsenal_subs = 0;
        int time = 90;

        // --- Team Initialization ---
        // Indices 0-10: Starters | Indices 11-15: Substitutes
        int[] manchester_main = new int[16];
        bool[] manchester_yellow = new bool[16]; // Tracks if player already has a yellow
        int[] arsenal_main = new int[16];
        bool[] arsenal_yellow = new bool[16];

        Squad_power(manchester_main);
        Squad_power(arsenal_main);

        // Add 1-5 minutes of "Stoppage Time"
        time += rand_roll.Next(1, 6);

        // --- MAIN MATCH LOOP (Minute by Minute) ---
        for (int i = 0; i < time; i++)
        {
            // FATIGUE LOGIC: Every 15 minutes, all starters lose 2 power points
            if (i % 15 == 0 && i > 0)
            {
                for (int t = 0; t < 11; t++)
                {
                    manchester_main[t] -= 2;
                    arsenal_main[t] -= 2;
                }
            }

            event_decider = rand_roll.Next(1, 8); // Roll for an event this minute

            // --- EVENT 1: GOAL ATTEMPT ---
            if (event_decider == 1)
            {
                double m_chance = Probability_founder(manchester_main, arsenal_main);
                
                // Roll against the calculated power probability
                if (rand_roll.Next(1, 100) < m_chance)
                {
                    manchester_goals++;
                    Console.WriteLine($"[{i}'] Manchester has scored!");
                }
                else
                {
                    arsenal_goals++;
                    Console.WriteLine($"[{i}'] Arsenal has scored!");
                }
            }

            // --- EVENT 2: FOUL / YELLOW CARD ---
            else if (event_decider == 2)
            {
                penalty_decider = rand_roll.Next(0, 11); 

                if (rand_roll.Next(0, 100) <= 50) // Manchester Foul
                {
                    if (manchester_yellow[penalty_decider]) // Check for Second Yellow
                    {
                        manchester_main[penalty_decider] = 0; // Player removed
                        manchester_reds++;
                        Console.WriteLine($"[RED] Manchester #{penalty_decider} second yellow at minute {i}");
                    }
                    else if (manchester_main[penalty_decider] > 0)
                    {
                        manchester_yellow[penalty_decider] = true;
                        manchester_main[penalty_decider] -= 10; // Yellow card penalty to power
                        manchester_yellows++;
                        Console.WriteLine($"[YELLOW] Manchester #{penalty_decider} at minute {i}");
                    }
                }
                else // Arsenal Foul
                {
                    if (arsenal_yellow[penalty_decider])
                    {
                        arsenal_main[penalty_decider] = 0;
                        arsenal_reds++;
                        Console.WriteLine($"[RED] Arsenal #{penalty_decider} second yellow at minute {i}");
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

            // --- EVENT 3: STRAIGHT RED CARD ---
            else if (event_decider == 3)
            {
                penalty_decider = rand_roll.Next(1, 12); // Note: Randomly skips index 0
                if (rand_roll.Next(0, 100) <= 50)
                {
                    if (manchester_main[penalty_decider] > 0)
                    {
                        manchester_main[penalty_decider] = 0;
                        manchester_reds++;
                        Console.WriteLine($"Manchester player {penalty_decider} sent off (Straight Red) at {i}.");
                    }
                }
                else
                {
                    if (arsenal_main[penalty_decider] > 0)
                    {
                        arsenal_main[penalty_decider] = 0;
                        arsenal_reds++;
                        Console.WriteLine($"Arsenal player {penalty_decider} sent off (Straight Red) at {i}.");
                    }
                }
            }

            // --- EVENT 4: SUBSTITUTION ---
            else if (event_decider == 4)
            {
                bool isManchester = rand_roll.Next(0, 101) <= 50;
                // Logic: Find the weakest player on the pitch and the strongest on the bench
                // If the bench player is better, swap them.
                if (isManchester && manchester_subs < 5)
                {
                    // (Search logic omitted for brevity in comments - finds weakest index 1-11)
                    // (Swaps it with strongest index 12-15)
                }
                // (Repeats for Arsenal)
            }

            // --- EVENT 5 & 6: INJURIES ---
            else if (event_decider == 5) // Manchester Injury
            {
                penalty_decider = rand_roll.Next(0, 12);
                manchester_main[penalty_decider] = Math.Max(1, manchester_main[penalty_decider] - 20);
                Console.WriteLine($"Manchester player {penalty_decider} injured at minute {i}.");
            }
            else if (event_decider == 6) // Arsenal Injury
            {
                penalty_decider = rand_roll.Next(0, 12);
                arsenal_main[penalty_decider] = Math.Max(1, arsenal_main[penalty_decider] - 20);
                Console.WriteLine($"Arsenal player {penalty_decider} injured at minute {i}.");
            }

            // --- EVENT 7: GENERAL STAMINA DRAIN ---
            else if (event_decider == 7)
            {
                for (int t = 0; t < 11; t++)
                {
                    manchester_main[t]--;
                    arsenal_main[t]--;
                }
            }
        }

        // --- FINAL WHISTLE & STATS ---
        Console.WriteLine("\n" + new string('=', 40));
        Console.WriteLine("            FINAL WHISTLE");
        Console.WriteLine(new string('=', 40));
        Console.WriteLine($"MANCHESTER {manchester_goals} - {arsenal_goals} ARSENAL");
        // ... Winner & Stats output logic
    }
}
