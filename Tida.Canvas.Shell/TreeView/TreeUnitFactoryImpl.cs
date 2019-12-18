using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tida.Canvas.Shell.Contracts.TreeView;

namespace Tida.Canvas.Shell.TreeView {
    [Export(typeof(ITreeUnitFactory))]
    class TreeUnitFactoryImpl : ITreeUnitFactory {
        public ITreeUnit CreateNew(string typeGUID) => new TreeUnit(typeGUID);
    }
}
