using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Spectre.Console.Cli;
using Spectre.Console;
using Stargazer.Dbus;

namespace Stargazer.Commands {
    public class Remove : AsyncCommand<RemoveSettings> {
        public override async Task<int> ExecuteAsync(CommandContext context, RemoveSettings settings) {
            await DbusClient.ConnectAsync();

            string query = settings.name;
            if (settings.name.Contains("-")) {
                query = string.Join(" ", query.Split("-"));
            }

            Mod[] mods = await DbusClient.ListModsAsync(settings.profile);

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