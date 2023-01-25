namespace Stargazer.Repos.Models {
    public struct ProjectModel {
        public string slug { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string[] categories { get; set; } 
        public string client_side { get; set; } 
        public string server_side { get; set; } 
        public string body { get; set; } 
        public string issues_url { get; set; } 
        public string source_url { get; set; } 
        public string wiki_url { get; set; } 
        public string discord_url { get; set; } 
        public donation_url[] donation_urls { get; set; } 
        public string project_type { get; set; }
        int downloads { get; set; }
        public string icon_url { get; set; }
        public string id { get; set; }
        public string team { get; set; }
        public string body_url { get; set; }
        public string moderator_message { get; set; }
        public string published { get; set; }
        public string updated { get; set; }
        public int followers { get; set; }
        public string status { get; set; }
        public license license { get; set; }
        public string[] versions { get; set; }
        public gallery[] gallery { get; set; }
    }

    public struct donation_url {
        public string id { get; set; }
        public string platform { get; set; }
        public string url { get; set; }
    }
    public struct license {
        public string id { get; set; }
        public string name { get; set; }
        public string url { get; set; } 
    }
    public struct gallery {
        public string url { get; set; }
        public bool featured { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string created { get; set; }
    }
    public struct SearchModel {
        public Hit[] hits { get; set; }
        public int offset { get; set; }
        public int limit { get; set; }
        public int total_hits { get; set; }
    }
    public struct Hit {
        public string slug { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string[] categories { get; set; }
        public string client_side { get; set; }
        public string server_side { get; set; }
        public string project_type { get; set; }
        public int downloads { get; set; }
        public string icon_url { get; set; }
        public string project_id { get; set; }
        public string author { get; set; }
        public string[] versions { get; set; }
        public int follows { get; set; }
        public string date_created { get; set; }
        public string date_modified { get; set; }
        public string latest_version { get; set; }
        public string license { get; set; }
        public string[] gallery { get; set; }
    }
    public struct VersionModel {
        public string name { get; set; }
        public string version_number { get; set; }
        public string changelog { get; set; }
        public Dependency[] dependencies { get; set; }
        public string[] game_versions { get; set; }
        public string version_type { get; set; }
        public string[] loaders { get; set; }
        public bool featured { get; set; }
        public string id { get; set; }
        public string project_id { get; set; }
        public string author_id { get; set; }
        public string date_published { get; set; }
        public int downloads { get; set; }
        public string changelog_url { get; set; }
        public modFile[] files { get; set; } 
    }
    public struct Dependency {
        public string version_id { get; set; }
        public string project_id { get; set; }
        public string dependency_type { get; set; }
    }
    public struct modFile {
        public hash hashes { get; set; }
        public string url { get; set; }
        public string filename { get; set; }
        public bool primary { get; set; }
    }
    public struct hash {
        public string sha512 { get; set; }
        public string sha1 { get; set; }
    }
}