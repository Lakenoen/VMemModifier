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
        }
        catch (InjectionException ex)
        {
            Console.WriteLine("Fail: Injection error: " + ex.Message);
        }
        catch (FormatException ex)
        {
            Console.WriteLine("Fail: Format Error or Info: The parameter is incorrectly introduced or the flags are incorrectly placed, enter -help to obtain detailed information");
        }
        catch (ApplicationException e)
        {
            Console.WriteLine("Fail: " + e.Message);
        }
        catch (InvalidOperationException e)
        {
            Console.WriteLine("Fail: " + e.Message + ", perhaps the wrong parameters were introduced");
        }
        catch (Exception e)
        {
            Console.WriteLine("Fail: " + e.Message);
        }
    }
}
