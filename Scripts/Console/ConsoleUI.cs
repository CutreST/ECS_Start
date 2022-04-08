using Godot;
using MySystems;
using System;
using System.Collections.Generic;
using Base.Interfaces;

namespace Godot
{
    /// <summary>
    /// A simple console <see cref="Control"/>. Used for diferents projects, now it's for debug
    /// prourposes
    /// </summary>
    public class ConsoleUI : Control
    {
        /// <summary>
        /// Chache for populating the <see cref="_unusedLabels"/>
        /// </summary>
        [Export]
        private readonly byte TEXT_HOLDER_CACHE;

        /// <summary>
        /// Name of the <see cref="_textHolder"/>. It's a horizontal container that organizes the labels
        /// </summary>
        [Export]
        private readonly string TEXT_HOLDER_NAME = "TextHolder";

        /// <summary>
        /// Static control that act as <see cref="_background"/> to use <see cref="_background.RectSize.y"/> to check if a label is inside
        /// the container
        /// </summary>
        [Export]
        private readonly string STATIC_CONTROL_NAME = "StaticControl";

        /// <summary>
        /// The time between <see cref="_toWrite"/> enqueess
        /// </summary>
        [Export]
        private readonly float DISPLAY_TIME = 0.1f;

        /// <summary>
        /// Font of the label, is an overrride
        /// </summary>
        [Export]
        private readonly DynamicFont FONT;

        /// <summary>
        /// Background needed for the rect
        /// </summary>
        private Control _background;

        /// <summary>
        /// Parent of the labels
        /// </summary>
        private Control _textHolder;

        /// <summary>
        /// Unused labels. We use a simple pool (TODO: create a generic)
        /// </summary>
        private List<Label> _unusedLabels = new List<Label>();

        /// <summary>
        /// Used labels.
        /// </summary>
        private List<Label> _usedLabels = new List<Label>();

        /// <summary>
        /// Labels that are waiting to be spawned at the console.
        /// </summary>
        private Queue<Label> _toWrite;

        /// <summary>
        /// A height offset for the height check.
        /// TODO: check the true value.
        /// </summary>
        private const int HEIGHT_OFFSET = 3;

        /// <summary>
        /// Index used for a lot of things
        /// </summary>
        private byte index;

        /// <summary>
        /// current time, to check if enquere or not
        /// </summary>
        private float time;

        /// <summary>
        /// Default color of the messages written in the console.
        /// Used on <see cref="ConsoleUI.WriteDefaultMessage(in string)"
        /// </summary>
        [Export]
        private readonly Color COLOR_DEFAULT;

        /// <summary>
        /// The color of the Error messages written in the console.
        /// Used on <see cref="ConsoleUI.WriteErrorMessage(in string)"
        /// </summary>
        [Export]
        private readonly Color COLOR_WARNING;

        /// <summary>
        /// The color of the Error messages written in the console.
        /// Used on <see cref="ConsoleUI.WriteWarningMessage(in string)"
        /// </summary>
        [Export]
        private readonly Color COLOR_ERROR;


        /// <summary>
        /// Initialices the visual console, setting all the children.
        /// OJU! call this after inserting the children. It doesn't work
        /// with _EnterTree
        /// </summary>
        public void Init(){
            GD.Print("ConsoleUI entered the tree");

            this._textHolder = this.TryGetFromChild_Rec<Control>(TEXT_HOLDER_NAME);

            if (_textHolder != null)
            {
                Messages.Print("Text holder found!!!");
            }

            this.LabelListsInit();
            index = 0;

            this._background = this.TryGetFromChild_Rec<Control>(STATIC_CONTROL_NAME);
            base.SetProcess(false);
        }

        public override void _Ready()
        {
            MyConsole.Write("Damm, this is MY console");
        }

        /// <summary>
        /// Initializes the <see cref="_unusedLabels"/>, <see cref="_usedLabels"/> and <see cref="_toWrite"/>. Also, populates
        /// the <see cref="_unusedLabels"/> with a <see cref="TEXT_HOLDER_CACHE"/> number of labels
        /// </summary>
        private void LabelListsInit()
        {
            _unusedLabels = new List<Label>(TEXT_HOLDER_CACHE);
            _usedLabels = new List<Label>();
            _toWrite = new Queue<Label>();

            for (byte i = 0; i < TEXT_HOLDER_CACHE; i++)
            {
                _unusedLabels.Add(this.LabelInit());
            }
        }

        /// <summary>
        /// Initialices a new label with the basic settings and not visible
        /// </summary>
        /// <returns>A new label</returns>
        private Label LabelInit()
        {
            Label l = new Label();
            l.Hide();
            l.AddFontOverride("font", FONT);
            return l;
        }

        #region Test spawn
        float time2 = 0;
        const float SPAWN = 0.75f;
        const byte MAX_SIZE = 12;
        int turn = 1;
        #endregion


        # region Write methods 

        public void WriteDefaultMessage(in string message){
            this.WriteOnConsole(message, COLOR_DEFAULT);
        }

        public void WriteWarningMessage(in string message){
            this.WriteOnConsole(message, COLOR_WARNING);
        }

        public void WriteErrorMessage(in string message){
            this.WriteOnConsole(message, COLOR_ERROR);
        }


        /// <summary>
        /// Writes a message with a font color on the console
        /// </summary>
        /// <param name="message">Message to write</param>
        /// <param name="color">Font color</param>
        public void WriteOnConsole(in string message, in Color color)
        {
            Label label;

            //check for used and unused
            if (_unusedLabels.Count > 0)
            {
                label = _unusedLabels[0];
                _unusedLabels.RemoveAt(0);
            }
            else
            {
                label = this.LabelInit();
            }


            _usedLabels.Add(label);
            _textHolder.AddChild(label);
            this.SetLabel(ref label, message, color);

            //add to the queue
            _toWrite.Enqueue(label);            

            // We use the godot process BUT
            // this has to go to the appUpdate, somehow
            base.SetProcess(true);
            // set the timer

        }

        # endregion


        private void SetLabel(ref Label label, in string message, in Color color){
            label.Name = "Label_num_" + index;
            label.Autowrap = true;
            label.Text = message;
            label.Show();
            label.VisibleCharacters = 0;
            index++;
            label.SelfModulate = color;
        }

        /// <summary>
        /// Puts a label on a position inside <see cref="_textHolder"/> using the <see cref="_backgroung.RectSize.y"/>.
        /// the label is inside the box and handles the elimination of the most ancient label, if needed (yes, ancient, as Chutlhu)
        /// </summary>
        /// <param name="lbl">The label to put</param>
        private void PutLabelOnPosition(in Label lbl)
        {
            bool isMinor = false;


            while (isMinor == false)
            {
                #region height code         

                float totalHeight = 0;
                int index = _textHolder.GetChildCount();
                Label temp;

                //get each child and, if the visible characters > 0, sum his height to add to the total height.
                //if not, then pass.
                //also, if the label is the one that we want to put, show all the chars
                for (int i = 0; i < index; i++)
                {
                    //Messages.Print("Position label " + i.ToString(), _textHolder.GetChild<Label>(i).RectGlobalPosition.ToString());
                    temp = _textHolder.GetChild<Label>(i);

                    if (temp == lbl)
                    {
                        temp.VisibleCharacters = temp.GetTotalCharacterCount();
                    }

                    if (temp.VisibleCharacters > 0)
                        totalHeight += temp.RectSize.y + HEIGHT_OFFSET;
                }

                //if the totalheight is more than the _background.y, then we remove the first child as it is the older one
                //else, well, it fits perfectly, so no need more
                if (totalHeight > _background.RectSize.y)
                {
                    temp = this._textHolder.GetChild<Label>(0);
                    this._textHolder.RemoveChild(temp);
                    this.RemoveLabel(temp);
                }
                else
                {
                    isMinor = true;
                }
                #endregion
            }
        }

        /// <summary>
        /// Remove a label from <see cref="_usedLabels"/> and move to
        /// <see cref="_unusedLabels"/>
        /// </summary>
        /// <param name="label"></param>
        private void RemoveLabel(in Label label)
        {
            _unusedLabels.Add(label);
            label.Hide();
            _usedLabels.Remove(label);
            label.Text = "";
        }       
       
        /// <summary>
        /// Clears the text of the console
        /// </summary>
        public void ClearConsoleText()
        {
            Label label;
            for (int i = 0; i < _textHolder.GetChildCount(); i++)
            {
                label = _textHolder.GetChild<Label>(i);
                _textHolder.RemoveChild(label);
                this.RemoveLabel(label);
            }
        }
        
        public override void _Process(float delta)
        {
            if (_toWrite.Count > 0)
            {
                //time += delta;

                if (AppManager_GO.Time > time)
                {
                    this.PutLabelOnPosition(_toWrite.Dequeue());
                    time = AppManager_GO.Time + DISPLAY_TIME;
                }
            }
            else
            {
                base.SetProcess(false);
            }
        }
    }
}
