using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using CommandLine;
using CommandLine.Text;

namespace DriveRemap
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                var parser = new Parser(config => config.HelpWriter = Console.Out);
                parser.ParseArguments<Options>(args).WithParsed(Remap);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void Remap(Options options)
        {
            using (var netuse = CreateProcess("net use"))
            {
                var result = GetProcessOutput(netuse).Replace("New connections will be remembered.", "").Replace("Microsoft Windows Network", "");

                var regex = new Regex($@"(\w:)\s*\\\\(?:{options.OldServerName})(.*)\s", RegexOptions.Compiled | RegexOptions.IgnoreCase);

                var count = 0;

                foreach (var line in result.Split('\n'))
                {
                    var match = regex.Match(line);

                    if (match.Success)
                    {
                        var letter = match.Groups[1].Value;
                        var path = match.Groups[2].Value.Trim();
                        var oldPath = ToPath(options.OldServerName, path);
                        var newPath = ToPath(options.NewServerName, path);

                        Console.WriteLine($"{oldPath} is mapped to {letter}");

                        using (var delete = CreateProcess("net use", letter, "/d /y"))
                        {
                            ProcessCommand("", delete);
                            count++;

                            using (var map = CreateProcess("net use", "/y", letter, newPath, "/persistent:yes"))
                            {
                                Console.WriteLine($"Remapping {letter} to {newPath}...");
                                ProcessCommand("", map);
                            }
                        }
                    }
                }

                if (count > 0)
                {
                    Console.WriteLine($"{count} drives remapped");
                }
            }
        }

        private static Process CreateProcess(string command, params string[] args)
        {
            var argstr = string.Join(" ", args ?? new string[] {});

            var process = Process.Start(new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $"/c {command} {argstr}",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            });

            return process;
        }

        private static string GetProcessOutput(Process process)
        {
            var result = string.Empty;
            if (process != null)
            {              
                result = process.StandardOutput.ReadToEnd();
                var err = process.StandardError.ReadToEnd();

                process.WaitForExit();

                if (process.ExitCode != 0)
                {
                    Console.WriteLine(err);
                    Environment.Exit(process.ExitCode);
                }
            }

            return result;
        }

        private static void ProcessCommand(string message, Process process)
        {
            Console.Write(message);

            var o = GetProcessOutput(process);
            Console.Write(o);
        }

        private static string ToPath(string serverName, string path)
        {
            return $@"""\\{serverName}{path}""";
        }

        
    }
}
