﻿using DsmSuite.DsmViewer.Application.Interfaces;
using DsmSuite.DsmViewer.Model.Interfaces;

namespace DsmSuite.DsmViewer.Application.Actions.Base
{
    public abstract class ActionBase : IAction
    {
        protected ActionBase(IDsmModel model)
        {
            Model = model;
        }

        protected IDsmModel Model { get; }

        public abstract void Do();

        public abstract void Undo();

        public string Type { get; protected set; }
        public string Details { get; protected set; }

        public string Description => $"{Type} : {Details}";
    }
}
