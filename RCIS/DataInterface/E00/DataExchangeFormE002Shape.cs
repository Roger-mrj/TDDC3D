using System;
using System.IO ;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using RCIS.DataExchange .E00 ;
namespace RCIS.DataExchange.E00
{
	/// <summary>
	/// DataExchangeForm 的摘要说明。
	/// </summary>
	public class DataExchangeFormE002Shape : System.Windows.Forms.Form
	{
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private DevExpress.XtraEditors.ButtonEdit beShapeFolder;
        private DevExpress.XtraEditors.SimpleButton btnOK;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.ButtonEdit beE00Path;
		/// <summary>
		/// 必需的设计器变量。
		/// </summary>
		private System.ComponentModel.Container components = null;

		public DataExchangeFormE002Shape()
		{
			//
			// Windows 窗体设计器支持所必需的
			//
			InitializeComponent();

			//
			// TODO: 在 InitializeComponent 调用后添加任何构造函数代码
			//
		}

		/// <summary>
		/// 清理所有正在使用的资源。
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows 窗体设计器生成的代码
		/// <summary>
		/// 设计器支持所需的方法 - 不要使用代码编辑器修改
		/// 此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{
            this.beE00Path = new DevExpress.XtraEditors.ButtonEdit();
            this.beShapeFolder = new DevExpress.XtraEditors.ButtonEdit();
            this.btnOK = new DevExpress.XtraEditors.SimpleButton();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.beE00Path.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.beShapeFolder.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // beE00Path
            // 
            this.beE00Path.EditValue = "";
            this.beE00Path.Location = new System.Drawing.Point(96, 16);
            this.beE00Path.Name = "beE00Path";
            this.beE00Path.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.beE00Path.Size = new System.Drawing.Size(336, 21);
            this.beE00Path.TabIndex = 0;
            this.beE00Path.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.beVCTPath_ButtonClick);
            // 
            // beShapeFolder
            // 
            this.beShapeFolder.EditValue = "";
            this.beShapeFolder.Location = new System.Drawing.Point(96, 56);
            this.beShapeFolder.Name = "beShapeFolder";
            this.beShapeFolder.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.beShapeFolder.Size = new System.Drawing.Size(336, 21);
            this.beShapeFolder.TabIndex = 1;
            this.beShapeFolder.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.beShapeFolder_ButtonClick);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(264, 96);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "开始";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(352, 96);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(16, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 23);
            this.label1.TabIndex = 4;
            this.label1.Text = "E00文件";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(16, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 23);
            this.label2.TabIndex = 5;
            this.label2.Text = "Shape目录";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // DataExchangeFormE002Shape
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(448, 134);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.beShapeFolder);
            this.Controls.Add(this.beE00Path);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DataExchangeFormE002Shape";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "数据转换(E00==>Shapefile)";
            ((System.ComponentModel.ISupportInitialize)(this.beE00Path.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.beShapeFolder.Properties)).EndInit();
            this.ResumeLayout(false);

        }
		#endregion
        private void beVCTPath_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            OpenFileDialog ofd=new OpenFileDialog ();
            ofd.Title ="打开需要读取的E00文件";
            ofd.Filter ="E00文件|*.e00";
            ofd.Multiselect =false;
            ofd.CheckFileExists=true;
            if(ofd.ShowDialog (this)==DialogResult.OK )
            {
               this.beE00Path .Text =ofd.FileName ;
            }
        }

        private void beShapeFolder_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
          FolderBrowserDialog fbd=new FolderBrowserDialog ();
            fbd.Description="选择一个目录来存放Shapefile";
            if(fbd.ShowDialog (this)==DialogResult.OK )
            {
                this.beShapeFolder .Text =fbd.SelectedPath ;
            }
        }

        private void btnOK_Click(object sender, System.EventArgs e)
        {
            if(!File.Exists (this.beE00Path .Text ))
            {
                MessageBox.Show ("E00文件不存在!");
                return;
            }
            if(!Directory.Exists (this.beShapeFolder .Text ))
            {
                MessageBox.Show ("Shapefile目录不存在");
                return;
            }
            E00Exporter ep=new   E00Exporter ();
            ep.E00FilePath =this.beE00Path .Text ;
            ep.ShapefileFolder =this.beShapeFolder .Text ;
            this.Cursor = Cursors.WaitCursor;
            try
            {
                if (ep.BeginExport())
                {
                    ep.ExportToShapefile(null, null, false);
                    ep.FinishExport();
                    MessageBox.Show("转换完毕！", "提示",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("转换失败" + ex.ToString());
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void btnCancel_Click(object sender, System.EventArgs e)
        {
          this.Close ();
        }
	}
}
