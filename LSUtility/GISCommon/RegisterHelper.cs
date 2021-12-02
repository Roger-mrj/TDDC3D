using System;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using System.Windows.Forms;

namespace RCIS.GISCommon
{
    public  class VersionRegisterHelper
    {
        /// <summary>
        /// 注册版本
        /// </summary>
        /// <param name="workspace"></param>
        /// <param name="DatasetName"></param>
        public static void RegisterWorkspace(IWorkspace workspace, string DatasetName)
        {
            if (workspace.Type == esriWorkspaceType.esriRemoteDatabaseWorkspace)
            {
                IFeatureWorkspace pFWS = workspace as IFeatureWorkspace;
                IFeatureDataset pFDS = pFWS.OpenFeatureDataset(DatasetName);
                IName pName = pFDS.FullName;
                IVersionedObject3 versionedObject = pName.Open() as IVersionedObject3;
                try
                {
                    if (versionedObject != null)
                    {
                        bool IsRegistered;    bool IsMovingEditsToBase;       
                        versionedObject.GetVersionRegistrationInfo(out IsRegistered, out IsMovingEditsToBase);
                        if(IsRegistered)    
                        {        
                            if(IsMovingEditsToBase)        
                            {            
                                //first unregister without compressing edits      
                                versionedObject.UnRegisterAsVersioned3(false  );

                                //then register as fully versioned            
                                versionedObject.RegisterAsVersioned3(true   );        
                            }    
                        }    
                        else    
                        {        
                            //registering as fully versioned        
                            versionedObject.RegisterAsVersioned3(false );   
                        }
                    }
                    MessageBox.Show(DatasetName+"数据集注册完成！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("注册版本或取消版本时，请断开与该数据集的其他连接\r\n");

                }
            }
        }

        /// <summary>
        /// 取消注册版本
        /// </summary>
        /// <param name="workspace"></param>
        /// <param name="DatasetName"></param>
        public static void UnregisterWorkspace(IWorkspace workspace, string DatasetName)
        {
            if (workspace.Type == esriWorkspaceType.esriRemoteDatabaseWorkspace)
            {
                IFeatureWorkspace pFWS = workspace as IFeatureWorkspace;
                IFeatureDataset pFDS = pFWS.OpenFeatureDataset(DatasetName);
                IName pName = pFDS.FullName;
                IVersionedObject3 vo = pName.Open() as IVersionedObject3;
                try
                {

                    if (vo != null)
                    {
                        //vo.RegisterAsVersioned(false);
                        vo.UnRegisterAsVersioned3(true);
                    }
                    MessageBox.Show(DatasetName+"反注册完成！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("注册版本或取消版本时，请断开与该数据集的其他连接\r\n");

                }
            }
        }

        //public static void RegisterWorkspace(bool regOrUnReg, IWorkspace workspace,string DatasetName)
        //{


        //    if (workspace.Type == esriWorkspaceType.esriRemoteDatabaseWorkspace)
        //    {
        //        bool userConfig = true;
        //        if (!regOrUnReg)
        //        {//取消注册需要警告
        //            DialogResult dr = MessageBox.Show("警告:取消数据库版本,可能会导致版本化以后所做的数据修改丢失！\n是否继续取消版本?"
        //                , "取消版本----"+DatasetName, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        //            if (DialogResult.No == dr)
        //            {
        //                userConfig = false;
        //            }
        //            else userConfig = true;
        //        }
        //        if (!userConfig) return;
        //        IFeatureWorkspace pFWS = workspace as IFeatureWorkspace;
        //        IFeatureDataset pFDS = pFWS.OpenFeatureDataset(DatasetName);
        //        IName pName = pFDS.FullName;
        //        IVersionedObject vo = pName.Open() as IVersionedObject;
        //        try
        //        {
        //            if (vo != null)
        //            {
        //                if (regOrUnReg)
        //                {
        //                    if (!vo.IsRegisteredAsVersioned)
        //                    {
        //                        vo.RegisterAsVersioned(true);
        //                    }
        //                }
        //                else
        //                {
        //                    if (vo.IsRegisteredAsVersioned)
        //                    {
        //                        vo.RegisterAsVersioned(false);

        //                    }
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show("注册版本或取消版本时，请断开与该数据集的其他连接");

        //        }
        //    }
        //}
    }
}
