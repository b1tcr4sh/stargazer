using System.Diagnostics.CodeAnalysis;
using Stargazer.Dbus;
using Stargazer.Repos;
using Stargazer.Repos.Models;
using Spectre.Console.Cli;
using Spectre.Console;

namespace Stargazer.Commands {
    public class Sync : AsyncCommand<CheckoutSettings> {
        public override async Task<int> ExecuteAsync([NotNull] CommandContext context, [NotNull] CheckoutSettings settings) {
            await DbusClient.ConnectAsync();

            if (!(await DbusClient.CheckProfileExistsAsync(settings.ProfileName))) {
                throw new Exception($"Profile {settings.ProfileName} doesn't exist!");
            }

            if (settings.Name is not null) {
                ModrinthAPI api = new ModrinthAPI();
                SearchModel search = await api.SearchAsync(settings.Name);

                if (search.hits.Length <= 0) {
                    throw new Exception("No results... ?");
                }

                string project = ChooseProject(search);

                AnsiConsole.WriteLine("Adding mod to profile...");
                Mod[] installed = await DbusClient.AddModAsync(settings.ProfileName, project, Repo.modrinth, settings.IgnoreDeps);
            }

        
            if (!settings.Dry) {
                AnsiConsole.WriteLine("Syncing Profile...");
                await DbusClient.SyncProfileAsync(settings.ProfileName);
            }

            AnsiConsole.WriteLine("Complete!");
            return 0;
        }

        private string ChooseProject(SearchModel search) {
            AnsiConsole.WriteLine("~Displaying {0} / {1} results~", search.hits.Length, search.total_hits);

            for (int option = 0; option < search.hits.Length; option++) {
                AnsiConsole.WriteLine("{0} - {1} / {2} -- {3}\n   {4}", option, search.hits[option].title, search.hits[option].latest_version, search.hits[option].author, search.hits[option].description);
            }

            int selection = AnsiConsole.Ask<int>("Mod Selection > ");

            return search.hits[selection].project_id;
        }
    }

    public class CheckoutSettings : CommandSettings {
        [CommandArgument(0, "<PROFILE>")]
        public string ProfileName { get; set; }
        [CommandArgument(1, "[MOD]")]
        public string Name { get; set; }
        [CommandOption("-d|--dry-run")]
        public bool Dry { get; set; }
        [CommandOption("-i|--ignore-deps")]
        public bool IgnoreDeps { get; set; }
    }
}