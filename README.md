# 🗺️ Map Routing Engine

A high-performance route computation engine that determines the **fastest path** between two geographic coordinates using a **graph-based road network**. The system supports combined **walking and vehicle travel**, visualizes paths with a WinForms interface, and handles large-scale queries efficiently.

---

## 🔍 Key Features

- Fastest route using walking + vehicle
- Real-world walking limits (radius `R`)
- Support for roads with **changing speeds** over time
- Map + path visualization with GUI
- Execution time tracking and analysis chart

---

## 📂 Project Structure

| Component         | Description                                       |
|------------------|---------------------------------------------------|
| `MapRoutingGUI`   | WinForms frontend for file input & path display   |
| `MapRoutingLogic` | Core route-finding algorithm using Dijkstra's     |
| `docs/`           | Presentation and documentation                    |

---

## 📄 Input Format

### 📌 Map File
```
N
ID X Y
...
M
ID1 ID2 Length(km) Speed(km/h)
...
```

### 📌 Query File
```
Q
SourceX SourceY DestX DestY R
...
```

---

## 📄 Output Format

Each query returns:
1. List of intersection IDs in path
2. Total travel time (in minutes)
3. Total distance (km)
4. Walking distance (km)
5. Vehicle distance (km)
6. Execution time in ms

---

## 🖼️ GUI Highlights

![Default View](docs/images/Screenshot_01.png)
*Default view after loading map and queries.*

![Query Highlighted](docs/images/Screenshot_02.png)
*Highlighted path result for selected query.*

- Load map and queries
- Run all or individual queries
- Path overlay with legend
- Bar chart for execution times
- Toggle map visual elements

---

## 🧪 Test Scenarios

- Sample: ≤ 20 nodes, 5 queries
- Medium: ≤ 20,000 nodes, 1,000 queries
- Large: ≤ 200,000 nodes, 1,000 queries

---

## 🧠 Algorithm Complexity

- Graph building: `O(|E|)`
- Shortest path: `O(S × E' × log V')`
  - `S` = candidate starts
  - `V'`, `E'` = reachable subgraph

---

## 🛠️ Technologies

- C# / .NET
- Windows Forms
- Stopwatch API
- Graphs + Priority Queue
- GDI+ for drawing


