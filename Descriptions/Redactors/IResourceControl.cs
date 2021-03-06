﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Resource_Redactor.Descriptions.Redactors
{
    public delegate void StateChangedEventHandler(object sender, EventArgs e);

    public interface IResourceControl
    {
        ToolStripMenuItem[] MenuTabs { get; }
        bool Saved { get; }
        bool UndoEnabled { get; }
        bool RedoEnabled { get; }
        
        event StateChangedEventHandler StateChanged;

        string ResourcePath { get; }
        string ResourceName { get; }

        int FPS { get; set; }
        void Activate();
        
        void Save(string path);
        void Undo();
        void Redo();
    }
}
