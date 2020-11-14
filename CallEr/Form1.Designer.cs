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
            this.async = new System.Windows.Forms.Button();
            this.Queue = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // EnumExtension
            // 
            this.EnumExtension.Location = new System.Drawing.Point(1, 8);
            this.EnumExtension.Margin = new System.Windows.Forms.Padding(2);
            this.EnumExtension.Name = "EnumExtension";
            this.EnumExtension.Size = new System.Drawing.Size(94, 23);
            this.EnumExtension.TabIndex = 0;
            this.EnumExtension.Text = "EnumExtension";
            this.EnumExtension.UseVisualStyleBackColor = true;
            this.EnumExtension.Click += new System.EventHandler(this.EnumExtension_Click);
            // 
            // LinqEx
            // 
            this.LinqEx.Location = new System.Drawing.Point(121, 8);
            this.LinqEx.Margin = new System.Windows.Forms.Padding(2);
            this.LinqEx.Name = "LinqEx";
            this.LinqEx.Size = new System.Drawing.Size(95, 24);
            this.LinqEx.TabIndex = 1;
            this.LinqEx.Text = "LinqEx";
            this.LinqEx.UseVisualStyleBackColor = true;
            this.LinqEx.Click += new System.EventHandler(this.LinqEx_Click);
            // 
            // async
            // 
            this.async.Location = new System.Drawing.Point(236, 9);
            this.async.Name = "async";
            this.async.Size = new System.Drawing.Size(85, 23);
            this.async.TabIndex = 2;
            this.async.Text = "async";
            this.async.UseVisualStyleBackColor = true;
            this.async.Click += new System.EventHandler(this.async_Click);
            // 
            // Queue
            // 
            this.Queue.Location = new System.Drawing.Point(337, 9);
            this.Queue.Name = "Queue";
            this.Queue.Size = new System.Drawing.Size(75, 23);
            this.Queue.TabIndex = 3;
            this.Queue.Text = "Queue";
            this.Queue.UseVisualStyleBackColor = true;
            this.Queue.Click += new System.EventHandler(this.Queue_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(692, 433);
            this.Controls.Add(this.Queue);
            this.Controls.Add(this.async);
            this.Controls.Add(this.LinqEx);
            this.Controls.Add(this.EnumExtension);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button EnumExtension;
        private System.Windows.Forms.Button LinqEx;
        private System.Windows.Forms.Button async;
        private System.Windows.Forms.Button Queue;
    }
}

