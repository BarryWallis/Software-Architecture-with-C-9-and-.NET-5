using System;
using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace performance_issues
{
    /// <summary>
    /// This simple program will give you good insights for writing good performance and 
    /// reusable code - Gabriel Lara Baptista - Software Architecture with C# 9 and .NET 5.0
    /// </summary>
    internal class Program
    {
        private enum MenuOption
        {
            Exit = '0',
            StringConcatenation = '1',
            Exceptions = '2'

        }

        private static void Main()
        {
            MenuOption menuOption;
            do
            {
                menuOption = DetectMenuOption();
                switch (menuOption)
                {
                    case MenuOption.Exit:
                        Console.WriteLine("You just pressed exit. See you around!");
                        break;
                    case MenuOption.StringConcatenation:
                        ShowStringConcatenationSample();
                        break;
                    case MenuOption.Exceptions:
                        ShowExceptionsSample();
                        break;
                    default:
                        Console.WriteLine("This one is not ready for you! :( Sorry!");
                        break;

                }
                Console.WriteLine("Press any key to continue...");
                _ = Console.ReadKey();
            }
            while (menuOption != MenuOption.Exit);

        }
        #region Exceptions
        private static string ParseIntWithTryParse()
        {
            string result = string.Empty;
            result = int.TryParse(result, out int value) ? value.ToString(CultureInfo.CurrentCulture) : "There is no int value";

            return $"Final result: {result}";
        }

        private static string ParseIntWithException()
        {
            string result = string.Empty;
            try
            {
                result = Convert.ToInt32(result, CultureInfo.CurrentCulture).ToString(CultureInfo.CurrentCulture);
            }
            catch (Exception)
            {
                result = "There is no int value";
            }
            return $"Final result: {result}";
        }
        private static void ShowExceptionsSample()
        {
            Console.WriteLine("Always remember! Exceptions take too much time to handle!");
            string? resultTest01 = RunTest(ParseIntWithException);
            string? resultTest02 = RunTest(ParseIntWithTryParse);
            if (resultTest01 == resultTest02)
            {
                Console.WriteLine("The results are the same! You can compare the numbers.");
            }
            else
            {
                Console.WriteLine("There is a difference in the results! Do not compare the numbers.");
            }
        }
        #endregion
        #region String Concatenation
        private static string ExecuteStringConcatenationWithNoComponent()
        {
            Console.WriteLine($"Concatenating 100000 strings....");
            string data = string.Empty;
            for (int i = 0; i < 100000; i++)
            {
                data += i.ToString(CultureInfo.CurrentCulture);
            }

            return $"Final result: {data}";
        }
        private static string ExecuteStringConcatenationWithStringBuilder()
        {
            Console.WriteLine($"Concatenating 100000 strings....");
            StringBuilder? stringBuilder = new();
            for (int i = 0; i < 100000; i++)
            {
                _ = stringBuilder.Append(i);
            }

            return $"Final result: {stringBuilder}";
        }
        private static void ShowStringConcatenationSample()
        {
            Console.WriteLine("This is a classic one! But you should remember about this, anyway!");
            string? resultTest01 = RunTest(ExecuteStringConcatenationWithNoComponent);
            string? resultTest02 = RunTest(ExecuteStringConcatenationWithStringBuilder);
            if (resultTest01 == resultTest02)
            {
                Console.WriteLine("The results are the same! You can compare the numbers.");
            }
            else
            {
                Console.WriteLine("There is a difference in the results! Do not compare the numbers.");
            }
        }


        #endregion

        #region Generic Methods

        private static string RunTest(Func<string> method)
        {
            string result;
            Console.WriteLine($"Start running method: {method.Method.Name}");
            Stopwatch? watch = new();
            watch.Reset();
            watch.Start();
            try
            {
                result = method.Invoke();
            }
            finally
            {
                watch.Stop();
            }
            Console.WriteLine($"The method {method.Method.Name} took {watch.Elapsed.TotalSeconds} second(s) ({watch.Elapsed.TotalMilliseconds} ms)");
            return result;
        }

        private static MenuOption DetectMenuOption()
        {
            Console.Clear();
            Console.WriteLine("Hello Readers!");
            Console.WriteLine("Here you have some samples regarding to performance issues.");
            Console.WriteLine("Please select the option you want to check:");
            Console.WriteLine("0 - Bye bye!");
            Console.WriteLine("1 - String Concatenation");
            Console.WriteLine("2 - Exceptions");
            ConsoleKeyInfo option = Console.ReadKey(true);
            return (MenuOption)option.Key;
        }
        #endregion
    }
}
