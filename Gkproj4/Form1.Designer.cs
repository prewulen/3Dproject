namespace Gkproj4
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.DrawArea = new System.Windows.Forms.Panel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.labelFps = new System.Windows.Forms.Label();
            this.LineFillCheckbox = new System.Windows.Forms.CheckBox();
            this.ButtonDraw3D = new System.Windows.Forms.Button();
            this.ButtonStart = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.RemoveItemButton = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.ButtonAddNewItem = new System.Windows.Forms.Button();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.BackfaceCullCheckBox = new System.Windows.Forms.CheckBox();
            this.ZBufferCheckBox = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel1.Controls.Add(this.DrawArea, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 1, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1071, 769);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // DrawArea
            // 
            this.DrawArea.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DrawArea.BackColor = System.Drawing.SystemColors.ControlLight;
            this.DrawArea.Location = new System.Drawing.Point(3, 3);
            this.DrawArea.Name = "DrawArea";
            this.DrawArea.Size = new System.Drawing.Size(915, 763);
            this.DrawArea.TabIndex = 1;
            this.DrawArea.Paint += new System.Windows.Forms.PaintEventHandler(this.DrawArea_Paint);
            this.DrawArea.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DrawArea_MouseDown);
            this.DrawArea.MouseMove += new System.Windows.Forms.MouseEventHandler(this.DrawArea_MouseMove);
            this.DrawArea.MouseUp += new System.Windows.Forms.MouseEventHandler(this.DrawArea_MouseUp);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.Controls.Add(this.labelFps);
            this.flowLayoutPanel1.Controls.Add(this.BackfaceCullCheckBox);
            this.flowLayoutPanel1.Controls.Add(this.ZBufferCheckBox);
            this.flowLayoutPanel1.Controls.Add(this.LineFillCheckbox);
            this.flowLayoutPanel1.Controls.Add(this.ButtonDraw3D);
            this.flowLayoutPanel1.Controls.Add(this.ButtonStart);
            this.flowLayoutPanel1.Controls.Add(this.listBox1);
            this.flowLayoutPanel1.Controls.Add(this.RemoveItemButton);
            this.flowLayoutPanel1.Controls.Add(this.comboBox1);
            this.flowLayoutPanel1.Controls.Add(this.ButtonAddNewItem);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(924, 3);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(144, 763);
            this.flowLayoutPanel1.TabIndex = 2;
            // 
            // labelFps
            // 
            this.labelFps.AutoSize = true;
            this.labelFps.Location = new System.Drawing.Point(3, 0);
            this.labelFps.Name = "labelFps";
            this.labelFps.Size = new System.Drawing.Size(24, 13);
            this.labelFps.TabIndex = 13;
            this.labelFps.Text = "fps:";
            // 
            // LineFillCheckbox
            // 
            this.LineFillCheckbox.AutoSize = true;
            this.LineFillCheckbox.Checked = true;
            this.LineFillCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.LineFillCheckbox.Location = new System.Drawing.Point(3, 62);
            this.LineFillCheckbox.Name = "LineFillCheckbox";
            this.LineFillCheckbox.Size = new System.Drawing.Size(136, 17);
            this.LineFillCheckbox.TabIndex = 15;
            this.LineFillCheckbox.Text = "Draw lines instead of fill";
            this.LineFillCheckbox.UseVisualStyleBackColor = true;
            // 
            // ButtonDraw3D
            // 
            this.ButtonDraw3D.Location = new System.Drawing.Point(3, 85);
            this.ButtonDraw3D.Name = "ButtonDraw3D";
            this.ButtonDraw3D.Size = new System.Drawing.Size(141, 52);
            this.ButtonDraw3D.TabIndex = 10;
            this.ButtonDraw3D.Text = "Draw3D";
            this.ButtonDraw3D.UseVisualStyleBackColor = true;
            this.ButtonDraw3D.Click += new System.EventHandler(this.ButtonDraw3D_Click);
            // 
            // ButtonStart
            // 
            this.ButtonStart.Location = new System.Drawing.Point(3, 143);
            this.ButtonStart.Name = "ButtonStart";
            this.ButtonStart.Size = new System.Drawing.Size(141, 52);
            this.ButtonStart.TabIndex = 12;
            this.ButtonStart.Text = "Start";
            this.ButtonStart.UseVisualStyleBackColor = true;
            this.ButtonStart.Click += new System.EventHandler(this.ButtonStart_Click);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(3, 201);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(141, 173);
            this.listBox1.TabIndex = 14;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.ListBox1_SelectedIndexChanged);
            // 
            // RemoveItemButton
            // 
            this.RemoveItemButton.Location = new System.Drawing.Point(3, 380);
            this.RemoveItemButton.Name = "RemoveItemButton";
            this.RemoveItemButton.Size = new System.Drawing.Size(141, 52);
            this.RemoveItemButton.TabIndex = 16;
            this.RemoveItemButton.Text = "Remove selected item";
            this.RemoveItemButton.UseVisualStyleBackColor = true;
            this.RemoveItemButton.Click += new System.EventHandler(this.RemoveItemButton_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(3, 438);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(141, 21);
            this.comboBox1.TabIndex = 17;
            // 
            // ButtonAddNewItem
            // 
            this.ButtonAddNewItem.Location = new System.Drawing.Point(3, 465);
            this.ButtonAddNewItem.Name = "ButtonAddNewItem";
            this.ButtonAddNewItem.Size = new System.Drawing.Size(141, 52);
            this.ButtonAddNewItem.TabIndex = 18;
            this.ButtonAddNewItem.Text = "Add new item";
            this.ButtonAddNewItem.UseVisualStyleBackColor = true;
            this.ButtonAddNewItem.Click += new System.EventHandler(this.ButtonAddNewItem_Click);
            // 
            // timer1
            // 
            this.timer1.Interval = 15;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // timer2
            // 
            this.timer2.Enabled = true;
            this.timer2.Interval = 1000;
            this.timer2.Tick += new System.EventHandler(this.Timer2_Tick);
            // 
            // BackfaceCullCheckBox
            // 
            this.BackfaceCullCheckBox.AutoSize = true;
            this.BackfaceCullCheckBox.Checked = true;
            this.BackfaceCullCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.BackfaceCullCheckBox.Location = new System.Drawing.Point(3, 16);
            this.BackfaceCullCheckBox.Name = "BackfaceCullCheckBox";
            this.BackfaceCullCheckBox.Size = new System.Drawing.Size(126, 17);
            this.BackfaceCullCheckBox.TabIndex = 19;
            this.BackfaceCullCheckBox.Text = "Use backface culling";
            this.BackfaceCullCheckBox.UseVisualStyleBackColor = true;
            // 
            // ZBufferCheckBox
            // 
            this.ZBufferCheckBox.AutoSize = true;
            this.ZBufferCheckBox.Checked = true;
            this.ZBufferCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ZBufferCheckBox.Location = new System.Drawing.Point(3, 39);
            this.ZBufferCheckBox.Name = "ZBufferCheckBox";
            this.ZBufferCheckBox.Size = new System.Drawing.Size(83, 17);
            this.ZBufferCheckBox.TabIndex = 20;
            this.ZBufferCheckBox.Text = "Use z-buffer";
            this.ZBufferCheckBox.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1077, 774);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel DrawArea;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Button ButtonDraw3D;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button ButtonStart;
        private System.Windows.Forms.Label labelFps;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.CheckBox LineFillCheckbox;
        private System.Windows.Forms.Button RemoveItemButton;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button ButtonAddNewItem;
        private System.Windows.Forms.CheckBox BackfaceCullCheckBox;
        private System.Windows.Forms.CheckBox ZBufferCheckBox;
    }
}

