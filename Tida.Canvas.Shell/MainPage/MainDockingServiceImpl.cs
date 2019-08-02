using Tida.Application.Contracts.App;
using Tida.Application.Contracts.Common;
using Tida.Application.Contracts.Docking;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Docking;
using static Tida.Canvas.Shell.MainPage.Constants;
using static Tida.Canvas.Shell.Contracts.MainPage.Constants;

namespace Tida.Canvas.Shell.MainPage {
    /// <summary>
    /// 主区域的停靠服务;
    /// </summary>
    [Export(DockingService_Main, typeof(IDockingService))]
    public partial class MainDockingServiceImpl : IDockingService {
        static MainDockingServiceImpl() {
            ThemeService.AddDictionary(_dockingThemesDict);
        }

        [ImportingConstructor]
        public MainDockingServiceImpl(
            [ImportMany]IEnumerable<Lazy<IDockingContainer,IDockingContainerMetaData>> dockingContainers,
            [ImportMany]IEnumerable<Lazy<IDockingGroup,IDockingGroupMetaData>> dockingGroups,
            [ImportMany]IEnumerable<Lazy<IDockingPane,IDockingPaneMetaData>> dockingPanes,
            Views.MainPage mainPage) {

            _radDocking = mainPage.RadDocking;
            _documentPaneGroup = mainPage.DocumentPaneGroup;
            
            _mefDockingContainers.AddRange(dockingContainers.
                Where(p => p.Metadata.DockingServiceGUID == DockingService_Main).
                Select(p => new CreatedDockingContainer(p.Value,p.Metadata)));

            _mefDockingGroups.AddRange(dockingGroups.OrderBy(p => p.Metadata.Order).
                Where(p => _mefDockingContainers.Any(q => q.DockingContainerMetaData.GUID == p.Metadata.ContainerGUID)).
                Select(p => new CreatedDockingGroup(p.Value, p.Metadata)));

            _mefDockingPanes.AddRange(dockingPanes.
                Where(p => _mefDockingGroups.Any(q => q.DockingGroupMetaData.GUID == p.Metadata.InitPaneGroupGUID)).
                Select(p => new CreatedDockingPane(p.Value, p.Metadata)));

            Initialize();
        }

        private readonly List<CreatedDockingContainer> _mefDockingContainers = new List<CreatedDockingContainer>();
        private readonly List<CreatedDockingGroup> _mefDockingGroups = new List<CreatedDockingGroup>();
        private readonly List<CreatedDockingPane> _mefDockingPanes = new List<CreatedDockingPane>();

        private readonly RadPaneGroup _documentPaneGroup;
        private readonly RadDocking _radDocking;

        public IEnumerable<CreatedDockingPane> DockingPanes => _radDocking.Panes.Select(p => p.Tag).OfType<CreatedDockingPane>();
        public IEnumerable<CreatedDockingGroup> DockingPaneGroups => _radDocking.SplitContainers.Select(p => p.Tag).OfType<CreatedDockingGroup>();
        public IEnumerable<CreatedDockingContainer> DockingContainers => _radDocking.SplitContainers.Select(p => p.Tag).OfType<CreatedDockingContainer>();
        
        private void Initialize() {
            _radDocking.Items.Clear();

            _mefDockingContainers.ForEach(dockingContainer => {
                var radSplitContainer = GenerateRadSplitContainer(dockingContainer);
                _radDocking.Items.Add(radSplitContainer);
            });
            
            _radDocking.Close += RadDocking_Close;
        }

        /// <summary>
        /// 当手动关闭时,设定停靠区域的隐藏属性;
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RadDocking_Close(object sender, StateChangeEventArgs e) {
            foreach (var radPane in e.Panes) {
                if (radPane.Tag is CreatedDockingPane dockingPane) {
                    dockingPane.DockingPane.IsHidden = true;
                }
            }
        }

        public void AddPane(CreatedDockingPane dockingPane) {
            if (dockingPane == null) {
                throw new ArgumentNullException(nameof(dockingPane));
            }

            var radPane = GetRadPaneByGUID(dockingPane.DockingPaneMetaData.GUID);
            if(radPane != null) {
                throw new ArgumentException($"The {nameof(dockingPane)} has already been added.");
            }
            
            var radPaneGroup = GetRadPaneGroupByGUID(dockingPane.DockingPaneMetaData.InitPaneGroupGUID);
            if(radPaneGroup == null) {
                return;
            }
            
            radPane = GenericRadPane(dockingPane);
            radPaneGroup.AddItem(radPane, DockPosition.Center);
        }
        
        public void RemovePane(CreatedDockingPane dockingPane) {
            var radPane = GetRadPaneByGUID(dockingPane.DockingPaneMetaData.InitPaneGroupGUID);
            if (radPane == null) {
                throw new ArgumentNullException($"The {nameof(dockingPane)} has not been added.");
            }

            radPane.RemoveFromParent();

            radPane.Content = null;
            radPane.Tag = null;
        }


        private RadPaneGroup GetRadPaneGroupByGUID(string dockingGroupGUID) {
            //寻找DockGroup是否已经存在;
            var radPaneGroup = _radDocking.SplitItems.Where(p => p is RadPaneGroup).Cast<RadPaneGroup>().
                FirstOrDefault(g => g.Tag is CreatedDockingGroup dockingGroup && dockingGroup.DockingGroupMetaData.GUID == dockingGroupGUID);

            return radPaneGroup;
        }

        private RadPane GetRadPaneByGUID(string dockingPaneGUID) {
            var radPane = _radDocking.Panes.
                FirstOrDefault(p => 
                    p.Tag is CreatedDockingPane dockingPane && dockingPane.DockingPaneMetaData.GUID == dockingPaneGUID
                );

            return radPane;
        }


        /// <summary>
        /// 向文档区域中加入停靠区域;
        /// </summary>
        /// <param name="dockingPane"></param>
        public void AddPaneToDocument(CreatedDockingPane dockingPane) {

            if (dockingPane == null) {
                throw new ArgumentNullException(nameof(dockingPane));
            }

            var radPane = GetRadPaneByGUID(dockingPane.DockingPaneMetaData.GUID);
            if (radPane != null) {
                throw new ArgumentException($"{nameof(dockingPane)} has already been added.");
            }

            radPane  = GenericRadPane(dockingPane);

            _documentPaneGroup.Items.Add(radPane);
        }

        /// <summary>
        /// 从文档区域中移除停靠区域;
        /// </summary>
        /// <param name="dockingPane"></param>
        public void RemovePaneFromDocument(CreatedDockingPane dockingPane) {
            if (dockingPane == null) {
                throw new ArgumentNullException(nameof(dockingPane));
            }

            var radPane = GetRadPaneByGUID(dockingPane.DockingPaneMetaData.GUID);
            if (radPane != null) {
                throw new ArgumentException($"{nameof(dockingPane)} has not been added.");
            }
            
        }
        
        private RadSplitContainer GenerateRadSplitContainer(CreatedDockingContainer dockingContainer) {
            var radSplitContainer = new RadSplitContainer {
                Tag = dockingContainer,
                InitialPosition = FromDockingPositionToDockState(dockingContainer.DockingContainerMetaData.InitDockingPosition),
                Orientation = dockingContainer.DockingContainerMetaData.Orientation
            };
            
            foreach (var dockingGroup in _mefDockingGroups) {
                if (dockingGroup.DockingGroupMetaData.ContainerGUID != dockingContainer.DockingContainerMetaData.GUID) {
                    continue;
                }

                var radPaneGroup = GenerateRadPaneGroup(dockingGroup);
                radSplitContainer.Items.Add(radPaneGroup);
            }

            return radSplitContainer;
        }

        private RadPaneGroup GenerateRadPaneGroup(CreatedDockingGroup dockingGroup) {
            var radPaneGroup = new RadPaneGroup {
                Tag = dockingGroup,
            };

            if (dockingGroup.DockingGroupMetaData.NoStyle) {
                radPaneGroup.Template = NoStyleContainerTemplate;
            };

            foreach (var dockingPane in _mefDockingPanes) {
                if(dockingPane.DockingPaneMetaData.InitPaneGroupGUID != dockingGroup.DockingGroupMetaData.GUID) {
                    continue;
                }

                var radPane = GenericRadPane(dockingPane);
                radPaneGroup.AddItem(radPane, DockPosition.Center);
            }

            return radPaneGroup;
        }
        
        /// <summary>
        /// 根据停靠区域实例生成一个<see cref="RadPane"/>
        /// </summary>
        /// <param name="dockingPane"></param>
        /// <returns></returns>
        private static RadPane GenericRadPane(CreatedDockingPane dockingPane) {
            
            var radPane = new RadPane {
                Tag = dockingPane,
                PaneHeaderVisibility = dockingPane.DockingPane.PaneHeaderVisibility,
                CanUserClose = dockingPane.DockingPaneMetaData.CanUserClose,
                Content = dockingPane.DockingPane.UIObject,
                CanFloat = dockingPane.DockingPaneMetaData.CanFloat,
                Header = dockingPane.DockingPane.Header,
                IsHidden = dockingPane.DockingPane.IsHidden
            };
            
            dockingPane.DockingPane.IsHiddenChanged += delegate {
                radPane.IsHidden = dockingPane.DockingPane.IsHidden;
            };

            DockingPanel.SetInitialSize(radPane, new Size(dockingPane.DockingPaneMetaData.InitialWidth, dockingPane.DockingPaneMetaData.InitialHeight));
            
            return radPane;
        }

    }

    public partial class MainDockingServiceImpl {
      
        private static DockState FromDockingPositionToDockState(DockingPosition dockPosition) {
            switch (dockPosition) {
                case DockingPosition.Top:
                    return DockState.DockedTop;
                case DockingPosition.Bottom:
                    return DockState.DockedBottom;
                case DockingPosition.Right:
                    return DockState.DockedRight;
                case DockingPosition.Left:
                    return DockState.DockedLeft;
                case DockingPosition.FloatingDockable:
                    return DockState.FloatingDockable;
                case DockingPosition.FloatingOnly:
                    return DockState.FloatingOnly;
                default:
                    return DockState.DockedLeft;
            }

        }


        /// <summary>
        /// 停靠资源字典;
        /// </summary>
        private static readonly ResourceDictionary _dockingThemesDict = new ResourceDictionary { Source = new Uri(DictPath_Docking, UriKind.RelativeOrAbsolute) };

        private static ControlTemplate _noStyleContainerTemplate;
        private static ControlTemplate NoStyleContainerTemplate {
            get {
                if (_noStyleContainerTemplate == null) {
                    _noStyleContainerTemplate = _dockingThemesDict["NoBordernessGroupTemplate"] as ControlTemplate;
                }
                return _noStyleContainerTemplate;
            }
        }
    }
}
