using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace MapRoutingGUI
{
    public static class MapRoutingLogic
    {
        public static (long processingMs, long totalMs) RunRoutingWithTiming(string mapFilePath, string queryFilePath)
        {
            var totalStopwatch = Stopwatch.StartNew();
            List<string> outputLines = new List<string>();

            //o(n+m)
            //n: num of nodes
            //m:num of edges
            var (nodes, adjDict, nodeIdToIndex, indexToNodeId, roadLengths) = ReadMapFile(mapFilePath);
            var queries = ReadQueriesFile(queryFilePath);

            var processingStopwatch = Stopwatch.StartNew();

            foreach (var (sourceX, sourceY, destX, destY, R) in queries)
            {
                List<int> S = new();
                Dictionary<int, double> walkTimeS = new();
                //o(n)
                // minim from begin
                foreach (var nodeId in nodes.Keys)
                {
                    double distKm = Distance(sourceX, sourceY, nodes[nodeId].X, nodes[nodeId].Y);
                    if (distKm * 1000 <= R)
                    {
                        S.Add(nodeId);
                        walkTimeS[nodeId] = (distKm / 5.0) * 60.0;
                    }
                }

                //o(n)
                //minim to end
                List<int> F = new();
                Dictionary<int, double> walkTimeF = new();
                foreach (var nodeId in nodes.Keys)
                {
                    double distKm = Distance(destX, destY, nodes[nodeId].X, nodes[nodeId].Y);
                    if (distKm * 1000 <= R)
                    {
                        F.Add(nodeId);
                        walkTimeF[nodeId] = (distKm / 5.0) * 60.0;
                    }
                }

                if (S.Count == 0 || F.Count == 0)
                {
                    outputLines.AddRange(Enumerable.Repeat("", 6));
                    continue;
                }

                //initialize the dijistra params (only declaration)
                int n = indexToNodeId.Count;
                double[] dist = Enumerable.Repeat(double.PositiveInfinity, n).ToArray();
                int[] prev = Enumerable.Repeat(-1, n).ToArray();
                var pq = new PriorityQueue<int, double>();

                //initialize the dijistra params 
                foreach (var s in S)
                {
                    int idx = nodeIdToIndex[s];
                    dist[idx] = walkTimeS[s];
                    pq.Enqueue(idx, dist[idx]);
                }

                //o((v+e)log(v))
                while (pq.TryDequeue(out int u, out double d))
                {
                    if (d > dist[u]) continue;
                    //o((v+e)log(v))
                    foreach (var (v, time, _) in adjDict[u])
                    {
                        double newDist = dist[u] + time;
                        if (newDist < dist[v])
                        {
                            dist[v] = newDist;
                            prev[v] = u;
                            pq.Enqueue(v, newDist);
                        }
                    }
                }

                //o(n)
                double minTime = double.PositiveInfinity;
                int bestF = -1;
                foreach (var f in F)
                {
                    int idx = nodeIdToIndex[f];
                    double totalTime = dist[idx] + walkTimeF[f];
                    if (totalTime < minTime)
                    {
                        minTime = totalTime;
                        bestF = idx;
                    }
                }

                if (bestF == -1 || double.IsInfinity(minTime))
                {
                    outputLines.AddRange(Enumerable.Repeat("", 6));
                    continue;
                }


                List<int> path = new();
                // o(n)
                for (int u = bestF; u != -1; u = prev[u])
                    path.Add(u);
                path.Reverse();

                List<int> pathNodeIds = path.Select(i => indexToNodeId[i]).ToList();

                double vehicleDist = 0;
                //o(m)
                for (int i = 0; i < path.Count - 1; i++)
                {
                    int u = path[i], v = path[i + 1];
                    vehicleDist += roadLengths[u][v];
                }

                double walkStart = walkTimeS[indexToNodeId[path[0]]] / 60 * 5;
                double walkEnd = walkTimeF[indexToNodeId[bestF]] / 60 * 5;
                double walkDist = walkStart + walkEnd;
                double totalDist = walkDist + vehicleDist;

                outputLines.Add(string.Join(" ", pathNodeIds));
                outputLines.Add($"{minTime:F2} mins");
                outputLines.Add($"{totalDist:F2} km");
                outputLines.Add($"{walkDist:F2} km");
                outputLines.Add($"{vehicleDist:F2} km");
                outputLines.Add("");
            }

            processingStopwatch.Stop();

            outputLines.Add($"{processingStopwatch.ElapsedMilliseconds} ms");
            outputLines.Add("");
            totalStopwatch.Stop();
            outputLines.Add($"{totalStopwatch.ElapsedMilliseconds} ms");

            File.WriteAllLines("MyOutput.txt", outputLines);

            return (processingStopwatch.ElapsedMilliseconds, totalStopwatch.ElapsedMilliseconds);
        }

        public static double Distance(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
        }

        public static (
            Dictionary<int, (double X, double Y)> nodes,
            List<List<(int, double, double)>> adjDict,
            Dictionary<int, int> nodeIdToIndex,
            List<int> indexToNodeId,
            Dictionary<int, Dictionary<int, double>> roadLengths
        ) ReadMapFile(string filePath)
        {
            var lines = File.ReadAllLines(filePath);
            int n = int.Parse(lines[0]);

            var nodes = new Dictionary<int, (double X, double Y)>();
            var nodeIdToIndex = new Dictionary<int, int>();
            var indexToNodeId = new List<int>();

            //o(n)
            for (int i = 1; i <= n; i++)
            {
                var parts = lines[i].Split();
                int id = int.Parse(parts[0]);
                double x = double.Parse(parts[1]);
                double y = double.Parse(parts[2]);
                nodes[id] = (x, y);
                nodeIdToIndex[id] = indexToNodeId.Count;
                indexToNodeId.Add(id);
            }

            int m = int.Parse(lines[n + 1]);
            var adjDict = new List<List<(int, double, double)>>(n);
            var roadLengths = new Dictionary<int, Dictionary<int, double>>();

            //o(n)
            for (int i = 0; i < n; i++)
            {
                adjDict.Add(new List<(int, double, double)>());
                roadLengths[i] = new Dictionary<int, double>();
            }

            //o(m)
            for (int i = n + 2; i < n + 2 + m; i++)
            {
                var parts = lines[i].Split();
                int u = int.Parse(parts[0]);
                int v = int.Parse(parts[1]);
                double length = double.Parse(parts[2]);
                double speed = double.Parse(parts[3]);
                double time = (length / speed) * 60;

                int uIdx = nodeIdToIndex[u];
                int vIdx = nodeIdToIndex[v];

                adjDict[uIdx].Add((vIdx, time, length));
                adjDict[vIdx].Add((uIdx, time, length));

                roadLengths[uIdx][vIdx] = length;
                roadLengths[vIdx][uIdx] = length;
            }

            return (nodes, adjDict, nodeIdToIndex, indexToNodeId, roadLengths);
        }

        public static List<(double, double, double, double, int)> ReadQueriesFile(string filePath)
        {
            var lines = File.ReadAllLines(filePath);
            int q = int.Parse(lines[0]);
            var queries = new List<(double, double, double, double, int)>();

            //o(q)
            for (int i = 1; i <= q; i++)
            {
                var parts = lines[i].Split();
                double sx = double.Parse(parts[0]);
                double sy = double.Parse(parts[1]);
                double dx = double.Parse(parts[2]);
                double dy = double.Parse(parts[3]);
                int r = int.Parse(parts[4]);
                queries.Add((sx, sy, dx, dy, r));
            }

            return queries;
        }
    }
}