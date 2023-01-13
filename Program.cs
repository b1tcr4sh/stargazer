using Spectre.Console.Cli;
using Stargazer.Commands;
using Stargazer.Dbus;

namespace Stargazer {
    public class Program {
        public static async Task<int> Main(string[] args) {
            CommandApp app = new CommandApp();

            app.Configure(config => {
                config.AddBranch("profile", branch => {
                    branch.AddCommand<Create>("create")
                    .WithDescription("Creates a new profile.")
                    .WithExample(new string[] { "profile", "create", "uwu", "1.19.2", "fabric" });

                    branch.AddCommand<Delete>("delete")
                    .WithDescription("Deletes an existing profile.");
                });

#if DEBUG
                config.PropagateExceptions();
#endif
            });

            await DbusClient.ConnectAsync();

            return app.Run(args);
        }
    }
}