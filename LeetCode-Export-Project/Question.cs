using LeetCode_Export;
using System.Text;

public class Question
{
    string titleSlug;
    List<Submission>? submissions;
    string? questionFrontEndId;
    int? questionId;
    string? title;
    string? difficulty;
    string? status;
    string? stats;
    bool? isPaidOnly;
    int? likes;
    int? dislikes;
    bool? isLiked;
    string? content;
    string? sampleTestCase;

    public string TitleSlug { get => titleSlug; set => titleSlug = value; }
    public List<Submission>? Submissions { get => submissions; set => submissions = value; }
    public string? QuestionFrontEndId { get => questionFrontEndId; set => questionFrontEndId = value; }
    public int? QuestionId { get => questionId; set => questionId = value; }
    public string? Title { get => title; set => title = value; }
    public string? Difficulty { get => difficulty; set => difficulty = value; }
    public string? Status { get => status; set => status = value; }
    public string? Stats { get => stats; set => stats = value; }
    public bool? IsPaidOnly { get => isPaidOnly; set => isPaidOnly = value; }
    public int? Likes { get => likes; set => likes = value; }
    public int? Dislikes { get => dislikes; set => dislikes = value; }
    public bool? IsLiked { get => isLiked; set => isLiked = value; }
    public string? Content { get => content; set => content = value; }
    public string? SampleTestCase { get => sampleTestCase; set => sampleTestCase = value; }

    //string[] topicTags;
    /*
query can also include:
    codeSnippets {
        lang
        langSlug
        code
    }
Leaving out for now...
*/


    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.AppendLine($"Title Slug: {titleSlug}");
        sb.AppendLine($"Question FrontEnd Id: {questionFrontEndId}");
        //sb.AppendLine($"Question Id: {questionId}");
        sb.AppendLine($"Title: {title}");
        sb.AppendLine($"Difficulty: {difficulty}");
        sb.AppendLine($"Status: {status}");
        sb.AppendLine($"Stats: {stats}");
        sb.AppendLine($"Submissions: {submissions.Count}");
        foreach(Submission sub in submissions)
        {
            sb.AppendLine(sub.ToString());
        }

        //sb.AppendLine($"Is Paid Only: {isPaidOnly}");
       // sb.AppendLine($"Likes: {likes}");
        //sb.AppendLine($"Dislikes: {dislikes}");
        //sb.AppendLine($"Is Liked: {isLiked}");
        //sb.AppendLine($"Content: {content}");
        sb.AppendLine($"Sample Test Case: {sampleTestCase}");
        return sb.ToString();
    }

}

