using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace VMemReaderCore;
public class Log
{
    private static Log? instance = null;
    private ILoggerFactory factory;

    public ILoggerFactory Factory
    {
        get => factory;
        init
        {
            this.factory = value;
        }
    }


    public static Log Instance
    {
        get
        {
            if (instance == null)
                instance = new Log();
            return instance;
        }
    }

    protected Log()
    {
        factory = LoggerFactory.Create(builder => builder.AddConsole().AddDebug().SetMinimumLevel(
#if DEBUG
            LogLevel.Debug
#else
            LogLevel.Error
#endif
            ));
    }

}
