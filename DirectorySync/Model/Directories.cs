using System.Text.Json.Serialization;

namespace DirectorySync.Model
{
    public class Directories
    {
        public string Source { get; private set; }
        public string Target { get; private set; }

        [JsonIgnore]
        public string SyncStatus { get; private set; }

        public Directories(string source, string target)
        {
            Source = source;
            Target = target;
            SyncStatus = "";
        }

        public void SetSyncStatus(string status)
        {
            SyncStatus = status;
        }
    }
}
