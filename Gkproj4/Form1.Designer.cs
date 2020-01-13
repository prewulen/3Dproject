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
            this.ButtonLightPos = new System.Windows.Forms.Button();
            this.ColorPicker = new System.Windows.Forms.Button();
            this.InterpolationCheckBox = new System.Windows.Forms.CheckBox();
            this.ButtonDraw3D = new System.Windows.Forms.Button();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.ButtonStart = new System.Windows.Forms.Button();
            this.labelFps = new System.Windows.Forms.Label();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
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
            this.tableLayoutPanel1.Size = new System.Drawing.Size(751, 529);
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
            this.DrawArea.Size = new System.Drawing.Size(595, 523);
            this.DrawArea.TabIndex = 1;
            this.DrawArea.Paint += new System.Windows.Forms.PaintEventHandler(this.DrawArea_Paint);
            this.DrawArea.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DrawArea_MouseDown);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.Controls.Add(this.labelFps);
            this.flowLayoutPanel1.Controls.Add(this.ButtonLightPos);
            this.flowLayoutPanel1.Controls.Add(this.ColorPicker);
            this.flowLayoutPanel1.Controls.Add(this.InterpolationCheckBox);
            this.flowLayoutPanel1.Controls.Add(this.ButtonDraw3D);
            this.flowLayoutPanel1.Controls.Add(this.numericUpDown1);
            this.flowLayoutPanel1.Controls.Add(this.ButtonStart);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(604, 3);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(144, 523);
            this.flowLayoutPanel1.TabIndex = 2;
            // 
            // ButtonLightPos
            // 
            this.ButtonLightPos.Location = new System.Drawing.Point(3, 16);
            this.ButtonLightPos.Name = "ButtonLightPos";
            this.ButtonLightPos.Size = new System.Drawing.Size(141, 52);
            this.ButtonLightPos.TabIndex = 2;
            this.ButtonLightPos.Text = "Choose light position";
            this.ButtonLightPos.UseVisualStyleBackColor = true;
            this.ButtonLightPos.Click += new System.EventHandler(this.ButtonLightPos_Click);
            // 
            // ColorPicker
            // 
            this.ColorPicker.Location = new System.Drawing.Point(3, 74);
            this.ColorPicker.Name = "ColorPicker";
            this.ColorPicker.Size = new System.Drawing.Size(141, 52);
            this.ColorPicker.TabIndex = 8;
            this.ColorPicker.Text = "Pick light color";
            this.ColorPicker.UseVisualStyleBackColor = true;
            this.ColorPicker.Click += new System.EventHandler(this.ColorPicker_Click);
            // 
            // InterpolationCheckBox
            // 
            this.InterpolationCheckBox.AutoSize = true;
            this.InterpolationCheckBox.Location = new System.Drawing.Point(3, 132);
            this.InterpolationCheckBox.Name = "InterpolationCheckBox";
            this.InterpolationCheckBox.Size = new System.Drawing.Size(105, 17);
            this.InterpolationCheckBox.TabIndex = 9;
            this.InterpolationCheckBox.Text = "Use interpolation";
            this.InterpolationCheckBox.UseVisualStyleBackColor = true;
            // 
            // ButtonDraw3D
            // 
            this.ButtonDraw3D.Location = new System.Drawing.Point(3, 155);
            this.ButtonDraw3D.Name = "ButtonDraw3D";
            this.ButtonDraw3D.Size = new System.Drawing.Size(141, 52);
            this.ButtonDraw3D.TabIndex = 10;
            this.ButtonDraw3D.Text = "Draw3D";
            this.ButtonDraw3D.UseVisualStyleBackColor = true;
            this.ButtonDraw3D.Click += new System.EventHandler(this.ButtonDraw3D_Click);
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(3, 213);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(120, 20);
            this.numericUpDown1.TabIndex = 11;
            // 
            // ButtonStart
            // 
            this.ButtonStart.Location = new System.Drawing.Point(3, 239);
            this.ButtonStart.Name = "ButtonStart";
            this.ButtonStart.Size = new System.Drawing.Size(141, 52);
            this.ButtonStart.TabIndex = 12;
            this.ButtonStart.Text = "Start";
            this.ButtonStart.UseVisualStyleBackColor = true;
            this.ButtonStart.Click += new System.EventHandler(this.ButtonStart_Click);
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
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(757, 534);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel DrawArea;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button ButtonLightPos;
        private System.Windows.Forms.Button ColorPicker;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.CheckBox InterpolationCheckBox;
        private System.Windows.Forms.Button ButtonDraw3D;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button ButtonStart;
        private System.Windows.Forms.Label labelFps;
        private System.Windows.Forms.Timer timer2;
    }
}

