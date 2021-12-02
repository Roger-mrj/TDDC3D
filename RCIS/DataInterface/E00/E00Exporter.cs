using System;
using System.IO ;
using System.Text ;
using System.Collections ;
using RCIS.DataExchange;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;

namespace RCIS.DataExchange.E00
{
	/// <summary>
	/// E00Exporter 的摘要说明。
	/// </summary>
	public class E00Exporter:Exporter
	{
        private String m_e00FilePath;
        private String m_shapeFolder;

        private IWorkspace m_workspace;
        private StreamReader m_reader;
        private ArrayList    m_arcList;
        private ArrayList    m_polyList;
        private ArrayList    m_labList;
        private AttributeTable m_attrTable;
        private int          m_lineNO=0;
		public E00Exporter()
		{
			
        }
        #region 外部参数
        public String E00FilePath
        {
            get
            {
                return this.m_e00FilePath ;
            }
            set
            {
                this.m_e00FilePath =value;
            }
        }
        public String ShapefileFolder
        {
            get
            {
                return this.m_shapeFolder ;
            }
            set
            {
                this.m_shapeFolder =value;
            }
        }
        #endregion
        #region 辅助方法
        private IFeatureWorkspace FeatureWorkspace
        {
            get
            {
                return this.m_workspace as IFeatureWorkspace ;
            }
        }
        private IWorkspaceEdit WorkspaceEdit
        {
            get
            {
                return this.m_workspace as IWorkspaceEdit ;
            }
        }
        private AttributeTable AttributeTable
        {
            get
            {
                return this.m_attrTable ;
            }
        }
        private esriGeometryType GeometryType
        {//根据数据量来确定几何图形的类型
            get
            {
                if(this.m_polyList .Count >0)return esriGeometryType.esriGeometryPolygon;
                if(this.m_arcList .Count >0)return esriGeometryType.esriGeometryPolyline;
                return esriGeometryType.esriGeometryPoint;
            }
        }
        private String ReadRawLine()
        {
            String aLine=this.m_reader .ReadLine ();
            this.m_lineNO ++;
            return aLine;
        }
        private String ReadLine()
        {
            String aLine=this.m_reader .ReadLine();
            this.m_lineNO ++;
            while(aLine!=null&&aLine.Trim ().Equals (""))
            {
                aLine=this.m_reader .ReadLine ();
                this.m_lineNO ++;
            }
            return aLine;
        }    
        private int ParseInt(String aLine,int pDefInt)
        {
            int result=pDefInt;
            try
            {
                result=Convert.ToInt32 (aLine);
            }
            catch
            {
            }
            return result;
        }
        
        private IPoint ParsePoint(String aLine)
        {
            IPoint rPt=null;
            try
            {
                aLine=this.Compress (aLine);
                String[] aTextAry=aLine.Split (" ".ToCharArray ());
                String dx=aTextAry[0];
                String dy=aTextAry[1];
                double px=Double.NaN ;
                double py=Double.NaN ;
                bool hasX=Double.TryParse (dx.Trim (),System.Globalization .NumberStyles .Any 
                    ,new System.Globalization .NumberFormatInfo (),out px);
                bool hasY=Double.TryParse (dy.Trim (),System.Globalization .NumberStyles .Any 
                    ,new System.Globalization .NumberFormatInfo (),out py);
                if(hasX&&hasY)
                {
                    rPt=new PointClass();
                    rPt.PutCoords (px,py);
                }
            }
            catch
            {
            }
            return rPt;
        }
        private ISegment ParseLine(String aLine)
        {
            ILine rLine=null;
            try
            {
                double px=Double.NaN ;double py=Double.NaN ;
                bool hasX=false;bool hasY=false;
                IPoint fromPt=null;IPoint toPt=null;
                aLine=this.Compress (aLine);
                String[] aTextAry=aLine.Split (" ".ToCharArray ());
                String aFromDx=aTextAry[0].Trim ();
                String aFromDy=aTextAry[1].Trim ();
                String aToDx=aTextAry[2].Trim ();
                String aToDy=aTextAry[3].Trim ();
                
                hasX=Double.TryParse (aFromDx,System.Globalization .NumberStyles .Any 
                    ,new System.Globalization .NumberFormatInfo (),out px);
                hasY=Double.TryParse (aFromDy,System.Globalization .NumberStyles .Any 
                    ,new System.Globalization .NumberFormatInfo (),out py);
                if(hasX&&hasY)
                {
                    fromPt=new PointClass();
                    fromPt.PutCoords (px,py);
                }
                hasX=Double.TryParse (aToDx,System.Globalization .NumberStyles .Any 
                    ,new System.Globalization .NumberFormatInfo (),out px);
                hasY=Double.TryParse (aToDy,System.Globalization .NumberStyles .Any 
                    ,new System.Globalization .NumberFormatInfo (),out py);
                if(hasX&&hasY)
                {
                    toPt=new PointClass();
                    toPt.PutCoords (px,py);
                }
                if(fromPt!=null&&toPt!=null)
                {
                    rLine=new LineClass ();
                    rLine.FromPoint =fromPt;
                    rLine.ToPoint =toPt;
                }
            }
            catch
            {
            }
            return rLine as ISegment;
        }
        
        private String Compress(String aLine)
        {
            aLine=aLine.Trim ();
            StringBuilder fromBuilder=new StringBuilder (aLine);
            StringBuilder toBuilder=new StringBuilder ();
            bool hasSpace=false;
            int cCount=fromBuilder.Length;
            for(int ci=0;ci<cCount ;ci++)
            {
                char aChar=fromBuilder[ci];
                if(aChar.Equals (' ')
                    ||aChar.Equals ('\t'))
                {
                    hasSpace=true;
                }
                else 
                {
                    if(hasSpace)
                    {
                        toBuilder.Append (" ").Append (aChar);
                        hasSpace=false;
                    }
                    else
                    {
                        toBuilder.Append (aChar);
                    }
                }
            }
            String rLine=toBuilder.ToString ();
            return rLine;
        }
        private String Compact(String aLine)
        {
            aLine=aLine.Trim ();
            StringBuilder fromBuilder=new StringBuilder (aLine);
            StringBuilder toBuilder=new StringBuilder ();            
            int cCount=fromBuilder.Length;
            for(int ci=0;ci<cCount ;ci++)
            {
                char aChar=fromBuilder[ci];
                if(aChar.Equals (' ')
                    ||aChar.Equals ('\t'))
                {
                    //什么也不做
                }
                else 
                {
                 toBuilder.Append (aChar);
                }
            }
            String rLine=toBuilder.ToString ();
            return rLine;
        }
        private AttributeTable ParseTableHead(String aLine)
        {
            try
            {
                String aTableName=aLine.Substring (0,32).Trim ();
                String aFieldCountStr=aLine.Substring (34,4).Trim ();
                String aFieldCountTotalStr=aLine.Substring (38,4).Trim ();
                String aBlockSizeStr=aLine.Substring (42,4).Trim ();
                String aRecordCountStr=aLine.Substring (46,10).Trim ();
                int aFieldCount=Convert.ToInt32 (aFieldCountStr);
                int aBlockSize=Convert.ToInt32 (aBlockSizeStr);
                int aRecordCount=Convert.ToInt32 (aRecordCountStr);
                AttributeTable aTs=new AttributeTable (aTableName,aFieldCount,aBlockSize,aRecordCount);
                return aTs;
            }
            catch
            {
            }
            return null;
        }
        private TableField ParseTableField(String aLine)
        {
            try
            {
                String aFieldName=aLine.Substring (0,16).Trim ();
                String aBinarySize=aLine.Substring (16,3).Trim ();
                String aWidthStr=aLine.Substring (28,4).Trim ();
                String aScaleStr=aLine.Substring (32,2).Trim ();
                String aFtStr=aLine.Substring (34,3).Trim ();
                esriFieldType aFt=esriFieldType.esriFieldTypeString;
                if(aFtStr.Equals ("10"))aFt=esriFieldType.esriFieldTypeDate;
                else if(aFtStr.Equals ("20"))aFt=esriFieldType.esriFieldTypeString;
                else if(aFtStr.Equals ("30"))aFt=esriFieldType.esriFieldTypeString;
                else if(aFtStr.Equals ("40"))aFt=esriFieldType.esriFieldTypeSingle;
                else if(aFtStr.Equals ("50"))
                {
                    if(aBinarySize.Equals ("4"))aFt=esriFieldType.esriFieldTypeInteger;
                    else aFt=esriFieldType.esriFieldTypeSmallInteger;
                }
                else if(aFtStr.Equals ("60"))
                {
                    if(aBinarySize.Equals ("4"))aFt=esriFieldType.esriFieldTypeSingle;
                    else aFt=esriFieldType.esriFieldTypeDouble;
                }
                int aWidth=Convert.ToInt32 (aWidthStr);
                int aScale=Convert.ToInt32 (aScaleStr);
                TableField aField=new TableField (aFieldName,aFt,aWidth,aScale);
                return aField;
            }
            catch
            {
            }
            return null;
        }
        private int GetFormatWidth(esriFieldType pFT,int pFieldWidth)
        {
            if(esriFieldType.esriFieldTypeDate==pFT)
            {
                return 8;
            }
            else if(esriFieldType.esriFieldTypeString==pFT)
            {
                return pFieldWidth;
            }
            else if(esriFieldType.esriFieldTypeSingle==pFT)
            {
                return 14;
            }
            else if(esriFieldType.esriFieldTypeDouble==pFT)
            {
                return 24;
            }
            else if(esriFieldType.esriFieldTypeSmallInteger==pFT)
            {
                return 6;
            }
            else if(esriFieldType.esriFieldTypeInteger==pFT)
            {
                return 11;
            }
            return pFieldWidth;
        }
        #endregion



        #region Exporter 成员        
        public bool BeginExport()
        {
            try
            {
                if(!File.Exists (this.E00FilePath))return false;
                if(!Directory.Exists (this.ShapefileFolder ))return false;
                Encoding gb2312=null;
                try
                {
                    gb2312=Encoding.GetEncoding ("GB2312");
                }
                catch
                {
                    gb2312=Encoding.Default ;
                }
                this.m_reader =new StreamReader(this.E00FilePath ,gb2312);
                ShapefileWorkspaceFactoryClass factory=new ShapefileWorkspaceFactoryClass ();
                this.m_workspace =factory.OpenFromFile(this.ShapefileFolder,0);
                this.m_arcList =new ArrayList ();
                this.m_polyList =new ArrayList ();
                this.m_labList =new ArrayList ();
                this.m_lineNO =0;
                this.m_attrTable =null;
                return true;
            }
            catch
            {
            }
            return false;
        }

        public void FinishExport()
        {
            // TODO:  添加 E00Exporter.FinishExport 实现
        }

        public void ExportToShapefile(string pSrcName, string pDestName, bool pSchemaOnly)
        {
            String aLine=this.ReadLine ();
            while(aLine!=null)
            {
                String aUpperLine=aLine.ToUpper ().Trim ();
                #region 读取数据
                if(aUpperLine.StartsWith ("ARC "))
                {
                    //弧
                    //判断精度
                    aUpperLine=this.Compress (aUpperLine);
                    string aScale=aUpperLine.Split (" ".ToCharArray ())[1];
                    
                    #region 读取Arc
                    aLine=this.ReadLine ();
                    while(aLine!=null)
                    {           
                        String aCompactLine=this.Compress(aLine);
                        if(aCompactLine.Equals ("-1 0 0 0 0 0 0"))break;                        
                        #region 读取一个ArcObject
                        aLine=this.Compress (aLine);
                        String[] aTextAry=aLine.Split (" ".ToCharArray ());
                        int aTextLen=aTextAry.Length ;
                        String aArcIDStr=aTextAry[0].Trim ();
                        String aFromNodeStr=aTextAry[2].Trim ();
                        String aToNodeStr=aTextAry[3].Trim ();
                        String aPtCountStr=aTextAry[6].Trim ();
                        int aArcID=this.ParseInt (aArcIDStr,-1);
                        int aFromNode=this.ParseInt (aFromNodeStr,-1);
                        int aToNode=this.ParseInt (aToNodeStr,-1);
                        int aPtCount=this.ParseInt (aPtCountStr,-1);
                        IPolyline pline=new PolylineClass ();
                        IPointCollection ptCol=pline as IPointCollection ;
                        if(aScale.Equals ("2"))
                        {
                            
                            int aLineCount=aPtCount/2;
                            for(int li=0;li<aLineCount;li++)
                            {
                                aLine=this.ReadLine();
                                ISegment aSeg=this.ParseLine(aLine);
                                if(aSeg!=null)
                                {
                                    IPoint fromPt=aSeg.FromPoint ;
                                    IPoint toPt=aSeg.ToPoint ;
                                    ptCol.AddPoints (1,ref fromPt);
                                    ptCol.AddPoints (1,ref toPt);
                                }
                            }
                            if(aPtCount%2!=0)
                            {
                                aLine=this.ReadLine ();
                                IPoint aPt=this.ParsePoint (aLine);
                                ptCol.AddPoints (1,ref aPt);
                            }
                        }
                        else
                        {
                            for(int li=0;li<aPtCount;li++)
                            {
                                aLine=this.ReadLine ();
                                IPoint aPt=this.ParsePoint (aLine);
                                ptCol.AddPoints (1,ref aPt);
                            }
                        }
                        (pline as ITopologicalOperator ).Simplify ();
                        ArcObject aArcObj=new ArcObject (aArcID,aFromNode,aToNode,pline);
                        this.m_arcList .Add (aArcObj);
                        #endregion
                        aLine=this.ReadLine ();
                    }
                    #endregion
                }
                else if(aUpperLine.StartsWith ("LAB "))
                {//点
                    #region 读取点
                    String escape="";
                    aLine=this.Compress(aLine);
                   
                    String[] aTextAry=aLine.Split (" ".ToCharArray ());
                    String dataScale=aTextAry[1];
                    if(dataScale.Equals ("2"))
                    {
                        escape="-1 0 0.0000000E+00 0.0000000E+00";
                    }
                    else
                    {
                        escape="-1 0 0.00000000000000E+00 0.00000000000000E+00";
                    }
                    aLine=this.ReadLine ();
                    while(aLine!=null)
                    {
                        String aCompactLine=this.Compress (aLine);
                        if(aCompactLine.ToUpper ().Equals(escape))break;
                        #region 读取一个点
                        aLine=this.Compress (aLine);
                        Console.WriteLine(this.m_lineNO +"=="+aLine);
                        aTextAry=aLine.Split (" ".ToCharArray ());
                        String aLabIDStr=aTextAry[1];
                        String dx=aTextAry[2];
                        String dy=aTextAry[3];
                        try
                        {
                            int aLabID=Convert.ToInt32 (aLabIDStr);
                            double px=double.NaN ;double py=double.NaN ;
                            bool hasX=Double.TryParse (dx,System.Globalization .NumberStyles .Any 
                                ,new System.Globalization .NumberFormatInfo (),out px);
                            bool hasY=Double.TryParse (dy,System.Globalization .NumberStyles .Any 
                                ,new System.Globalization .NumberFormatInfo (),out py);
                            if(hasX&&hasY)
                            {
                                IPoint aPt=new PointClass();
                                aPt.PutCoords (px,py);
                                LabObject aLabObj=new LabObject (aLabID,aPt);
                                this.m_labList .Add (aLabObj);
                            }
                        }
                        catch
                        {
                        }
                        //读取冗余的一行
                        aLine=this.ReadLine ();
                        if(dataScale.Equals ("3"))
                        {//多一行冗余的。
                            aLine=this.ReadLine ();
                        }
                        #endregion
                        aLine=this.ReadLine ();
                    }
                    #endregion
                }
                else if(aUpperLine.StartsWith ("PAL "))
                {//面拓扑
                    #region 读取一个面
                    aUpperLine=this.Compress (aUpperLine);
                    String[] aTextAry=aUpperLine.Split (" ".ToCharArray ());                   
                    String dataScale=aTextAry[1];
                    aLine=this.ReadLine ();
                    while(aLine!=null)
                    {
                        #region 读取一个PolyObject
                        
                        String aCompactLine=this.Compress(aLine);
                        if(aCompactLine.Equals ("-1 0 0 0 0 0 0"))break;
                        PolyObject polyObj=new PolyObject();
                        aLine=this.Compress (aLine);
                        aTextAry=aLine.Split (" ".ToCharArray ());
                        String aArcCountStr=aTextAry[0];//弧段的数目
                        int aArcCount=0;
                        try
                        {
                            aArcCount=Convert.ToInt32 (aArcCountStr);
                        }
                        catch
                        {
                        }
                        if(dataScale.Equals ("3"))
                        {//多余的一行
                            aLine=this.ReadLine ();
                        }
                        int aLineCount=aArcCount/2;
                        for(int li=0;li<aLineCount;li++)
                        {
                            #region 读取Arc数据
                            aLine=this.ReadLine ();
                            aLine=this.Compress (aLine);
                            aTextAry=aLine.Split (" ".ToCharArray ());
                            int aArcID=0;
                            try
                            {
                                aArcID=Convert.ToInt32 (aTextAry[0]);
                                polyObj.AddArc (aArcID);
                            }
                            catch
                            {
                            }
                            try
                            {
                                aArcID=Convert.ToInt32 (aTextAry[3]);
                                polyObj.AddArc (aArcID);
                            }
                            catch
                            {
                            }   
                            #endregion
                        }
                        if(aArcCount%2!=0)
                        {
                            #region 读取最后一个孤立的弧
                            aLine=this.ReadLine ();
                            aLine=this.Compress (aLine);
                            aTextAry=aLine.Split (" ".ToCharArray ());
                            try
                            {
                                int aArcID=Convert.ToInt32 (aTextAry[0]);
                                polyObj.AddArc (aArcID);
                            }
                            catch
                            {
                            }
                            #endregion
                        }
                        this.m_polyList .Add (polyObj);
                        #endregion
                        aLine=this.ReadLine ();
                    }   
                    if(dataScale.Equals ("3"))
                    {//多一行冗余
                        aLine=this.ReadLine ();
                    }
                    #endregion
                }
                else if(aUpperLine.StartsWith ("PRJ "))
                {//投影参数
                    aLine=this.ReadLine();
                    while(aLine!=null)
                    {
                        if(aLine.ToUpper ().Equals ("EOP"))break;
                        aLine=this.ReadLine ();
                    }
                }
                else if(aUpperLine.StartsWith ("IFO "))
                {//属性表
                    aLine=this.ReadLine ();
                    while(aLine!=null)
                    {
                        String aPadLine=aLine.PadRight (80,' ').ToUpper ();                        
                        String aTableName=aPadLine.Substring (0,32).Trim();                        
                        if(aTableName.EndsWith (".PAT")
                            ||(aTableName.EndsWith (".AAT")
                            &&this.m_attrTable ==null))
                        {//PAT可以覆盖AAT.但是AAT不可以覆盖PAT表
                            #region 读取表结构
                            this.m_attrTable=this.ParseTableHead (aLine);
                            int fCount=this.m_attrTable.FieldCount ;
                            int aRecordSize=0;
                            for(int fi=0;fi<fCount;fi++)
                            {
                                aLine=this.ReadLine ();
                                TableField aField=this.ParseTableField(aLine);
                                if(aField!=null)
                                {
                                    this.m_attrTable.AppendField (aField);
                                    aRecordSize+=this.GetFormatWidth(aField.FieldType,aField.FieldLength);
                                }
                            }
                            #endregion
                            #region 开始读取数据
                            for(int ii=0;ii<this.m_attrTable.RecordCount;ii++)
                            {
                                int aLineCount=aRecordSize/80;
                                if(aRecordSize%80!=0)aLineCount+=1;
                                StringBuilder aBuilder=new StringBuilder ();
                                for(int li=0;li<aLineCount;li++)
                                {
                                    try
                                    {
                                        aLine=this.ReadRawLine ();
                                        aLine=aLine.PadRight (80,' ');
                                        aBuilder.Append (aLine);
                                    }
                                    catch(Exception ex)
                                    {
                                        System.Windows .Forms .MessageBox .Show (this.m_lineNO +"\n"+ex.StackTrace );
                                    }
                                }
                                String aRecord=aBuilder.ToString ();
                                ArrayList aItemList=new ArrayList ();
                                int aStartIndex=0;
                                for(int fi=0;fi<fCount;fi++)
                                {
                                    try
                                    {
                                        TableField aField=this.m_attrTable.GetTableField(fi);
                                        int aWidth=this.GetFormatWidth(aField.FieldType,aField.FieldLength);
                                        String aItem=aRecord.Substring (aStartIndex,aWidth);
                                        aItemList.Add (aItem);
                                        aStartIndex+=aWidth;
                                    }
                                    catch(Exception ex)
                                    {
                                        System.Windows .Forms .MessageBox.Show(this.m_lineNO +"\n"+ex.StackTrace);
                                    }
                                }
                                this.m_attrTable.AppendRecord(aItemList);
                            }
                            #endregion
                        }
                        aLine=this.ReadLine ();
                    }
                }
                #endregion
                aLine=this.ReadLine ();
            }
            #region 开始写出
            #region 首先创建Shapefile
            FieldsClass aFlds=new FieldsClass ();
            aFlds.AddField (this.CreateOIDField ());
            int fRealCount=this.AttributeTable .GetFieldCount ();
            for(int fi=0;fi<fRealCount;fi++)
            {
                TableField aTF=this.AttributeTable .GetTableField(fi);
                IField aEsriFld=this.CreateField (aTF);
                aFlds.AddField (aEsriFld);
            }
            aFlds.AddField (this.CreateGeometryField (this.GeometryType ));
            String aTableTitle=this.AttributeTable .TableName ;
            int extIndex=aTableTitle.LastIndexOf (".");
            if(extIndex>=0)
                aTableTitle=aTableTitle.Substring (0,extIndex);
            IFeatureClass aClass= this.FeatureWorkspace.CreateFeatureClass
                (aTableTitle,aFlds,null,null,esriFeatureType.esriFTSimple
                ,"SHAPE",null);
            #endregion
            #region 开始写数据
            this.WorkspaceEdit .StartEditing (false);
            this.WorkspaceEdit .StartEditOperation ();
            try
            {
                if(this.m_polyList .Count >0)
                {
                    this.ExportPolygon(aClass);
                }
                else if(this.m_arcList .Count >0)
                {
                    this.ExportPolyline(aClass);
                }
                else if(this.m_labList .Count >0)
                {
                    this.ExportPoint(aClass);
                }
                this.WorkspaceEdit .StopEditOperation ();
                this.WorkspaceEdit .StopEditing (true);
            }
            catch
            {
                this.WorkspaceEdit .AbortEditOperation();
                this.WorkspaceEdit .StopEditing (false);
            }
            
            #endregion
            #endregion
        }      
        #endregion

        #region 书写数据的方法
        private IPolyline GetArcGeometry(int pArcID)
        {
            int arcCount=this.m_arcList .Count ;
            for(int ai=0;ai<arcCount;ai++)
            {
                ArcObject aArc=this.m_arcList [ai] as ArcObject;
                if(aArc.ArcID .Equals (pArcID))
                {
                    return aArc.Geometry;
                }
            }
            return null;
        }
        private void ExportPolygon(IFeatureClass pClass )
        {
            //第一个Polygon是最外边界.不是实际数据.
            if(this.m_polyList .Count >0)
                this.m_polyList .RemoveAt (0);
            int pCount=this.m_polyList.Count ;
            for(int pi=0;pi<pCount;pi++)
            {
                try
                {
                    PolyObject aPoly=this.m_polyList [pi] as PolyObject ;
                    IPolygon aPolyGeom=new  PolygonClass();
                    int aArcCount=aPoly.ArcCount ;
                    for(int ai=0;ai<aArcCount;ai++)
                    {
                        int aArcID=aPoly.GetArcID (ai);                   
                        IPolyline pline=this.GetArcGeometry (Math.Abs (aArcID));
                        if(pline!=null)
                        {
                            if(aArcID<0)pline.ReverseOrientation();
                            (aPolyGeom as ISegmentCollection )
                                .AddSegmentCollection(pline as ISegmentCollection );
                        }
                    }
                    aPolyGeom.Close ();
                    (aPolyGeom as ITopologicalOperator ).Simplify ();
                    IFeature aFea=pClass.CreateFeature ();
                    try
                    {
                        ArrayList aRecord=this.m_attrTable .GetRecord (pi);
                        for(int fi=0;fi<aRecord.Count ;fi++)
                        {
                            String obj=aRecord[fi].ToString ().Trim ();
                            if(!obj.Equals (""))
                            {
                                aFea.set_Value (fi+2,obj);
                            }
                        }
                    }
                    catch
                    {
                    }
                    aFea.Shape =aPolyGeom;
                    aFea.Store ();
                }
                catch
                {
                }
            }
        }
        private void ExportPolyline(IFeatureClass pClass)
        {
            int arcCount=this.m_arcList .Count ;
            for(int ai=0;ai<arcCount;ai++)
            {
                ArcObject aArcObj=this.m_arcList [ai] as ArcObject ;
                IFeature aFea=pClass.CreateFeature ();
                try
                {
                    ArrayList aRecord=this.m_attrTable .GetRecord (ai);
                    for(int fi=0;fi<aRecord.Count ;fi++)
                    {
                        String obj=aRecord[fi].ToString ().Trim ();
                        if(!obj.Equals (""))
                        {
                            aFea.set_Value (fi+2,obj);
                        }
                    }
                }
                catch
                {
                }
                aFea.Shape =aArcObj.Geometry ;
                aFea.Store ();
            }
        }
        private void ExportPoint(IFeatureClass pClass)
        {
            int aPtCount=this.m_labList .Count ;
            for(int pi=0;pi<aPtCount;pi++)
            {
                LabObject aLabObj=this.m_labList [pi] as LabObject ;
                IFeature aFea=pClass.CreateFeature ();
                
                try
                {
                    ArrayList aRecord=this.m_attrTable .GetRecord (pi);
                    for(int ai=0;ai<aRecord.Count ;ai++)
                    {
                        String obj=aRecord[ai].ToString ().Trim ();
                        if(!obj.Equals (""))
                        {
                            aFea.set_Value (ai+2,obj);
                        }
                    }
                }
                catch
                {
                }
                aFea.Shape =aLabObj.Geometry ;
                aFea.Store ();
            }
        }
        #endregion
        #region 创建字段的方法
        private IField CreateField(TableField pTF)
        {
            IField rFld=null;
            if(pTF.FieldType ==esriFieldType.esriFieldTypeSmallInteger
                ||pTF.FieldType ==esriFieldType.esriFieldTypeInteger)
            {
                rFld=this.CreateIntField(pTF.FieldName,pTF.FieldLength);
            }
            else if(pTF.FieldType ==esriFieldType.esriFieldTypeSingle)
            {
                rFld=this.CreateFloatField (pTF.FieldName,pTF.FieldLength,pTF.FieldScale);
            }
            else if(pTF.FieldType ==esriFieldType.esriFieldTypeDouble)
            {
                rFld=this.CreateDoubleField (pTF.FieldName,pTF.FieldLength,pTF.FieldScale);
            }
            else
            {
                rFld=this.CreateTextField (pTF.FieldName ,pTF.FieldLength);
            }
            return rFld;
        }
        private IField CreateOIDField()
        {
            FieldClass aField=new FieldClass ();
            IFieldEdit aFieldEdit=aField as IFieldEdit ;
            aFieldEdit.Name_2 ="OBJECTID";
            aFieldEdit.Type_2 =esriFieldType.esriFieldTypeOID;
            return aField as IField;
        }
        private IField CreateGeometryField(esriGeometryType pGT)
        {
            FieldClass aField=new FieldClass();
            IFieldEdit aFieldEdit=aField as IFieldEdit ;
            aFieldEdit.Name_2 ="SHAPE";
            aFieldEdit.Type_2 =esriFieldType.esriFieldTypeGeometry;
            GeometryDefClass geomDef=new GeometryDefClass();
            geomDef.IGeometryDefEdit_GeometryType_2=pGT;
            geomDef.IGeometryDefEdit_HasM_2=false;
            geomDef.IGeometryDefEdit_HasZ_2 =false;
            
            geomDef.IGeometryDefEdit_GridCount_2=1;
            geomDef.set_GridSize (0,1000);
            geomDef.IGeometryDefEdit_SpatialReference_2 =new UnknownCoordinateSystemClass();
            aFieldEdit.GeometryDef_2 =geomDef;
            return aField as IField;
            
        }
        private IField CreateTextField(string pFieldName,int pFieldLen)
        {
            FieldClass aField=new FieldClass ();
            IFieldEdit aFieldEdit= aField as IFieldEdit ;
            aFieldEdit.Name_2=pFieldName.ToUpper ();            
            aFieldEdit.Type_2=esriFieldType.esriFieldTypeString;
            aFieldEdit.Length_2=pFieldLen;                      
            return aField as IField;
        }
        private IField CreateFloatField(String pFieldName,int pFieldLen,int pFieldScale)
        {
            FieldClass aField=new FieldClass ();
            IFieldEdit aFieldEdit=aField as IFieldEdit ;
            aFieldEdit.Name_2 =pFieldName;
            aFieldEdit.Type_2 =esriFieldType.esriFieldTypeSingle;
            aFieldEdit.Precision_2 =pFieldLen;
            aFieldEdit.Scale_2=pFieldScale;
            return aField as IField;
        }
        private IField CreateDoubleField(String pFieldName,int pFieldLen,int pFieldScale)
        {
            FieldClass aField=new FieldClass ();
            IFieldEdit aFieldEdit=aField as IFieldEdit ;
            aFieldEdit.Name_2 =pFieldName;
            aFieldEdit.Type_2 =esriFieldType.esriFieldTypeDouble;
            aFieldEdit.Precision_2 =pFieldLen;
            aFieldEdit.Scale_2=pFieldScale;
            return aField as IField;
        }
        private IField CreateIntField(String pFieldName,int pFieldLen)
        {
            FieldClass aField=new FieldClass ();
            IFieldEdit aFieldEdit=aField as IFieldEdit;
            aFieldEdit.Name_2 =pFieldName;
            aFieldEdit.Precision_2 =pFieldLen;
            aFieldEdit.Type_2 =esriFieldType.esriFieldTypeInteger;
            return aField as IField;
        }
        private IField CreateDateField(String pFieldName)
        {
            FieldClass aField=new FieldClass ();
            IFieldEdit aFieldEdit=aField as IFieldEdit;
            aFieldEdit.Name_2 =pFieldName;
            aFieldEdit.Type_2 =esriFieldType.esriFieldTypeDate;
            return aField as IField;
        }
        #endregion
    }

}
