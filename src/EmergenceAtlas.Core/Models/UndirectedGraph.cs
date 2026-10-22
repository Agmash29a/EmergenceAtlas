namespace EmergenceAtlas.Core.Models;

/// <summary>
/// Simple undirected graph modelling the connections among A particles.
/// A structural motif is a structural feature, not automatically emergence.
/// </summary>
public sealed class UndirectedGraph
{
    private readonly bool[,] adjacency;

    public int VertexCount { get; }
    public int EdgeCount { get; private set; }

    public UndirectedGraph(int vertexCount)
    {
        if (vertexCount < 1) throw new ArgumentOutOfRangeException(nameof(vertexCount));
        VertexCount = vertexCount;
        adjacency = new bool[vertexCount, vertexCount];
    }

    public void AddEdge(int a, int b)
    {
        ValidateVertex(a);
        ValidateVertex(b);
        if (a == b) throw new ArgumentException("Self-edges are not supported.");
        if (adjacency[a, b]) return;
        adjacency[a, b] = adjacency[b, a] = true;
        EdgeCount++;
    }

    public bool HasEdge(int a, int b)
    {
        ValidateVertex(a);
        ValidateVertex(b);
        return adjacency[a, b];
    }

    public int Degree(int vertex)
    {
        ValidateVertex(vertex);
        int count = 0;
        for (int v = 0; v < VertexCount; v++)
            if (adjacency[vertex, v]) count++;
        return count;
    }

    public bool IsConnected()
    {
        var visited = new bool[VertexCount];
        var queue = new Queue<int>();
        queue.Enqueue(0);
        visited[0] = true;
        int count = 1;
        while (queue.Count > 0)
        {
            int current = queue.Dequeue();
            for (int next = 0; next < VertexCount; next++)
            {
                if (!adjacency[current, next] || visited[next]) continue;
                visited[next] = true;
                count++;
                queue.Enqueue(next);
            }
        }
        return count == VertexCount;
    }

    public bool HasTriangle()
    {
        for (int a = 0; a < VertexCount; a++)
        for (int b = a + 1; b < VertexCount; b++)
        for (int c = b + 1; c < VertexCount; c++)
            if (adjacency[a, b] && adjacency[b, c] && adjacency[a, c]) return true;
        return false;
    }

    public bool HasCycle()
    {
        var visited = new bool[VertexCount];
        for (int start = 0; start < VertexCount; start++)
            if (!visited[start] && DepthFirstCycle(start, -1, visited)) return true;
        return false;
    }

    private bool DepthFirstCycle(int node, int parent, bool[] visited)
    {
        visited[node] = true;
        for (int next = 0; next < VertexCount; next++)
        {
            if (!adjacency[node, next]) continue;
            if (!visited[next])
            {
                if (DepthFirstCycle(next, node, visited)) return true;
            }
            else if (next != parent) return true;
        }
        return false;
    }

    /// <summary>Enumerate one graph for each assignment of edges to a labelled vertex set.</summary>
    public static UndirectedGraph FromEdgeMask(int vertexCount, int edgeMask)
    {
        if (vertexCount < 1 || vertexCount > 5)
            throw new ArgumentOutOfRangeException(nameof(vertexCount), "The exhaustive demo supports 1..5 vertices.");
        int edgeSlots = vertexCount * (vertexCount - 1) / 2;
        if (edgeMask < 0 || edgeMask >= (1 << edgeSlots))
            throw new ArgumentOutOfRangeException(nameof(edgeMask));
        var graph = new UndirectedGraph(vertexCount);
        int bit = 0;
        for (int a = 0; a < vertexCount; a++)
        for (int b = a + 1; b < vertexCount; b++, bit++)
            if ((edgeMask & (1 << bit)) != 0) graph.AddEdge(a, b);
        return graph;
    }

    /// <summary>
    /// Canonical upper-triangle adjacency under every permutation of vertex labels.
    /// Equal signatures mean isomorphic for this exhaustive small-n implementation.
    /// </summary>
    public string CanonicalSignature()
    {
        var permutation = Enumerable.Range(0, VertexCount).ToArray();
        string? smallest = null;
        Permute(0);
        return smallest!;

        void Permute(int position)
        {
            if (position == permutation.Length)
            {
                var representation = new char[VertexCount * (VertexCount - 1) / 2];
                int bit = 0;
                for (int a = 0; a < VertexCount; a++)
                for (int b = a + 1; b < VertexCount; b++)
                    representation[bit++] = adjacency[permutation[a], permutation[b]] ? '1' : '0';
                var signature = new string(representation);
                if (smallest is null || string.CompareOrdinal(signature, smallest) < 0)
                    smallest = signature;
                return;
            }
            for (int next = position; next < permutation.Length; next++)
            {
                (permutation[position], permutation[next]) = (permutation[next], permutation[position]);
                Permute(position + 1);
                (permutation[position], permutation[next]) = (permutation[next], permutation[position]);
            }
        }
    }

    private void ValidateVertex(int vertex)
    {
        if (vertex < 0 || vertex >= VertexCount) throw new ArgumentOutOfRangeException(nameof(vertex));
    }
}
