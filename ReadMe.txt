
1.
Path‐finding algorithms differ in how they explore the search space, what cost metrics they use, and how they prioritise nodes. Below are common algorithms and their behaviours:

• Breadth-First Search (BFS)

Keeps a queue of frontier nodes (nodes discovered but not processed).

Expands in “layers” (all nodes at distance d from the start are expanded before any node at distance d+1).

Since each step is treated equally (unweighted grid), every edge cost = constant, so BFS guarantees the shortest number of steps path (in terms of hops) if no weights.

No special heuristics: prioritisation is purely distance from start (by number of expansions).

Good when all moves cost the same and grid is moderate size.

• Dijkstra’s Algorithm

Similar to BFS but supports weighted edges (non‐uniform cost).

Maintains a priority queue (min‐heap) keyed by cost so far (distance from start to node).

At each step extracts the node with lowest cost, relaxes its neighbours (updating their cost if a cheaper path is found).

Guarantees the lowest total cost path (if all weights are non‐negative).

Prioritisation: lowest g(n) cost (actual cost from start to n).

Doesn’t use a heuristic (so it explores many unnecessary nodes if target is far).

• A* (A-star) Search

Enhances Dijkstra by adding a heuristic h(n) estimating cost from node n to goal.

Maintains f(n) = g(n) + h(n).

g(n) = cost from start to n.

h(n) = estimated cost from n to goal (must be admissible to guarantee optimality).

Extracts the node with lowest f(n) from the frontier.

Prioritisation: nodes that appear in the best trade-off of known cost + heuristic estimate.

If the heuristic is good (close to actual cost), A* will explore far fewer nodes than Dijkstra and reach the goal faster.

On a uniform grid with Manhattan or Euclidean heuristics, A* is the usual choice.


2.
When the obstacles in the grid change while the pathfinding agent is operating (e.g., moving obstacles, newly blocked cells, emerging hazards), the following issues arise:

Invalidated paths: A path that was valid when computed may become blocked or sub-optimal if an obstacle appears/departs.

Recomputation cost: Re-running the full search from scratch each time an obstacle changes can be expensive (especially on large grids).

Latency / responsiveness: If you wait until an obstacle change happens then compute a full new path, the agent may stall or behave unnaturally.

Partial updates / incremental search: Need algorithms that can update the search result rather than recompute everything (e.g., incremental A*, D*).

Consistency issues: If multiple agents are computing paths or sharing the grid state, dynamic changes can lead to race conditions or inconsistent views.

Heuristic invalidation: In A*, if obstacles appear, the previously estimated cost-to-goal may no longer be admissible or optimal leading to suboptimality unless re-handled.

Performance vs smoothness trade-off: Frequent updates might degrade performance, but infrequent updates degrade path quality / realism.

Memory / frontier retention: Some algorithms may hold large frontiers or visited sets, which might need clearing or patching when the grid changes.

Agent behaviour and replanning frequency: Deciding when and how often to replan (every obstacle change? periodic? only when path is blocked?) is a design challenge.

3.
For large/open‐world or frequently changing environments → Use A* with optimisation (pruning, hierarchical methods) or specialised algorithms for dynamic replanning (D* Lite, etc).
Larger or open-world grids pose scale and performance challenges.

Heuristic tuning: For A*, choose a heuristic that is as close as possible to the true cost (but admissible), to minimise node expansion. On a grid, Manhattan/Euclidean heuristics are good starting points.

Hierarchical pathfinding: Break the world into regions or clusters; compute high-level path between regions first, then local path within region. This reduces the search space.

Navigation meshes or waypoint graphs: Instead of searching on every grid cell, abstract the world into nodes (waypoints) and edges connecting them, reducing nodes significantly.

Bounding search area: Limit the search to a “window” around the agent/goal rather than the whole world.

Dynamic resolution / multi-resolution grids: Use coarse grids for long-range planning, then refine locally.

Incremental search / reuse search results: If many path queries share similar start/goal or obstacles change slowly, reuse prior computation and update only what changed.

Parallelism / asynchronous planning: If you have multiple agents, run planning on background threads, or spread search across frames.

Memory and data structure optimisation: Use efficient open/closed sets, spatial hashing, bit-grids or compressed representations for large worlds.

Prune unreachable areas: Precompute “reachability” or “clusters” to avoid exploring disconnected regions.

Agent avoidance & multi-agent considerations: In open‐world with many agents, you may also need dynamic avoidance, so planning must integrate with steering behaviours rather than pure grid pathfinding.

4.
When you introduce cell weights (cells that cost more to traverse: e.g., muddy ground, water, rough terrain), you need to adjust your algorithm and cost model accordingly:

Cost model: Each cell/edge has an associated traversal cost (for instance, moving into a “difficult terrain” cell might cost 2 or 5 instead of 1).

Algorithm choice: Weighted cost means BFS (which assumes equal cost) is not suitable for optimal results. Use Dijkstra or A* (with heuristic that considers weights).

Heuristic adaptation: For A*, your heuristic must remain admissible (i.e., never overestimate the true minimal cost) and typically must consider the minimum possible cost through the hardest terrain. Example: if the worst‐case cost per cell is w_max, you can adjust your heuristic: h(n) = (distance(n, goal) * cost_min_per_move).

Cost accumulation: g(n) must accumulate the weight values for each move. The priority queue should order by f(n) = g(n) + h(n).

Grid representation: Store the weight for each cell (or movement between cells). When expanding a node you add the cell’s cost.

Dynamic weighting: If terrain cost may change (e.g., becomes more difficult due to weather), you may need dynamic replanning.

Multiple difficulty levels: If some terrain is impassable (infinite cost) vs. some are just costly, you treat impassable as obstacles and exclude from expansions.

Speed vs optimality tradeoff: If weights vary widely, heuristic may lose accuracy, and A* may degrade toward Dijkstra in performance. Consider more aggressive pruning or hierarchical abstraction.

Visualisation / debugging: It helps to visualise the terrain cost map so you can see how your algorithm deals with high‐cost areas.

Tuning: Sometimes you might approximate weights (e.g., discretise into few cost categories) to keep performance manageable.