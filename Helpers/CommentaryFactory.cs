namespace GBS.Api.Helpers
{
    public static class CommentaryFactory
    {
        public static string Build(int runs, int extras, string? extrasType, bool isWicket, string? wicketType,
                                    string? batter, string? bowler, string? fielder)
        {
            batter ??= "the batter";
            bowler ??= "the bowler";

            if (isWicket)
            {
                var t = (wicketType ?? "Bowled").ToLowerInvariant();
                return t switch
                {
                    "bowled"     => $"OUT! {bowler} cleans up {batter} — castled!",
                    "caught"     => $"WICKET! {batter} caught by {fielder ?? "the fielder"} off {bowler}.",
                    "lbw"        => $"LBW! {bowler} traps {batter} plumb in front.",
                    "runout"     => $"RUN OUT! {batter} is short of the crease, brilliant work.",
                    "stumped"    => $"STUMPED! Lightning hands behind the stumps, {batter} has to walk.",
                    "hitwicket"  => $"HIT WICKET! {batter} disturbs the timber on the way down.",
                    _            => $"WICKET! {batter} is out ({wicketType}).",
                };
            }

            if (!string.IsNullOrEmpty(extrasType))
            {
                return extrasType switch
                {
                    "Wide"    => $"WIDE — {bowler} sprays it down leg, {extras} extra.",
                    "NoBall"  => $"NO BALL! Free hit looming — {bowler} oversteps. {extras + runs} off the delivery.",
                    "Bye"     => $"{runs} bye{(runs == 1 ? "" : "s")} — slipped past everybody.",
                    "LegBye"  => $"{runs} leg-bye{(runs == 1 ? "" : "s")} — taken off the pads.",
                    "Penalty" => $"PENALTY runs awarded — {extras} added.",
                    _         => $"Extras: {extrasType}, {extras} run(s).",
                };
            }

            return runs switch
            {
                0 => $"Dot ball — {bowler} keeps it tight, {batter} blocks it.",
                1 => $"Single — {batter} works it for one.",
                2 => $"Couple — {batter} picks the gap, two runs.",
                3 => $"Three! {batter} threads it into the deep.",
                4 => $"FOUR! {batter} times it beautifully through the gap.",
                6 => $"SIX! {batter} clears the rope — that has gone the distance!",
                _ => $"{batter} takes {runs} off {bowler}.",
            };
        }

        public static (string? milestoneType, string? milestoneLine) MilestoneFor(int batterRunsBefore, int batterRunsAfter, string batter)
        {
            if (batterRunsBefore < 50 && batterRunsAfter >= 50 && batterRunsAfter < 100)
                return ("Fifty", $"FIFTY! Well-played knock from {batter} — half-century.");
            if (batterRunsBefore < 100 && batterRunsAfter >= 100)
                return ("Hundred", $"HUNDRED! Brilliant century from {batter}!");
            return (null, null);
        }
    }
}
