// MainForm.cs
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace MapRoutingGUI
{
    public partial class MainForm : Form
    {
        private string mapFilePath;
        private string queryFilePath;

        private Dictionary<int, (double X, double Y)> nodes;
        private List<List<(int v, double time, double length)>> adjDict;
        private Dictionary<int, int> nodeIdToIndex;
        private List<int> indexToNodeId;
        private Dictionary<int, Dictionary<int, double>> roadLengths;

        private List<(double sx, double sy, double dx, double dy, int r)> queries;

        private Color[] pathColors = new Color[]
        {
            Color.Red, Color.Blue, Color.Green, Color.Orange, Color.Purple,
            Color.Brown, Color.Cyan, Color.Magenta, Color.LimeGreen, Color.DarkGoldenrod
        };

        private ToolTip nodeTooltip = new ToolTip();
        private Dictionary<DataPoint, int> pointToNodeId = new Dictionary<DataPoint, int>();

        public MainForm()
        {
            InitializeComponent();
            btnSaveMapImage.Click += btnSaveMapImage_Click;
            chartMap.MouseMove += ChartMap_MouseMove;
            lstQueries.SelectedIndexChanged += lstQueries_SelectedIndexChanged;
        }

        private void btnLoadMap_Click(object sender, EventArgs e)
        {
            using OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Map files (*.txt)|*.txt|All files (*.*)|*.*";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                mapFilePath = dlg.FileName;
                LoadMap(mapFilePath);
                DrawMap();
                txtOutput.AppendText($"Loaded map: {Path.GetFileName(mapFilePath)}\r\n");
            }
        }

        private void LoadMap(string path)
        {
            (nodes, adjDict, nodeIdToIndex, indexToNodeId, roadLengths) = MapRoutingLogic.ReadMapFile(path);
        }

        private void btnLoadQueries_Click(object sender, EventArgs e)
        {
            using OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Query files (*.txt)|*.txt|All files (*.*)|*.*";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                queryFilePath = dlg.FileName;
                queries = MapRoutingLogic.ReadQueriesFile(queryFilePath);
                lstQueries.Items.Clear();
                for (int i = 0; i < queries.Count; i++)
                {
                    var q = queries[i];
                    lstQueries.Items.Add($"Query {i + 1}: ({q.sx}, {q.sy}) to ({q.dx}, {q.dy}), R={q.r}m");
                }
                txtOutput.AppendText($"Loaded queries: {Path.GetFileName(queryFilePath)}\r\n");
            }
        }

        private async void btnRunAllQueries_Click(object sender, EventArgs e)
        {
            if (mapFilePath == null || queryFilePath == null)
            {
                MessageBox.Show("Please load both map and queries files first.", "Missing data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            txtOutput.Clear();
            lstQueries.Enabled = false;
            btnRunAllQueries.Enabled = false;

            RemoveAllPathSeries();

            var (procMs, totalMs) = await Task.Run(() => MapRoutingLogic.RunRoutingWithTiming(mapFilePath, queryFilePath));

            lblTotalExecutionTime.Text = $"Total Execution Time: {totalMs} ms";

            var outputLines = File.ReadAllLines("MyOutput.txt");
            txtOutput.Lines = outputLines;

            chartPerformance.Series.Clear();
            var series = new Series("Processing Time") { ChartType = SeriesChartType.Column };
            series.Points.AddXY("Processing", procMs);
            series.Points.AddXY("Total", totalMs);
            chartPerformance.Series.Add(series);

            lstQueries.Enabled = true;
            btnRunAllQueries.Enabled = true;

            MessageBox.Show("All queries processed. Select a query to view its path.", "Completed", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void DrawMap()
        {
            if (nodes == null || nodes.Count == 0)
            {
                MessageBox.Show("No nodes loaded to draw.");
                return;
            }

            if (chartMap.ChartAreas.Count == 0)
                chartMap.ChartAreas.Add(new ChartArea("MainArea"));

            var area = chartMap.ChartAreas[0];

            area.AxisX.Minimum = nodes.Values.Min(n => n.X) - 0.01;
            area.AxisX.Maximum = nodes.Values.Max(n => n.X) + 0.01;
            area.AxisY.Minimum = nodes.Values.Min(n => n.Y) - 0.01;
            area.AxisY.Maximum = nodes.Values.Max(n => n.Y) + 0.01;

            chartMap.Series.Clear();

            var edgeSeries = new Series("Roads")
            {
                ChartType = SeriesChartType.Line,
                Color = Color.LightGray,
                BorderWidth = 1,
                MarkerStyle = MarkerStyle.None
            };

            var drawnEdges = new HashSet<(int, int)>();
            for (int u = 0; u < adjDict.Count; u++)
            {
                var (x1, y1) = nodes[indexToNodeId[u]];
                foreach (var (v, _, _) in adjDict[u])
                {
                    if (drawnEdges.Contains((v, u))) continue;
                    drawnEdges.Add((u, v));

                    var (x2, y2) = nodes[indexToNodeId[v]];
                    edgeSeries.Points.AddXY(x1, y1);
                    edgeSeries.Points.AddXY(x2, y2);
                }
            }
            chartMap.Series.Add(edgeSeries);

            var nodeSeries = new Series("Nodes")
            {
                ChartType = SeriesChartType.Point,
                Color = Color.DarkBlue,
                MarkerSize = 10,
                MarkerStyle = MarkerStyle.Circle
            };

            pointToNodeId.Clear();

            foreach (var nodeId in nodes.Keys)
            {
                var (x, y) = nodes[nodeId];
                int pointIdx = nodeSeries.Points.AddXY(x, y);
                pointToNodeId[nodeSeries.Points[pointIdx]] = nodeId;
            }
            chartMap.Series.Add(nodeSeries);
        }

        private void ChartMap_MouseMove(object sender, MouseEventArgs e)
        {
            var hit = chartMap.HitTest(e.X, e.Y);
            if (hit.Series != null && hit.Series.Name == "Nodes" && hit.PointIndex >= 0)
            {
                if (pointToNodeId.TryGetValue(hit.Series.Points[hit.PointIndex], out int nodeId))
                {
                    var (x, y) = nodes[nodeId];
                    nodeTooltip.Show($"Node ID: {nodeId}\nX: {x:F6}, Y: {y:F6}", chartMap, e.X + 15, e.Y + 15);
                    return;
                }
            }
            nodeTooltip.Hide(chartMap);
        }

        private void RemoveAllPathSeries()
        {
            var toRemove = chartMap.Series.Where(s => s.Name.StartsWith("PathQuery") || s.Name.StartsWith("Overlay")).ToList();
            foreach (var s in toRemove)
            {
                chartMap.Series.Remove(s);
            }
        }

        private void RemoveOverlaySeries()
        {
            var overlays = chartMap.Series.Where(s => s.Name.StartsWith("-Source") || s.Name.StartsWith("-Dest")).ToList();
            foreach (var s in overlays)
                chartMap.Series.Remove(s);
        }

        private async Task AnimatePath(List<int> pathNodeIds, Color pathColor, string seriesName)
        {
            var pathSeries = new Series(seriesName)
            {
                ChartType = SeriesChartType.Line,
                Color = pathColor,
                BorderWidth = 3,
                MarkerStyle = MarkerStyle.Circle,
                MarkerSize = 7,
                MarkerColor = pathColor
            };

            chartMap.Series.Add(pathSeries);

            foreach (var nodeId in pathNodeIds)
            {
                var (x, y) = nodes[nodeId];
                pathSeries.Points.AddXY(x, y);
                await Task.Delay(150);
            }
        }

        private class PathResult
        {
            public List<int> PathNodeIds { get; set; }
            public double MinTime { get; set; }
            public double TotalDist { get; set; }
            public double WalkDist { get; set; }
            public double VehicleDist { get; set; }
        }

        private PathResult CalculateSingleQueryPath((double sx, double sy, double dx, double dy, int r) query)
        {
            int startNode = FindClosestNode(query.sx, query.sy);
            int destNode = FindClosestNode(query.dx, query.dy);

            var prev = new int[adjDict.Count];
            var dist = new double[adjDict.Count];
            for (int i = 0; i < dist.Length; i++) dist[i] = double.PositiveInfinity;
            dist[startNode] = 0;

            var pq = new PriorityQueue<int, double>();
            pq.Enqueue(startNode, 0);

            while (pq.Count > 0)
            {
                int u = pq.Dequeue();
                if (u == destNode) break;

                foreach (var (v, time, _) in adjDict[u])
                {
                    double alt = dist[u] + time;
                    if (alt < dist[v])
                    {
                        dist[v] = alt;
                        prev[v] = u;
                        pq.Enqueue(v, alt);
                    }
                }
            }

            if (double.IsInfinity(dist[destNode]))
            {
                return new PathResult { PathNodeIds = new List<int>() };
            }

            List<int> path = new();
            for (int at = destNode; at != startNode; at = prev[at])
            {
                path.Add(indexToNodeId[at]);
            }
            path.Add(indexToNodeId[startNode]);
            path.Reverse();

            return new PathResult
            {
                PathNodeIds = path,
                MinTime = dist[destNode],
                TotalDist = 0,
                WalkDist = 0,
                VehicleDist = 0
            };
        }

        private int FindClosestNode(double x, double y)
        {
            double minDist = double.PositiveInfinity;
            int closestIndex = -1;
            for (int i = 0; i < indexToNodeId.Count; i++)
            {
                var nodeId = indexToNodeId[i];
                var (nx, ny) = nodes[nodeId];
                double dx = nx - x;
                double dy = ny - y;
                double dist = dx * dx + dy * dy;
                if (dist < minDist)
                {
                    minDist = dist;
                    closestIndex = i;
                }
            }
            return closestIndex;
        }

        private async void lstQueries_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstQueries.SelectedIndex < 0) return;
            if (nodes == null || adjDict == null) return;

            RemoveAllPathSeries();
            RemoveOverlaySeries();

            var query = queries[lstQueries.SelectedIndex];
            var pathResult = CalculateSingleQueryPath(query);

            if (pathResult.PathNodeIds.Count == 0)
            {
                MessageBox.Show("No path found for the selected query.", "No Path", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            AddCircleMarker("-Source", query.sx, query.sy, Color.Red, 6);
            AddCircleMarker("-Dest", query.dx, query.dy, Color.Blue, 6);
            AddCircleOverlay("-SourceRadius", query.sx, query.sy, query.r, Color.FromArgb(80, Color.Red));
            AddCircleOverlay("-DestRadius", query.dx, query.dy, query.r, Color.FromArgb(80, Color.Blue));

            await AnimatePath(pathResult.PathNodeIds, pathColors[lstQueries.SelectedIndex % pathColors.Length], $"PathQuery{lstQueries.SelectedIndex}");

            txtOutput.AppendText($"\r\nDisplayed path for Query {lstQueries.SelectedIndex + 1}\r\n");
        }

        private void AddCircleMarker(string name, double x, double y, Color color, int size)
        {
            if (!chartMap.Series.IsUniqueName(name))
                chartMap.Series.Remove(chartMap.Series[name]);

            var s = new Series(name)
            {
                ChartType = SeriesChartType.Point,
                MarkerStyle = MarkerStyle.Circle,
                MarkerSize = size,
                Color = color
            };
            s.Points.AddXY(x, y);
            chartMap.Series.Add(s);
        }

        private void AddCircleOverlay(string name, double centerX, double centerY, int radiusMeters, Color fillColor)
        {
            if (!chartMap.Series.IsUniqueName(name))
                chartMap.Series.Remove(chartMap.Series[name]);

            double radiusDegrees = radiusMeters / 111000.0;
            var s = new Series(name)
            {
                ChartType = SeriesChartType.Point,
                MarkerStyle = MarkerStyle.Circle,
                MarkerSize = (int)(radiusDegrees * 10000),
                Color = fillColor
            };
            s.Points.AddXY(centerX, centerY);
            chartMap.Series.Add(s);
        }

        private void btnSaveMapImage_Click(object sender, EventArgs e)
        {
            using SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "PNG Image (*.png)|*.png";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                using var bmp = new Bitmap(chartMap.Width, chartMap.Height);
                chartMap.DrawToBitmap(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height));
                bmp.Save(dlg.FileName, System.Drawing.Imaging.ImageFormat.Png);
                MessageBox.Show("Map image saved successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
