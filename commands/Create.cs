using System.Diagnostics.CodeAnalysis;
using Spectre.Console.Cli;

namespace Stargazer.Commands {
    public class Create : Command<CreateSettings> {

        public override int Execute([NotNull] CommandContext context, [NotNull] CreateSettings settings)
        {
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