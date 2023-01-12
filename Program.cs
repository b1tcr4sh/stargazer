using Spectre.Console.Cli;
using Stargazer.Commands;

namespace Stargazer {
    public class Program {
        public static int Main(string[] args) {
            CommandApp app = new CommandApp();

            app.Configure(config => {
                config.AddCommand<Create>("create");
            });

            return app.Run(args);
        }
    }
}