using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeetCode_Export
{
    public static class Utilities
    {
        public static readonly Dictionary<string, string> fileExtensions = new()
        {
            { "python", "py" },
            { "c", "c" },
            { "cpp", "cpp" },
            { "csharp", "cs" },
            { "java", "java" },
            { "kotlin", "kt" },
            { "mysql", "sql" },
            { "mssql", "sql" },
            { "oraclesql", "sql" },
            { "javascript", "js" },
            { "html", "html" },
            { "php", "php" },
            { "golang", "go" },
            { "scala", "scala" },
            { "pythonml", "py" },
            { "rust", "rs" },
            { "ruby", "rb" },
            { "bash", "sh" },
            { "swift", "swift" },

        };

        public static readonly Dictionary<string, string> singleLineComment = new()
        {
            { "python", "#" },
            { "c", "//" },
            { "cpp", "//" },
            { "csharp", "//" },
            { "java", "//" },
            { "kotlin", "//" },
            { "mysql", "--" },
            { "mssql", "--" },
            { "oraclesql", "--" },
            { "javascript", "//" },
            { "html", "<!-- -->" },
            { "php", "//" },
            { "golang", "//" },
            { "scala", "//" },
            { "pythonml", "#" },
            { "rust", "//" },
            { "ruby", "#" },
            { "bash", "#" },
            { "swift", "//" },
        };

        public static string getFileExtension(string fileType)
        {
            if (fileExtensions.ContainsKey(fileType))
                return fileExtensions[fileType];
            Console.WriteLine($"Error: Utilities does not have the extension for {fileType}. Placing into .txt file");
            return "txt";
        }

        public static string getSingleLineCommentInput(string fileType)
        {
            if(singleLineComment.ContainsKey(fileType)) return singleLineComment[fileType];
            Console.WriteLine($"Error: Utilities does not have the single line comment input for {fileType}.");
            return "";
        }

        public static void writeFiles(User user, string downloadLocation)
        {
            string mainFolder = $"{downloadLocation}/{user.Username}'s LeetCode Information";

            try
            {
                Directory.CreateDirectory(mainFolder);
                Directory.CreateDirectory($"{mainFolder}/DumpData");
                File.AppendAllText($"{mainFolder}/DumpData/DumpData.txt", "Dumping Data Here\n\n\n");
                File.AppendAllText($"{mainFolder}/DumpData/DumpData.txt", user.ToString());

                Directory.CreateDirectory($"{mainFolder}/General Coding Info");
                string statsFolder = $"{mainFolder}/General Coding Info/{user.Username} Stats.txt";
                File.WriteAllText(statsFolder, $"This page contains the general information about {user.Username}. This includes:\n\tTheir submission stats\n\tTheir contest rating\n\tThe breakdown of all the contests they have attended\n");
                File.AppendAllText(statsFolder, "Submission Stats:\n");
                File.AppendAllText(statsFolder, user.AcceptedSubmissionNumbersToString());
                File.AppendAllText(statsFolder, $"\n\n\n{user.Username}'s contest ranking stats\n\n");
                File.AppendAllText(statsFolder, $"Rating: {user.CurrentRating}\nTop Percentage: {user.TopPercentage}%\nGlobal Ranking: {user.GlobalRanking}/{user.TotalParticipants}\nContests Attended: {user.AttendedContestsCount}\n");
                File.AppendAllText(statsFolder, $"\n\n\n{user.Username}'s contests stats\n\n");
                File.AppendAllText(statsFolder, user.ContestsToString());

                writeSubmissionFiles(user, mainFolder);

                Console.WriteLine("Finished writing to file");
            } catch (Exception e)
            {
                Console.WriteLine($"Error creating files: {e.Message}");
            }
        }

        public static void writeSubmissionFiles(User user, string mainFolder)
        {
            string answerfolder = $"{mainFolder}/LeetCodeAnswers";
            Directory.CreateDirectory(answerfolder);
            if (user.Questions == null) return;
            foreach(Question question in user.Questions)
            {
                if (question.Submissions==null || question.Submissions.Count==0) continue;

                File.WriteAllText($"{answerfolder}/{question.QuestionId}. [{question.Difficulty}] - {question.Title}.{getFileExtension(question.Submissions[0].Lang_name)}", $"{getSingleLineCommentInput(question.Submissions[0].Lang_name)}This problem can be found at: https://leetcode.com/problems/{question.TitleSlug}\n\n");
                File.AppendAllLines($"{answerfolder}/{question.QuestionId}. [{question.Difficulty}] - {question.Title}.{getFileExtension(question.Submissions[0].Lang_name)}", new string[] {question.Submissions[0].Code});

            }


        }

    }
}
