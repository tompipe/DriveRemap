using System.Collections.Generic;
using CommandLine;
using CommandLine.Text;

namespace DriveRemap
{
    internal class Options
    {
        [Value(1, MetaName = "Old", Required = true, HelpText = "Old server name or IP Address")]
        public string OldServerName { get; set; }

        [Value(2, MetaName = "New", Required = true, HelpText = "New server name or IP Address (and optional path)")]
        public string NewServerName { get; set; }

        [Usage(ApplicationAlias = "DriveRemap")]
        public static IEnumerable<Example> Examples
        {
            get
            {
                yield return new Example("Moving drives using NETBIOS Names", new Options {OldServerName = "OldServer", NewServerName = "NewServer\n"});
                yield return new Example("Moving drives using IP Addresses", new Options {OldServerName = "10.1.100.1", NewServerName = "10.1.100.2\n"});
                yield return new Example("Moving drives to a new server and path", new Options {OldServerName = "OldServer", NewServerName = "NewServer\\NewPath\n" });
            }
        }

        
    }
}