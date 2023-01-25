using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Spectre.Console.Cli;
using Spectre.Console;
using Stargazer.Dbus;

namespace Stargazer.Commands {
    public class ListMods : AsyncCommand<ListModsSettings> {
        public override async Task<int> ExecuteAsync(CommandContext context, ListModsSettings settings) {
            await DbusClient.ConnectAsync();

            if (!(await DbusClient.CheckProfileExistsAsync(settings.profile))) {
                throw new Exception($"Profile {settings.profile} doesn't exist!");
            }

            ProfileInfo info = await DbusClient.GetProfileInfoAsync(settings.profile);
            Mod[] mods = await DbusClient.ListModsAsync(settings.profile);

            AnsiConsole.WriteLine("Profile {0} -- {1} {2}\n", info.Name, info.Loader, info.MinecraftVersion);

            foreach (Mod mod in mods) {
                AnsiConsole.WriteLine("{0} / {1}", mod.Title, mod.ModVersion);
                if (mod.DependencyVersions.Count() > 0) {
                    AnsiConsole.WriteLine(" Depends on:");

                    foreach (string dependency in mod.DependencyVersions) {
                        IEnumerable<Mod> depMods = mods.Where<Mod>(mod => mod.VersionId.Equals(dependency));

                        foreach (Mod dep in depMods) {
                            AnsiConsole.WriteLine(" - {0} / {1}", dep.Title, dep.ModVersion);
                        }
                    }
                }

                AnsiConsole.WriteLine("---");
            }

            return 0;
        }
    }
    public class ListModsSettings : CommandSettings {
        [CommandArgument(0, "<PROFILE>")]
        public string profile { get; set; }
    }
}