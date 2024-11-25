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
            int answer=-1;

            userRepository.ShowTop10();

            Console.WriteLine("Which operation you want to complete?");
            Console.WriteLine("1) Register");
            Console.WriteLine("2) Login");
            Console.WriteLine("3) Exit");
            Console.Write("Enter number: ");
            try
            {
                answer=Byte.Parse(Console.ReadLine());
                
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
            int UserId = 0;

            if (answer == 1)
            {
                string username = "";

                while (string.IsNullOrEmpty(username))
                {
                    Console.Write("Enter your username: ");
                    username = Console.ReadLine()?.Trim();

                    if (string.IsNullOrEmpty(username))
                    {
                        Console.WriteLine("Username must not be empty. Please try again.");
                    }
                }

                while (userRepository.IsUsernameAvailable(username) == 0)
                {

                    Console.WriteLine("Sorry! This username is busy, try another one. Enter new username:");
                    username= Console.ReadLine()?.Trim();
  
                }

                User userToRegister = new User
                {
                    Username = username
                };

                userRepository.Register(userToRegister);
                Console.WriteLine("Registration successful!");
                    
            }




            else if (answer == 2) 
            {
                Console.Write("Enter your username: ");
                string username = Console.ReadLine().Trim();
                User LoggedInUser=new User() { Username = username };
                UserId=userRepository.Login(LoggedInUser);
                Console.WriteLine($"Your id is:{UserId}");
                Console.WriteLine("You can make operations: ");
                Console.WriteLine("1) Create quiz");
                Console.WriteLine("2) Update quiz");
                Console.WriteLine("3) Delete quiz");
                Console.WriteLine("4) Solve quizes");
                Console.WriteLine("5) See your quizes");
                Console.Write("Enter operation: ");
                byte userChoice=Byte.Parse(Console.ReadLine());
                Console.WriteLine();
                //Create quiz
                if (userChoice == 1) 
                {
                    List<Question> Questions = new List<Question>();
                    Console.Write("Enter quiz title: ");
                    string QuizTitle = Console.ReadLine().Trim();



                    for(int i = 0; i < 5; i++)
                    {
                        Console.Write($"Enter question #{i+1}: ");
                        string Question = Console.ReadLine();

                        Console.Write("Enter choice A: ");
                        string FirstChoice= Console.ReadLine();

                        Console.Write("Enter choice B: ");
                        string SecondChoice = Console.ReadLine();

                        Console.Write("Enter choice C: ");
                        string ThirdChoice = Console.ReadLine();

                        Console.Write("Enter choice D: ");
                        string FourthChoice = Console.ReadLine();

                        Console.Write("Enter correct answer: ");
                        string CorrectAnswer = Console.ReadLine();


                        Question question = new Question()
                        {
                            QuestionText = Question,
                            A = FirstChoice,
                            B = SecondChoice,
                            C = ThirdChoice,
                            D = FourthChoice,
                            CorrectVersion=Char.Parse(CorrectAnswer)

                        };
                        Questions.Add(question);

                        Console.WriteLine();
                    }

                    Quiz quiz= new Quiz()
                    {
                        OwnerId=UserId,
                        Title=QuizTitle,
                        Questions = Questions

                    };



                   
                    quizRepository.CreateQuiz(quiz);



                }


                //Update quiz
                else if(userChoice == 2)
                {
                    Console.WriteLine("Here are list of your quizes");
                    List<Quiz> currentUserQuizes=quizRepository.GetUsersQuizes(UserId);
                    //List<Question> UpdatedQuestions = new List<Question>();
                    foreach (var item in currentUserQuizes)
                    {
                        Console.WriteLine($"id: {item.QuizId}, title:{item.Title}");
                    }

                    Console.Write("Enter id of the quiz you want to update:");
                    Console.WriteLine();
                    int QuizToUpdate=int.Parse(Console.ReadLine());

                    Quiz CurrentQuiz=quizRepository.GetQuizById(QuizToUpdate);
                    List<Question> CurrentQuestions=quizRepository.GetQuestionsOfQuiz(QuizToUpdate);

                    //aqedan viwyeb gadaketebas
                    Quiz UpdatedQuiz = new Quiz();
                    List<Question> UpdatedQuestions = quizRepository.GetQuestionsOfQuiz(QuizToUpdate);
                    UpdatedQuiz.OwnerId= CurrentQuiz.OwnerId;
                    UpdatedQuiz.QuizId= CurrentQuiz.QuizId;

                    Console.Write($"This is title of quiz: {CurrentQuiz.Title} \n");
                    Console.Write("Do you want to change title? type 1(for yes) or 0(for no): ");
                    int AnswerToUpdate = int.Parse(Console.ReadLine());


                    if(AnswerToUpdate == 1)
                    {
                        Console.Write("Enter new title: ");
                        string NewTitle=Console.ReadLine();
                        CurrentQuiz.Title = NewTitle;
                    }


                    
                    foreach(var item in CurrentQuestions)
                    {
                        Console.WriteLine("Do you want to change this question?");
                        Console.WriteLine(item.QuestionText);
                        Console.WriteLine($"answers: {item.A} {item.B} {item.C} {item.D}");
                        int AnswerToChangeQuestion=int.Parse(Console.ReadLine());
                        if (AnswerToChangeQuestion == 0)
                        {
                            UpdatedQuestions.Add(item);
                        }

                        else
                        {

                            Console.Write($"Enter updated question: ");
                            string Question = Console.ReadLine();

                            Console.Write("Enter choice A: ");
                            string FirstChoice = Console.ReadLine();

                            Console.Write("Enter choice B: ");
                            string SecondChoice = Console.ReadLine();

                            Console.Write("Enter choice C: ");
                            string ThirdChoice = Console.ReadLine();

                            Console.Write("Enter choice D: ");
                            string FourthChoice = Console.ReadLine();

                            Console.Write("Enter correct answer: ");
                            string CorrectAnswer = Console.ReadLine();


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
                            


                        }


                    }
                    CurrentQuiz.OwnerId = UserId;
                    
                    CurrentQuiz.Questions = UpdatedQuestions;
                    quizRepository.UpdateQuiz(CurrentQuiz);

 
                }

                //Delete quiz
                else if(userChoice == 3)
                {
                    Console.WriteLine("Here are your quizes!");
                    List<Quiz> CurrentUsersQuizes = quizRepository.GetUsersQuizes(UserId);
                    foreach (var item in CurrentUsersQuizes)
                    {
                        Console.WriteLine($"id:{item.QuizId}, title:{item.Title}");                        
                    }

                    Console.Write("Enter ID to delete: ");
                    int quizIdToDelete=int.Parse(Console.ReadLine());

                    quizRepository.DeleteQuiz(quizIdToDelete);
                    CurrentUsersQuizes = quizRepository.GetUsersQuizes(UserId);
                    Console.WriteLine();
                    Console.WriteLine("Here is updated list of your quizes");

                    foreach (var item in CurrentUsersQuizes)
                    {
                        Console.WriteLine($"id:{item.QuizId}, title:{item.Title}");
                    }

                }

                //Solve quizes
                else if (userChoice == 4)
                {
                    int points = 0;
                    Console.Write("You can choose any quiz you want and solve it.");
                    Console.WriteLine();
                    var OthersQuizes = quizRepository.GetOtherUsersQuizes(UserId);
                    foreach (var item in OthersQuizes)
                    {
                        Console.WriteLine($"Owner: {item.OwnerId}, Quiz title:{item.Title}, Quiz ID:{item.QuizId}");
                    }
                    Console.Write("Enter quiz id to solve: ");
                    int QuizIdToSolve = int.Parse(Console.ReadLine());

                    Quiz CurrentQuiz = quizRepository.GetQuizById(QuizIdToSolve);
                    List<Question> CurrentQuesions = quizRepository.GetQuestionsOfQuiz(QuizIdToSolve);

                            
             

                    foreach (var item in CurrentQuesions)
                    {


                        Console.WriteLine(item.QuestionText);
                        Console.WriteLine($"A {item.A}");
                        Console.WriteLine($"B {item.B}");
                        Console.WriteLine($"C {item.C}");
                        Console.WriteLine($"D {item.D}");

                        Console.Write("Enter answer: ");
                        char input = char.Parse(Console.ReadLine());



                        if (input.ToString().ToLower() == item.CorrectVersion.ToString().ToLower())
                        {
                            points += 20;
                        }
                        else
                        {
                            points -= 20;
                        }
                        Console.WriteLine();
                    }


                    Console.WriteLine($"You collected {points} points");

                    if (points > userRepository.GetHighScore(UserId))
                    {
                        userRepository.UpdateHighScore(UserId, points);
                    }
                }



                //see own quizes
                else if(userChoice == 5)
                {
                    var currentQuizes=quizRepository.GetUsersQuizes(UserId);
                    foreach (var item in currentQuizes)
                    {
                        Console.WriteLine(item.Title);
                        
                    }
                }

            }

        }
    }
}
