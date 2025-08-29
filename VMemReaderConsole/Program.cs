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
            logger.LogInformation(e.Message);
        }
        catch (InjectionException ex)
        {
            logger.LogError("Injection error: " + ex.Message);
        }
        catch (FormatException ex)
        {
            logger.LogError("Format Error or Info: The parameter is incorrectly introduced or the flags are incorrectly placed, enter -help to obtain detailed information");
        }
        catch (ApplicationException e)
        {
            logger.LogError(e.Message);
        }
        catch (InvalidOperationException e)
        {
            logger.LogError(e.Message + ", perhaps the wrong parameters were introduced");
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
        }
    }
}
