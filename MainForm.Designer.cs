using System.Windows.Forms.DataVisualization.Charting;

namespace MapRoutingGUI
{
    partial class MainForm
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
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea4 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend4 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            chartMap = new System.Windows.Forms.DataVisualization.Charting.Chart();
            splitContainer1 = new SplitContainer();
            splitContainer2 = new SplitContainer();
            groupBox1 = new GroupBox();
            btnSaveMapImage = new Button();
            btnRunAllQueries = new Button();
            btnLoadQueries = new Button();
            btnLoadMap = new Button();
            lstQueries = new ListBox();
            groupBox2 = new GroupBox();
            txtOutput = new TextBox();
            chartPerformance = new System.Windows.Forms.DataVisualization.Charting.Chart();
            lblTotalExecutionTime = new Label();
            toolTip = new ToolTip(components);
            ((System.ComponentModel.ISupportInitialize)chartMap).BeginInit();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer2).BeginInit();
            splitContainer2.Panel1.SuspendLayout();
            splitContainer2.Panel2.SuspendLayout();
            splitContainer2.SuspendLayout();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)chartPerformance).BeginInit();
            SuspendLayout();
            // 
            // chartMap
            // 
            chartArea3.Name = "ChartArea1";
            chartMap.ChartAreas.Add(chartArea3);
            chartMap.Dock = DockStyle.Fill;
            legend3.Name = "Legend1";
            chartMap.Legends.Add(legend3);
            chartMap.Location = new Point(0, 0);
            chartMap.Margin = new Padding(4, 5, 4, 5);
            chartMap.Name = "chartMap";
            chartMap.Size = new Size(993, 1055);
            chartMap.TabIndex = 0;
            chartMap.Text = "Map";
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Margin = new Padding(4, 5, 4, 5);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(chartMap);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(splitContainer2);
            splitContainer1.Size = new Size(1579, 1055);
            splitContainer1.SplitterDistance = 993;
            splitContainer1.SplitterWidth = 5;
            splitContainer1.TabIndex = 1;
            // 
            // splitContainer2
            // 
            splitContainer2.Dock = DockStyle.Fill;
            splitContainer2.Location = new Point(0, 0);
            splitContainer2.Margin = new Padding(4, 5, 4, 5);
            splitContainer2.Name = "splitContainer2";
            splitContainer2.Orientation = Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            splitContainer2.Panel1.Controls.Add(groupBox1);
            // 
            // splitContainer2.Panel2
            // 
            splitContainer2.Panel2.Controls.Add(groupBox2);
            splitContainer2.Size = new Size(581, 1055);
            splitContainer2.SplitterDistance = 526;
            splitContainer2.SplitterWidth = 6;
            splitContainer2.TabIndex = 0;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(btnSaveMapImage);
            groupBox1.Controls.Add(btnRunAllQueries);
            groupBox1.Controls.Add(btnLoadQueries);
            groupBox1.Controls.Add(btnLoadMap);
            groupBox1.Controls.Add(lstQueries);
            groupBox1.Dock = DockStyle.Fill;
            groupBox1.Location = new Point(0, 0);
            groupBox1.Margin = new Padding(4, 5, 4, 5);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new Padding(4, 5, 4, 5);
            groupBox1.Size = new Size(581, 526);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Queries";
            // 
            // btnSaveMapImage
            // 
            btnSaveMapImage.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnSaveMapImage.Location = new Point(473, 481);
            btnSaveMapImage.Margin = new Padding(4, 5, 4, 5);
            btnSaveMapImage.Name = "btnSaveMapImage";
            btnSaveMapImage.Size = new Size(100, 35);
            btnSaveMapImage.TabIndex = 4;
            btnSaveMapImage.Text = "Save Map";
            btnSaveMapImage.UseVisualStyleBackColor = true;
            // 
            // btnRunAllQueries
            // 
            btnRunAllQueries.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnRunAllQueries.Location = new Point(8, 481);
            btnRunAllQueries.Margin = new Padding(4, 5, 4, 5);
            btnRunAllQueries.Name = "btnRunAllQueries";
            btnRunAllQueries.Size = new Size(133, 35);
            btnRunAllQueries.TabIndex = 3;
            btnRunAllQueries.Text = "Run All Queries";
            btnRunAllQueries.UseVisualStyleBackColor = true;
            btnRunAllQueries.Click += btnRunAllQueries_Click;
            // 
            // btnLoadQueries
            // 
            btnLoadQueries.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnLoadQueries.Location = new Point(197, 483);
            btnLoadQueries.Margin = new Padding(4, 5, 4, 5);
            btnLoadQueries.Name = "btnLoadQueries";
            btnLoadQueries.Size = new Size(120, 35);
            btnLoadQueries.TabIndex = 2;
            btnLoadQueries.Text = "Load Queries";
            btnLoadQueries.UseVisualStyleBackColor = true;
            btnLoadQueries.Click += btnLoadQueries_Click;
            // 
            // btnLoadMap
            // 
            btnLoadMap.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnLoadMap.Location = new Point(352, 483);
            btnLoadMap.Margin = new Padding(4, 5, 4, 5);
            btnLoadMap.Name = "btnLoadMap";
            btnLoadMap.Size = new Size(113, 35);
            btnLoadMap.TabIndex = 1;
            btnLoadMap.Text = "Load Map";
            btnLoadMap.UseVisualStyleBackColor = true;
            btnLoadMap.Click += btnLoadMap_Click;
            // 
            // lstQueries
            // 
            lstQueries.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lstQueries.FormattingEnabled = true;
            lstQueries.Location = new Point(8, 29);
            lstQueries.Margin = new Padding(4, 5, 4, 5);
            lstQueries.Name = "lstQueries";
            lstQueries.Size = new Size(563, 444);
            lstQueries.TabIndex = 0;
            // lstQueries.SelectedIndexChanged += new EventHandler(lstQueries_SelectedIndexChanged);
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(txtOutput);
          
            groupBox2.Controls.Add(chartPerformance);
            groupBox2.Controls.Add(lblTotalExecutionTime);
            groupBox2.Dock = DockStyle.Fill;
            groupBox2.Location = new Point(0, 0);
            groupBox2.Margin = new Padding(4, 5, 4, 5);
            groupBox2.Name = "groupBox2";
            groupBox2.Padding = new Padding(4, 5, 4, 5);
            groupBox2.Size = new Size(581, 523);
            groupBox2.TabIndex = 0;
            groupBox2.TabStop = false;
            groupBox2.Text = "Output";
            // 
            // txtOutput
            // 
            txtOutput.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtOutput.Location = new Point(8, 29);
            txtOutput.Margin = new Padding(4, 5, 4, 5);
            txtOutput.Multiline = true;
            txtOutput.Name = "txtOutput";
            txtOutput.ReadOnly = true;
            txtOutput.ScrollBars = ScrollBars.Vertical;
            txtOutput.Size = new Size(563, 152);
            txtOutput.TabIndex = 2;
            // 
   
            // 
            // chartPerformance
            // 
            chartPerformance.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            chartArea4.Name = "ChartArea1";
            chartPerformance.ChartAreas.Add(chartArea4);
            legend4.Name = "Legend1";
            chartPerformance.Legends.Add(legend4);
            chartPerformance.Location = new Point(8, 237);
            chartPerformance.Margin = new Padding(4, 5, 4, 5);
            chartPerformance.Name = "chartPerformance";
            chartPerformance.Size = new Size(565, 226);
            chartPerformance.TabIndex = 0;
            chartPerformance.Text = "Performance";
            // 
            // lblTotalExecutionTime
            // 
            lblTotalExecutionTime.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblTotalExecutionTime.AutoSize = true;
            lblTotalExecutionTime.Location = new Point(8, 468);
            lblTotalExecutionTime.Margin = new Padding(4, 0, 4, 0);
            lblTotalExecutionTime.Name = "lblTotalExecutionTime";
            lblTotalExecutionTime.Size = new Size(185, 20);
            lblTotalExecutionTime.TabIndex = 3;
            lblTotalExecutionTime.Text = "Total Execution Time: 0 ms";
            // 
            // toolTip
            // 
            toolTip.ToolTipTitle = "Node Information";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1579, 1055);
            Controls.Add(splitContainer1);
            Margin = new Padding(4, 5, 4, 5);
            Name = "MainForm";
            Text = "Map Routing GUI";
            ((System.ComponentModel.ISupportInitialize)chartMap).EndInit();
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            splitContainer2.Panel1.ResumeLayout(false);
            splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer2).EndInit();
            splitContainer2.ResumeLayout(false);
            groupBox1.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)chartPerformance).EndInit();
            ResumeLayout(false);

        }


   

        private System.Windows.Forms.DataVisualization.Charting.Chart chartMap;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListBox lstQueries;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartPerformance;
        private System.Windows.Forms.Button btnLoadMap;
        private System.Windows.Forms.Button btnLoadQueries;
        private System.Windows.Forms.Button btnRunAllQueries;
        private System.Windows.Forms.TextBox txtOutput;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Button btnSaveMapImage;
        private System.Windows.Forms.Label lblTotalExecutionTime; // Declare the label here
    }
}