using System.Threading.Tasks;
using Spectre.Console.Cli;
using Stargazer.Commands;
using Stargazer.Dbus;

namespace Stargazer {
    public class Program {
        public static int Main(string[] args) {
            CommandApp app = new CommandApp();

            app.Configure(config => {
                config.AddBranch("profile", branch => {
                    branch.AddCommand<Create>("create")
                    .WithDescription("Creates a new profile.")
                    .WithExample(new string[] { "profile", "create", "uwu", "1.19.2", "fabric" });

                    branch.AddCommand<Delete>("delete")
                    .WithDescription("Deletes an existing profile.");

                    branch.AddCommand<List>("list")
                    .WithDescription("Lists all loaded profiles.");
                });

                config.AddCommand<Sync>("sync")
                .WithDescription("Add mod to profile");
#if DEBUG
                config.PropagateExceptions();
#endif
            });

            return app.Run(args);
        }
    }
}