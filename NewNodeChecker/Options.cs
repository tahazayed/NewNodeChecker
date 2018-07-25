using System.ComponentModel.DataAnnotations;
using CommandLine;

namespace NewNodeChecker
{
    internal class Options
    {
        [Option('d', "defination-setting", Required = false, HelpText = "defination name used to compare servers", Default = "Default")]
        public string DefinationSetting { get; set; } = "Default";

        [Option('t', "timeout", Required = false, HelpText = "to set the connection timeout", Default = 30)]
        public int TimeOut { get; set; } = 30;

        [Option('m', "mode", Required = false, HelpText = "run mode i=inspect(default mode), c=compare results", Default = 'i')]
        public char Mode { get; set; } = 'i';

        // Omitting long name, defaults to name of property, ie "--verbose"
        [Option(Default = false, HelpText = "Prints all messages to standard output.")]
        public bool Verbose { get; set; }

        [Option('x', "server-one", Required = false, HelpText = "to secify the first server to be compared")]
        public string ServerOne { get; set; }

        [Option('y', "server-two", Required = false, HelpText = "to secify the second server to be compared")]
        public string ServerTwo { get; set; }
    }
}
