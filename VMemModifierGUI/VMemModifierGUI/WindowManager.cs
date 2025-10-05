using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.VisualTree;

namespace VMemReaderGUI;
internal class WindowManager<T> where T : new()
{
    private static WindowManager<T>? instance = null;
    public T? Value { get; set; }

    public static WindowManager<T> Instance
    {
        get
        {
            if(instance == null)
                instance = new WindowManager<T>();
            return instance;
        }
    }

    private WindowManager() { }

    public T Create()
    {
        Value = new T();
        return Value;
    }

}
