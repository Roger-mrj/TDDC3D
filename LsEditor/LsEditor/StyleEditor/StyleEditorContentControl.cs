using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Display;
using RCIS.Style.StyleEditor.LineStyle;
namespace RCIS.Style.StyleEditor
{
    public partial class StyleEditorContentControl : UserControl
    {
        public StyleEditorContentControl()
        {
            InitializeComponent();
            this.m_styleEditorList = new List<IStyleEditor>();
            this.AppendStyleEditor(new SimpleMarkerStyleEditor());
            this.AppendStyleEditor(new CharacterMarkerStyleEditor());
            this.AppendStyleEditor(new PictureMarkerStyleEditor());
            this.AppendStyleEditor(new ArrowMarkerStyleEditor());
            this.AppendStyleEditor(new SimpleLineStyleEditor());
            this.AppendStyleEditor(new LineStyleEditor());
            this.AppendStyleEditor(new SimpleFillStyleEditor());
            this.AppendStyleEditor(new LineFillStyleEditor());
            this.AppendStyleEditor(new MarkerFillStyleEditor());
            this.AppendStyleEditor(new GradientFillStyleEditor());
            this.AppendStyleEditor(new PictureFillStyleEditor());
          //  this.AppendStyleEditor(new DotDensityFillStyleEditor());
            this.AppendStyleEditor(new CartographicLineStyleEditor());
            this.AppendStyleEditor(new HashLineStyleEditor());
            this.AppendStyleEditor(new PictureLineStyleEditor());
            this.AppendStyleEditor(new TextSymbolStyleEditor());
        }
        private List<IStyleEditor> m_styleEditorList;
        private string m_styleClassFilter = StyleClass.StyleClassAny;
        public void AppendStyleEditor(IStyleEditor pEditor)
        {
            if (pEditor != null)
            {
                this.m_styleEditorList.Add(pEditor);
                pEditor.OnEditedStyleChanged += new EditedStyleChangedEventHandler(CurrentStyleEditor_OnEditedStyleChanged);
            }
        }
        public string StyleClassFilter
        {
            get
            {
                return this.m_styleClassFilter;
            }
            set
            {
                this.m_styleClassFilter = value;
                this.UpdateEditorList();
            }
        }
        private void UpdateEditorList()
        {
            this.cbEditor.Items.Clear();
            foreach (IStyleEditor aEditor in this.m_styleEditorList)
            {
                if (aEditor is Control)
                {
                    if (aEditor.StyleClass.Equals(this.StyleClassFilter))
                    {
                        int order = this.cbEditor.Items.Count + 1;
                        ComboBoxItem aItem = new ComboBoxItem(aEditor, aEditor.EditorName, order);
                        this.cbEditor.Items.Add(aItem);
                    }
                }
            }
        }
        public IStyleEditor CurrentStyleEditor
        {
            get
            {
                ComboBoxItem aSelItem = this.cbEditor.SelectedItem
                as ComboBoxItem;
                if (aSelItem != null)
                {
                    return aSelItem.ItemObject as IStyleEditor;
                }
                return null;
            }
        }
        private bool m_switchAsSymbol = false;
        public void InitializeStyleEditor(ISymbol pSymbol)
        {
            this.StyleClassFilter = StyleClass.GetStyleClass(pSymbol);
            foreach (ComboBoxItem aItem in this.cbEditor.Items)
            {
                IStyleEditor aEditor = aItem.ItemObject as IStyleEditor;
                if (aEditor.CanEdit(pSymbol))
                {
                    this.m_switchAsSymbol = true;
                    aEditor.EditedStyle = pSymbol;
                    this.cbEditor.SelectedItem = aItem;
                    this.m_switchAsSymbol = false;
                    break;
                }
            }
        }
        private void cbEditor_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.plContent.Controls.Clear();
            if (this.CurrentStyleEditor != null)
            {
                Control ctrl = this.CurrentStyleEditor as Control;
                ctrl.Dock = DockStyle.Fill;
                this.plContent.Controls.Add(ctrl);
                if (!this.m_switchAsSymbol
                    || this.CurrentStyleEditor.EditedStyle == null)
                {
                    this.CurrentStyleEditor.CreateInitializeStyle();
                }
            }
        }
        void CurrentStyleEditor_OnEditedStyleChanged(object pSender, EventArgs pArg)
        {
            if (this.OnEditedStyleChanged != null)
                this.OnEditedStyleChanged(this, new EventArgs());
        }


        public event EditedStyleChangedEventHandler OnEditedStyleChanged;

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            RCIS.Style.StyleEditor.StyleHelpForm frm = new StyleHelpForm();
            Helper.ControlStyleHelper.InitFormStyle(frm);
        }
    }

}
