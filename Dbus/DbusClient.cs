using Tmds.DBus;
using Spectre.Console;

namespace Stargazer.Dbus {
    public class DbusClient {
        private static Connection _connection;
        public static async Task ConnectAsync() {
            Connection client = new Connection("tcp:host=localhost,port=44881");

            try {
                await client.ConnectAsync();

            } catch (ConnectException e) {
                AnsiConsole.WriteLine("Could not connect to Lodestar server... ?\n---\n{0}", e.Message);
                Environment.Exit(-1);
            }

            _connection = client;
        }
        
        public static async Task<bool> CreateProfileAsync(string name, string version, ModLoader loader) {
            IProfileMessenger messenger = _connection.CreateProxy<IProfileMessenger>("org.mercurius", "/org/mercurius/ProfileMessenger");

            return await messenger.CreateProfileAsync(name, version, loader, false);
        }
        public static async Task DeleteProfileAsync(string name) {
            IProfileMessenger messenger = _connection.CreateProxy<IProfileMessenger>("org.mercurius", "/org/mercurius/ProfileMessenger");
            
            await messenger.DeleteProfileAsync(name);
        }
    }
}