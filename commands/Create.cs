using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Spectre.Console.Cli;
using Spectre.Console;
using Stargazer.Dbus;

namespace Stargazer.Commands {
    public class Create : AsyncCommand<CreateSettings> {

        public override async Task<int> ExecuteAsync([NotNull] CommandContext context, [NotNull] CreateSettings settings)
        {
            ModLoader loader;

            if (!Enum.TryParse<ModLoader>(settings.ModLoader, false, out loader)) {
                throw new Exception($"{settings.ModLoader} is not a valid mod loader... ?");
            }

            await DbusClient.ConnectAsync();

            bool result = await DbusClient.CreateProfileAsync(settings.Name, settings.MinecraftVersion, loader);

            if (result) {
                AnsiConsole.MarkupLine("Created profile [underline]{0}[/]", settings.Name);
            } else {
                throw new Exception($"Profile {settings.Name} already exists!");
            }

            return 0;
        }
    }
    public class CreateSettings : CommandSettings {
        [CommandArgument(0, "<name>")]
        public string Name { get; set; }

        [CommandArgument(1, "<version>")]
        public string MinecraftVersion { get; set; }

        [CommandArgument(2, "<loader>")]
        public string ModLoader { get; set; }
    }
}