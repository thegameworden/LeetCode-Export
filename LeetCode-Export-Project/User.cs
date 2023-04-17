using LeetCode_Export;
using System.Text;

    
public class User
{

    string? username;

    List<Question>? questions;
    //getUserData Query
    int? attendedContestsCount;
    int? currentRating;
    int? globalRanking;
    int? totalParticipants;
    int? topPercentage;
    List<Contest>? contests;
    List<AcceptedSubmissionNumber> acceptedSubmissionNumbers;





    public int? AttendedContestsCount { get => attendedContestsCount; set => attendedContestsCount = value; }
    public int? CurrentRating { get => currentRating; set => currentRating = value; }
    public int? GlobalRanking { get => globalRanking; set => globalRanking = value; }
    public int? TotalParticipants { get => totalParticipants; set => totalParticipants = value; }
    public int? TopPercentage { get => topPercentage; set => topPercentage = value; }
    public string Username { get => username; set => username = value; }
    public List<Contest>? Contests { get=> contests; set=> contests = value; }
    public List<AcceptedSubmissionNumber> AcceptedSubmissionNumbers { get => acceptedSubmissionNumbers; set => acceptedSubmissionNumbers = value; }
    public List<Question>? Questions { get => questions; set => questions = value; }

    public string QuestionsToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("[");
        foreach(Question q in Questions)
        {
            sb.AppendLine(q.ToString());
        }
        sb.AppendLine("]");
        return sb.ToString();
    }

    public string AcceptedSubmissionNumbersToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("[");
        foreach (AcceptedSubmissionNumber a in AcceptedSubmissionNumbers)
        {
            sb.AppendLine(a.ToString());
        }
        sb.AppendLine("]");
        return sb.ToString();
    }
    public string ContestsToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("[");
        foreach (Contest c in Contests)
        {
            sb.AppendLine(c.ToString());
        }
        sb.AppendLine("]");
        return sb.ToString();
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();

        sb.AppendLine($"\n\n\nUsername: {username}");
        sb.AppendLine($"----------------------------------------------------------\n\n\n");

        sb.AppendLine("Submission stas:");
        if (acceptedSubmissionNumbers == null)
        {
            sb.AppendLine("No accepted submissions found.");
        }
        else
        {
            sb.AppendLine("[");
            foreach (var data in acceptedSubmissionNumbers)
            {
                sb.AppendLine($"- {data.ToString()}");
            }
            sb.AppendLine("]");
        }




        sb.AppendLine($"\n\n\n----------------------------------------------------------\n\n\n");




        sb.AppendLine("Questions:");
        if (Questions == null)
        {
            sb.AppendLine("No questions found.");
        }
        else
        {
            sb.AppendLine("[");
            foreach (var question in Questions)
            {
                sb.AppendLine($"- {question.ToString()}");
            }
            sb.AppendLine("]");
        }


        sb.AppendLine($"\n\n\n----------------------------------------------------------\n\n\n");
        sb.AppendLine("Contest Overview:");
        sb.AppendLine($"Attended contests count: {attendedContestsCount}");
        sb.AppendLine($"Current rating: {currentRating}");
        sb.AppendLine($"Global ranking: {globalRanking}/{totalParticipants}");
        //sb.AppendLine($"Total participants: {totalParticipants}");
        sb.AppendLine($"Top percentage: {topPercentage}%");

        sb.AppendLine($"\n\n\n----------------------------------------------------------\n\n\n");

        sb.AppendLine("Contests:");
        if (Contests == null)
        {
            sb.AppendLine("No contests found.");
        }
        else
        {
            sb.AppendLine("[");
            foreach (var contest in Contests)
            {
                sb.AppendLine($"- {contest.ToString()}");
            }
            sb.AppendLine("]");
        }

        return sb.ToString();
    }

}