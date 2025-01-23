using System;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArabizeCli.Arguments;
using ArabizeCli.Commands;

namespace ArabizeCli
{
    public partial class Program
    {
        public static readonly string version = "1.0.0";
        private static readonly Command listCommand = new("list", "List mappings for letters, diacritics, or macros.");
        private static readonly Command editCommand = new("edit", "Open the macros file using your default text editor.");

        [STAThread]
        static int Main(string[] args)
        {
            var ListArgument = new Argument<ListOption>("map", "Option for (l)etters, (d)iacritics, or (m)acros.");
            ListArgument.SetDefaultValue(ListOption.L);
            listCommand.AddArgument(ListArgument);
            listCommand.AddAlias("ls");
            listCommand.SetHandler(ListHandler.Handle, ListArgument);
            editCommand.SetHandler(EditHandler.Handle);
            var rootCommand = new RootCommand(
@$"The {Defaults.applicationName} is a tool to translate Arabic-transliterated letters into Arabic Unicode characters.
Pass transliterated letters such as 'ya%waw-seen%fa' to output 'يُوسُف'."
            )
            {
                listCommand,
                editCommand
            };
            var cli = new CommandLineBuilder(rootCommand)
                .AddMiddleware(InitializeHandler)
                .UseHelp()
                .AddMiddleware(VersionHandler)
                .UseParseErrorReporting()
                .UseExceptionHandler(ExceptionHandler)
                .AddMiddleware(Intercept)
                .Build();
            return cli.Invoke(args);
        }

        private static Task InitializeHandler(InvocationContext context, Func<InvocationContext, Task> next)
        {
            Console.InputEncoding = Console.OutputEncoding = Encoding.UTF8;
            return next(context);
        }

        private static void ExceptionHandler(Exception ex, InvocationContext context)
        {
            var s = ex.Message.ToString().Trim();
            s = s.EndsWith('.') ? s : s + '.';
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Error.WriteLine($"[ERROR] {s}".PadRight(Console.WindowWidth));
            Console.ResetColor();
#if DEBUG
            Console.WriteLine(ex.StackTrace);
#endif
            context.ExitCode = 1;
        }

        private static bool MatchesCommandName(Command command, string name) => command.Aliases.Contains(name) || listCommand.Name == name;
        private static Task Intercept(InvocationContext context, Func<InvocationContext, Task> next)
        {
            var tokens = context.ParseResult.Tokens;
            if (tokens.Count == 0) return Task.CompletedTask;
            var firstToken = tokens[0].ToString();
            if (MatchesCommandName(listCommand, firstToken) || MatchesCommandName(editCommand, firstToken))
            {
                return next(context);
            }
            var args = tokens.Select(token => token.ToString()).ToArray();
            try
            {
                ArabizeHandler.Handle(args);
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex, context);
            }
            return Task.CompletedTask;
        }

        private static Task VersionHandler(InvocationContext context, Func<InvocationContext, Task> next)
        {
            var tokens = context.ParseResult.Tokens;
            if (tokens.Count != 1) return next(context);
            var firstToken = tokens[0].ToString();
            if (firstToken == "-v" || firstToken == "--version" || firstToken == "version")
            {
                Console.WriteLine(version);
                return Task.CompletedTask;
            }
            return next(context);
        }
    }
}