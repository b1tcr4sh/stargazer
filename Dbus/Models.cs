using System.Threading.Tasks;
using Tmds.DBus;
using System.Runtime.InteropServices;

namespace Stargazer.Dbus {
    [DBusInterface("org.mercurius.ProfileMessenger")]
    public interface IProfileMessenger : IDBusObject {
        Task<string[]> ListProfilesAsync();
        Task<bool> CreateProfileAsync(string name, string minecraftVersion, ModLoader loader, bool serverSide);
        Task DeleteProfileAsync(string name);
    }
    [DBusInterface("org.mercurius.profile")]
    public interface IDbusProfile : IDBusObject {
        public Task<ProfileInfo> GetProfileInfoAsync();
        public Task<Mod[]> AddModAsync(string id, Repo service, bool ignoreDependencies);
        public Task<Mod[]> AddVersionAsync(string version, Repo service, bool ignoreDependencies);
        public Task<bool> RemoveModAsync(string id, bool force);
        public Task<bool> SyncAsync();
        public Task<Mod[]> ListModsAsync();
        public Task<ValidityReport> VerifyAsync(); // Should check to make sure all dependencies are met and everything is compatible; auto fix incompatibilities or return false if can't
        public Task CheckForUpdatesAsync(); // Should return struct describing mods and if they're outdated
        public Task UpdateModAsync(string id); // Should fetch newest compatible version of mod
        public Task GenerateAsync(bool startFromCleanSlate); // Should generate mod metadata from mod files (properly this time)
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Mod {
        public string Title { get; set; }
        public string FileName { get; set; }
        public string DownloadURL { get; set; }
        public string ProjectId { get; set; }
        public string VersionId { get; set; }
        public string MinecraftVersion { get; set; }
        public string ModVersion { get; set; }
        public ModLoader[] Loaders { get; set; }
        public IEnumerable<string> DependencyVersions { get; set; }
        public ClientDependency ClientDependency { get; set; }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ProfileInfo {
        public string Name;
        public string MinecraftVersion;
        public bool IsServerSide;
        public ModLoader Loader;
        public string FilePath;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ValidityReport {
        public Mod[] incompatible;
        public string[] missingDependencies;
        public bool synced;
        public bool repaired;
    }
}