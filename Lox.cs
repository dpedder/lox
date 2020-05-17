using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Lox.Lexical;

namespace Lox
{
    class Lox
    {
        private static bool hadError;

        private static void Main(string[] args)
        {
            // 1+ arg, too many args
            // 1 arg, clox <script>
            // 0 arg, repl
            if (args.Length > 1)
            {
                Console.WriteLine("Usage clox <script file>");
                Environment.Exit(64);
            }
            else if (args.Length == 1)
            {
                runFile(args[0]);
            }
            else
            {
                runPrompt();
            }
        }

        private static void runFile(string path)
        {
            byte[] source = File.ReadAllBytes(Path.GetFullPath(path));
            run(new string(Encoding.Default.GetString(source)));

            if (hadError) Environment.Exit(65);
        }

        private static void runPrompt()
        {
            Console.WriteLine("------");
            Console.WriteLine("clox interpreter");
            Console.WriteLine("by Dan Pedder (hello@danielpedder.com) 2020");
            Console.WriteLine("------");
            Console.WriteLine("");

            while (true)
            {
                Console.Write("> ");
                run(Console.ReadLine());
                hadError = false;
            }
        }

        private static void run(string source)
        {
            Scanner scanner = new Scanner(source);
            List<Token> tokens = scanner.scanTokens();

            foreach (var token in tokens)
                Console.WriteLine(token.ToString());
        }

        public static void Error(int line, string message)
        {
            report(line, "", message);
        }

        private static void report(int line, string where, string message)
        {
            Console.Error.WriteLine($"[Line {line}] Error{where}: {message}");
            hadError = true;
        }
    }
}