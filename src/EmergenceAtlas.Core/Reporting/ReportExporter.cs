using System.Globalization;
using System.Net;
using System.Text;
using System.Text.Json;
using EmergenceAtlas.Core.Analysis;
using EmergenceAtlas.Core.Experiments;

namespace EmergenceAtlas.Core.Reporting;

/// <summary>Zero-dependency export: machine-readable data and a standalone browser report.</summary>
public sealed class ReportExporter
{
    private static readonly CultureInfo Invariant = CultureInfo.InvariantCulture;

    public void Export(string outputDirectory, IReadOnlyList<ExperimentResult> results)
    {
        Directory.CreateDirectory(outputDirectory);
        WriteCsv(Path.Combine(outputDirectory, "measurements.csv"), results.SelectMany(r => r.Rows));
        var jsonOptions = new JsonSerializerOptions { WriteIndented = true };
        File.WriteAllText(Path.Combine(outputDirectory, "atlas.json"),
            JsonSerializer.Serialize(new AtlasCatalogue().Definitions(), jsonOptions), Encoding.UTF8);
        File.WriteAllText(Path.Combine(outputDirectory, "findings.json"),
            JsonSerializer.Serialize(results.SelectMany(r => r.Findings ?? Array.Empty<PropertyFinding>()), jsonOptions), Encoding.UTF8);
        File.WriteAllText(Path.Combine(outputDirectory, "report.html"), BuildHtml(results), Encoding.UTF8);
    }

    private static void WriteCsv(string path, IEnumerable<ExperimentRow> rows)
    {
        using var writer = new StreamWriter(path, false, new UTF8Encoding(false));
        writer.WriteLine("Experiment,Variant,Particles,Parameter,Metric,Value,Unit,Interpretation");
        foreach (var row in rows)
        {
            writer.WriteLine(string.Join(",", new[]
            {
                Csv(row.Experiment), Csv(row.Variant), row.Particles.ToString(Invariant),
                Csv(row.Parameter), Csv(row.Metric), row.Value.ToString("G17", Invariant),
                Csv(row.Unit), Csv(row.Interpretation)
            }));
        }
    }

    private static string Csv(string value) =>
        "\"" + value.Replace("\"", "\"\"", StringComparison.Ordinal) + "\"";

    private static string BuildHtml(IReadOnlyList<ExperimentResult> results)
    {
        var html = new StringBuilder();
        html.Append("""
            <!doctype html>
            <html lang="en"><head><meta charset="utf-8"><meta name="viewport" content="width=device-width, initial-scale=1">
            <title>Emergence Atlas: experimental report</title>
            <style>
            :root{color-scheme:dark;font-family:system-ui,-apple-system,Segoe UI,Arial,sans-serif}
            body{margin:0;background:#0c1220;color:#e8eef9;line-height:1.55}
            main{max-width:1040px;margin:auto;padding:28px 22px 90px}
            h1{font-size:clamp(2rem,5vw,3.2rem);margin-bottom:.2em;letter-spacing:-.05em}
            h2{font-size:1.45rem;margin-top:3rem}h3{font-size:1.05rem}
            .lead{max-width:780px;color:#bdc9dd;font-size:1.08rem}
            .eyebrow{text-transform:uppercase;letter-spacing:.2em;color:#81b9f7;font-size:.75rem;font-weight:700}
            .panel{background:#151e31;border:1px solid #29374e;border-radius:16px;margin:18px 0;padding:20px;overflow:auto}
            .note{color:#bdc9dd}.em{color:#90d2ae}.axis{fill:#a6b9d1;font-size:12px}
            .gridline{stroke:#33445d;stroke-width:1}.legend{display:flex;gap:20px;flex-wrap:wrap;font-size:.86rem;color:#c4d0e1}
            .swatch{display:inline-block;width:11px;height:11px;margin-right:7px;border-radius:2px}
            table{width:100%;border-collapse:collapse;font-size:.86rem}th,td{padding:7px 10px;text-align:left;border-bottom:1px solid #29374e}
            th{color:#c2d8f4;white-space:nowrap}td.num{text-align:right;font-variant-numeric:tabular-nums}
            pre{font-size:.8rem;overflow:auto;background:#0c1424;padding:12px;border-radius:8px;line-height:1.4}
            details{margin-top:14px}summary{cursor:pointer;color:#9bcbfc;font-weight:650}
            code{background:#263047;padding:2px 4px;border-radius:4px}
            .footer{font-size:.88rem;color:#a6b9d1;margin-top:3rem}
            </style></head><body><main>
            <div class="eyebrow">Computational research prototype · reproducible experiments</div>
            <h1>An Atlas of Emergence</h1>
            <p class="lead">From counting collections to comparing structures, joint information, and collective dynamics. These are <strong>operational demonstrations</strong> — not a universal detector of emergence.</p>
            <div class="panel"><strong>Interpretation first.</strong> Different counting conventions give different numbers of entities. Graph motifs measure structural possibilities. XOR demonstrates joint-information gain. The finite Ising model illustrates collective order relative to an interaction-free control. None proves strong emergence.</div>
            """);

        html.Append("<section class='panel'><h2>Operational property detectors</h2><p class='note'>The criterion is an explicit decision rule, not a claim of strong emergence. An <strong>illustrative pass</strong> means the stated threshold was met in the experiment; a failed criterion is equally informative.</p><table><thead><tr><th>Candidate</th><th>Scenario</th><th>Result</th><th>Score</th><th>Threshold</th></tr></thead><tbody>");
        foreach (var finding in results.SelectMany(r => r.Findings ?? Array.Empty<PropertyFinding>()))
        {
            html.Append("<tr><td title='").Append(H(finding.Limitation)).Append("'>").Append(H(finding.PropertyName))
                .Append("</td><td>").Append(H(finding.Scenario)).Append("</td><td>")
                .Append(finding.CriterionMet ? "<span class='em'>Pass</span>" : "Not met")
                .Append("</td><td class='num'>").Append(F(finding.Score))
                .Append("</td><td class='num'>").Append(F(finding.Threshold)).Append("</td></tr>");
        }
        html.Append("</tbody></table><p class='note'>See <code>findings.json</code> for controls, full criteria and caveats.</p></section>");

        AddCountChart(results, html);
        AddGraphChart(results, html);
        AddInformationChart(results, html);
        AddIsingChart(results, html);

        foreach (var result in results)
        {
            html.Append("<section class='panel'><h2>").Append(H(result.Name)).Append(" experiment</h2><p class='note'>")
                .Append(H(result.Description)).Append("</p><details><summary>Console table and methodological notes</summary><pre>")
                .Append(H(result.Details)).Append("</pre></details><details><summary>All numeric observations</summary>")
                .Append("<table><thead><tr><th>Variant</th><th>n</th><th>Parameter</th><th>Metric</th><th>Value</th></tr></thead><tbody>");
            foreach (var row in result.Rows)
            {
                html.Append("<tr><td>").Append(H(row.Variant)).Append("</td><td class='num'>")
                    .Append(row.Particles.ToString(Invariant)).Append("</td><td>")
                    .Append(H(row.Parameter)).Append("</td><td>").Append(H(row.Metric))
                    .Append("</td><td class='num'>").Append(row.Value.ToString("0.######", Invariant))
                    .Append("</td></tr>");
            }
            html.Append("</tbody></table></details></section>");
        }
        html.Append("""
            <p class="footer">Data files: <code>measurements.csv</code> (raw numerical observations) and <code>atlas.json</code> (explicit hypotheses and controls), plus <code>findings.json</code> (detector results). Run more seeds and lattice sizes before making statistical or universality claims.</p>
            </main></body></html>
            """);
        return html.ToString();
    }

    private static void AddCountChart(IEnumerable<ExperimentResult> results, StringBuilder html)
    {
        var rows = results.FirstOrDefault(r => r.Name == "count")?.Rows;
        if (rows is null) return;
        var groups = rows.Select(r => r.Particles).Distinct().OrderBy(n => n).Select(n => n.ToString(Invariant)).ToArray();
        Plot(html, "1. Collections are not properties", "Three ways of counting the same n particles. The curves depend on the definition of 'entity'.",
            groups, new[]
            {
                Series("Floor-partition sum", "#72b9ed", rows, "MultiscaleCount"),
                Series("Size categories", "#83d5a8", rows, "SizeCategories")
            });
    }

    private static void AddGraphChart(IEnumerable<ExperimentResult> results, StringBuilder html)
    {
        var rows = results.FirstOrDefault(r => r.Name == "graphs")?.Rows;
        if (rows is null) return;
        var groups = rows.Select(r => r.Particles).Distinct().OrderBy(n => n).Select(n => n.ToString(Invariant)).ToArray();
        Plot(html, "2. Same ingredients, different structures", "Graphs related by relabelling are counted once. No physical properties are assumed.",
            groups, new[]
            {
                Series("Topologies", "#72b9ed", rows, "DistinctTopologies"),
                Series("Connected", "#83d5a8", rows, "ConnectedTopologies"),
                Series("With triangles", "#e9b46b", rows, "TriangleTopologies")
            });
    }

    private static void AddInformationChart(IEnumerable<ExperimentResult> results, StringBuilder html)
    {
        var rows = results.FirstOrDefault(r => r.Name == "information")?.Rows;
        if (rows is null) return;
        var groups = new[] { "CopyLeft", "And", "Xor" };
        Plot(html, "3. Joint information can exceed what either input provides", "Fair independent inputs; plotted values are mutual information about the output, in bits.",
            groups, new[]
            {
                SeriesByVariant("From A1", "#72b9ed", rows, groups, "LeftMutualInformation"),
                SeriesByVariant("From A2", "#83d5a8", rows, groups, "RightMutualInformation"),
                SeriesByVariant("From both", "#e9b46b", rows, groups, "JointMutualInformation")
            });
    }

    private static void AddIsingChart(IEnumerable<ExperimentResult> results, StringBuilder html)
    {
        var rows = results.FirstOrDefault(r => r.Name == "ising")?.Rows;
        if (rows is null) return;
        var observations = rows.Where(r => r.Metric == "MeanAbsMagnetization").ToArray();
        string[] groups = observations.Select(r => r.Parameter.Split(';')[0].Split('=')[1]).Distinct().ToArray();
        Plot(html, "4. Interactions change collective order", "Mean absolute magnetization vs temperature; a finite-size Monte Carlo comparison, not a proof of a phase transition.",
            groups, new[]
            {
                new PlotSeries("Independent J=0", "#72b9ed", groups.Select(t => GetIsing(observations, "Independent J=0", t)).ToArray()),
                new PlotSeries("Coupled J=1", "#e9b46b", groups.Select(t => GetIsing(observations, "Coupled J=1", t)).ToArray())
            }, 1);
    }

    private static double GetIsing(IEnumerable<ExperimentRow> rows, string variant, string temperature) =>
        rows.Single(row => row.Variant == variant && row.Parameter.StartsWith($"T={temperature};", StringComparison.Ordinal)).Value;

    private sealed record PlotSeries(string Name, string Colour, double[] Values);

    private static PlotSeries Series(string name, string colour, IEnumerable<ExperimentRow> rows, string metric) =>
        new(name, colour, rows.Where(r => r.Metric == metric).OrderBy(r => r.Particles).Select(r => r.Value).ToArray());

    private static PlotSeries SeriesByVariant(string name, string colour, IEnumerable<ExperimentRow> rows,
        IEnumerable<string> variants, string metric) =>
        new(name, colour, variants.Select(variant => rows.Single(row => row.Variant == variant && row.Metric == metric).Value).ToArray());

    private static void Plot(StringBuilder html, string title, string note, string[] groups,
        PlotSeries[] series, double? maximum = null)
    {
        if (groups.Length == 0 || series.Length == 0) return;
        const double width = 800, height = 300, left = 56, right = 18, top = 18, bottom = 55;
        double plotWidth = width - left - right, plotHeight = height - top - bottom;
        double peak = maximum ?? (series.SelectMany(s => s.Values).DefaultIfEmpty(1).Max() * 1.12);
        peak = Math.Max(peak, 0.01);
        double groupWidth = plotWidth / groups.Length;
        double barWidth = Math.Min(26, groupWidth * 0.78 / series.Length);
        html.Append("<section class='panel'><h2>").Append(H(title)).Append("</h2><p class='note'>").Append(H(note)).Append("</p>");
        html.Append("<svg role='img' aria-label='").Append(H(title)).Append("' viewBox='0 0 800 300' style='width:100%;height:auto'>");
        for (int step = 0; step <= 4; step++)
        {
            double value = peak * step / 4.0;
            double y = top + plotHeight * (1 - value / peak);
            html.Append($"<line class='gridline' x1='{F(left)}' y1='{F(y)}' x2='{F(width - right)}' y2='{F(y)}'/>" +
                $"<text class='axis' x='{F(left - 9)}' y='{F(y + 4)}' text-anchor='end'>{F(value)}</text>");
        }
        for (int i = 0; i < groups.Length; i++)
        {
            double center = left + (i + 0.5) * groupWidth;
            html.Append($"<text class='axis' x='{F(center)}' y='{F(height - 19)}' text-anchor='middle'>{H(groups[i])}</text>");
            for (int s = 0; s < series.Length; s++)
            {
                double value = series[s].Values[i];
                double barHeight = Math.Max(0, value / peak * plotHeight);
                double x = center + (s - (series.Length - 1) / 2.0) * barWidth - barWidth * 0.42;
                double y = top + plotHeight - barHeight;
                html.Append($"<rect x='{F(x)}' y='{F(y)}' width='{F(barWidth * 0.84)}' height='{F(barHeight)}' fill='{series[s].Colour}' rx='2'><title>{H(series[s].Name)}: {F(value)}</title></rect>");
            }
        }
        html.Append("</svg><div class='legend'>");
        foreach (var s in series)
            html.Append("<span><span class='swatch' style='background:").Append(s.Colour)
                .Append("'></span>").Append(H(s.Name)).Append("</span>");
        html.Append("</div></section>");
    }

    private static string F(double number) => number.ToString("0.###", Invariant);
    private static string H(string text) => WebUtility.HtmlEncode(text);
}
