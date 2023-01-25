using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Spectre.Console.Cli;
using Spectre.Console;
using Stargazer.Dbus;

namespace Stargazer.Commands {
    public class Verify : AsyncCommand<VerifySettings> {
        public override async Task<int> ExecuteAsync(CommandContext context, VerifySettings settings) {
            await DbusClient.ConnectAsync();

            ValidityReport validity = await DbusClient.VerifyAsync(settings.profile);

            AnsiConsole.Write("Verifying {0}...", settings.profile);

            if (validity.repaired) {
                AnsiConsole.WriteLine("\rVerifying {0} (Passed)", settings.profile);
            } else {
                AnsiConsole.WriteLine("\rVerifying {0} (Failed)", settings.profile);
            }

            Mod[] mods = await DbusClient.ListModsAsync(settings.profile);

            if (validity.missingDependencies.Length > 0) {
                AnsiConsole.WriteLine("Added missing dependencies:");
                foreach (string dep in validity.missingDependencies) {
                    Mod addedDep = mods.Where<Mod>(mod => mod.VersionId.Equals(dep)).First();

                    AnsiConsole.WriteLine(" {0} / {1}", addedDep.Title, addedDep.ModVersion);
                }
            }

            if (validity.incompatible.Length > 0) {
                AnsiConsole.WriteLine("Incompatible mods that were removed:");
                foreach (Mod incompatible in validity.incompatible) {
                    AnsiConsole.WriteLine(" {0} / {1}", incompatible.Title, incompatible.ModVersion);
                }
            }

            return 0;
        }
    }
    public class VerifySettings : CommandSettings {
        [CommandArgument(0, "<PROFILE>")]
        public string profile { get; set; }
    }
}