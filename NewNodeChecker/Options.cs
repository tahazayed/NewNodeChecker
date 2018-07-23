using System.ComponentModel.DataAnnotations;
using CommandLine;

namespace NewNodeChecker
{
    internal class Options
    {
        [Option('d', "Defination-Setting", Required = false, HelpText = "defination name used to compare servers", Default = "Default")]
        public string DefinationSetting { get; set; } = "Default";

        [Option('t', "timeout", Required = false, HelpText = "to set the connection timeout", Default = 30)]
        public int TimeOut { get; set; } = 30;

        // Omitting long name, defaults to name of property, ie "--verbose"
        [Option(Default = false, HelpText = "Prints all messages to standard output.")]
        public bool Verbose { get; set; }
        
    }
}
