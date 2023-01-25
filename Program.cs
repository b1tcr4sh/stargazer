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
                    branch.SetDescription("Create, delete, and list profiles");

                    branch.AddCommand<Create>("create")
                    .WithDescription("Creates a new profile.")
                    .WithExample(new string[] { "profile", "create", "uwu", "1.19.2", "fabric" });

                    branch.AddCommand<Delete>("delete")
                    .WithDescription("Deletes an existing profile.");

                    branch.AddCommand<List>("list")
                    .WithDescription("Lists all loaded profiles.");

                    branch.AddCommand<Verify>("verify")
                    .WithDescription("Verifies that the profile is useable");
                });
                config.AddBranch("mod", branch => {
                    branch.SetDescription("List and remove mods from a specific profile");

                    branch.AddCommand<Remove>("remove")
                    .WithDescription("Removes mods from a profile.");

                    branch.AddCommand<ListMods>("list")
                    .WithDescription("Lists all mods of a profile");
                });

                config.AddCommand<Sync>("sync")
                .WithDescription("Sync profiles with remote repositories")
                .WithExample(new string[] { "sync", "uwu", "sodium", "--", "Adds Sodium to profile uwu" });
#if DEBUG
                config.PropagateExceptions();
#endif
            });

            return app.Run(args);
        }
    }
}