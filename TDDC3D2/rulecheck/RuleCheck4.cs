using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using RCIS.GISCommon;
using ESRI.ArcGIS.Geometry;

namespace RuleCheck
{
    public class RuleCheck4
    {
        private IWorkspace currWs = null;

        private IEnvelope currEnv = null;

        private DevExpress.XtraEditors.LabelControl lblStatus = null;

        //IFeatureClass jzdClass = null; 已取消
        IFeatureClass CJDCQJXClass = null;
        IFeatureClass CjdcqClass = null;
        IFeatureClass xzqjxClass = null;
        IFeatureClass xzqClass = null;


              
        IFeatureClass dltbClass = null;

        public RuleCheck4(IWorkspace pWs, IEnvelope _currGeo, DevExpress.XtraEditors.LabelControl _label)
        {
            this.currWs = pWs;
            this.lblStatus = _label;
            this.currEnv = _currGeo;
        }

        private  void  getAllTpClass()
        {
            IFeatureWorkspace pFeatureWorkspace = this.currWs as IFeatureWorkspace;
            
            
            try
            {
                CJDCQJXClass = pFeatureWorkspace.OpenFeatureClass("CJDCQJX");
            }
            catch { }
            try
            {
                CjdcqClass = pFeatureWorkspace.OpenFeatureClass("CJDCQ");
            }
            catch { }
            try
            {
                xzqjxClass = pFeatureWorkspace.OpenFeatureClass("XZQJX");
            }
            catch { } 
            try
            {
                xzqClass = pFeatureWorkspace.OpenFeatureClass("XZQ");
            }
            catch { }
            

            try
            {
                dltbClass = pFeatureWorkspace.OpenFeatureClass("DLTB");
            }
            catch { }

            
            
        }

        /// <summary>
        /// 针对其他独立面创建 拓扑
        /// </summary>
        /// <param name="pWs"></param>
        /// <param name="topName"></param>
        /// <param name="pDataset"></param>
        /// <returns></returns>
        private bool CreateTpElse(IWorkspace pWs, string topName, IFeatureDataset pDataset)
        {
            if (pWs == null)
                return false;

            IWorkspace2 pWS2 = pWs as IWorkspace2;
            IFeatureWorkspace pFeatureWorkspace = pWs as IFeatureWorkspace;
            if (pDataset == null)
            {
                return false;
            }
            #region
            ISchemaLock schemaLock = (ISchemaLock)pDataset;

            try
            {
                schemaLock.ChangeSchemaLock(esriSchemaLock.esriExclusiveSchemaLock);
            }
            catch (Exception ex)
            {
            }
            string[] exceptClass = new string[] { "DLTB","XZQ","PDT","CJDCQ" };
            try
            {

                ITopology topology = null;               
                topology = TopologyHelper.CreateTopology(pDataset, topName);

                IFeatureClassContainer pClassContainer = pDataset as IFeatureClassContainer;
                for (int kk = 0; kk < pClassContainer.ClassCount; kk++)
                {
                    IFeatureClass pFC = pClassContainer.get_Class(kk);
                    string fcName = (pFC as IDataset).Name.ToUpper();
                    if (exceptClass.Contains(fcName))
                    {
                        continue;
                    }
                    if (pFC.ShapeType == esriGeometryType.esriGeometryPolygon)
                    {
                        topology.AddClass(pFC, 5, 1, 1, false);
                        TopologyHelper.AddTopRule(topology, pFC, null, esriTopologyRuleType.esriTRTAreaNoOverlap, fcName + "不能重叠");
                    }
                    
                }

                if (this.currEnv == null)
                {
                    topology.ValidateTopology((pDataset as IGeoDataset).Extent);
                }
                else
                {
                    topology.ValidateTopology(this.currEnv);
                }


            }
            catch (Exception ex) { return false; }
            finally
            {
                schemaLock.ChangeSchemaLock(esriSchemaLock.esriSharedSchemaLock);
            }
            #endregion


            return true;
        }

        private bool CreateTp(IWorkspace pWs, string topName, IFeatureDataset pDataset)
        {
            if (pWs == null)
                return false;

            IWorkspace2 pWS2 = pWs as IWorkspace2;
            IFeatureWorkspace pFeatureWorkspace = pWs as IFeatureWorkspace;
            if (pDataset == null)
            {
                return false;
            }
            #region
            ISchemaLock schemaLock = (ISchemaLock)pDataset;

            try
            {
                schemaLock.ChangeSchemaLock(esriSchemaLock.esriExclusiveSchemaLock);
            }
            catch(Exception ex)
            { 
            }
            
            try
            {

                ITopology topology = null;

                ITopologyContainer topologyContainer = (ITopologyContainer)pDataset;
                //删除所有拓扑
                for (int i = topologyContainer.TopologyCount-1; i >=0 ; i--)
                {
                    topology = topologyContainer.get_Topology(i);
                    IDataset ds = topology as IDataset;
                    ds.Delete();
                }
                //重建
                this.lblStatus.Text = "开始重建拓扑...";
                this.lblStatus.Update();

                topology = TopologyHelper.CreateTopology(pDataset, topName);

                
                getAllTpClass();
                #region 添加要素类
               // topology.AddClass(jzdClass, 5, 1, 1, false);
                topology.AddClass(CJDCQJXClass, 5, 1, 1, false);
                topology.AddClass(CjdcqClass, 5, 1, 1, false);
                topology.AddClass(xzqjxClass, 5, 1, 1, false);
                topology.AddClass(xzqClass, 5, 1, 1, false);
                
                topology.AddClass(dltbClass, 5, 1, 1, false);
                #endregion 

                //添加规则

                TopologyHelper.AddTopRule(topology, CJDCQJXClass, null, esriTopologyRuleType.esriTRTLineNoSelfOverlap, "CJDCQJX不能重叠");
                TopologyHelper.AddTopRule(topology, CJDCQJXClass, null, esriTopologyRuleType.esriTRTLineNoDangles, "CJDCQJX不能有悬挂点");

                TopologyHelper.AddTopRule(topology, CjdcqClass, null, esriTopologyRuleType.esriTRTAreaNoOverlap, "CJDCQ不能重叠");
                TopologyHelper.AddTopRule(topology, xzqClass, null, esriTopologyRuleType.esriTRTAreaNoOverlap, "XZQ不能重叠");
                TopologyHelper.AddTopRule(topology, dltbClass, null, esriTopologyRuleType.esriTRTAreaNoOverlap, "DLTB不能重叠");

                TopologyHelper.AddTopRule(topology, dltbClass, null, esriTopologyRuleType.esriTRTAreaNoGaps, "DLTB不能有裂隙");

                TopologyHelper.AddTopRule(topology, xzqjxClass, null, esriTopologyRuleType.esriTRTLineNoSelfOverlap, "XZQJX不能自相交");

                               

                TopologyHelper.AddTopRule(topology, xzqjxClass, xzqClass, esriTopologyRuleType.esriTRTLineCoveredByAreaBoundary, "XZQJX必须落在XZQ边界");
                TopologyHelper.AddTopRule(topology, xzqjxClass, xzqClass, esriTopologyRuleType.esriTRTLineInsideArea, "XZQJX不能超出XZQ");


                TopologyHelper.AddTopRule(topology, CJDCQJXClass, CjdcqClass, esriTopologyRuleType.esriTRTLineCoveredByAreaBoundary, "CJDCQJX必须落在CJDCQ边界");
             
                TopologyHelper.AddTopRule(topology, dltbClass, xzqClass, esriTopologyRuleType.esriTRTAreaCoveredByArea, "DLTB必须落在XZQ内");
                TopologyHelper.AddTopRule(topology, xzqClass, dltbClass, esriTopologyRuleType.esriTRTAreaCoveredByArea, "DLTB不能覆盖XZQ");

                TopologyHelper.AddTopRule(topology, dltbClass, CjdcqClass, esriTopologyRuleType.esriTRTAreaCoveredByArea, "DLTB必须落在CJDCQ内");
                TopologyHelper.AddTopRule(topology, CjdcqClass, xzqClass, esriTopologyRuleType.esriTRTAreaCoveredByArea, "CJDCQ必须落在XZQ内");

                this.lblStatus.Text = "正在进行空间数据检查...";
                this.lblStatus.Update();

                if (this.currEnv == null)
                {
                    topology.ValidateTopology((pDataset as IGeoDataset).Extent);
                }
                else
                {
                    topology.ValidateTopology(this.currEnv);
                }
                

            }
            catch (Exception ex) { return false; }
            finally
            {
                schemaLock.ChangeSchemaLock(esriSchemaLock.esriSharedSchemaLock);
            }
            #endregion


            return true;
        }

        private void GetTopErrors(ITopology topology, string ruleId, string errorLevel)
        {

            IFeatureDataset tdghDataset = topology.FeatureDataset;  //获取topology所在数据集，为了后面根据classid找到该class
            IFeatureClassContainer tdghClassContainer = tdghDataset as IFeatureClassContainer;

            //获取拓扑错误
            List<ACheckErrorObj> lstError = new List<ACheckErrorObj>();
            IErrorFeatureContainer errorFeatureContainer = (IErrorFeatureContainer)topology;
            IGeoDataset geoDataset = (IGeoDataset)topology;
            ISpatialReference spatialReference = geoDataset.SpatialReference;

            IEnumTopologyErrorFeature enumTopologyErrorFeature = errorFeatureContainer.get_ErrorFeatures(spatialReference,
                null, geoDataset.Extent, true, false);

            int iCount = 0;
            // Get the first error feature (if any exist) and display its properties.
            ITopologyErrorFeature topologyErrorFeature = enumTopologyErrorFeature.Next();
            while (topologyErrorFeature != null)
            {
                if (topologyErrorFeature.TopologyRule == null)
                {
                    topologyErrorFeature = enumTopologyErrorFeature.Next();
                    continue;
                }
                ACheckErrorObj aError = new ACheckErrorObj();
                aError.ruleId = ruleId;
                aError.errorDescription = (topologyErrorFeature.TopologyRule as ITopologyRule).Name;
                IFeatureClass originClass = tdghClassContainer.get_ClassByID(topologyErrorFeature.OriginClassID);

                aError.errorLayer = (originClass as IDataset).Name;


                if (topologyErrorFeature.OriginOID > 0)
                {

                    IFeature oringinFea = originClass.GetFeature(topologyErrorFeature.OriginOID);
                    string sBsm = FeatureHelper.GetFeatureStringValue(oringinFea, "BSM");
                    int iBsm = -1;
                    int.TryParse(sBsm, out iBsm);

                    aError.errorType = "拓扑关系不正确";
                    aError.featureId = topologyErrorFeature.OriginOID;
                    aError.errorLevel = errorLevel;
                    aError.featureBSM = iBsm;
                    aError.featureId = oringinFea.OID;
                    lstError.Add(aError);
                    iCount++;
                    if (iCount > 300)  //最多只显示300条，分都扣完了
                        break;
                }

                topologyErrorFeature = enumTopologyErrorFeature.Next();
            }

            //插入错误明细
            foreach (ACheckErrorObj aError in lstError)
            {
                CheckErrorHelper.InsertAError(aError);
            }

        }

        private void InsertTopError(IFeatureDataset pDataset, string topName, string ruleName, string errorLevel)
        {
            if (pDataset == null)
                return;
            ITopologyContainer topContainer = pDataset as ITopologyContainer;
            try
            {
                ITopology topology = topContainer.get_TopologyByName(topName);

                if (topology == null)
                    return;
                GetTopErrors(topology, ruleName, errorLevel);
            }
            catch { }

        }


        public void Check4601()
        {
            if (this.currWs == null)
            {
                return;
            }
            this.lblStatus.Text = "开始检查碎片多边形...";
            this.lblStatus.Update();

            IFeatureWorkspace pFeaWs = this.currWs as IFeatureWorkspace;
            //检擦碎面
            //获取所有面层，
            System.Data.DataTable table = RCIS.Database.LS_SetupMDBHelper.GetDataTable("select CLASSNAME from Sys_ysdm where type='POLYGON' ","tmp");
            foreach (System.Data.DataRow aRow in table.Rows)
            {
                string aClassName = aRow["CLASSNAME"].ToString();
                IFeatureClass aClass = null;
                try
                {
                    aClass = pFeaWs.OpenFeatureClass(aClassName);
                }
                catch { }
                if (aClass == null) continue;

                string shpfld = aClass.ShapeFieldName + "_AREA";

                IQueryFilter pQF = new QueryFilterClass();
                pQF.WhereClause = shpfld + " <10 ";
                IFeatureCursor acursor = aClass.Search(pQF, false);
                try
                {
                    IFeature aErrFea = acursor.NextFeature();
                    int iCount = 1;
                    while (aErrFea != null)
                    {
                        ACheckErrorObj aError = new ACheckErrorObj();
                        aError.ruleId = "4601";
                        aError.errorDescription = "要素类" + aClass.AliasName + "的存在碎片多边形";
                        aError.errorLevel = "3";
                        aError.errorType = "碎片多边形";
                        aError.featureId = aErrFea.OID;
                        long lbsm = 0;
                        long.TryParse(FeatureHelper.GetFeatureStringValue(aErrFea, "BSM").ToString(), out lbsm);
                        aError.featureBSM = lbsm;
                        aError.errorLayer = (aClass as IDataset).Name;

                        CheckErrorHelper.InsertAError(aError);
                        iCount++;
                        if (iCount > 200)
                        {
                            break;
                        }
                        aErrFea = acursor.NextFeature();
                    }
                }
                catch { }
                finally
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(acursor);
                }

            }

          

        }


        public void Check4701()
        {
            if (this.currWs == null)
            {
                return;
            }
            this.lblStatus.Text = "开始检查碎线...";
            this.lblStatus.Update();

            IFeatureWorkspace pFeaWs = this.currWs as IFeatureWorkspace;
            //检擦碎面
            string[] polygonClass = new string[] { "CJDCQJX", "XZQJX",  };
            foreach (string aClassName in polygonClass)
            {
                IFeatureClass aClass = null;
                try
                {
                    aClass = pFeaWs.OpenFeatureClass(aClassName);
                }
                catch { }
                if (aClass == null) continue;
                IQueryFilter pQF = new QueryFilterClass();
                pQF.WhereClause = " Shape_Length < 2";
                IFeatureCursor acursor = aClass.Search(pQF, false);
                try
                {
                    IFeature aErrFea = acursor.NextFeature();
                    int iCount = 1;
                    while (aErrFea != null)
                    {
                        ACheckErrorObj aError = new ACheckErrorObj();
                        aError.ruleId = "4701";
                        aError.errorDescription = "要素类" + aClass.AliasName + "的存在碎线";
                        aError.errorLevel = "3";
                        aError.errorType = "碎线";
                        aError.featureId = aErrFea.OID;
                        long lbsm = 0;
                        long.TryParse(FeatureHelper.GetFeatureStringValue(aErrFea, "BSM").ToString(), out lbsm);
                        aError.featureBSM = lbsm;
                        aError.errorLayer = (aClass as IDataset).Name;

                        CheckErrorHelper.InsertAError(aError);
                        iCount++;
                        if (iCount > 200)
                        {
                            break;
                        }
                        aErrFea = acursor.NextFeature();
                    }
                }
                catch { }
                finally
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(acursor);
                }


            }
        }

        public void Check4101()
        {            

            //找所有要素类 和Table
            if (this.currWs == null) return;
            IFeatureDataset pFeaDataset = null;
            try
            {
                pFeaDataset = (this.currWs as IFeatureWorkspace).OpenFeatureDataset(RCIS.Global.AppParameters.DATASET_DEFAULT_NAME);
            }
            catch { }
            if (pFeaDataset == null)
            {                

                IEnumDataset pEnumDatasets = this.currWs.get_Datasets(esriDatasetType.esriDTFeatureDataset);
                IDataset pDataset = pEnumDatasets.Next();
                if (pDataset != null)
                    pFeaDataset = pDataset as IFeatureDataset;
            }

            try
            {
                //找到 这个数据集
                if (!CreateTp(this.currWs, "TP_CHECK", pFeaDataset))
                {
                    System.Windows.Forms.MessageBox.Show("创建拓扑失败或者图形检查失败，将略过图形检查!", "提示", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                }
                InsertTopError(pFeaDataset, "TP_CHECK", "4301", "2");


            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("创建拓扑失败，将略过图形检查!\r\n" + ex.Message, "提示", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
            }
            finally
            {

            }

        }

        public void Check4201()
        {
            if (this.currWs == null) return;
            IFeatureDataset pFeaDataset = null;
            try
            {
                pFeaDataset = (this.currWs as IFeatureWorkspace).OpenFeatureDataset(RCIS.Global.AppParameters.DATASET_DEFAULT_NAME);
            }
            catch { }
            if (pFeaDataset == null)
            {

                IEnumDataset pEnumDatasets = this.currWs.get_Datasets(esriDatasetType.esriDTFeatureDataset);
                IDataset pDataset = pEnumDatasets.Next();
                if (pDataset != null)
                    pFeaDataset = pDataset as IFeatureDataset;
            }

            try
            {
                //找到 这个数据集
                if (!CreateTpElse(this.currWs, "TP_CHECK2", pFeaDataset))
                {
                    System.Windows.Forms.MessageBox.Show("创建拓扑失败或者图形检查失败，将略过部分图形检查!", "提示", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                }
                InsertTopError(pFeaDataset, "TP_CHECK2", "4301", "2");


            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("创建拓扑失败，将略过独立面图形检查!\r\n" + ex.Message, "提示", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
            }
           
        }
    }
}
