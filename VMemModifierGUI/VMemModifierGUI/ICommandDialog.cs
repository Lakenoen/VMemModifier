using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Interactivity;

namespace VMemModifierGUI;
internal interface ICommandDialog
{
    public void OnClick(object sender, RoutedEventArgs args);
    public void OnClose(object sender, RoutedEventArgs args);

}
