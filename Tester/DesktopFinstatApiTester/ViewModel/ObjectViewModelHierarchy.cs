using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

// code from https://stackoverflow.com/questions/3668802/looking-for-an-object-graph-tree-view-control-for-wpf?utm_medium=organic&utm_source=google_rich_qa&utm_campaign=google_rich_qa
namespace DesktopFinstatApiTester.ViewModel
{
    public class ObjectViewModelHierarchy
    {
        readonly ReadOnlyCollection<ObjectViewModel> _firstGeneration;
        readonly ObjectViewModel _rootObject;

        public ObjectViewModelHierarchy(object rootObject)
        {
            _rootObject = new ObjectViewModel(rootObject);
            _firstGeneration = new ReadOnlyCollection<ObjectViewModel>(new ObjectViewModel[] { _rootObject });
        }

        public ReadOnlyCollection<ObjectViewModel> FirstGeneration
        {
            get { return _firstGeneration; }
        }
    }
}
