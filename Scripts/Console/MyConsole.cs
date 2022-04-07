
using System;

// usamos el namespace de godot y así sólo tenemos que meter Console.Write -> etc;
namespace Godot
{

    public static class MyConsole
    {
        private static bool _isOn;

        private const string CONSOLE_PATH = "res://Scenes/Console/Console.tscn";

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

        public static void Write(in string message)
        {
            GD.Print(message);

            if (_isOn)
            {
                _console.WriteOnConsole(message, Colors.Beige);
            }
        }

        private static void OpenConsole()
        {
            PackedScene sc = GD.Load<PackedScene>(CONSOLE_PATH);
            _console = sc.Instance<ConsoleUI>();
            GD.Print("Console no UI created");
            GD.Print(_console);
            //n.CallDeferred("add_child", _console);
            //MySystems.SystemManager.GetInstance(_console).NodeManager.CallDeferred("add_child", _console);
            MySystems.SystemManager.GetInstance(_console).NodeManager.AddChild(_console);
            _console.Init();
        }

        private static void CloseConsole()
        {
            try{
                _console.Dispose();
            }catch(Exception e){
                GD.PrintErr("There is no console");
            }
            
        }
        /*
        readonly string CONSOLE_PATH = "res://Scenes/Console/Console.tscn";
        private static UI.Console _console;

        #region colors
        private readonly Color COLOR_ATTACK = Colors.Red;
        private static readonly Color COLOR_DEFAULT = Colors.White;
        private readonly Color COLOR_FAIL = Colors.Blue;
        private readonly Color COLOR_DEAD = Colors.LightSlateGray;
        #endregion

        private void Init(){
            //first, cheeck if a console already exists
            Node n = MyManager.NodeManager.GetParent();
            _console = n.TryGetFromChild_Rec<UI.Console>();

            if(_console != null){
                Messages.Print("yelooooow, is it me");
            }else{
                PackedScene  sc= GD.Load<PackedScene>(CONSOLE_PATH);
                _console = sc.Instance<UI.Console>();
                //n.CallDeferred("add_child", _console);
                MyManager.NodeManager.AddChild(_console);
            }
        }

        #region Main console methods
        public void Show(){
            _console.Show();
        }

        public void Hide(){
            _console.Hide();
        }

        public void ClearConsoleText(){
            _console.ClearConsoleText();
        }
        #endregion

        #region Write Methods (TODO: Another class?)

        public static void Write(in string mess){
            Messages.Print("yelloooow");
            _console.WriteOnConsole(mess, COLOR_DEFAULT);
        }

        #endregion


        #region System methods
        public override void OnEnterSystem(params object[] obj)
        {
            Messages.EnterSystem(this);
            this.Init();
        }

        public override void OnExitSystem(params object[] obj)
        {
            Messages.ExitSystem(this);
        }
        #endregion
        */
    }
}
