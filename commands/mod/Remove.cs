using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Spectre.Console.Cli;
using Spectre.Console;
using Stargazer.Dbus;

namespace Stargazer.Commands {
    public class Remove : AsyncCommand<RemoveSettings> {
        public override async Task<int> ExecuteAsync(CommandContext context, RemoveSettings settings) {
            await DbusClient.ConnectAsync();

            Mod[] mods = await DbusClient.ListModsAsync(settings.profile);

            if (settings.name is not null) {
                string query = settings.name;
                if (settings.name.Contains("-")) {
                    query = string.Join(" ", query.Split("-"));
                }

                bool found = false;
                string id = string.Empty;
                foreach (Mod mod in mods) {
                    if (mod.Title.ToLower().Equals(query)) {
                        found = true;
                        id = mod.VersionId;
                    }
                }

                if (!found) {
                    throw new Exception($"Mod {query} doesn't exist on profile {query}!");
                }

                if (await DbusClient.RemoveModAsync(settings.profile, id, settings.force)) {
                    AnsiConsole.WriteLine("Success!");
                    return 0;
                } else {
                    AnsiConsole.WriteLine($"{query} is a dependency!  Use -f to force removal");
                    return -1;
                }
            } else {
                bool confirm = AnsiConsole.Confirm($"This will clear all mods from profile {settings.profile}, are you sure?", false);
            
                if (!confirm) {
                    AnsiConsole.WriteLine("Exiting...");
                    return 0;
                }

                foreach (Mod mod in mods) {
                    AnsiConsole.WriteLine("removing {0}...", mod.Title);
                    if (await DbusClient.RemoveModAsync(settings.profile, mod.VersionId, true)) {
                        AnsiConsole.WriteLine("Success!");
                    } else {
                        AnsiConsole.WriteLine("Error removing, continuing...");
                    }
                }

                return 0;
            }
        }
    }
    public class RemoveSettings : CommandSettings {
        [CommandArgument(0, "<PROFILE>")]
        public string profile { get; set; }
        [CommandArgument(1, "[MOD NAME]")]
        public string name { get; set; }
        [CommandOption("-f|--force")]
        public bool force { get; set; }
    }
}