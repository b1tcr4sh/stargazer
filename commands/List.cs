using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Spectre.Console.Cli;
using Spectre.Console;
using Stargazer.Dbus;

using Tmds.DBus;


namespace Stargazer.Commands {
    public class List : AsyncCommand<ListSettings> {
        public override async Task<int> ExecuteAsync([NotNull] CommandContext context, [NotNull] ListSettings settings) {
            await DbusClient.ConnectAsync();

            ProfileInfo[] profiles = await DbusClient.ListProfilesAsync();

            if (profiles.Length <= 0) {
                AnsiConsole.WriteLine("There are no profiles to list...");
                return 0;
            }

            foreach (ProfileInfo profile in profiles) {
                AnsiConsole.MarkupLine("# {0} -- {1} {2}", profile.Name, profile.Loader, profile.MinecraftVersion);
            }

            return 0;
        }
    }
    public class ListSettings : CommandSettings {
        [CommandOption("-v|--version <version>")]
        public string Version { get; set; }

        [CommandOption("-l|--loader <loader>")]
        public string Loader { get; set; }
    }
}