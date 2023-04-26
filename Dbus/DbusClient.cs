using System.Threading.Tasks;
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

#if DEBUG
            AnsiConsole.WriteLine("Connected to Lodestar server");
#endif
            _connection = client;
        }
        
        public static async Task<bool> CreateProfileAsync(string name, string version, ModLoader loader) {
            IProfileMessenger messenger = _connection.CreateProxy<IProfileMessenger>("org.mercurius", "/org/mercurius/ProfileMessenger");

            return await messenger.CreateProfileAsync(name, version, loader, false);
        }
        public static async Task DeleteProfileAsync(string profileName) {
            if (!(await CheckProfileExistsAsync(profileName))) {
                throw new Exception($"Profile {profileName} doesn't exist!");
            }

            IProfileMessenger messenger = _connection.CreateProxy<IProfileMessenger>("org.mercurius", "/org/mercurius/ProfileMessenger");
            
            await messenger.DeleteProfileAsync(profileName);
        }
        public static async Task<ProfileInfo[]> ListProfilesAsync() {
            IProfileMessenger messenger = _connection.CreateProxy<IProfileMessenger>("org.mercurius", "/org/mercurius/ProfileMessenger");

            List<ProfileInfo> profiles = new List<ProfileInfo>();

            string[] names = await messenger.ListProfilesAsync();

            foreach (string name in names) {
                IDbusProfile profile = _connection.CreateProxy<IDbusProfile>("org.mercurius.profile", $"/org/mercurius/profile/{name}");

                profiles.Add(await profile.GetProfileInfoAsync());
            }

            return profiles.ToArray<ProfileInfo>();
        }
        public static async Task<Mod[]> ListModsAsync(string profileName) {
             if (!(await CheckProfileExistsAsync(profileName))) {
                throw new Exception($"Profile {profileName} doesn't exist!");
            }

            IDbusProfile profile = _connection.CreateProxy<IDbusProfile>("org.mercurius.profile", $"/org/mercurius/profile/{profileName}");
        
            return await profile.ListModsAsync();
        }
        public static async Task<bool> SyncProfileAsync(string profileName) {
            IDbusProfile profile = _connection.CreateProxy<IDbusProfile>("org.mercurius.profile", $"/org/mercurius/profile/{profileName}");
        
            return await profile.SyncAsync();
        }
        public static async Task<Mod[]> AddModAsync(string profileName, string projectId, Remote repo, bool ignoreDeps) {
            IDbusProfile profile = _connection.CreateProxy<IDbusProfile>("org.mercurius.profile", $"/org/mercurius/profile/{profileName}");
            
            return await profile.AddModAsync(projectId, repo, ignoreDeps);
        }
        public static async Task<bool> CheckProfileExistsAsync(string profileName) {
            IProfileMessenger messenger = _connection.CreateProxy<IProfileMessenger>("org.mercurius.ProfileMessenger", "/org/mercurius/ProfileMessenger");

            string[] profiles = await messenger.ListProfilesAsync();
            if (!profiles.Contains<string>(profileName)) {
                return false;   
            }

            return true;
        }
        public static async Task<ProfileInfo> GetProfileInfoAsync(string profileName) {
            if (!(await CheckProfileExistsAsync(profileName))) {
                throw new Exception($"Profile {profileName} doesn't exist!");
            }
            
            IDbusProfile profile = _connection.CreateProxy<IDbusProfile>("org.mercurius.profile", $"/org/mercurius/profile/{profileName}");
            return await profile.GetProfileInfoAsync();
        }
        public static async Task<bool> RemoveModAsync(string profileName, string id, bool force) {
            if (!(await CheckProfileExistsAsync(profileName))) {
                throw new Exception($"Profile {profileName} doesn't exist!");
            }

            IDbusProfile profile = _connection.CreateProxy<IDbusProfile>("org.mercurius.profile", $"/org/mercurius/profile/{profileName}");
            try {
                await profile.RemoveModAsync(id, force);
            } catch (DBusException) {
                return false;
            }

            return true;
        }
        public static async Task<ValidityReport> VerifyAsync(string profileName) {
            if (!(await CheckProfileExistsAsync(profileName))) {
                throw new Exception($"Profile {profileName} doesn't exist!");
            }

            IDbusProfile profile = _connection.CreateProxy<IDbusProfile>("org.mercurius.profile", $"/org/mercurius/profile/{profileName}");

            return await profile.VerifyAsync();
        }
    }
}