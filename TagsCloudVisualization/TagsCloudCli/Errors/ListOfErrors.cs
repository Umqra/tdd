using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ResultOf;

namespace TagsCloudCli.Errors
{
    public class ReadInputFileError : CliError
    {
        public ReadInputFileError(string message, IError innerError = null) : base(message, innerError)
        {
        }
    }

    public class WriteOutputFileError : CliError
    {
        public WriteOutputFileError(string message, IError innerError = null) : base(message, innerError)
        {
        }
    }

    public class InvalidLayouterError : CliError
    {
        public InvalidLayouterError(string message, IError innerError = null) : base(message, innerError)
        {
        }
    }

    public class InvalidColorError : CliError
    {
        public InvalidColorError(string message, IError innerError = null) : base(message, innerError)
        {
        }
    }

    public class InitializingError : CliError
    {
        public InitializingError(string initializedField, IError innerError = null) : 
            base($"Error while initializing {initializedField}", innerError)
        {
        }
    }

    public class UnknownFontError : CliError
    {
        public UnknownFontError(string message, IError innerError = null) : base(message, innerError)
        {
        }
    }

    public class ConfigLoadError : CliError
    {
        public ConfigLoadError(string message, IError innerError = null) : base(message, innerError)
        {
        }
    }

    public class NullOptionError : CliError
    {
        public NullOptionError(string optionName, IError innerError = null) : 
            base($"Option {optionName} has null value", innerError)
        {
        }
    }
}
