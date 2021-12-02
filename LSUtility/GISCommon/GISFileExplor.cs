using System;
using System.IO;
using Microsoft.VisualBasic.FileIO;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;

namespace RCIS.GISCommon
{
    public class GISFileExplor
    {
        public GISFileExplor() { }

        /* Method :CreateTree
        * Author : Chandana Subasinghe
        * Date   : 10/03/2006
        * Discription : This is use to creat and build the tree
        *          
        */

        public bool CreateTree(TreeView treeView)
        {
            bool returnValue = false;

            try
            {
                // Create Desktop
                TreeNode desktop = new TreeNode();
                desktop.Text = "桌面";
                desktop.Tag = "Desktop";
                desktop.Nodes.Add("");
                treeView.Nodes.Add(desktop);
                // Get driveInfo
                foreach (DriveInfo drv in DriveInfo.GetDrives())
                {

                    TreeNode fChild = new TreeNode();
                    if (drv.DriveType == DriveType.CDRom)
                    {
                        fChild.ImageIndex = 1;
                        fChild.SelectedImageIndex = 1;
                    }
                    else if (drv.DriveType == DriveType.Fixed)
                    {
                        fChild.ImageIndex = 0;
                        fChild.SelectedImageIndex = 0;
                    }
                    fChild.Text = drv.Name;
                    fChild.Nodes.Add("");
                    treeView.Nodes.Add(fChild);
                    returnValue = true;
                }

            }
            catch (Exception ex)
            {
                returnValue = false;
            }
            return returnValue;

        }


        //如果该目录下的文件包含.shp文件，则作为工作空间打开，每个结点带FeatureLayer
        /* Method :EnumerateDirectory
         * Author : Chandana Subasinghe
         * Date   : 10/03/2006
         * Discription : This is use to Enumerate directories and files
         *          
         */
        public TreeNode EnumerateDirectory(TreeNode parentNode)
        {

            try
            {
                DirectoryInfo rootDir;

                // To fill Desktop
                Char[] arr = { '\\' };
                string[] nameList = parentNode.FullPath.Split(arr);
                string path = "";

                #region 桌面
                if ((nameList.GetValue(0).ToString() == "Desktop") || (nameList.GetValue(0).ToString().Trim()=="桌面"))
                {
                    path = SpecialDirectories.Desktop + "\\";

                    for (int i = 1; i < nameList.Length; i++)
                    {
                        path = path + nameList[i] + "\\";
                    }

                    rootDir = new DirectoryInfo(path);
                }
                // for other Directories
                else
                {

                    rootDir = new DirectoryInfo(parentNode.FullPath + "\\");
                }
                #endregion 

                if (parentNode.Tag is IDataset)
                {
                    #region 如果是数据集，则添加数据要素结点
                    parentNode.Nodes.Clear();

                    IDataset pDataset = parentNode.Tag as IDataset;
                    IFeatureClassContainer pClassContainer = (pDataset as IFeatureDataset) as IFeatureClassContainer;
                    for (int i = 0; i < pClassContainer.ClassCount; i++)
                    {
                        IFeatureClass currClass = pClassContainer.get_Class(i);
                        IFeatureLayer currLyr = new FeatureLayerClass();
                        currLyr.FeatureClass = currClass;
                        IDataset pDS = currClass as IDataset;
                        currLyr.Name = pDS.Name;
                        //添加结点
                        TreeNode node = new TreeNode();
                        node.Text = pDS.Name;
                        node.ImageIndex = 2;
                        node.SelectedImageIndex = 2;
                        parentNode.Nodes.Add(node);
                        node.Tag = currLyr;
                    }
                    #endregion 
                }
                else if (parentNode.Tag is string && (string)parentNode.Tag != "Desktop")
                {
                    #region 如果是mdb
                    if (parentNode.Tag.ToString().ToUpper() == "MDB")
                    {
                        rootDir = new DirectoryInfo(rootDir.FullName.Substring(0, rootDir.FullName.Length - 1));
                        parentNode.Nodes.Clear();
                        IWorkspace mdbWs = WorkspaceHelper2.GetAccessWorkspace(rootDir.FullName);
                        if (mdbWs != null)
                        {
                            IEnumDataset pEnumDS = mdbWs.get_Datasets(esriDatasetType.esriDTAny);
                            IDataset pDS = pEnumDS.Next();
                            while (pDS != null)
                            {
                                if (pDS.Type == esriDatasetType.esriDTFeatureClass)
                                {

                                    IFeatureClass currClass = pDS as IFeatureClass;
                                    IFeatureLayer currLyr = new FeatureLayerClass();
                                    currLyr.FeatureClass = currClass;                                    
                                    currLyr.Name = pDS.Name;
                                    //添加结点
                                    TreeNode node = new TreeNode();
                                    node.Text = pDS.Name;
                                    node.ImageIndex = 2;
                                    node.SelectedImageIndex = 2;
                                    parentNode.Nodes.Add(node);
                                    node.Tag = currLyr;

                                }
                                else if (pDS.Type == esriDatasetType.esriDTFeatureDataset)
                                {
                                    //添加数据集
                                    TreeNode childNode = new TreeNode();
                                    childNode.Text = pDS.Name;
                                    childNode.Tag = pDS;  //显示数据集名称的时候，附带一个当前数据集 对象
                                    childNode.Nodes.Add("");
                                    childNode.ImageIndex = 3;
                                    childNode.SelectedImageIndex = 3;
                                    parentNode.Nodes.Add(childNode);

                                    //提取出其内的FeatureClass:

                                    IFeatureDataset Feats = pDS as IFeatureDataset;
                                    IFeatureClassContainer FeatCon = (IFeatureClassContainer)Feats;
                                    IEnumFeatureClass featClasses = FeatCon.Classes;

                                    featClasses.Reset();
                                    IFeatureClass myFeatClass = featClasses.Next();
                                    while (myFeatClass != null)
                                    {
                                        
                                        IFeatureLayer currLyr = new FeatureLayerClass();
                                        currLyr.FeatureClass = myFeatClass;
                                        currLyr.Name = pDS.Name;
                                        //添加结点
                                        TreeNode aNode = new TreeNode();
                                        aNode.Text = pDS.Name;
                                        aNode.ImageIndex = 2;
                                        aNode.SelectedImageIndex = 2;
                                        childNode.Nodes.Add(aNode);
                                        aNode.Tag = currLyr;

                                        myFeatClass = featClasses.Next();
                                    } //while(...
                                }

                                pDS = pEnumDS.Next();
                            }


                            
                        }
                        return parentNode;
                    }
                    #endregion
                    else if (parentNode.Tag.ToString().ToLower() == "gdb")
                    {
                        #region  如果是gdb
                        parentNode.Nodes.Clear();
                        rootDir = new DirectoryInfo(rootDir.FullName.Substring(0, rootDir.FullName.Length - 1));
                        parentNode.Nodes.Clear();
                        IWorkspace mdbWs = WorkspaceHelper2.GetFileGdbWorkspace(rootDir.FullName);
                        if (mdbWs != null)
                        {
                            IEnumDataset pEnumDS = mdbWs.get_Datasets(esriDatasetType.esriDTAny);
                            IDataset pDS = pEnumDS.Next();
                            while (pDS != null)
                            {
                                if (pDS.Type == esriDatasetType.esriDTFeatureClass)
                                {
                                    IFeatureClass currClass = pDS as IFeatureClass;
                                    IFeatureLayer currLyr = new FeatureLayerClass();
                                    currLyr.FeatureClass = currClass;
                                    currLyr.Name = pDS.Name;
                                    //添加结点
                                    TreeNode node = new TreeNode();
                                    node.Text = pDS.Name;
                                    node.ImageIndex = 2;
                                    node.SelectedImageIndex = 2;
                                    parentNode.Nodes.Add(node);
                                    node.Tag = currLyr;


                                }
                                else if (pDS.Type == esriDatasetType.esriDTFeatureDataset)
                                {
                                    //添加数据集
                                    TreeNode childNode = new TreeNode();
                                    childNode.Text = pDS.Name;
                                    childNode.Tag = pDS;  //显示数据集名称的时候，附带一个当前数据集 对象
                                    childNode.Nodes.Add("");
                                    childNode.ImageIndex = 3;
                                    childNode.SelectedImageIndex = 3;
                                    parentNode.Nodes.Add(childNode);

                                    //提取出其内的FeatureClass:

                                    IFeatureDataset Feats = pDS as IFeatureDataset;
                                    IFeatureClassContainer FeatCon = (IFeatureClassContainer)Feats;
                                    IEnumFeatureClass featClasses = FeatCon.Classes;

                                    featClasses.Reset();
                                    IFeatureClass myFeatClass = featClasses.Next();
                                    while (myFeatClass != null)
                                    {

                                        IFeatureLayer currLyr = new FeatureLayerClass();
                                        currLyr.FeatureClass = myFeatClass;
                                        currLyr.Name = pDS.Name;
                                        //添加结点
                                        TreeNode aNode = new TreeNode();
                                        aNode.Text = pDS.Name;
                                        aNode.ImageIndex = 2;
                                        aNode.SelectedImageIndex = 2;
                                        childNode.Nodes.Add(aNode);
                                        aNode.Tag = currLyr;

                                        myFeatClass = featClasses.Next();
                                    } //while(...
                                }

                                pDS = pEnumDS.Next();
                            }

                            return parentNode;

                        }
                        #endregion
                    }

                }
                else
                {

                    //是文件夹
                    parentNode.Nodes.Clear();
                    foreach (DirectoryInfo dir in rootDir.GetDirectories())
                    {
                        TreeNode node = new TreeNode();
                        node.Text = dir.Name;
                        node.Nodes.Add("");
                        parentNode.Nodes.Add(node);
                        if (dir.Name.ToUpper().EndsWith(".GDB"))
                        {
                            node.Tag = "gdb";
                        }
                    }

                    #region 如果该文件夹下有shp文件，则将之作为一个工作空间打开

                    string[] sFiles = System.IO.Directory.GetFiles(rootDir.FullName, "*.shp");
                    if (sFiles.Length > 0)
                    {
                        IWorkspace pWS = WorkspaceHelper2.GetShapefileWorkspace(rootDir.FullName);
                        if (pWS != null)
                        {
                            IEnumDataset pEnumDS = pWS.get_Datasets(esriDatasetType.esriDTFeatureClass);
                            IFeatureClass currClass = pEnumDS.Next() as IFeatureClass;
                            while (currClass != null)
                            {
                                try
                                {
                                    IDataset pDS = currClass as IDataset;

                                    IFeatureLayer currLyr = new FeatureLayerClass();
                                    currLyr.FeatureClass = currClass;
                                    currLyr.Name = pDS.Name;
                                    //添加结点
                                    TreeNode node = new TreeNode();
                                    node.Text = pDS.Name;
                                    node.ImageIndex = 2;
                                    node.SelectedImageIndex = 2;
                                    parentNode.Nodes.Add(node);
                                    node.Tag = currLyr;
                                }
                                catch { }

                                currClass = pEnumDS.Next() as IFeatureClass;
                            }
                        }

                    }
                    #endregion 
                }
                //Fill files
                //如果是mdb结尾的                
                FileInfo[] fileInfos = rootDir.GetFiles();
                foreach (FileInfo file in fileInfos)
                {

                    //如果是mdb，也添加
                    if (file.Name.ToUpper().EndsWith(".MDB"))
                    {
                        TreeNode node = new TreeNode();
                        node.Text = file.Name;
                        node.ImageIndex = 2;
                        node.Tag = "MDB";
                        node.SelectedImageIndex = 2;
                        parentNode.Nodes.Add(node);
                        node.Nodes.Add("");
                    }
                    else if (file.Name.ToUpper().EndsWith(".IMG")
                        || file.Name.ToUpper().EndsWith(".JPG")
                        || file.Name.ToUpper().EndsWith(".BMP")
                        || file.Name.ToUpper().EndsWith(".GIF")
                        || file.Name.ToUpper().EndsWith(".TIF")
                        )
                    {
                        try
                        {
                            TreeNode node = new TreeNode();
                            node.Text = file.Name;
                            node.ImageIndex = 2;
                            node.SelectedImageIndex = 2;


                            IRasterLayer currLyr = new RasterLayerClass();
                            currLyr.CreateFromFilePath(file.FullName);
                            currLyr.Name = file.Name;
                            node.Tag = currLyr as ILayer;

                            parentNode.Nodes.Add(node);
                        }
                        catch { }
                    }

                }





            }

            catch (Exception ex)
            {
                //TODO : 
            }

            return parentNode;
        }

    }
}
