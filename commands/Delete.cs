using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Spectre.Console.Cli;
using Spectre.Console;
using Stargazer.Dbus;

namespace Stargazer.Commands {
    public class Delete : AsyncCommand<DeleteSettings> {
        public override async Task<int> ExecuteAsync([NotNull] CommandContext context, [NotNull] DeleteSettings settings) {
            await DbusClient.ConnectAsync();

            await DbusClient.DeleteProfileAsync(settings.Name);
            AnsiConsole.WriteLine("Deleted profile {0}", settings.Name);

            return 0;
        }
    }
    public class DeleteSettings : CommandSettings {
        [CommandArgument(0, "<name>")]
        public string Name { get; set; }
    }
}