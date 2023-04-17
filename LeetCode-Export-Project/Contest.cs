using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Contest
{
    //userContestRankingHistory Query
    bool? attended;
    string? trendDirection;
    int? problemsSolved;
    int? totalProblems;
    int? finishTimeInSeconds;
    int? rating;
    int? ranking;
    string? contestName;

    public bool? Attended { get => attended; set => attended = value; }
    public string? TrendDirection { get => trendDirection; set => trendDirection = value; }
    public int? ProblemsSolved { get => problemsSolved; set => problemsSolved = value; }
    public int? TotalProblems { get => totalProblems; set => totalProblems = value; }
    public int? FinishTimeInSeconds { get => finishTimeInSeconds; set => finishTimeInSeconds = value; }
    public int? Rating { get => rating; set => rating = value; }
    public int? Ranking { get => ranking; set => ranking = value; }
    public string? ContestName { get => contestName; set => contestName = value; }

    public override string ToString()
    {
        TimeSpan t = TimeSpan.FromSeconds((double)finishTimeInSeconds);
        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"Contest Name: {contestName}");
        sb.AppendLine($"Attended: {attended}");
        sb.AppendLine($"Trend direction: {trendDirection}");
        sb.AppendLine($"Problems solved: {problemsSolved}");
        sb.AppendLine($"Total problems: {totalProblems}");
        sb.AppendLine($"Finish time in seconds: {t}");
        sb.AppendLine($"Rating: {rating}");
        sb.AppendLine($"Ranking: {ranking}");
        

        return sb.ToString();
    }

}


