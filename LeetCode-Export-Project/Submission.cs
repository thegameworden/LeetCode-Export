using LeetCode_Export;
using System.Text;

public class Submission
{
    int? id;
    string? runtime;
    string? statusDisplay;
    string? timestamp;
    string? url;
    string? isPending;
    string? memory;
    string? runtimeDisplay;
    int? runtimePercentile;
    string? runtimeDistribution;
    string? memoryDisplay;
    int? memoryPercentile;
    string? memoryDistribution;
    string? code;
    int? statusCode;
    string? notes;
    string? lang_name;
    string? lang_verboseName;
    int? questionId;
    string? runtimeError;
    string? compileError;
    string? lastTestCase;
    /*
    #topicTags {
        #tagId
        #slug
        #name
    #}
    */
    public int? Id { get => id; set => id = value; }
    public string? Runtime { get => runtime; set => runtime = value; }
    public string? StatusDisplay { get => statusDisplay; set => statusDisplay = value; }
    public string? Timestamp { get => timestamp; set => timestamp = value; }
    public string? Url { get => url; set => url = value; }
    public string? IsPending { get => isPending; set => isPending = value; }
    public string? Memory { get => memory; set => memory = value; }
    public string? RuntimeDisplay { get => runtimeDisplay; set => runtimeDisplay = value; }
    public int? RuntimePercentile { get => runtimePercentile; set => runtimePercentile = value; }
    public string? RuntimeDistribution { get => runtimeDistribution; set => runtimeDistribution = value; }
    public string? MemoryDisplay { get => memoryDisplay; set => memoryDisplay = value; }
    public int? MemoryPercentile { get => memoryPercentile; set => memoryPercentile = value; }
    public string? MemoryDistribution { get => memoryDistribution; set => memoryDistribution = value; }
    public string? Code { get => code; set => code = value; }
    public int? StatusCode { get => statusCode; set => statusCode = value; }
    public string? Notes { get => notes; set => notes = value; }
    public string? Lang_name { get => lang_name; set => lang_name = value; }
    public string? Lang_verboseName { get => lang_verboseName; set => lang_verboseName = value; }
    public int? QuestionId { get => questionId; set => questionId = value; }
    public string? RuntimeError { get => runtimeError; set => runtimeError = value; }
    public string? CompileError { get => compileError; set => compileError = value; }
    public string? LastTestCase { get => lastTestCase; set => lastTestCase = value; }

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.AppendLine($"ID: {id}");
        sb.AppendLine($"Language Name: {lang_name}");
        sb.AppendLine($"Language Verbose Name: {lang_verboseName}");
        sb.AppendLine($"Question ID: {questionId}");
        sb.AppendLine($"URL: {url}");
        sb.AppendLine($"Code: {code}");
        //sb.AppendLine($"Runtime: {runtime}");
        //sb.AppendLine($"Status Display: {statusDisplay}");
        // sb.AppendLine($"Timestamp: {timestamp}");
        
        //sb.AppendLine($"Is Pending: {isPending}");
        //sb.AppendLine($"Memory: {memory}");
        //sb.AppendLine($"Runtime Display: {runtimeDisplay}");
        //sb.AppendLine($"Runtime Percentile: {runtimePercentile}");
        //sb.AppendLine($"Runtime Distribution: {runtimeDistribution}");
        //sb.AppendLine($"Memory Display: {memoryDisplay}");
        //sb.AppendLine($"Memory Percentile: {memoryPercentile}");
        //sb.AppendLine($"Memory Distribution: {memoryDistribution}");
        //sb.AppendLine($"Status Code: {statusCode}");
        //sb.AppendLine($"Notes: {notes}");
        //sb.AppendLine($"Runtime Error: {runtimeError}");
        //sb.AppendLine($"Compile Error: {compileError}");
       // sb.AppendLine($"Last Test Case: {lastTestCase}");
        return sb.ToString();
    }



}

