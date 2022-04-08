
using System;

// usamos el namespace de godot y así sólo tenemos que meter Console.Write -> etc;
namespace Godot
{

    public static class MyConsole
    {   
        /// <summary>
        /// is the console on?
        /// </summary>
        private static bool _isOn;

        /// <summary>
        /// Path to the <see cref="ConsoleUI">.
        /// <p>Here is where the visual is.
        /// </summary>
        private const string CONSOLE_PATH = "res://Scenes/Console/Console.tscn";

        /// <summary>
        /// Reference to the <see cref="ConsoleUI">
        /// </summary>
        private static ConsoleUI _console;

        public static bool IsOn
        {
            get => _isOn;
            set
            {
                _isOn = value;

                if (_isOn)
                {
                    OpenConsole();
                }
                else
                {
                    CloseConsole();
                }
            }
        }

        /// <summary>
        /// Writes a message on the debugger and on the <see cref="ConsoleUI"> 
        /// if <see cref="MyConsole.IsOn"> is true.
        /// The color is setted in <see cref="ConsoleUI.COLOR_DEFAULT">
        /// </summary>
        /// <param name="message">the message to be written</param>
        public static void Write(in string message)
        {
            GD.Print(message);

            if (_isOn)
            {
                _console.WriteDefaultMessage(message);
            }
        }

        /// <summary>
        /// Writes a message on the debugger and on the <see cref="ConsoleUI"> 
        /// if <see cref="MyConsole.IsOn"> is true.
        /// Here the call uses a custom color
        /// </summary>
        /// <param name="message">the message to be written</param>
        public static void Write(in string message, in Color color)
        {
            GD.Print(message);

            if (_isOn)
            {
                _console.WriteOnConsole(message, color);
            }
        }

        /// <summary>
        /// Writes a Warning message on the debugger and on the <see cref="ConsoleUI"> 
        /// if <see cref="MyConsole.IsOn"> is true.
        /// The color is setted in <see cref="ConsoleUI.COLOR_WARNING">
        /// </summary>
        /// <param name="message">the message to be written</param>
        public static void WriteWarning(in string message)
        {
            GD.PushWarning(message);

            if (_isOn)
            {
                _console.WriteWarningMessage(message);
            }
        }

        /// <summary>
        /// Writes an Error message on the debugger and on the <see cref="ConsoleUI"> 
        /// if <see cref="MyConsole.IsOn"> is true.
        /// The color is setted in <see cref="ConsoleUI.COLOR_ERROR">
        /// </summary>
        /// <param name="message">the message to be written</param>
        public static void WriteError(in string message)
        {
            GD.PrintErr(message);

            if (_isOn)
            {
                _console.WriteErrorMessage(message);
            }
        }

        /// <summary>
        /// Opens the console loading the <see cref="ConsoleUI"/> and setting it 
        /// to the <see cref="AppManager_Go"> as a children node.
        /// </summary>
        private static void OpenConsole()
        {
            PackedScene sc = GD.Load<PackedScene>(CONSOLE_PATH);
            _console = sc.Instance<ConsoleUI>();
            GD.Print("Console no UI created");
            GD.Print(_console);
            MySystems.SystemManager.GetInstance(_console).NodeManager.CallDeferred("add_child", _console);
            _console.Init();
        }

        /// <summary>
        /// We dispose the console.
        /// </summary>
        private static void CloseConsole()
        {
            try
            {
                _console.Dispose();
                _console = null;
            }
            catch (Exception e)
            {
                GD.PrintErr(e.Message);
            }

        }
    }
}
