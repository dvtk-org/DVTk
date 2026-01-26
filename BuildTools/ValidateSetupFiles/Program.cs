using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace ValidateSetupFiles
{
    class ReferenceItem
    {
        public string ProjectFile;
        public string RawReference;
        public string ResolvedPath;
        public string FileName;
        public bool Exists;
    }

    class Program
    {
        // entry: optional arg = solution root. Defaults to current dir.
        static int Main(string[] args)
        {
            try
            {
                var root = args.Length > 0 ? args[0] : Directory.GetCurrentDirectory();
                Console.WriteLine($"ValidateSetupFiles: scanning for .vdproj under '{root}'");

                var vdprojFiles = Directory.GetFiles(root, "*.vdproj", SearchOption.AllDirectories);
                if (vdprojFiles.Length == 0)
                {
                    Console.WriteLine("No .vdproj files found. Nothing to validate.");
                    return 0;
                }

                var references = new List<ReferenceItem>();
                foreach (var project in vdprojFiles)
                {
                    var text = File.ReadAllText(project);
                    // 1) explicit Resources\Libs references: '...Resources\Libs\something.dll'
                    var rxResourceLib = new Regex(@"'([^']*Resources\\Libs\\[^']+)'", RegexOptions.IgnoreCase);
                    foreach (Match m in rxResourceLib.Matches(text))
                    {
                        var value = m.Groups[1].Value;
                        references.Add(MakeRef(project, value, root));
                    }

                    // 2) absolute file paths in quotes (C:\...)
                    var rxAbsolute = new Regex(@"'([A-Za-z]:\\[^']+\.(dll|exe|pdb|lib|so|dat|txt|xml))'", RegexOptions.IgnoreCase);
                    foreach (Match m in rxAbsolute.Matches(text))
                    {
                        var value = m.Groups[1].Value;
                        references.Add(MakeRef(project, value, root));
                    }

                    // 3) filename-only references (e.g. 'msvcp140.dll') - collect but try to locate in repo
                    var rxFilename = new Regex(@"'([^'\\]+\.(dll|exe|pdb|lib|dat|txt|xml))'", RegexOptions.IgnoreCase);
                    foreach (Match m in rxFilename.Matches(text))
                    {
                        var value = m.Groups[1].Value;
                        // avoid duplicate capture of already captured full paths
                        if (rxResourceLib.IsMatch("'" + value + "'") || rxAbsolute.IsMatch("'" + value + "'"))
                            continue;
                        references.Add(MakeRef(project, value, root));
                    }
                }

                // Resolve existence
                foreach (var r in references)
                {
                    if (Path.IsPathRooted(r.RawReference))
                    {
                        r.ResolvedPath = r.RawReference;
                        r.Exists = File.Exists(r.ResolvedPath);
                    }
                    else if (r.RawReference.IndexOf("Resources\\Libs", StringComparison.OrdinalIgnoreCase) >= 0 ||
                             r.RawReference.IndexOf("Resources/Libs", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        // try resolve relative to project directory and repo root
                        var projDir = Path.GetDirectoryName(r.ProjectFile);
                        var attempt = Path.GetFullPath(Path.Combine(projDir ?? "", r.RawReference));
                        if (File.Exists(attempt))
                        {
                            r.ResolvedPath = attempt;
                            r.Exists = true;
                        }
                        else
                        {
                            var alt = Path.GetFullPath(Path.Combine(root, r.RawReference.Replace('/', '\\')));
                            if (File.Exists(alt))
                            {
                                r.ResolvedPath = alt;
                                r.Exists = true;
                            }
                            else
                            {
                                // last resort: search repo for filename
                                r.FileName = Path.GetFileName(r.RawReference);
                                var found = FindFileInTree(root, r.FileName);
                                if (found != null)
                                {
                                    r.ResolvedPath = found;
                                    r.Exists = true;
                                }
                                else
                                {
                                    r.ResolvedPath = attempt;
                                    r.Exists = false;
                                }
                            }
                        }
                    }
                    else
                    {
                        // filename-only or other relative path: try project dir, then repo search
                        var projDir = Path.GetDirectoryName(r.ProjectFile);
                        var attempt = Path.GetFullPath(Path.Combine(projDir ?? "", r.RawReference));
                        if (File.Exists(attempt))
                        {
                            r.ResolvedPath = attempt;
                            r.Exists = true;
                        }
                        else
                        {
                            r.FileName = Path.GetFileName(r.RawReference);
                            var found = FindFileInTree(root, r.FileName);
                            if (found != null)
                            {
                                r.ResolvedPath = found;
                                r.Exists = true;
                            }
                            else
                            {
                                r.ResolvedPath = attempt;
                                r.Exists = false;
                            }
                        }
                    }

                    if (string.IsNullOrEmpty(r.FileName))
                        r.FileName = Path.GetFileName(r.ResolvedPath) ?? r.RawReference;
                }

                // Missing file errors
                var missing = references.Where(x => !x.Exists).ToList();
                var duplicates = references
                    .GroupBy(x => x.FileName, StringComparer.OrdinalIgnoreCase)
                    .Where(g => g.Select(i => i.ProjectFile).Distinct().Count() > 1)
                    .ToList();

                Console.WriteLine();
                Console.WriteLine("Validation summary:");
                Console.WriteLine($"  Projects scanned: {vdprojFiles.Length}");
                Console.WriteLine($"  References discovered: {references.Count}");
                Console.WriteLine($"  Missing files: {missing.Count}");
                Console.WriteLine($"  Potential duplicate target files referenced by >1 project: {duplicates.Count}");
                Console.WriteLine();

                if (missing.Count > 0)
                {
                    Console.WriteLine("ERRORS - Missing source files:");
                    foreach (var m in missing.OrderBy(x => x.FileName, StringComparer.OrdinalIgnoreCase))
                    {
                        Console.WriteLine($"  ERROR: '{m.FileName}' referenced by project '{Path.GetFileName(m.ProjectFile)}'");
                        Console.WriteLine($"         expected at: {m.ResolvedPath}");
                        Console.WriteLine($"         raw reference: {m.RawReference}");
                    }
                    Console.WriteLine();
                    Console.WriteLine("Tip: Put required native DLLs into the referenced folder (e.g. Resources\\Libs) or update setup project file paths.");
                }

                if (duplicates.Count > 0)
                {
                    Console.WriteLine("WARNINGS - Duplicate filenames packaged by multiple projects:");
                    foreach (var g in duplicates)
                    {
                        var distinctProjects = g.Select(i => Path.GetFileName(i.ProjectFile)).Distinct();
                        Console.WriteLine($"  WARN: '{g.Key}' referenced by: {string.Join(", ", distinctProjects)}");
                    }
                    Console.WriteLine();
                    Console.WriteLine("Tip: Ensure unique target locations in setup projects or deliberate dedupe handling so installers won't conflict.");
                }

                // return non-zero if missing files found so CI can fail fast
                if (missing.Count > 0)
                {
                    Console.WriteLine("Validation failed: missing files detected.");
                    return 2;
                }

                Console.WriteLine("Validation passed.");
                return 0;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("ValidateSetupFiles: unexpected failure: " + ex);
                return 3;
            }
        }

        static ReferenceItem MakeRef(string projectFile, string rawRef, string root)
        {
            return new ReferenceItem
            {
                ProjectFile = projectFile,
                RawReference = rawRef,
                ResolvedPath = null,
                FileName = null,
                Exists = false
            };
        }

        static string FindFileInTree(string root, string fileName)
        {
            try
            {
                var matches = Directory.EnumerateFiles(root, fileName, SearchOption.AllDirectories).FirstOrDefault();
                return matches;
            }
            catch
            {
                return null;
            }
        }
    }
}