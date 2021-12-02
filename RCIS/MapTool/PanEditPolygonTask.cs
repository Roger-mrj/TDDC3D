using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ADF.CATIDs;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using System.Windows.Forms;


namespace MYEngineEditTasks
{
    [Guid("9d213ccc-99af-4218-aa33-444b278ad676")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("MYEngineEditTasks.PanEditPolygonTask")]
    public class PanEditPolygonTask : ESRI.ArcGIS.Controls.IEngineEditTask
    {

        #region Private Members
        IEngineEditor m_engineEditor;
        IEngineEditSketch m_editSketch;
        IEngineEditLayers m_editLayer;
        #endregion



        #region COM Registration Function(s)
        [ComRegisterFunction()]
        [ComVisible(false)]
        static void RegisterFunction(Type registerType)
        {
            // Required for ArcGIS Component Category Registrar support
            ArcGISCategoryRegistration(registerType);

            //
            // TODO: Add any COM registration code here
            //
        }

        [ComUnregisterFunction()]
        [ComVisible(false)]
        static void UnregisterFunction(Type registerType)
        {
            // Required for ArcGIS Component Category Registrar support
            ArcGISCategoryUnregistration(registerType);

            //
            // TODO: Add any COM unregistration code here
            //
        }

        #region ArcGIS Component Category Registrar generated code
        /// <summary>
        /// Required method for ArcGIS Component Category registration -
        /// Do not modify the contents of this method with the code editor.
        /// </summary>
        private static void ArcGISCategoryRegistration(Type registerType)
        {
            string regKey = string.Format("HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID);
            EngineEditTasks.Register(regKey);

        }
        /// <summary>
        /// Required method for ArcGIS Component Category unregistration -
        /// Do not modify the contents of this method with the code editor.
        /// </summary>
        private static void ArcGISCategoryUnregistration(Type registerType)
        {
            string regKey = string.Format("HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID);
            EngineEditTasks.Unregister(regKey);

        }

        #endregion
        #endregion


        IEngineEditTask m_oldTask;


        #region "IEngineEditTask Implementations"
        public void Activate(ESRI.ArcGIS.Controls.IEngineEditor editor, ESRI.ArcGIS.Controls.IEngineEditTask oldTask)
        {
            // TODO: Add PanEditPolygonTask.Activate implementation
            if (editor == null)
                return;

            this.m_oldTask = oldTask;
            //Initialize class member variables.
            m_engineEditor = editor;
            m_editSketch = editor as IEngineEditSketch;
            m_editSketch.GeometryType = esriGeometryType.esriGeometryPolyline;
            m_editLayer = m_editSketch as IEngineEditLayers;

            //Wire editor events.        
            ((IEngineEditEvents_Event)m_editSketch).OnTargetLayerChanged += new IEngineEditEvents_OnTargetLayerChangedEventHandler(OnTargetLayerChanged);
            ((IEngineEditEvents_Event)m_editSketch).OnCurrentTaskChanged += new IEngineEditEvents_OnCurrentTaskChangedEventHandler(OnCurrentTaskChanged);
        }

        public void Deactivate()
        {
            // TODO: Add PanEditPolygonTask.Deactivate implementation
            ((IEngineEditEvents_Event)m_engineEditor).OnTargetLayerChanged -= OnTargetLayerChanged;
            ((IEngineEditEvents_Event)m_engineEditor).OnCurrentTaskChanged -= OnCurrentTaskChanged;

            //Release object references.
            m_engineEditor = null;
            m_editSketch = null;
            m_editLayer = null;
        }

        public void OnCurrentTaskChanged()
        {
            UpdateSketchToolStatus();
        }

        public void OnTargetLayerChanged()
        {
            UpdateSketchToolStatus();
        }

        public string GroupName
        {
            get
            {
                // TODO: Add PanEditPolygonTask.GroupName getter implementation
                return "Modify Tasks";
            }
        }

        public string Name
        {
            get
            {
                // TODO: Add PanEditPolygonTask.Name getter implementation
                return default(string);
            }
        }

        public void OnDeleteSketch()
        {
            // TODO: Add PanEditPolygonTask.OnDeleteSketch implementation
        }

        public void OnFinishSketch()
        {
            // TODO: Add PanEditPolygonTask.OnFinishSketch implementation
            if (m_editSketch == null)
                return;

            if (m_engineEditor.SelectionCount < 1)
            {
                System.Windows.Forms.MessageBox.Show("请首先选中要平移的要素!", "提示", System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Warning);
                return;
            }
           

            

            IGeometry cutGeometry = m_editSketch.Geometry;
            IPolyline pPolyline=cutGeometry as IPolyline;
            if (pPolyline == null)
            {
                System.Windows.Forms.MessageBox.Show("请绘制一条平移直线!", "提示", System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Warning);
                return;
            }


            

            this.m_engineEditor.StartOperation();

            IPoint startPt = pPolyline.FromPoint;
            IPoint endPt = pPolyline.ToPoint;

            ILine cutLine = new LineClass();
            cutLine.PutCoords(startPt, endPt);
            ISet moveset = new SetClass();
            //改一下  看看

            IEnumFeature enumFeature=m_engineEditor.EditSelection;

            IFeature editFeature = enumFeature.Next();
            while (editFeature != null)
            {
                moveset.Add(editFeature);
                editFeature = enumFeature.Next();
            }
            System.Windows.Forms.Cursor.Current = Cursors.WaitCursor;
           // m_engineEditor.StartOperation();
            try
            {
                enumFeature.Reset();
                editFeature = enumFeature.Next();

                IFeatureEdit pFeaEdit = editFeature as IFeatureEdit;                
                pFeaEdit.MoveSet(moveset, cutLine);
              //  m_engineEditor.StopOperation("OK");
                MessageBox.Show("平移成功!", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                IActiveView activeView = this.m_engineEditor.Map as IActiveView;
                activeView.PartialRefresh(esriViewDrawPhase.esriViewGeography|esriViewDrawPhase.esriViewGeoSelection, null , activeView.Extent);
                

            }
            catch (Exception ex)
            {
                MessageBox.Show("未能成功完成任务.\n" + ex.Message);
              //  m_engineEditor.AbortOperation();
            }
            finally
            {
                System.Windows.Forms.Cursor.Current = Cursors.Default;
                this.m_engineEditor.StopOperation("panfeature");
                ResetTask();
            }
          

        }

        private void ResetTask()
        {
            //ControlToolsEditing_CreateNewFeatureTask
            if (this.m_engineEditor.EditState == esriEngineEditState.esriEngineStateEditing)
            {
                for (int i = 0; i < m_engineEditor.TaskCount; i++)
                {
                    IEngineEditTask task = m_engineEditor.get_Task(i);
                    string name = task.UniqueName;
                    if (name.ToUpper().Trim().Equals("ControlToolsEditing_CreateNewFeatureTask".ToUpper()))
                    {
                        m_engineEditor.CurrentTask = task;
                        break;
                    }
                }
            }
        }

        public string UniqueName
        {
            get
            {
                // TODO: Add PanEditPolygonTask.UniqueName getter implementation
                return "EngineEditTasksPanEditPolygon";
            }
        }
        #endregion

        #region IEngineEditTask private methods
        private void UpdateSketchToolStatus()
        {
            if (m_editLayer == null)
                return;

            ////Only enable the sketch tool if there is a polygon target layer.
            //if (m_editLayer.TargetLayer.FeatureClass.ShapeType != esriGeometryType.esriGeometryPolygon)
            //    m_editSketch.GeometryType = esriGeometryType.esriGeometryNull;
            //else
            //    //Set the edit sketch geometry type to be esriGeometryPolyline.
            //    m_editSketch.GeometryType = esriGeometryType.esriGeometryPolyline;
        }
        #endregion

    }
}
