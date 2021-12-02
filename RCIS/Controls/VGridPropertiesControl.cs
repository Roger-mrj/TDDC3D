using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using DevExpress.XtraVerticalGrid;
using DevExpress.XtraVerticalGrid.Rows;
using DevExpress.XtraEditors.Repository;




namespace RCIS.Controls
{
    public partial class VGridPropertiesControl : UserControl, PropertyEditor
    {
        public VGridPropertiesControl()
        {
            InitializeComponent();
            this.vGridControl.RowHeaderWidthChanged += new EventHandler(vGridControl_RowHeaderWidthChanged);
            this.vGridControl.SizeChanged += new EventHandler(vGridControl_SizeChanged);

        }

        void vGridControl_SizeChanged(object sender, EventArgs e)
        {
            this.AutoFitWidth();
        }

        void vGridControl_RowHeaderWidthChanged(object sender, EventArgs e)
        {
            this.AutoFitWidth();
        }
        private void AutoFitWidth()
        {
            this.vGridControl.RecordWidth = this.vGridControl.Width - this.vGridControl.RowHeaderMinWidth;
        }
        private bool mPropertyChanged = false;
        private bool mShouldAction = true;

        private IFeature mFeature;

      


        public bool CanEdit
        {
            get
            {
                return this.vGridControl.Enabled;
            }
            set
            {
                this.vGridControl.Enabled = value;
            }
        }

        /// <summary>
        /// 当前必须传入的参数，当前Feature
        /// </summary>
        public IFeature  DJFeature
        {
            get
            {
                return this.mFeature;
            }
            set
            {
                this.mFeature = value;
                this.ShowFeature();
            }
        }


        public void OnApply()
        {
            if (this.mFeature == null) return;
            if (!this.PropertyChanged) return;

            this.vGridControl.EndUpdate();

            int rc = this.vGridControl.Rows.Count;
            for (int ri = 0; ri < rc; ri++)
            {
                
                EditorRow aRow = this.vGridControl.Rows[ri] as EditorRow;
                
                if (aRow != null)
                {
                    int fi = -1;
                    try
                    {
                        fi = (int)aRow.Tag;
                    }
                    catch  { }
                    if (fi >= 0)
                    {
                        IField aCurFld = this.mFeature.Fields.get_Field(fi);// mCurrFeatureClass.Fields.get_Field(fi);
                        string aObj = aRow.Properties.Value.ToString();

                      

                        string aOldObj = this.mFeature.get_Value(fi).ToString();// .GetValue(fi).ToString();
                        if (!aObj.Equals(aOldObj))
                        {
                            try
                            {
                                if (aCurFld.CheckValue(aObj))
                                {
                                    this.mFeature.set_Value(fi, aObj);
                                }
                            }
                            catch { }
                        }
                    }
                }
            }
            this.mFeature.Store();
        }

        public void OnCancel()
        {

        }

        public bool PropertyChanged
        {
            get
            {
                return this.mPropertyChanged;
            }
            set
            {
                this.mPropertyChanged = value;
            }
        }
        private void ShowFeature()
        {
            this.mPropertyChanged = false;
            this.vGridControl.Rows.Clear();
            this.CreatePlainRow("属性名称", "属性值", -1, true);
            if (this.mFeature == null) return;
            else
            {

                IField aAreaFld = null;
                
                IField aLenFld = null;
               
                int fc = mFeature.Fields.FieldCount;
                for (int fi = 0; fi < fc; fi++)
                {
                    IField aCurFld = mFeature.Fields.get_Field(fi);
                    if (aCurFld.Name.ToUpper().Contains("SHAPE_ARE") || aCurFld.Name.ToUpper().Contains("SHAPE.AREA"))
                    {
                        aAreaFld = aCurFld;
                    }
                    if (aCurFld.Name.ToUpper().Contains("SHAPE_LEN") || aCurFld.Name.ToUpper().Contains("SHAPE.LEN"))
                    {
                        aLenFld = aCurFld;
                    }                    


                    if (aCurFld.Type == esriFieldType.esriFieldTypeOID)
                    {
                        string aObj = this.mFeature.get_Value(fi).ToString();//  .GetValue(fi).ToString();
                        EditorRow aRow = this.CreatePlainRow("对象编号"
                        , aObj, fi, true);
                    }
                    else if (aCurFld.Type == esriFieldType.esriFieldTypeGeometry)
                    {
                        IGeometry aGeom = this.mFeature.ShapeCopy;
                        string aObj = "";
                        if (aGeom == null || aGeom.IsEmpty)
                        {
                            aObj = "图形对象为空";
                        }
                        else if (aGeom is IPoint)
                            aObj = "点";
                        else if (aGeom is IPolyline)
                            aObj = "线";
                        else if (aGeom is IPolygon)
                            aObj = "面";

                        EditorRow aRow = this.CreatePlainRow("几何图形"
                        , aObj, fi, true);
                    }
                    else if (aLenFld == aCurFld)
                    {
                        string aObj = (this.mFeature.Shape as ICurve ).Length.ToString();
                        EditorRow aRow = this.CreatePlainRow("图形计算长度"
                        , aObj, fi, true);
                    }
                    else if (aAreaFld == aCurFld)
                    {
                        string aObj = (this.mFeature.Shape as IArea).Area.ToString();//  .Length.ToString();
                        EditorRow aRow = this.CreatePlainRow("图形计算面积"
                        , aObj, fi, true);
                    }
                    else
                    {
                        string aObj = "";
                        try
                        {
                            aObj = this.mFeature.get_Value(fi).ToString();
                        }
                        catch  { }

                        bool isPlain = true;
                      
                        if (isPlain)
                        {
                            EditorRow aRow = this.CreatePlainRow(aCurFld.AliasName
                            , aObj, fi, false);
                        }
                    }
                }
            }
        }
        private EditorRow CreatePlainRow(string aCaption
            , string pValue
            , int pIndex
            , bool pReadOnly)
        {
            EditorRow rRow = new EditorRow();
            RepositoryItemTextEdit aEditor = new RepositoryItemTextEdit();
            aEditor.EditValueChanged += new EventHandler(aEditor_EditValueChanged);
            aEditor.Tag = pIndex;
            rRow.Tag = pIndex;
            rRow.Properties.RowEdit = aEditor;
            rRow.Properties.Caption = aCaption;
            rRow.Properties.Value = pValue;
            rRow.Properties.ReadOnly = pReadOnly;
            this.vGridControl.Rows.Add(rRow);
            return rRow;
        }
        private EditorRow CreateComboRow(string aCaption
            , string pValue
            , int pIndex
            , bool pReadOnly
            , string pDictTable)
        {
            EditorRow rRow = new EditorRow();
            RepositoryItemComboBox aEditor = new RepositoryItemComboBox();
            aEditor.EditValueChanged += new EventHandler(aEditor_EditValueChanged);
            aEditor.Tag = pIndex;
            rRow.Tag = pIndex;
            aEditor.AutoComplete = true;
            rRow.Properties.RowEdit = aEditor;
            rRow.Properties.Caption = aCaption;
            rRow.Properties.Value = pValue;
            rRow.Properties.ReadOnly = pReadOnly;

           
            this.vGridControl.Rows.Add(rRow);
            return rRow;
        }
        void aEditor_EditValueChanged(object sender, EventArgs e)
        {
            this.mPropertyChanged = true;
        }
    }
}
