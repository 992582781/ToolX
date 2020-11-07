namespace CallEr
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.EnumExtension = new System.Windows.Forms.Button();
            this.LinqEx = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // EnumExtension
            // 
            this.EnumExtension.Location = new System.Drawing.Point(2, 12);
            this.EnumExtension.Name = "EnumExtension";
            this.EnumExtension.Size = new System.Drawing.Size(141, 34);
            this.EnumExtension.TabIndex = 0;
            this.EnumExtension.Text = "EnumExtension";
            this.EnumExtension.UseVisualStyleBackColor = true;
            this.EnumExtension.Click += new System.EventHandler(this.EnumExtension_Click);
            // 
            // LinqEx
            // 
            this.LinqEx.Location = new System.Drawing.Point(2, 68);
            this.LinqEx.Name = "LinqEx";
            this.LinqEx.Size = new System.Drawing.Size(142, 36);
            this.LinqEx.TabIndex = 1;
            this.LinqEx.Text = "LinqEx";
            this.LinqEx.UseVisualStyleBackColor = true;
            this.LinqEx.Click += new System.EventHandler(this.LinqEx_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1038, 649);
            this.Controls.Add(this.LinqEx);
            this.Controls.Add(this.EnumExtension);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button EnumExtension;
        private System.Windows.Forms.Button LinqEx;
    }
}

