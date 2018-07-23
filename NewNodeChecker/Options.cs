using CommandLine;

namespace NewNodeChecker
{
    internal class Options
    {
        [Option('d', "Defination-Setting", Required = false, HelpText = "defination name used to compare servers")]
        public string DefinationSetting { get; set; }

        [Option('t', "timeout", Required = false, HelpText = "to set the connection timeout")]
        public int TimeOut { get; set; }

        // Omitting long name, defaults to name of property, ie "--verbose"
        [Option(Default = false, HelpText = "Prints all messages to standard output.")]
        public bool Verbose { get; set; }
        
    }
}
