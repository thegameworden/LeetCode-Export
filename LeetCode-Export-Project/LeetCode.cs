using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using Newtonsoft.Json;
using System.Net;
using System.Text.RegularExpressions;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Threading.Tasks;

namespace LeetCode_Export
{

    public class LeetCode
    {

        public HttpClient session;
        DateTime userLoggedExpiration;
        Dictionary<string, string> cookie_dict;
        public HttpRequestMessage request;
        public GraphQLHttpClient client;

        public LeetCode()
        {
            userLoggedExpiration = DateTime.Now;
            cookie_dict = new();  
        }


        public async Task<User> login()
        {
            User user = new();
            try
            {
                // Configure ChromeDriver to disable logging
                var chromeOptions = new ChromeOptions();
                chromeOptions.AddArgument("--log-level=3");

                var driverService = ChromeDriverService.CreateDefaultService();
                driverService.SuppressInitialDiagnosticInformation = true;
                driverService.HideCommandPromptWindow = true;

                using (var driver = new ChromeDriver(chromeOptions))
                {
                    driver.Navigate().GoToUrl("https://leetcode.com/accounts/login/");



                    // Wait for the user to log in manually
                    Console.Write("Please log in manually on the popup, then and press 'Enter'.");
                    Console.ReadLine();
                    Console.Clear();

                    bool isLoggedIn = driver.Url!= "https://leetcode.com/account/login/";
                    Console.WriteLine(isLoggedIn ? "Logged in successfully." : "Failed to log in.");

                    if (isLoggedIn)
                    {
                        // Get the cookies from the Selenium WebDriver's CookieJar
                        var seleniumCookies = driver.Manage().Cookies.AllCookies;

                        // Create an HttpClient instance and initialize its CookieContainer
                        var handler = new HttpClientHandler { CookieContainer = new CookieContainer() };
                        session = new HttpClient(handler);
                        
                            // Add the cookies from the WebDriver to the HttpClient's CookieContainer
                            foreach (var seleniumCookie in seleniumCookies)
                            {
                                var netCookie = new System.Net.Cookie(seleniumCookie.Name, seleniumCookie.Value, seleniumCookie.Path, seleniumCookie.Domain);
                            if (seleniumCookie.Name.Contains("gr_last_sent_cs1"))
                            {
                                user.Username = seleniumCookie.Value; 
                            }
                                handler.CookieContainer.Add(netCookie);
                            }

                            var options = new GraphQLHttpClientOptions
                            {
                                EndPoint = new Uri("https://leetcode.com/graphql")
                            };

                            client = new GraphQLHttpClient(options, new NewtonsoftJsonSerializer(), session);



                        if (user.Username != null) return user;

                        
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error logging in: {ex.Message}");
                return null;
            }
            return null;
        }



 

        public async Task GetUser(User user)
        {
            string q = @"
        query getUserData($username: String!) {
            userContestRanking(username: $username) {
                attendedContestsCount
                rating
                globalRanking
                totalParticipants
                topPercentage
            }

            userContestRankingHistory(username: $username)
            {
                attended
                trendDirection
                problemsSolved
                totalProblems
                finishTimeInSeconds
                rating
                ranking
                contest 
                {
                    title
                    startTime
                }
            }

            matchedUser(username: $username) {
                submitStatsGlobal {
                    acSubmissionNum {
                    difficulty
                    count
                    }
                }
                #problemsSolvedBeatsStats {
                    #difficulty
                    #percentage
                #}
            }

            #allQuestionsCount {
                #difficulty
                #count
            #}
            
    
        }";
        
            var query = new GraphQLRequest
            {
                Query = q,
                Variables = new
                {
                    username = user.Username
                }
            };
            var response = await client.SendQueryAsync<dynamic>(query);
            
            if (response.Errors != null)
            {
                Console.WriteLine($"GraphQL errors during GetUser({user}):");
                foreach (var error in response.Errors)
                {
                    Console.WriteLine("- " + error.Message);
                }
            }
            else
            {

                //based on  userContestRanking
                Console.WriteLine("Getting general user contest info");
                user.AttendedContestsCount = response.Data.userContestRanking.attendedContestsCount;
                user.CurrentRating = response.Data.userContestRanking.rating;
                user.GlobalRanking = response.Data.userContestRanking.globalRanking;
                user.TotalParticipants = response.Data.userContestRanking.totalParticipants;
                user.TopPercentage = response.Data.userContestRanking.topPercentage;
                Console.WriteLine("Done getting general user contest info");


                Console.WriteLine("Getting all the users contest info");
                foreach (var data in response.Data.userContestRankingHistory) //print if attended contest using userContestRankingHistory
                {
                    if (data.attended==false) continue;

                    Contest contest = new Contest();
                    contest.Attended = true;
                    contest.Rating = data.rating;
                    contest.TrendDirection = data.trendDirection;
                    contest.ProblemsSolved = data.problemsSolved;
                    contest.TotalProblems = data.totalProblems;
                    contest.FinishTimeInSeconds = data.finishTimeInSeconds;
                    contest.Rating =data.rating;
                    contest.ContestName = data.contest.title;
                    contest.Ranking = data.ranking;
                    if (user.Contests == null) user.Contests = new();

                    user.Contests.Add(contest);
                }
                Console.WriteLine("Done getting contest info");

                Console.WriteLine("Getting accepted submission numbers...");
                foreach(var data in response.Data.matchedUser.submitStatsGlobal.acSubmissionNum)
                {
                    if (user.AcceptedSubmissionNumbers == null) user.AcceptedSubmissionNumbers= new();

                    AcceptedSubmissionNumber adder = new();
                    adder.Difficulty = data.difficulty;
                    adder.Count = data.count;
                    user.AcceptedSubmissionNumbers.Add(adder);

                }
                Console.WriteLine("Done getting the accepted submission numbers!");
            }
        }

        public async Task<List<Question>?> getAllQuestionsAnswered()
        {
            Console.WriteLine("Parsing through all questions");



            var request = new GraphQLHttpRequest
            {

                 Query = @"
             query {
                 allQuestions {
                     titleSlug
                     #questionFrontendId
                     #questionId
                     #title
                     #difficulty
                     status
                     #stats
                     #isPaidOnly
                 }
             }"
             };

            var response = await client.SendQueryAsync<dynamic>(request);

            int seconds = 0;
            while (response.Data == null && seconds<30)
            {
                Console.SetCursorPosition(0, Console.CursorTop - 1);
                Console.WriteLine($"Question was null. Timer: {seconds} ");
                await Task.Delay(TimeSpan.FromSeconds(1));
                seconds++;
            }


            if (response.Errors != null)
            {
                Console.WriteLine("GraphQL errors during getAll():");
                foreach (var error in response.Errors)
                {
                    Console.WriteLine("- " + error.Message);
                }
                return null;
            }
            else
            {
                List<Question> questions = new();

                
                foreach (var q in response.Data.allQuestions)
                {
                   
                    if (q.status == "ac")
                    {
                        Question question = await getQuestionDetails((string)q["titleSlug"]);
                        if(question != null)
                        questions.Add(question);
                    }
                }

                Console.WriteLine($"DONE! returning {questions.Count} question info to user");
                
                return questions;
            }
        }


        public async Task<Question?> getQuestionDetails(string slug)
        {
            Console.WriteLine("Getting info on question: " +slug);
            // Define the GraphQL query
            var query = new GraphQLRequest
            {
                Query = @"
            query getQuestionDetail($titleSlug: String!) {
                question(titleSlug: $titleSlug) {
                    questionFrontendId                    
                    questionId
                    title
                    difficulty
                    status
                    stats
                    isPaidOnly
                    likes
                    dislikes
                    isLiked
                    content
                    sampleTestCase

                    #topicTags {
                        #name
                        #}
                    #codeSnippets {
                        #lang
                        #langSlug
                        #code
                        #}

                    }
            }",
                Variables = new
                {
                    titleSlug = slug
                }
            };

            var response = await client.SendQueryAsync<dynamic>(query);
            int seconds = 0;
            while (response.Data == null && seconds<30)
            {
                Console.SetCursorPosition(0, Console.CursorTop - 1);
                Console.WriteLine($"Question Details was null. Timer: {seconds} ");
                await Task.Delay(TimeSpan.FromSeconds(1));
                seconds++;
            }

            // Handle the response
            if (response.Errors != null)
            {
                Console.WriteLine($"GraphQL errors during getQuestionDetails({slug}):");
                foreach (var error in response.Errors)
                {
                    Console.WriteLine("- " + error.Message);
                }
                return null;
            }
            else
            {
                
                var data = response.Data.question;
                Question question = new();
                question.TitleSlug = slug;
                question.QuestionFrontEndId = data.questionFrontendId;
                question.QuestionId = data.questionId;
                question.Title = data.title;
                question.Difficulty = data.difficulty;
                question.Status = data.status;
                question.Stats = data.stats;
                question.IsPaidOnly = data.isPaidOnly;
                question.Likes= data.likes;
                question.Dislikes= data.dislikes;
                question.IsLiked = data.isLiked;
                question.Content = data.content;
                question.SampleTestCase = data.sampleTestCase;

                List<Submission> submissions = await getFirstAccSubmission(slug);
                if(submissions!=null)
                    question.Submissions=submissions;

                Console.WriteLine("Done getting info on: " +question.TitleSlug);
                return question;
            }

        }

        public async Task<List<Submission>?> getFirstAccSubmission(string slugTitle)
        {
            Console.WriteLine("Getting submissions for " +slugTitle);
            int offset = 0;
            int limit = 20;
            bool hasNext = true;
            string slug = slugTitle;
            List<Submission> submissions = new();
            while (hasNext)
            {
                var request = new GraphQLHttpRequest
                {
                    Query = @"
            query Submissions($offset: Int!, $limit: Int!, $questionSlug: String!) {
                submissionList(offset: $offset, limit: $limit, questionSlug: $questionSlug) {
                    lastKey
                    hasNext
                    submissions {
                        id
                        statusDisplay
                        lang
                        runtime
                        timestamp
                        url
                        isPending
                        memory
                            
                        }
                    }
            }",
                    Variables = new
                    {
                        offset = offset,
                        limit = limit,
                        questionSlug = slug
                    }
                };

                var response = await client.SendQueryAsync<dynamic>(request);
                int seconds = 0;
                while (response.Data == null && seconds <30)
                {
                    Console.SetCursorPosition(0, Console.CursorTop - 1);
                    Console.WriteLine($"Submissions list was null. Timer: {seconds} ");
                    await Task.Delay(TimeSpan.FromSeconds(1));
                    seconds++;
                }

                if (response.Errors != null)
                {

                    Console.WriteLine($"GraphQL errors during getFirstAccSubmission({slugTitle}):");
                    foreach (var error in response.Errors)
                    {
                        Console.WriteLine("- " + error.Message);
                    }
                    return null;
                }
                else
                {
                    if (response.Data.submissionList.submissions == null) {
                        Console.WriteLine("Unable get submissions for: "+slugTitle);
                        return submissions;
                    }
                    foreach (var submission in response.Data.submissionList.submissions)
                    {
                        if (submission.statusDisplay != "Accepted") continue;
                        Submission sub = await getSubmissionDetails((int)submission.id);
                        await Task.Delay(TimeSpan.FromSeconds(1));

                        if (sub != null)
                        submissions.Add(sub);

                        if(submissions.Count  > 0 && submissions[submissions.Count-1] !=null)
                        {
                            return submissions;
                        }
                    }

                    if (response.Data["hasNext"] == null) hasNext = false;
                    else
                        hasNext = true;
                        offset += limit;

                }
            }
            Console.WriteLine($"Done going through {submissions.Count} for " + slugTitle);
            return submissions;

        }

        public async Task<Submission> getSubmissionDetails(int submissionId)
        {
            Submission submission = new();
            Console.WriteLine("Starting with: " +submissionId);
            var request = new GraphQLHttpRequest
            {
                Query = @"
                    query submissionDetails($submissionId: Int!) {
                      submissionDetails(submissionId: $submissionId) {
                        runtime
                        runtimeDisplay
                        runtimePercentile
                        runtimeDistribution
                        memory
                        memoryDisplay
                        memoryPercentile
                        memoryDistribution
                        code
                        timestamp
                        statusCode
                        user {
                          username
                          #profile {
                            #realName
                            #userAvatar
                          #}
                        }
                        lang {
                          name
                          verboseName
                          
                        }
                        question {
                          questionId
                        }
                        notes
                        #topicTags {
                          #tagId
                          #slug
                          #name
                        #}
                        runtimeError
                        compileError
                        lastTestcase
                      }
                    }",
                Variables = new
                {
                    submissionId = submissionId
                }
            };


            try
            {

                var response = await client.SendQueryAsync<dynamic>(request);

                int requestCount = 0;
                while ((response.Data == null && requestCount < 3) || (response.Data.submissionDetails==null && requestCount<3 ))
                {
                    int seconds = 0;
                    while (response.Data.submissionDetails == null && seconds <= 10)
                    {

                        Console.SetCursorPosition(0, Console.CursorTop-1);
                        Console.WriteLine($"Submission was null. Waiting 10 seconds Timer: {seconds} ");
                        await Task.Delay(TimeSpan.FromSeconds(1));
                        seconds++;
                    }
                    Console.WriteLine("Sending request again...");
                    requestCount++;
                    response = await client.SendQueryAsync<dynamic>(request);
                }

                

                if (response.Errors != null)
                {

                    Console.WriteLine($"GraphQL errors during getSubmissionDetails({submissionId}):");
                    foreach (var error in response.Errors)
                    {
                        Console.WriteLine("- " + error.Message);
                    }
                    return null;
                }
                else
                {
                    if (response.Data.submissionDetails== null) {
                        Console.WriteLine("Received null for submission: " +submissionId);

                        return null; 
                    }
                    submission.Id = submissionId;

                    submission.QuestionId = response.Data.submissionDetails.question.questionId;
                    submission.Runtime = response.Data.submissionDetails.runtime;
                    submission.Code = response.Data.submissionDetails.code;
                    submission.Url = response.Data.submissionDetails.url;
                    submission.Lang_name = response.Data.submissionDetails.lang.name;
                    submission.Lang_verboseName = response.Data.submissionDetails.lang.verboseName;
                    submission.RuntimeError = response.Data.submissionDetails.runtimeError;
                    submission.CompileError = response.Data.submissionDetails.compileError;
                    submission.IsPending = response.Data.submissionDetails.isPending;
                    submission.Memory = response.Data.submissionDetails.memory;
                    submission.RuntimeDisplay = response.Data.submissionDetails.runtimeDisplay;
                    submission.RuntimePercentile = response.Data.submissionDetails.runtimePercentile;
                    submission.RuntimeDistribution = response.Data.submissionDetails.runtimeDistribution;
                    submission.MemoryDisplay = response.Data.submissionDetails.memoryDisplay;
                    submission.MemoryDistribution = response.Data.submissionDetails.memoryDistribution;
                    submission.StatusCode = response.Data.submissionDetails.statusCode;
                    submission.Timestamp = response.Data.submissionDetails.timestamp;
                    submission.StatusCode = response.Data.submissionDetails.statusCode;
                    submission.Notes = response.Data.submissionDetails.notes;
                    submission.LastTestCase = response.Data.submissionDetails.lastTestCase;
  
                Console.WriteLine("finished getting data for submission: " + submission.Id);
                }
            }
            catch (GraphQLHttpRequestException ex)
            {
                Console.WriteLine("Status code: " + ex.StatusCode);
                Console.WriteLine("Message: " + ex.Message);
                Console.WriteLine(ex.Content);
                Console.WriteLine("Inner exception: " + ex.InnerException?.Message); ;
                return null;
            }

            return submission;
        }






        public void printHttpClientHeaders()
        {
            Console.WriteLine("HttpClient Headers:");
            foreach (var header in session.DefaultRequestHeaders)
            {
                Console.WriteLine($"{header.Key}: {string.Join(", ", header.Value)}");
            }
        }


















        //Deprecated
        public bool setCookies()
        {

            Console.Write("Enter Cookie: ");
            string cookieInput = Console.ReadLine();


            Console.Clear();
            Console.WriteLine("Parsing Cookies...");

            cookie_dict = new Dictionary<string, string>();
            string[] cookies = cookieInput.Split(';');
            foreach (string cookie2 in cookies)
            {
                string[] cookie_split = cookie2.Split("=");
                cookie_dict.Add(cookie_split[0].Trim(), Uri.EscapeDataString(cookie_split[1].Trim()));
            }

            var cookieContainer = new CookieContainer();
            var httpClientHandler = new HttpClientHandler
            {
                CookieContainer = cookieContainer,
                UseCookies = true,

            };
            var baseAddress = new Uri("https://leetcode.com/");
            foreach (var kvp in cookie_dict)
            {
                cookieContainer.Add(baseAddress, new System.Net.Cookie(kvp.Key, kvp.Value));
            }
            session = new HttpClient(httpClientHandler)
            {
                Timeout = TimeSpan.FromSeconds(30)
            };
            session.BaseAddress = baseAddress;
            session.DefaultRequestHeaders.Referrer = new Uri("https://leetcode.com/");
            session.DefaultRequestHeaders.Add("X-CSRFToken", cookie_dict["csrftoken"]);

            var options = new GraphQLHttpClientOptions
            {
                EndPoint = new Uri("https://leetcode.com/graphql")
            };

            client = new GraphQLHttpClient(options, new NewtonsoftJsonSerializer(), session);
            Console.WriteLine("Done Parsing!!");
            return cookie_dict.ContainsKey("csrftoken") && cookie_dict.ContainsKey("LEETCODE_SESSION");

        }


        //works but dont really use... not with GraphQL
        public async Task getSubmissions()
        {
            int current = 0;
            int limit = 20;
            Dictionary<string, dynamic> response_json = new Dictionary<string, dynamic>
        {
            {"has_next", true}
        };

            while (!response_json.ContainsKey("detail") && response_json.ContainsKey("has_next") && response_json["has_next"])
            {
                Console.WriteLine($"Exporting submissions from {current} to {current + limit}");
                string url = string.Format($"https://leetcode.com/api/submissions/?offset={current}&limit={limit}");
                HttpResponseMessage response = await session.GetAsync(url);






                string json = await response.Content.ReadAsStringAsync();

                response_json = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(json);


                if (response_json.ContainsKey("submissions_dump"))
                {

                    var submissions_dump = response_json["submissions_dump"];
                    var submissions = JsonConvert.DeserializeObject<List<Dictionary<string, dynamic>>>(submissions_dump.ToString());


                    foreach (var submission in submissions)
                    {
                        var submission_dict = submission as Dictionary<string, dynamic>;
                        if (submission_dict["status_display"] != "Accepted")
                        {
                            Console.WriteLine("Skipping..... " + submission_dict["status_display"]);
                            continue;
                        }
                        submission_dict["runtime"] = submission_dict["runtime"].Replace(" ", "");
                        submission_dict["memory"] = submission_dict["memory"].Replace(" ", "");
                        submission_dict["date_formatted"] = DateTimeOffset.FromUnixTimeSeconds(submission_dict["timestamp"]).ToString("yyyy-MM-dd HH.mm.ss");

                        await getQuestionDetails(submission_dict["title_slug"]);
                        
                    }

                }


                current += limit;
                // Thread.Sleep(5000); // cooldown time for GET request
                Thread.Sleep(2000);
            }

            Console.WriteLine(response_json.ToArray()[0]);

            if (response_json.ContainsKey("detail"))
            {
                Console.WriteLine("LeetCode API error, detail found in response_json. response_json[\"detail\"]: " + response_json["detail"]);
            }


        }


    }
}