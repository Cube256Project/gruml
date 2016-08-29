using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace GRUML.Model
{
    class CommandBinding : EventBinding
    {
        public string Command { get; private set; }

        public string ExecuteHandler { get; private set; }

        public string CanExecuteHandler { get; private set; }

        public override bool Load(XmlElement e)
        {
            if (e.HasAttribute("Command"))
            {
                Command = e.GetAttribute("Command");
            }

            if (e.HasAttribute("Execute"))
            {
                ExecuteHandler = e.GetAttribute("Execute");
            }

            if (e.HasAttribute("CanExecute"))
            {
                CanExecuteHandler = e.GetAttribute("CanExecute");
            }

            return base.Load(e);
        }
    }
}
