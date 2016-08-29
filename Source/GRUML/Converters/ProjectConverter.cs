using GRUML.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRUML.Converters
{
    class ProjectConverter : ElementConverter
    {
        private Project _project;

        public ProjectConverter(Project project)
        {
            _project = project;
        }

        protected override void ConvertOverride()
        {
            // emit related dictionaries before the control
            if (null != _project.Dictionary)
            {
                Context.Convert(_project.Dictionary);
            }

            foreach (var e in _project.Controls)
            {
                Context.Convert(e);
            }

            foreach(var e in _project.Pages)
            {
                Context.Convert(e);
            }
        }
    }
}
