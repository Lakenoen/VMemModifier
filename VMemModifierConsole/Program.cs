using System.Linq.Expressions;
using System.Text;
using Microsoft.Extensions.Logging;
using VMemReaderConsole;
using VMemReaderCore;
using static VMemReaderConsole.AbstractCommand;
using static VMemReaderCore.VMemStream;

class Program
{

    private static ILogger logger = Log.Instance.Factory.CreateLogger<Program>();
    public static void Main(string[] args)
    {
        try
        {
            ConsoleInterface.Instance.process(args);
        }
        catch (CommandInfoException e)
        {
            Console.WriteLine("Info: " + e.Message);
#if DEBUG
            logger.LogError(e.StackTrace);
#endif
        }
        catch (InjectionException ex)
        {
            Console.WriteLine("Error: Injection error: " + ex.Message);
#if DEBUG
            logger.LogError(ex.StackTrace);
#endif
        }
        catch (FormatException ex)
        {
            Console.WriteLine("Error: Format Error or Info: The parameter is incorrectly introduced or the flags are incorrectly placed, enter -help to obtain detailed information");
#if DEBUG
            logger.LogError(ex.StackTrace);
#endif
        }
        catch (ApplicationException e)
        {
            Console.WriteLine("Error: " + e.Message);
#if DEBUG
            logger.LogError(e.StackTrace);
#endif
        }
        catch (InvalidOperationException e)
        {
            Console.WriteLine("Error: " + e.Message + ", perhaps the wrong parameters were introduced");
#if DEBUG
            logger.LogError(e.StackTrace);
#endif
        }
        catch (Exception e)
        {
            Console.WriteLine("Error: " + e.Message);
#if DEBUG
            logger.LogError(e.StackTrace);
#endif
        }

        Console.Out.Flush();
    }
}
