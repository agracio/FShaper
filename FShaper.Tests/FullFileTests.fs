namespace Tests

open NUnit.Framework
open FShaper.Core
open FsUnit
open Swensen.Unquote.Assertions
open CodeFormatter


[<TestFixture>]
type FullFileTests () =

    [<Test>]
    member this.``multiple attributes`` () = 
        let csharp = 
             """
                using System;
                using Android.App;
                using Firebase.Iid;
                using Android.Util;

                namespace FCMClient
                {
                    [Service]
                    [IntentFilter(new[] { "com.google.firebase.INSTANCE_ID_EVENT" })]
                    public class MyFirebaseIIDService : FirebaseInstanceIdService
                    {
                        const string TAG = "MyFirebaseIIDService";
                        public override void OnTokenRefresh()
                        {
                            var refreshedToken = FirebaseInstanceId.Instance.Token;
                            Log.Debug(TAG, "Refreshed token: " + refreshedToken);
                            SendRegistrationToServer(refreshedToken);
                        }
                        void SendRegistrationToServer(string token)
                        {
                            // Add custom implementation, as needed.
                        }
                    }
                }"""

        let fsharp = 
            """
                namespace FCMClient

                open System
                open Android.App
                open Firebase.Iid
                open Android.Util

                [<Service; IntentFilter([| "com.google.firebase.INSTANCE_ID_EVENT" |])>]
                type MyFirebaseIIDService() =
                    inherit FirebaseInstanceIdService()
                    let TAG = "MyFirebaseIIDService"

                    override this.OnTokenRefresh() =
                        let mutable refreshedToken = FirebaseInstanceId.Instance.Token
                        Log.Debug(TAG, "Refreshed token: " + (refreshedToken.ToString()))
                        this.SendRegistrationToServer(refreshedToken)
                
                    member this.SendRegistrationToServer(token: string) = ()"""
                   
        csharp
        |> reduceIndent
        |> Converter.run 
        |> logConverted
        |> should equal (formatFsharp fsharp)

    [<Test>]
    member this.``convert full file - namespace with class`` () = 
        let csharp = 
            """    
                using System;
                using System.Collections.Specialized;
                using System.Windows.Input;
                using Android.App;
                using Android.Views;
                using Android.Widget;
                using MvvmCross.Binding.BindingContext;
                using MvvmCross.Navigation;
                using MvvmCross.ViewModels;

                namespace StarWarsSample.Forms.Droid
                {
                    // This class is never actually executed, but when Xamarin linking is enabled it does how to ensure types and properties
                    // are preserved in the deployed app
                    [Android.Runtime.Preserve(AllMembers = true)]
                    public class LinkerPleaseInclude
                    {
                        public void Include(Button button)
                        {
                            button.Click += (s, e) => button.Text = button.Text + "";
                        }

                    public void Include(CheckBox checkBox)
                    {
                        checkBox.CheckedChange += (sender, args) => checkBox.Checked = !checkBox.Checked;
                    }

                    public void Include(View view)
                    {
                        view.Click += (s, e) => view.ContentDescription = view.ContentDescription + "";
                    }

                    public void Include(TextView text)
                    {
                        text.AfterTextChanged += (sender, args) => text.Text = "" + text.Text;
                        text.Hint = "" + text.Hint;
                    }

                    public void Include(CheckedTextView text)
                    {
                        text.AfterTextChanged += (sender, args) => text.Text = "" + text.Text;
                        text.Hint = "" + text.Hint;
                    }

                    public void Include(CompoundButton cb)
                    {
                        cb.CheckedChange += (sender, args) => cb.Checked = !cb.Checked;
                    }

                    public void Include(SeekBar sb)
                    {
                        sb.ProgressChanged += (sender, args) => sb.Progress = sb.Progress + 1;
                    }

                    public void Include(RadioGroup radioGroup)
                    {
                        radioGroup.CheckedChange += (sender, args) => radioGroup.Check(args.CheckedId);
                    }

                    public void Include(RadioButton radioButton)
                    {
                        radioButton.CheckedChange += (sender, args) => radioButton.Checked = args.IsChecked;
                    }

                    public void Include(RatingBar ratingBar)
                    {
                        ratingBar.RatingBarChange += (sender, args) => ratingBar.Rating = 0 + ratingBar.Rating;
                    }

                    public void Include(Activity act)
                    {
                        act.Title = act.Title + "";
                    }

                    public void Include(INotifyCollectionChanged changed)
                    {
                        changed.CollectionChanged += (s, e) => { var test = $"{e.Action}{e.NewItems}{e.NewStartingIndex}{e.OldItems}"; };
                    }

                    public void Include(ICommand command)
                    {
                        command.CanExecuteChanged += (s, e) => { if (command.CanExecute(null)) command.Execute(null); };
                    }

                    public void Include(MvvmCross.IoC.MvxPropertyInjector injector)
                    {
                        injector = new MvvmCross.IoC.MvxPropertyInjector();
                    }

                    public void Include(System.ComponentModel.INotifyPropertyChanged changed)
                    {
                        changed.PropertyChanged += (sender, e) =>
                        {
                            var test = e.PropertyName;
                        };
                    }

                    public void Include(MvxTaskBasedBindingContext context)
                    {
                        context.Dispose();
                        var context2 = new MvxTaskBasedBindingContext();
                        context2.Dispose();
                    }

                    public void Include(MvxNavigationService service, IMvxViewModelLoader loader)
                    {
                        service = new MvxNavigationService(null, loader);
                    }

                    public void Include(ConsoleColor color)
                    {
                        Console.Write("");
                        Console.WriteLine("");
                        color = Console.ForegroundColor;
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                    }

                    public void Include(MvvmCross.Plugin.Json.Plugin plugin)
                    {
                        plugin.Load();
                    }
                }
            }"""

        let fsharp = 
             """
                namespace StarWarsSample.Forms.Droid

                open System
                open System.Collections.Specialized
                open System.Windows.Input
                open Android.App
                open Android.Views
                open Android.Widget
                open MvvmCross.Binding.BindingContext
                open MvvmCross.Navigation
                open MvvmCross.ViewModels

                [<Android.Runtime.Preserve(AllMembers = true)>]
                // This class is never actually executed, but when Xamarin linking is enabled it does how to ensure types and properties
                // are preserved in the deployed app
                type LinkerPleaseInclude() =
                    member this.Include(button: Button) = button.Click.AddHandler<_>(fun (s, e) -> button.Text <- button.Text + "")
                    member this.Include(checkBox: CheckBox) =
                        checkBox.CheckedChange.AddHandler<_>(fun (sender, args) -> checkBox.Checked <- not checkBox.Checked)
                    member this.Include(view: View) =
                        view.Click.AddHandler<_>(fun (s, e) -> view.ContentDescription <- view.ContentDescription + "")

                    member this.Include(text: TextView) =
                        text.AfterTextChanged.AddHandler<_>(fun (sender, args) -> text.Text <- "" + text.Text)
                        text.Hint <- "" + text.Hint

                    member this.Include(text: CheckedTextView) =
                        text.AfterTextChanged.AddHandler<_>(fun (sender, args) -> text.Text <- "" + text.Text)
                        text.Hint <- "" + text.Hint

                    member this.Include(cb: CompoundButton) =
                        cb.CheckedChange.AddHandler<_>(fun (sender, args) -> cb.Checked <- not cb.Checked)
                    member this.Include(sb: SeekBar) =
                        sb.ProgressChanged.AddHandler<_>(fun (sender, args) -> sb.Progress <- sb.Progress + 1)
                    member this.Include(radioGroup: RadioGroup) =
                        radioGroup.CheckedChange.AddHandler<_>(fun (sender, args) -> radioGroup.Check(args.CheckedId))
                    member this.Include(radioButton: RadioButton) =
                        radioButton.CheckedChange.AddHandler<_>(fun (sender, args) -> radioButton.Checked <- args.IsChecked)
                    member this.Include(ratingBar: RatingBar) =
                        ratingBar.RatingBarChange.AddHandler<_>(fun (sender, args) -> ratingBar.Rating <- 0 + ratingBar.Rating)
                    member this.Include(act: Activity) = act.Title <- act.Title + ""

                    member this.Include(changed: INotifyCollectionChanged) =
                        changed.CollectionChanged.AddHandler<_>(fun (s, e) ->
                            let mutable test = sprintf "%O%O%O%O" (e.Action) (e.NewItems) (e.NewStartingIndex) (e.OldItems)
                            ())

                    member this.Include(command: ICommand) =
                        command.CanExecuteChanged.AddHandler<_>(fun (s, e) ->
                            if command.CanExecute(null) then command.Execute(null))

                    member this.Include(injector: MvvmCross.IoC.MvxPropertyInjector) =
                        injector <- new MvvmCross.IoC.MvxPropertyInjector()

                    member this.Include(changed: System.ComponentModel.INotifyPropertyChanged) =
                        changed.PropertyChanged.AddHandler<_>(fun (sender, e) ->
                            let mutable test = e.PropertyName
                            ())

                    member this.Include(context: MvxTaskBasedBindingContext) =
                        context.Dispose()
                        let mutable context2 = new MvxTaskBasedBindingContext()
                        context2.Dispose()

                    member this.Include(service: MvxNavigationService, loader: IMvxViewModelLoader) =
                        service <- new MvxNavigationService(null, loader)

                    member this.Include(color: ConsoleColor) =
                        Console.Write("")
                        Console.WriteLine("")
                        color <- Console.ForegroundColor
                        Console.ForegroundColor <- ConsoleColor.Red
                        Console.ForegroundColor <- ConsoleColor.Yellow
                        Console.ForegroundColor <- ConsoleColor.Magenta
                        Console.ForegroundColor <- ConsoleColor.White
                        Console.ForegroundColor <- ConsoleColor.Gray
                        Console.ForegroundColor <- ConsoleColor.DarkGray

                    member this.Include(plugin: MvvmCross.Plugin.Json.Plugin) = plugin.Load()"""

        csharp
        |> reduceIndent
        |> Converter.run 
        |> logConverted
        |> should equal (formatFsharpWithSource fsharp)

    [<Test>]
    member this.``convert full file - namespace with class 2`` () = 
        let csharp = 
            """    
            using System;
            using System.Collections.Concurrent;
            using System.Collections.Generic;
            using System.IO;
            using System.Linq;
            using System.Reflection;
            using System.Text.RegularExpressions;
            using System.Threading.Tasks;
            using Microsoft.CodeAnalysis;
            using Microsoft.CodeAnalysis.CSharp;
            using Microsoft.CodeAnalysis.Emit;

            // ReSharper disable once CheckNamespace
            public class EdgeCompiler
            {
                private static readonly Regex ReferenceRegex = new Regex(@"", RegexOptions.Multiline);
                private static readonly Regex UsingRegex = new Regex(@"", RegexOptions.Multiline);
                private static readonly bool DebuggingEnabled = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("EDGE_CS_DEBUG"));
                private static readonly bool DebuggingSelfEnabled = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("EDGE_CS_DEBUG_SELF"));
                private static readonly bool CacheEnabled = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("EDGE_CS_CACHE"));
                private static readonly ConcurrentDictionary<string, Func<object, Task<object>>> FuncCache = new ConcurrentDictionary<string, Func<object, Task<object>>>();
                private static Func<Stream, Assembly> _assemblyLoader;
                
                static EdgeCompiler()
                {
                    AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
                }

                public static void SetAssemblyLoader(Func<Stream, Assembly> assemblyLoader)
                {
                    _assemblyLoader = assemblyLoader;
                }

                public void DebugMessage(string format, params object[] args)
                {
                    if (DebuggingSelfEnabled)
                    {
                        //Console.WriteLine(format, args);
                    }
                }

                public Func<object, Task<object>> CompileFunc(IDictionary<string, object> parameters, IDictionary<string, string> compileAssemblies)
                {
                    DebugMessage("EdgeCompiler::CompileFunc (CLR) - Starting");

                    string source = (string) parameters["source"];
                    string lineDirective = string.Empty;
                    string fileName = null;
                    int lineNumber = 1;

                    // Read source from file
                    if (source.EndsWith(".cs", StringComparison.OrdinalIgnoreCase) || source.EndsWith(".csx", StringComparison.OrdinalIgnoreCase))
                    {
                        // Retain filename for debugging purposes
                        if (DebuggingEnabled)
                        {
                            fileName = source;
                        }

                        source = File.ReadAllText(source);
                    }

                    DebugMessage("EdgeCompiler::CompileFunc (CLR) - Func cache size: {0}", FuncCache.Count);

                    string originalSource = source;

                    if (FuncCache.ContainsKey(originalSource))
                    {
                        DebugMessage("EdgeCompiler::CompileFunc (CLR) - Serving func from cache");
                        return FuncCache[originalSource];
                    }

                    DebugMessage("EdgeCompiler::CompileFunc (CLR) - Func not found in cache, compiling");

                    // Add assembly references provided explicitly through parameters, along with some default ones
                    List<string> references = new List<string>
                    {
                        "System.Runtime",
                        "System.Threading.Tasks",
                        "System.Dynamic.Runtime",
                        "Microsoft.CSharp"
                    };

                    object providedReferences;

                    if (parameters.TryGetValue("references", out providedReferences))
                    {
                        foreach (object reference in (object[]) providedReferences)
                        {
                            references.Add((string)reference);
                        }
                    }

                    // Add assembly references provided in code as [//]#r "assembly name" lines
                    Match match = ReferenceRegex.Match(source);

                    while (match.Success)
                    {
                        references.Add(match.Groups[1].Value);
                        source = source.Substring(0, match.Index) + source.Substring(match.Index + match.Length);
                        match = ReferenceRegex.Match(source);
                    }

                    if (DebuggingEnabled)
                    {
                        object jsFileName;

                        if (parameters.TryGetValue("jsFileName", out jsFileName))
                        {
                            fileName = (string) jsFileName;
                            lineNumber = (int) parameters["jsLineNumber"];
                        }

                        if (!string.IsNullOrEmpty(fileName))
                        {
                            lineDirective = string.Format("#line {0} \"{1}\"\n", lineNumber, fileName.Replace("\\", "\\\\"));
                        }
                    }

                    // Try to compile source code as a class library
                    Assembly assembly;
                    string errorsClass;

                    if (!TryCompile(lineDirective + source, references, compileAssemblies, out errorsClass, out assembly))
                    {
                        // Try to compile source code as an async lambda expression

                        // Extract using statements first
                        string usings = "";
                        match = UsingRegex.Match(source);

                        while (match.Success)
                        {
                            usings += match.Groups[1].Value;
                            source = source.Substring(0, match.Index) + source.Substring(match.Index + match.Length);
                            match = UsingRegex.Match(source);
                        }

                        string errorsLambda;
                        source = usings + @"
            using System;
            using System.Threading.Tasks;

            public class Startup 
            {
                public async Task<object> Invoke(object ___input) 
                {
            " + lineDirective + @"
                    Func<object, Task<object>> func = " + source + @";
            #line hidden
                    return await func(___input);
                }
            }";

                        DebugMessage("EdgeCompiler::CompileFunc (CLR) - Trying to compile async lambda expression:{0}{1}", Environment.NewLine, source);

                        if (!TryCompile(source, references, compileAssemblies, out errorsLambda, out assembly))
                        {
                            throw new InvalidOperationException("Unable to compile C# code.\n----> Errors when compiling as a CLR library:\n" + errorsClass +
                                                                "\n----> Errors when compiling as a CLR async lambda expression:\n" + errorsLambda);
                        }
                    }

                    // Extract the entry point to a class method
                    Type startupType = assembly.GetType((string) parameters["typeName"]);

                    if (startupType == null)
                    {
                        throw new TypeLoadException("Could not load type '" + (string) parameters["typeName"] + "'");
                    }

                    object instance = Activator.CreateInstance(startupType);
                    MethodInfo invokeMethod = startupType.GetMethod((string) parameters["methodName"], BindingFlags.Instance | BindingFlags.Public);

                    if (invokeMethod == null)
                    {
                        throw new InvalidOperationException("Unable to access CLR method to wrap through reflection. Make sure it is a public instance method.");
                    }

                    // Ereate a Func<object,Task<object>> delegate around the method invocation using reflection
                    Func<object, Task<object>> result = input => (Task<object>) invokeMethod.Invoke(instance, new object[]
                    {
                        input
                    });

                    if (CacheEnabled)
                    {
                        FuncCache[originalSource] = result;
                    }

                    return result;
                }

                private bool TryCompile(string source, List<string> references, IDictionary<string, string> compileAssemblies, out string errors, out Assembly assembly)
                {
                    assembly = null;
                    errors = null;

                    string projectDirectory = Environment.GetEnvironmentVariable("EDGE_APP_ROOT") ?? Directory.GetCurrentDirectory();

                    SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(source);
                    List<MetadataReference> metadataReferences = new List<MetadataReference>();

                    DebugMessage("EdgeCompiler::TryCompile (CLR) - Resolving {0} references", references.Count);

                    // Search the NuGet package repository for each reference
                    foreach (string reference in references)
                    {
                        DebugMessage("EdgeCompiler::TryCompile (CLR) - Searching for {0}", reference);

                        // If the reference looks like a filename, try to load it directly; if we fail and the reference name does not contain a path separator (like
                        // System.Data.dll), we fall back to stripping off the extension and treating the reference like a NuGet package
                        if (reference.EndsWith(".dll", StringComparison.OrdinalIgnoreCase))
                        {
                            if (reference.Contains(Path.DirectorySeparatorChar.ToString()))
                            {
                                metadataReferences.Add(MetadataReference.CreateFromFile(Path.IsPathRooted(reference)
                                    ? reference
                                    : Path.Combine(projectDirectory, reference)));
                                continue;
                            }

                            if (File.Exists(Path.Combine(projectDirectory, reference)))
                            {
                                metadataReferences.Add(MetadataReference.CreateFromFile(Path.Combine(projectDirectory, reference)));
                                continue;
                            }
                        }

                        string referenceName = reference.EndsWith(".dll", StringComparison.OrdinalIgnoreCase)
                            ? reference.Substring(0, reference.Length - 4)
                            : reference;

                        if (!compileAssemblies.ContainsKey(referenceName))
                        {
                            throw new Exception(String.Format("Unable to resolve reference to {0}.", referenceName));
                        }

                        DebugMessage("EdgeCompiler::TryCompile (CLR) - Reference to {0} resolved to {1}", referenceName, compileAssemblies[referenceName]);
                        metadataReferences.Add(MetadataReference.CreateFromFile(compileAssemblies[referenceName]));
                    }

                    CSharpCompilationOptions compilationOptions = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, optimizationLevel: DebuggingEnabled
                        ? OptimizationLevel.Debug
                        : OptimizationLevel.Release);

                    DebugMessage("EdgeCompiler::TryCompile (CLR) - Starting compilation");

                    CSharpCompilation compilation = CSharpCompilation.Create(Guid.NewGuid() + ".dll", new SyntaxTree[]
                    {
                        syntaxTree
                    }, metadataReferences, compilationOptions);

                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        EmitResult compilationResults = compilation.Emit(memoryStream);

                        if (!compilationResults.Success)
                        {
                            IEnumerable<Diagnostic> failures =
                                compilationResults.Diagnostics.Where(
                                    diagnostic =>
                                        diagnostic.IsWarningAsError || diagnostic.Severity == DiagnosticSeverity.Error || diagnostic.Severity == DiagnosticSeverity.Warning);

                            foreach (Diagnostic diagnostic in failures)
                            {
                                if (errors == null)
                                {
                                    errors = String.Format("{0}: {1}", diagnostic.Id, diagnostic.GetMessage());
                                }

                                else
                                {
                                    errors += String.Format("\n{0}: {1}", diagnostic.Id, diagnostic.GetMessage());
                                }
                            }

                            DebugMessage("EdgeCompiler::TryCompile (CLR) - Compilation failed with the following errors: {0}{1}", Environment.NewLine, errors);
                            return false;
                        }

                        memoryStream.Seek(0, SeekOrigin.Begin);
                        assembly = _assemblyLoader(memoryStream);

                        DebugMessage("EdgeCompiler::TryCompile (CLR) - Compilation completed successfully");
                        return true;
                    }
                }
            }"""

        let fsharp = 
             """
                open System
                open System.Collections.Specialized
                open System.Windows.Input
                open Android.App
                open Android.Views
                open Android.Widget
                open MvvmCross.Binding.BindingContext
                open MvvmCross.Navigation
                open MvvmCross.ViewModels

                [<Android.Runtime.Preserve(AllMembers = true)>]
                // This class is never actually executed, but when Xamarin linking is enabled it does how to ensure types and properties
                // are preserved in the deployed app
                type LinkerPleaseInclude() =
                    member this.Include(button: Button) = button.Click.AddHandler<_>(fun (s, e) -> button.Text <- button.Text + "")
                    member this.Include(checkBox: CheckBox) =
                        checkBox.CheckedChange.AddHandler<_>(fun (sender, args) -> checkBox.Checked <- not checkBox.Checked)
                    member this.Include(view: View) =
                        view.Click.AddHandler<_>(fun (s, e) -> view.ContentDescription <- view.ContentDescription + "")

                    member this.Include(text: TextView) =
                        text.AfterTextChanged.AddHandler<_>(fun (sender, args) -> text.Text <- "" + text.Text)
                        text.Hint <- "" + text.Hint

                    member this.Include(text: CheckedTextView) =
                        text.AfterTextChanged.AddHandler<_>(fun (sender, args) -> text.Text <- "" + text.Text)
                        text.Hint <- "" + text.Hint

                    member this.Include(cb: CompoundButton) =
                        cb.CheckedChange.AddHandler<_>(fun (sender, args) -> cb.Checked <- not cb.Checked)
                    member this.Include(sb: SeekBar) =
                        sb.ProgressChanged.AddHandler<_>(fun (sender, args) -> sb.Progress <- sb.Progress + 1)
                    member this.Include(radioGroup: RadioGroup) =
                        radioGroup.CheckedChange.AddHandler<_>(fun (sender, args) -> radioGroup.Check(args.CheckedId))
                    member this.Include(radioButton: RadioButton) =
                        radioButton.CheckedChange.AddHandler<_>(fun (sender, args) -> radioButton.Checked <- args.IsChecked)
                    member this.Include(ratingBar: RatingBar) =
                        ratingBar.RatingBarChange.AddHandler<_>(fun (sender, args) -> ratingBar.Rating <- 0 + ratingBar.Rating)
                    member this.Include(act: Activity) = act.Title <- act.Title + ""

                    member this.Include(changed: INotifyCollectionChanged) =
                        changed.CollectionChanged.AddHandler<_>(fun (s, e) ->
                            let mutable test = sprintf "%O%O%O%O" (e.Action) (e.NewItems) (e.NewStartingIndex) (e.OldItems)
                            ())

                    member this.Include(command: ICommand) =
                        command.CanExecuteChanged.AddHandler<_>(fun (s, e) ->
                            if command.CanExecute(null) then command.Execute(null))

                    member this.Include(injector: MvvmCross.IoC.MvxPropertyInjector) =
                        injector <- new MvvmCross.IoC.MvxPropertyInjector()

                    member this.Include(changed: System.ComponentModel.INotifyPropertyChanged) =
                        changed.PropertyChanged.AddHandler<_>(fun (sender, e) ->
                            let mutable test = e.PropertyName
                            ())

                    member this.Include(context: MvxTaskBasedBindingContext) =
                        context.Dispose()
                        let mutable context2 = new MvxTaskBasedBindingContext()
                        context2.Dispose()

                    member this.Include(service: MvxNavigationService, loader: IMvxViewModelLoader) =
                        service <- new MvxNavigationService(null, loader)

                    member this.Include(color: ConsoleColor) =
                        Console.Write("")
                        Console.WriteLine("")
                        color <- Console.ForegroundColor
                        Console.ForegroundColor <- ConsoleColor.Red
                        Console.ForegroundColor <- ConsoleColor.Yellow
                        Console.ForegroundColor <- ConsoleColor.Magenta
                        Console.ForegroundColor <- ConsoleColor.White
                        Console.ForegroundColor <- ConsoleColor.Gray
                        Console.ForegroundColor <- ConsoleColor.DarkGray

                    member this.Include(plugin: MvvmCross.Plugin.Json.Plugin) = plugin.Load()"""

        csharp
        |> reduceIndent
        |> Converter.run 
        |> logConverted
        |> should equal (formatFsharpWithSource fsharp)

    [<Test>]
    member this.``parse multiple classes`` () = 
        let csharp = 
             """
                public class QuestStarted
                {
                    public string Name { get; set; }
                    public Guid Id { get; set; }

                    public override string ToString()
                    {
                        return $"Quest {Name} started";
                    }
                }

                public class QuestEnded
                {
                    public string Name { get; set; }
                    public Guid Id { get; set; }

                    public override string ToString()
                    {
                        return $"Quest {Name} ended";
                    }
                }"""
    
        let fsharp = 
             """
                type QuestStarted() =
                    member val Name: string = Unchecked.defaultof<string> with get, set
                    member val Id: Guid = Unchecked.defaultof<Guid> with get, set
                    override this.ToString(): string = sprintf "Quest %O started" (Name)

                type QuestEnded() =
                    member val Name: string = Unchecked.defaultof<string> with get, set
                    member val Id: Guid = Unchecked.defaultof<Guid> with get, set
                    override this.ToString(): string = sprintf "Quest %O ended" (Name)"""

        csharp
        |> reduceIndent
        |> Converter.run 
        |> logConverted
        |> should equal (formatFsharp fsharp)

    [<Test>]
    member this.``parse multiple classes with odd formatting`` () = 
        let csharp = 
             """
                public interface IService {
                    void Serve();
                }
                public class Service1 : IService {
                    public void Serve() { Console.WriteLine("Service1 Called"); }
                }
                public class Service2 : IService {
                    public void Serve() { Console.WriteLine("Service2 Called"); }
                }
                public class Client {
                    private IService _service;
                    public Client(IService service) {
                        this._service = service;
                    }
                    public void ServeMethod() { this._service.Serve(); }
                }"""
    
        let fsharp = 
             """
                type IService =
                    abstract Serve: unit -> unit
 
                type Service1() =
                    member this.Serve() = Console.WriteLine("Service1 Called")
                    interface IService with
                        member this.Serve() = this.Serve()

                type Service2() =
                    member this.Serve() = Console.WriteLine("Service2 Called")
                    interface IService with
                        member this.Serve() = this.Serve()
                        
                type Client(_service: IService) =
                    member this.ServeMethod() = this._service.Serve()"""

        csharp
        |> reduceIndent
        |> Converter.run 
        |> logConverted
        |> should equal (formatFsharp fsharp)     

    [<Test>]
    member this.``Correct order of classes`` () = 
        let csharp = 
             """
                public class Bar
                {
                    public Foo GetFoo()
                    {
                        return new Foo().MagicNumber;
                    }
                }

                public class Foo
                {
                    public int MagicNumber()
                    {
                        return 42;
                    }
                }
                 """
    
        let fsharp = 
             """
                type Foo() =
                    member this.MagicNumber(): int = 42

                type Bar() =
                    member this.GetFoo(): Foo = (new Foo()).MagicNumber"""

        csharp
        |> reduceIndent
        |> Converter.run 
        |> logConverted
        |> should equal (formatFsharp fsharp)    


    [<Test>]
    member this.``fixed complex class ordering`` () = 
        let csharp = 
             """
                public class D : C
                {
                }

                public class C : B
                {
                }

                public class B : A
                {
                }

                public class A
                {
                }"""
    
        let fsharp = 
             """
                type A() =



                type B() =
                    inherit A()

                
                type C() =
                    inherit B()


                type D() =
                    inherit C()"""

        csharp
        |> reduceIndent
        |> Converter.run 
        |> logConverted
        |> should equal (formatFsharp fsharp)   

    [<Test>]
    member this.``fixed two sets of related classes`` () = 
        let csharp = 
             """
                public class D : C
                {
                }

                public class C : B
                {
                }

                public class B
                {
                }

                public class A
                {
                }

                public class X : Z
                {
                }
                
                public class Z
                {
                }"""
    
        let fsharp = 
             """
                type B() =



                type A() =
                
                

                type Z() =
                
                
                
                type C() =
                    inherit B()


                type X() =
                    inherit Z()


                type D() =
                    inherit C()"""

        csharp
        |> reduceIndent
        |> Converter.run 
        |> logConverted
        |> should equal (formatFsharp fsharp) 

    [<Test>]
    member this.``reorder main method and classes`` () = 
        let csharp = 
             """
                using System;
                using System.Collections.Generic;
                using System.Linq;
                using System.Text;

                namespace Inheritance {
                   class Test {
                      static void Main(string[] args) {
                         Father f = new Father();
                         f.display();

                         Son s = new Son();
                         s.display();
                         s.displayOne();

                         Daughter d = new Daughter();
                         d.displayTwo();

                         Console.ReadKey();
                      }

                      class Father {
                         public void display() {
                            Console.WriteLine("Display...");
                         }
                      }

                      class Son : Father {
                         public void displayOne() {
                            Console.WriteLine("Display One");
                         }
                      }

                      class Daughter : Father {
                         public void displayTwo() {
                            Console.WriteLine("Display Two");
                         }
                      }
                   }
                }"""
    
        let fsharp = 
             """
                namespace Inheritance

                open System
                open System.Collections.Generic
                open System.Linq
                open System.Text
                
                type Father() =
                    member this.display() = Console.WriteLine("Display...")
                
                type Son() =
                    inherit Father()
                    member this.displayOne() = Console.WriteLine("Display One")
                
                type Daughter() =
                    inherit Father()
                    member this.displayTwo() = Console.WriteLine("Display Two")
                
                type Test() =
                    static member Main(args: string []) =
                        let mutable f = new Father()
                        f.display()
                        let mutable s = new Son()
                        s.display()
                        s.displayOne()
                        let mutable d = new Daughter()
                        d.displayTwo()
                        Console.ReadKey()"""

        csharp
        |> reduceIndent
        |> Converter.run 
        |> logConverted
        |> should equal (formatFsharp fsharp)