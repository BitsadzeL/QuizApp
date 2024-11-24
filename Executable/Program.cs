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

            //User user1 = new User()
            //{
            //    Username = "testuser"
            //};

            //User user2 = new User()
            //{
            //    Username = "bitsadzeLuka"
            //};

            //User user3 = new User()
            //{
            //    Username = "leomessi"
            //};

            //User DoesNotExists = new User()
            //{
            //    Username = "chemiuseri"
            //};

            //userRepository.Register(newUser3);

            //int userid = default(int);
            //userid = userRepository.Login(DoesNotExists);
            //Console.WriteLine(userid);

            Console.WriteLine("Which operation you want to complete?");
            Console.WriteLine("1) Register");
            Console.WriteLine("2) Login");
            Console.WriteLine("3) Exit");
            Console.Write("Enter number: ");
            byte answer=Byte.Parse(Console.ReadLine());
            int UserId = 0;

            if (answer == 1)
            {
                Console.Write("Enter your username: ");
                string username = Console.ReadLine().Trim();
                if (userRepository.IsUsernameAvailable(username) == 1)
                {
                    User UserToRegister = new User()
                    {
                        Username = username
                    };

                    userRepository.Register(UserToRegister);

                }
                else
                {
                    Console.Write("Sorry! This username is busy, try another one: ");
                }
            }


            //tu davlogindi shemilia update, create, delete, solve
            else if(answer == 2) 
            {
                Console.Write("Enter your username: ");
                string username = Console.ReadLine().Trim();
                User LoggedInUser=new User() { Username = username };
                UserId=userRepository.Login(LoggedInUser);
                Console.WriteLine($"Your id is:{UserId}");
                Console.WriteLine("You can makek operations: ");
                Console.WriteLine("1) Create quiz");
                Console.WriteLine("2) Update quiz");
                Console.WriteLine("3) Delete quiz");
                Console.WriteLine("4) Solve quizes");
                byte userChoice=Byte.Parse(Console.ReadLine());
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

                else if(userChoice == 2)
                {
                    Console.WriteLine("Here are list of your quizes");
                    List<Quiz> currentUserQuizes=quizRepository.GetUsersQuizes(UserId);
                    List<Question> UpdatedQuestions = new List<Question>();
                    foreach (var item in currentUserQuizes)
                    {
                        Console.WriteLine($"id: {item.QuizId}, title:{item.Title}");
                    }
                    Console.Write("Enter id of the quiz you want to update:");
                    Console.WriteLine();
                    int QuizToUpdate=int.Parse(Console.ReadLine());

                    Quiz currentQuiz=quizRepository.GetQuizById(QuizToUpdate);
                    //Console.WriteLine(currentQuiz.Title);
                    Console.Write("Enter new title for this quiz: ");
                    string newTitle=Console.ReadLine();

                    for (int i = 0; i < 5; i++)
                    {
                        Console.Write($"Enter question #{i + 1}: ");
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


                    currentQuiz.OwnerId = UserId;
                    currentQuiz.Title = newTitle;
                    currentQuiz.Questions = UpdatedQuestions;
                    quizRepository.UpdateQuiz(currentQuiz);
                }

                
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


                else if(userChoice == 4)
                {
                    Console.Write("You can choose any quiz you want and solve it. enter quiz ID:");
                    Console.WriteLine();
                    var OthersQuizes=quizRepository.GetOtherUsersQuizes(UserId);
                    foreach (var item in OthersQuizes)                       
                    {
                        Console.WriteLine($"Owner: {item.OwnerId}, Quiz title:{item.Title}");
                        
                    }
                }



            }


        }
    }
}
