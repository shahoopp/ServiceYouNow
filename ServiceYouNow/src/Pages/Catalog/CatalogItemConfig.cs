
using System.Collections.Generic;

namespace ServiceYouNow.src.catalog
{
    public class CatalogItemConfig
    {
        public string Name { get; set; } = string.Empty;
        // PSG DB Access Request
        public List<string> ActionTypes { get; set; } = new();
        public List<string> Environments { get; set; } = new();

        // Indicates whether this catalog item requires a server dropdown.
        public bool ServerName { get; set; }

        public bool AccessStartAndEndDateRequired { get; set; }
        public List<string> AccessTypes { get; set; } = new();
        public bool AssociatedWorkItemRequired { get; set; }
        public bool BusinessCaseRequired { get; set; }

        // ENV -> list of servers for that environment
        public Dictionary<string, List<string>> ServerNamesByEnvironment { get; set; } = new();

        public List<string> AccountTypes { get; set; } = new();

    }
}
