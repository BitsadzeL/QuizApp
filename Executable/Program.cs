using QuizApp.Models;
using QuizApp.Repository;


namespace Executable
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string UsersFilePath = @"../../../../QuizApp/TestData/Users.json";
            string QuizsFilePath = @"../../../../QuizApp/TestData/Quizs.json";
            UserRepository userRepository = new UserRepository(UsersFilePath);
            QuizRepository quizRepository = new QuizRepository(QuizsFilePath);
            bool loggedIn = false;
            Byte answer = 0;
            bool isInputValid = false;

            userRepository.ShowTop10();

            while (true)
            {

                Console.WriteLine("Which operation you want to complete?");
                Console.WriteLine("1) Register");
                Console.WriteLine("2) Login");
                Console.WriteLine("3) Exit");

                do
                {
                    try
                    {
                        Console.Write("Enter number: ");
                        answer = Byte.Parse(Console.ReadLine());
                        isInputValid = true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Invalid input. Please enter correct number");

                    }
                } while (!isInputValid);



                int UserId = 0;

                if (answer == 1)
                {
                    string username = GetInput("Enter your username: ");



                    bool usernameRegistered = false;
                    while (!usernameRegistered)
                    {
                        try
                        {

                            userRepository.IsUsernameAvailable(username);

                            User userToRegister = new User
                            {
                                Username = username
                            };

                            userRepository.Register(userToRegister);
                            Console.WriteLine("Registration successful!");
                            usernameRegistered = true;
                        }
                        catch (Exception ex)
                        {

                            Console.Write(ex.Message);
                            username = Console.ReadLine()?.Trim();
                        }
                    }

                }




                else if (answer == 2)
                {

                    string LoginUsername = "";
                    while (string.IsNullOrEmpty(LoginUsername))
                    {
                        Console.Write("Enter your username: ");
                        LoginUsername = Console.ReadLine()?.Trim();

                        if (string.IsNullOrEmpty(LoginUsername))
                        {
                            Console.WriteLine("Username must not be empty. Please try again.");
                        }
                    }



                    bool LoggedIn = false;
                    User LoggedInUser = new User();
                    while (!LoggedIn)
                    {
                        try
                        {
                            userRepository.IsUserRegistered(LoginUsername);

                            LoggedInUser.Username = LoginUsername;

                            Console.WriteLine("Logged in successfully");
                            LoggedIn = true;


                        }
                        catch (Exception ex)
                        {
                            Console.Write(ex.Message);
                            LoginUsername = Console.ReadLine()?.Trim();

                        }
                    }






                    UserId = userRepository.Login(LoggedInUser);
                    loggedIn = true;

                    while (loggedIn)
                    {
                        Console.WriteLine($"Your id is:{UserId}");
                        Console.WriteLine("You can make operations: ");
                        Console.WriteLine("1) Create quiz");
                        Console.WriteLine("2) Update quiz");
                        Console.WriteLine("3) Delete quiz");
                        Console.WriteLine("4) Solve quizes");
                        Console.WriteLine("5) See your quizes");
                        Console.WriteLine("6) Log out");
                        Console.Write("Enter operation: ");
                        byte userChoice = Byte.Parse(Console.ReadLine());
                        Console.WriteLine();
                        //Create quiz
                        if (userChoice == 1)
                        {
                            List<Question> Questions = new List<Question>();

                            string QuizTitle = GetInput("Enter quiz title: ");


                            for (int i = 0; i < 5; i++)
                            {
                                string Question = GetInput("Enter the question: ");
                                string FirstChoice = GetInput("Enter choice A: ");
                                string SecondChoice = GetInput("Enter choice B: ");
                                string ThirdChoice = GetInput("Enter choice C: ");
                                string FourthChoice = GetInput("Enter choice D: ");
                                string CorrectAnswer = GetCorrectOption("Enter the correct option for the question (a, b, c, d): ");




                                Question question = new Question()
                                {
                                    QuestionText = Question,
                                    A = FirstChoice,
                                    B = SecondChoice,
                                    C = ThirdChoice,
                                    D = FourthChoice,
                                    CorrectVersion = Char.Parse(CorrectAnswer)
                                };
                                Questions.Add(question);

                                Console.WriteLine();
                            }


                            Quiz quiz = new Quiz()
                            {
                                OwnerId = UserId,
                                Title = QuizTitle,
                                Questions = Questions
                            };

                            quizRepository.CreateQuiz(quiz);
                        }



                        //Update quiz
                        else if (userChoice == 2)
                        {
                            List<Quiz> currentUserQuizes = quizRepository.GetUsersQuizes(UserId);
                            if (currentUserQuizes.Count == 0)
                            {
                                Console.WriteLine("Your quiz list is empty");
                            }

                            else
                            {

                                int cnt = 1;
                                Console.WriteLine("Here are list of your quizes");                               
                                foreach (var item in currentUserQuizes)
                                {
                                    Console.WriteLine($"id: {item.QuizId}, title:{item.Title}");
                                }


                                int QuizToUpdate = default;
                                

                                bool CanUpdate = false;
                                while (CanUpdate == false)
                                {
                                    QuizToUpdate = int.Parse(GetInput("Enter quiz id to update: "));
                                    if (quizRepository.CanUpdateQuiz(UserId, QuizToUpdate) == true)
                                    {
                                        CanUpdate = true;
                                        break;
                                    }

                                    else
                                    {
                                        Console.WriteLine("You can not update other users quiz");
                                    }
                                }

                                Console.WriteLine();
                                




                                Quiz CurrentQuiz = quizRepository.GetQuizById(QuizToUpdate);
                                List<Question> CurrentQuestions = quizRepository.GetQuestionsOfQuiz(QuizToUpdate);


                                List<Question> UpdatedQuestions = new List<Question>();


                                Console.Write($"This is title of quiz: {CurrentQuiz.Title} \n");
                                Console.Write("Do you want to change title? type 1(for yes) or 0(for no): ");
                                int AnswerToUpdate = int.Parse(GetInput("Do you want to change the title? type 1(for yes) or 0(for no): "));


                                if (AnswerToUpdate == 1)
                                {
                                    Console.Write("Enter new title: ");
                                    string NewTitle = GetInput("Enter new title: ");
                                    CurrentQuiz.Title = NewTitle;
                                }



                                foreach (var item in CurrentQuestions)
                                {
                                    Console.WriteLine($"This is content of question #{cnt}");
                                    Console.WriteLine($"question:{item.QuestionText}");
                                    Console.WriteLine($"answers: {item.A} {item.B} {item.C} {item.D}");


                                    int AnswerToChangeQuestion = int.Parse(GetInput("Do you want to change this question? type 1(for yes) or 0(for no): "));
                                    if (AnswerToChangeQuestion == 0)
                                    {
                                        UpdatedQuestions.Add(item);
                                    }

                                    else
                                    {

                                        string Question = GetInput("Enter updated question: ");
                                        string FirstChoice = GetInput("Enter choice A: ");
                                        string SecondChoice = GetInput("Enter choice B: ");
                                        string ThirdChoice = GetInput("Enter choice C: ");
                                        string FourthChoice = GetInput("Enter choice D: ");
                                        string CorrectAnswer = GetInput("Enter correct option: ");






                                        Question question = new Question()
                                        {
                                            QuestionText = Question,
                                            A = FirstChoice,
                                            B = SecondChoice,
                                            C = ThirdChoice,
                                            D = FourthChoice,
                                            CorrectVersion = Char.Parse(CorrectAnswer)
                                        };
                                        UpdatedQuestions.Add(question);

                                        Console.WriteLine();
                                        cnt++;



                                    }


                                }


                                CurrentQuiz.Questions = UpdatedQuestions;
                                quizRepository.UpdateQuiz(CurrentQuiz);

                            }




                        }


                        //Delete quiz
                        else if (userChoice == 3)
                        {
                            List<Quiz> CurrentUsersQuizes = quizRepository.GetUsersQuizes(UserId);
                            if (CurrentUsersQuizes.Count == 0)
                            {
                                Console.WriteLine("You do not have any quizes, so you can not delete!");

                            }


                            else
                            {
                                Console.WriteLine("Here are your quizes!");
                                foreach (var item in CurrentUsersQuizes)
                                {
                                    Console.WriteLine($"id:{item.QuizId}, title:{item.Title}");
                                }

                                int quizIdToDelete;

                                while (true)
                                {
                                    Console.Write("Enter ID to delete: ");


                                    quizIdToDelete = int.Parse(Console.ReadLine());

                                    try
                                    {
                                        quizRepository.DeleteQuiz(UserId, quizIdToDelete);
                                        Console.WriteLine("Quiz deleted successfully.");
                                        break;
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(ex.Message);
                                        Console.WriteLine("Enter a correct quiz ID.");
                                    }
                                }

                                CurrentUsersQuizes = quizRepository.GetUsersQuizes(UserId);
                                Console.WriteLine();
                                Console.WriteLine("Here is updated list of your quizes");

                                foreach (var item in CurrentUsersQuizes)
                                {
                                    Console.WriteLine($"id:{item.QuizId}, title:{item.Title}");
                                }

                            }


                        }

                        //Solve quizes
                        else if (userChoice == 4)
                        {
                            int points = 0;
                            Console.Write("You can choose any quiz you want and solve it.\n");
                            var OthersQuizes = quizRepository.GetOtherUsersQuizes(UserId);

                            foreach (var item in OthersQuizes)
                            {
                                Console.WriteLine($" Quiz ID:{item.QuizId}, Quiz title:{item.Title}");
                            }

                            bool CanSolve=false;
                            int QuizIdToSolve=-1;

                            while(CanSolve == false)
                            {
                                QuizIdToSolve = int.Parse(GetInput("Enter quiz id to solve: "));
                                if(quizRepository.CanSolveQuiz(UserId,QuizIdToSolve) == true) 
                                {
                                    CanSolve = true;
                                    break;
                                }

                                else
                                {
                                    Console.WriteLine("You can not solve your own quiz");
                                }
                            }

                            Console.WriteLine();
                            


                            byte questionCounter = 1;

                            Quiz CurrentQuiz = quizRepository.GetQuizById(QuizIdToSolve);
                            List<Question> CurrentQuesions = quizRepository.GetQuestionsOfQuiz(QuizIdToSolve);



                            DateTime startTime = DateTime.Now; 
                            TimeSpan maxDuration = TimeSpan.FromMinutes(2);
                            bool completed = true;



                            Console.WriteLine("You have 2 minutes to solve this quiz! Good Luck!");
                            foreach (var item in CurrentQuesions)
                            {

                                if (DateTime.Now - startTime > maxDuration)
                                {
                                    points = 0;
                                    completed = false;
                                    Console.WriteLine($"\nTime's up! You failed to solve the quiz. Your result: {points} points");
                                    break; 
                                }


                                TimeSpan elapsedTime = DateTime.Now - startTime;                                
                                TimeSpan remainingTime = maxDuration - elapsedTime;

                                
                                Console.WriteLine($"Time remaining: {remainingTime.Minutes}:{remainingTime.Seconds}");
                                Console.WriteLine($"Question #{questionCounter}) {item.QuestionText}");
                                Console.WriteLine($"A {item.A}");
                                Console.WriteLine($"B {item.B}");
                                Console.WriteLine($"C {item.C}");
                                Console.WriteLine($"D {item.D}");
                                questionCounter++;


                                string userInput = GetInput("Enter your choice: ");



                                if (userInput.ToLower() == item.CorrectVersion.ToString().ToLower())
                                {
                                    points += 20;
                                }
                                else
                                {
                                    points -= 20;
                                }

                                Console.WriteLine();
                            }



                            if (completed)
                            {
                                Console.WriteLine($"Game Over! You collected {points} points!!!");
                            }



                            if (completed && points > userRepository.GetHighScore(UserId))
                            {
                                userRepository.UpdateHighScore(UserId, points);
                            }
                        }



                        //see own quizes
                        else if (userChoice == 5)
                        {
                            var currentQuizes = quizRepository.GetUsersQuizes(UserId);
                            if (currentQuizes.Count == 0)
                            {
                                Console.WriteLine("You have not created any quizes");
                            }
                            foreach (var item in currentQuizes)
                            {
                                Console.WriteLine(item.Title);

                            }
                        }



                        //break condition
                        else if(userChoice == 6) 
                        {
                            break;
                        }

                    }


                }


                else
                {
                    break;
                }
            }
        }

        public static string GetInput(string prompt)
        {
            string input;
            Console.Write(prompt);
            input = Console.ReadLine().Trim();

            while (string.IsNullOrEmpty(input))
            {
                Console.Write("Input cannot be empty. Please enter again: ");
                input = Console.ReadLine().Trim();
            }

            return input;
        }


        public static string GetCorrectOption(string prompt)
        {
            string[] validOptions = { "a", "b", "c", "d", "A", "B", "C", "D" };
            string input = GetInput(prompt); 

            while (!Array.Exists(validOptions, option => option.Equals(input, StringComparison.OrdinalIgnoreCase)))
            {
                Console.WriteLine("Invalid option. The correct option must be one of: a, b, c, or d.");
                input = GetInput(prompt); 
            }

            return input.ToLower(); 
        }

    } 
}
