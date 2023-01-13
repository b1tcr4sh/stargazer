using System.Diagnostics.CodeAnalysis;
using Spectre.Console.Cli;
using Stargazer.Dbus;

namespace Stargazer.Commands {
    public class Delete : AsyncCommand<DeleteSettings> {
        public override async Task<int> ExecuteAsync([NotNull] CommandContext context, [NotNull] DeleteSettings settings) {
            
                await DbusClient.DeleteProfileAsync(settings.Name);

            return 0;
        }
    }
    public class DeleteSettings : CommandSettings {
        [CommandArgument(0, "<name>")]
        public string Name { get; set; }
    }
}