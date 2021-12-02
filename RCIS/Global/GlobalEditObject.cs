using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using System.Windows.Forms;
using ESRI.ArcGIS.Geometry;


namespace RCIS.Global
{
    public class GlobalEditObject
    {

        


        /// <summary>
        /// 当前全局工作空间，主要用来编辑
        /// </summary>
        public static  IWorkspace GlobalWorkspace = null;

        /// <summary>
        /// 当前编辑目标图层记录
        /// </summary>
        public static IFeatureLayer CurrEditTargetLayer = null;

        /// <summary>
        /// 全局编辑对象
        /// </summary>
        public static IEngineEditor CurrentEngineEditor = null;


        //撤销
        public static void MapUndoEdit()
        {
            IWorkspace pWS = GlobalEditObject.CurrentEngineEditor.EditWorkspace;
            IWorkspaceEdit pWSEdit = pWS as IWorkspaceEdit;
            if (pWSEdit == null)
                return;
            bool bHasUndos = false;
            pWSEdit.HasUndos(ref bHasUndos);
            if (bHasUndos)
            {
                try
                {
                    pWSEdit.UndoEditOperation();
                }
                catch { }
            }

        }
        
        //重做
        public static void MapRedoEdit()
        {
            IWorkspace pWS = GlobalEditObject.CurrentEngineEditor.EditWorkspace;
            IWorkspaceEdit pWSEdit = pWS as IWorkspaceEdit;
            if (pWSEdit == null)
                return;
            bool bHasRedoes = false;

            pWSEdit.HasRedos(ref bHasRedoes);
            if (bHasRedoes)
                pWSEdit.RedoEditOperation();


            
        }
        

        /// <summary>
        /// 设置目标编辑层
        /// </summary>
        /// <param name="m_mapControl"></param>
        /// <param name="m_engineEditor"></param>
        /// <param name="lyrName"></param>
        public static void SetTargetEditLayer(IMapControl2 m_mapControl, IEngineEditor m_engineEditor, string lyrName)
        {
            IMap map = m_mapControl.Map;
            if (m_engineEditor.EditState == esriEngineEditState.esriEngineStateNotEditing)
                return;

            IFeatureLayer needLayer = RCIS.GISCommon.LayerHelper.QueryLayerByModelName(map, lyrName.ToUpper());
            if (needLayer != null)
            {
                ((IEngineEditLayers)m_engineEditor).SetTargetLayer(needLayer, 0);
                GlobalEditObject.CurrEditTargetLayer = needLayer;//当前编辑图层
            }

            //for (int layerCounter = 0; layerCounter <= map.LayerCount - 1; layerCounter++)
            //{
            //    ILayer currentLayer = map.get_Layer(layerCounter);
                
            //    if (!(currentLayer is IFeatureLayer))
            //        continue;

            //    IFeatureLayer featureLayer = currentLayer as IFeatureLayer;
            //    IFeatureClass featureClass = featureLayer.FeatureClass;
            //    IDataset dataset = featureClass as IDataset;
            //    if (dataset.Name.Trim().ToUpper().Equals(lyrName.ToUpper()))
            //    {                   
            //        ((IEngineEditLayers)m_engineEditor).SetTargetLayer(featureLayer, 0);

            //        GlobalEditObject.CurrEditTargetLayer = featureLayer;//当前编辑图层
            //        break;
            //    }


            //}

        }

        /// <summary>
        /// 开始编辑
        /// </summary>
        /// <param name="m_mapControl"></param>
        /// <param name="m_engineEditor"></param>
        public static void StartEditingObj(IMapControl2 m_mapControl, IEngineEditor m_engineEditor)
        {

            if (GlobalEditObject.CurrEditTargetLayer == null)
            {
                MessageBox.Show("还没有指定当前编辑图层对象1", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            IMap map = m_mapControl.ActiveView.FocusMap;
            //If an edit session has already been started, exit.
            if (m_engineEditor.EditState != esriEngineEditState.esriEngineStateNotEditing)
                return;
            m_engineEditor.EditSessionMode = esriEngineEditSessionMode.esriEngineEditSessionModeVersioned;

            IFeatureLayer featureLayer = GlobalEditObject.CurrEditTargetLayer;
            IFeatureClass featureClass = featureLayer.FeatureClass;
            IDataset dataset = featureClass as IDataset;
            IWorkspace workspace = dataset.Workspace;
            m_engineEditor.StartEditing(workspace, map);
            ((IEngineEditLayers)m_engineEditor).SetTargetLayer(featureLayer, 0);

            
        }



        /// <summary>
        /// 开始编辑
        /// </summary>
        /// <param name="m_mapControl"></param>
        /// <param name="m_engineEditor"></param>
        /// <param name="lyrName"></param>
        public static void StartEditingObj(IMapControl2 m_mapControl, IEngineEditor m_engineEditor,string lyrName)
        {
            if (lyrName.Trim() == "")
                return;

            IMap map = m_mapControl.Map;

            //If an edit session has already been started, exit.
            if (m_engineEditor.EditState != esriEngineEditState.esriEngineStateNotEditing)
                return;

            IFeatureLayer currLayer = RCIS.GISCommon.LayerHelper.QueryLayerByModelName(map, lyrName.ToUpper());
            if (currLayer != null)
            {
                IFeatureClass featureClass = currLayer.FeatureClass;
                IDataset dataset = featureClass as IDataset;
                IWorkspace workspace = dataset.Workspace;

                m_engineEditor.StartEditing(workspace, map);
                ((IEngineEditLayers)m_engineEditor).SetTargetLayer(currLayer, 0);
                GlobalEditObject.CurrEditTargetLayer = currLayer;//当前编辑图层
            }

           
            
            
        }

        /// <summary>
        /// 停止编辑
        /// </summary>
        /// <param name="m_EngineEditor"></param>
        public static void StopEditingObj(IEngineEditor m_EngineEditor)
        {

            //关闭捕捉
            IEngineSnapEnvironment snapEnvironment = m_EngineEditor as IEngineSnapEnvironment;
            for (int i = snapEnvironment.SnapAgentCount-1; i >= 0; i--)
            {
                snapEnvironment.RemoveSnapAgent(i);
            }


            if (m_EngineEditor.HasEdits() == false)
                m_EngineEditor.StopEditing(false);
            else
            {
                if (MessageBox.Show("您要保存当前修改么?", "保存提示", MessageBoxButtons.YesNo,MessageBoxIcon.Question)
                    == DialogResult.Yes)
                    m_EngineEditor.StopEditing(true);
                else
                    m_EngineEditor.StopEditing(false);
            }

            //System.Runtime.InteropServices.Marshal.ReleaseComObject(GlobalEditObject.CurrEditTargetLayer);
            //GlobalEditObject.CurrEditTargetLayer = null;
         
           

        }

        /// <summary>
        /// 打开捕捉
        /// </summary>
        /// <param name="m_EngineEditor"></param>
        public static void OpenSnaping(IEngineEditor m_EngineEditor)
        {
            IEngineFeatureSnapAgent featureSnapAgent = new EngineFeatureSnap();
            IEngineEditLayers editLayers = m_EngineEditor as IEngineEditLayers;
            IEngineSnapEnvironment snapEnvironment = m_EngineEditor as IEngineSnapEnvironment;
            if (editLayers.TargetLayer == null)
            {
                System.Windows.Forms.MessageBox.Show("请首先打开编辑开关！");
                return;
            }

            featureSnapAgent.FeatureClass = editLayers.TargetLayer.FeatureClass; ;
            featureSnapAgent.HitType = esriGeometryHitPartType.esriGeometryPartVertex | esriGeometryHitPartType.esriGeometryPartBoundary | esriGeometryHitPartType.esriGeometryPartEndpoint;
            snapEnvironment.AddSnapAgent(featureSnapAgent); 

            //打开捕捉提示

            ((IEngineEditProperties2)m_EngineEditor).SnapTips = true;
        }

        public static void CloseSnaping(IEngineEditor m_EngineEditor)
        {


            IEngineSnapEnvironment snapEnvironment = m_EngineEditor as IEngineSnapEnvironment;
            IEngineEditLayers editLayers = m_EngineEditor as IEngineEditLayers;
            int idx = -1;
            for (int i = 0; i < snapEnvironment.SnapAgentCount; i++)
            {
                IEngineFeatureSnapAgent featureSnapAgent = snapEnvironment.get_SnapAgent(i) as IEngineFeatureSnapAgent;
                if (featureSnapAgent.FeatureClass.Equals( editLayers.TargetLayer.FeatureClass  ))
                {
                    idx = i;
                    break;
                }
            }
            if (idx > -1)
            {
                snapEnvironment.RemoveSnapAgent(idx);
            }
        }

        /// <summary>
        /// 获取当前编辑图层
        /// </summary>
        /// <param name="m_EngineEditor"></param>
        /// <returns></returns>
        public static IFeatureLayer GetCurrentEditLayer(IEngineEditor m_EngineEditor)
        {
            IEngineEditLayers editLayers = m_EngineEditor as IEngineEditLayers;
            return editLayers.TargetLayer;
        }



        /// <summary>
        /// 改变当前编辑任务
        /// </summary>
        /// <param name="m_EngineEditor"></param>
        /// <param name="taskType"></param>
        public static void ChangeEditTask(IEngineEditor m_EngineEditor, string  taskName)
        {
            if (m_EngineEditor.EditState == esriEngineEditState.esriEngineStateEditing)
            {
                for (int i = 0; i < m_EngineEditor.TaskCount; i++)
                {
                    IEngineEditTask task = m_EngineEditor.get_Task(i);
                    string name = task.UniqueName;
                    if (taskName.ToUpper().Equals(name.ToUpper()))
                    {
                        m_EngineEditor.CurrentTask = task;
                        break;
                    }
                }
            }
        }
    }

   
}
