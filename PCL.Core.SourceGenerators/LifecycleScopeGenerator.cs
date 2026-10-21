using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PCL.Core.SourceGenerators;

[Generator(LanguageNames.CSharp)]
public class LifecycleScopeGenerator : IIncrementalGenerator
{
    private const string ScopeAttributeType = SharedConstants.LifecycleScopeAttribute;

    private const string StartMethodAttributeType = SharedConstants.LifecycleStartAttribute;
    private const string StopMethodAttributeType = SharedConstants.LifecycleStopAttribute;
    private const string CommandHandlerMethodAttributeType = SharedConstants.LifecycleCommandHandlerAttribute;
    private const string DependencyInjectionMethodAttributeType = SharedConstants.LifecycleDependencyInjectionAttribute;
    private const string NewDependencyInjectionPointAttributeType = SharedConstants.DependencyInjectionPointAttribute;

    private static readonly HashSet<string> _MethodAttributeTypes = [
        StartMethodAttributeType, StopMethodAttributeType,
        CommandHandlerMethodAttributeType, DependencyInjectionMethodAttributeType,
        NewDependencyInjectionPointAttributeType
    ];

    private record ScopeMethodModel(string MethodName, bool Awaitable);

    private record StartMethodModel(string MethodName, bool Awaitable)
        : ScopeMethodModel(MethodName, Awaitable);

    private record StopMethodModel(string MethodName, bool Awaitable)
        : ScopeMethodModel(MethodName, Awaitable);

    private record CommandHandlerMethodModel(
        string MethodName,
        bool Awaitable,
        string Command,
        bool HasCommandModelArg,
        bool HasIsCallbackArg,
        (string Name, string TypeName, bool hasDefaultValue, object? DefaultValue)[] SplitArgs
    ) : ScopeMethodModel(MethodName, Awaitable);

    private record DependencyInjectionMethodModel(
        string MethodName,
        bool Awaitable,
        string Identifier,
        int Targets,
        string ParameterType
    ) : ScopeMethodModel(MethodName, Awaitable);

    private record NewDependencyInjectionPointModel(
        string MethodName,
        bool Awaitable,
        string Identifier
    ) : ScopeMethodModel(MethodName, Awaitable);

    private class ScopeModel(
        string namespaceName,
        string typeName,
        string identifier,
        string name,
        bool supportAsync)
    {
        public string Namespace { get; } = namespaceName;
        public string TypeName { get; } = typeName;
        public string QualifiedTypeName => $"{Namespace}.{TypeName}";
        public string Identifier { get; } = identifier;
        public string Name { get; } = name;
        public bool SupportAsync { get; } = supportAsync;
        public List<ScopeMethodModel> Methods { get; } = [];
    }

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        // Debugger.Launch();
        var candidates = context.SyntaxProvider.ForAttributeWithMetadataName(
            ScopeAttributeType,
            static (node, _) => node is ClassDeclarationSyntax syntax && syntax.Modifiers.Any(m => m.ValueText == "partial"),
            static (INamedTypeSymbol, ScopeModel)? (ctx, _) =>
            {
                if (ctx.TargetSymbol is not INamedTypeSymbol typeSymbol) return null;
                var attr = ctx.Attributes[0];
                var args = attr.ConstructorArguments;
                if (args.Length < 2) return null;
                var scopeIdentifier = args[0].Value?.ToString();
                var scopeName = args[1].Value?.ToString();
                if (scopeIdentifier is null || scopeName is null) return null;
                var scopeAsyncStart = true;
                if (args.Length > 2 && args[2].Value is bool v) scopeAsyncStart = v;
                var ns = typeSymbol.ContainingNamespace.ToDisplayString();
                var typeName = typeSymbol.Name;
                return (typeSymbol, new ScopeModel(ns, typeName, scopeIdentifier, scopeName, scopeAsyncStart));
            }
        ).SelectMany(static (candidate, _) =>
            candidate is { } value
                ? new[] { value }
                : Array.Empty<(INamedTypeSymbol, ScopeModel)>());
        var collected = candidates.Collect();
        context.RegisterSourceOutput(collected, _CollectSources);
    }

    private static void _CollectSources(SourceProductionContext spc, ImmutableArray<(INamedTypeSymbol TypeSymbol, ScopeModel Model)> models)
    {
        foreach (var (symbol, model) in models)
        {
            model.Methods.Clear();
            foreach (var member in symbol.GetMembers())
            {
                if (member is not IMethodSymbol method) continue;
                var attr = method.GetAttributes().FirstOrDefault(data =>
                    data.AttributeClass is { } attributeClass
                    && _MethodAttributeTypes.Contains(attributeClass.GetSimplifiedTypeName()));
                if (attr is null) continue;
                var attrTypeName = attr.AttributeClass?.GetSimplifiedTypeName();
                if (attrTypeName is null) continue;
                var methodName = method.Name;
                var awaitable = method.IsAwaitable();
                ScopeMethodModel? methodModel = attrTypeName switch
                {
                    StartMethodAttributeType => new StartMethodModel(methodName, awaitable),
                    StopMethodAttributeType => new StopMethodModel(methodName, awaitable),
                    CommandHandlerMethodAttributeType => GetCommandHandlerMethodModel(),
                    DependencyInjectionMethodAttributeType => GetDependencyInjectionMethodModel(),
                    NewDependencyInjectionPointAttributeType => GetNewDependencyInjectionPointModel(),
                    _ => null
                };
                if (methodModel is not null) model.Methods.Add(methodModel);
                continue;
                CommandHandlerMethodModel? GetCommandHandlerMethodModel()
                {
                    if (awaitable) return null;
                    var constructorArgs = attr.ConstructorArguments;
                    if (constructorArgs.Length < 1
                        || constructorArgs[0].Value?.ToString() is not { } command)
                        return null;
                    var paraArray = method.Parameters;
                    var skip = 0;
                    var hasCommandModelArg = paraArray.Length > 0
                        && paraArray[0].Type.GetSimplifiedTypeName() == "PCL.Core.App.Cli.CommandLine";
                    if (hasCommandModelArg) skip++;
                    var hasIsCallbackArgIndex = hasCommandModelArg ? 1 : 0;
                    var hasIsCallbackArg = paraArray.Length > hasIsCallbackArgIndex
                        && paraArray[hasIsCallbackArgIndex].Type.SpecialType == SpecialType.System_Boolean
                        && paraArray[hasIsCallbackArgIndex].Name == "isCallback";
                    if (hasIsCallbackArg) skip++;
                    var splitArgs = (
                        from para in paraArray.Skip(skip)
                        let name = para.Name
                        let typeName = para.Type.GetFullyQualifiedName()
                        let hasDefaultValue = para.HasExplicitDefaultValue
                        select (name, typeName, hasDefaultValue, hasDefaultValue ? para.ExplicitDefaultValue : null)
                    ).ToArray();
                    return new CommandHandlerMethodModel(
                        methodName,
                        false,
                        command,
                        hasCommandModelArg,
                        hasIsCallbackArg,
                        splitArgs);
                }
                DependencyInjectionMethodModel? GetDependencyInjectionMethodModel()
                {
                    var args = attr.ConstructorArguments;
                    if (args.Length < 2
                        || args[0].Value?.ToString() is not { } identifier
                        || args[1].Value is not int targets)
                        return null;
                    if (method.Parameters.FirstOrDefault() is not { } param) return null;
                    var paramType = param.Type.GetFullyQualifiedName();
                    return new DependencyInjectionMethodModel(methodName, awaitable, identifier, targets, paramType);
                }
                NewDependencyInjectionPointModel? GetNewDependencyInjectionPointModel()
                {
                    var args = attr.ConstructorArguments;
                    if (args.Length > 1 && args[1].Value is false) return null; // lifecycleAutoInvoke is set to false
                    if (args.Length < 1 || args[0].Value?.ToString() is not { } identifier) return null;
                    return new NewDependencyInjectionPointModel(methodName, awaitable, identifier);
                }
            }
            spc.AddSource($"{model.QualifiedTypeName}.g.cs", _GenerateScopeSource(model));
        }
    }

    private static readonly HashSet<Type> _TypesIncludingInStartMethod = [
        typeof(StartMethodModel),
        typeof(CommandHandlerMethodModel),
        typeof(DependencyInjectionMethodModel),
        typeof(CommandHandlerMethodModel),
        typeof(NewDependencyInjectionPointModel),
    ];

    private static string _GenerateScopeSource(ScopeModel model)
    {
        var sb = new StringBuilder();

        // head
        sb.AppendLine("// 此文件由 Source Generator 自动生成，请勿手动修改");
        sb.AppendLine("// <auto-generated />");
        sb.AppendLine();
        sb.AppendLine("using System;");
        sb.AppendLine("using System.Threading.Tasks;");
        sb.AppendLine($"using {SharedConstants.AppNamespace};");
        sb.AppendLine($"using {SharedConstants.IocNamespace};");
        sb.AppendLine();
        sb.AppendLine("#nullable enable");
        sb.AppendLine();
        sb.AppendLine($"namespace {model.Namespace};");
        sb.AppendLine();

        // basic structure
        sb.AppendLine($"partial class {model.TypeName} : ILifecycleService");
        sb.AppendLine("{");
        sb.AppendLine($"    public string Identifier => {model.Identifier.ToLiteral()};");
        sb.AppendLine($"    public string Name => {model.Name.ToLiteral()};");
        sb.AppendLine($"    public bool SupportAsync => {(model.SupportAsync ? "true" : "false")};");
        sb.AppendLine();
        sb.AppendLine("    private static LifecycleContext Context { get => field ?? throw new InvalidOperationException(\"Not initialized\"); set; } = null!;");
        sb.AppendLine("    private static ILifecycleService Service => Context.ServiceInstance;");
        sb.AppendLine($"    public {model.TypeName}() {{ Context = Lifecycle.GetContext(this); }}");
        sb.AppendLine();

        // StopAsync() implementation
        sb.AppendLine("    public async Task StopAsync()");
        sb.AppendLine("    {");
        var stopCount = AppendMethodInvokes(2, model.Methods.Where(x => x is StopMethodModel));
        sb.AppendLine("    }");
        sb.AppendLine();

        // StartAsync() implementation
        sb.AppendLine("    public async Task StartAsync()");
        sb.AppendLine("    {");
        AppendMethodInvokes(2, model.Methods.Where(x => _TypesIncludingInStartMethod.Contains(x.GetType())));
        if (stopCount == 0) sb.AppendLine("        Context.DeclareStopped();");
        sb.AppendLine("    }");

        // structure tail
        sb.AppendLine("}");

        return sb.ToString();

        // method invokes implementation
        int AppendMethodInvokes(int indent, IEnumerable<ScopeMethodModel> models)
        {
            var count = 0;
            var indentStr = new string(' ', indent * 4);
            foreach (var methodModel in models)
            {
                count++;
                sb.Append(indentStr).AppendLine("{");
                foreach (var line in _EmitMethod(methodModel)) sb.Append(indentStr).Append("    ").AppendLine(line);
                sb.Append(indentStr).AppendLine("}");
            }
            return count;
        }
    }

    private static IEnumerable<string> _EmitMethod(ScopeMethodModel model)
    {
        if (model is StartMethodModel or StopMethodModel)
        {
            yield return MethodInvoke();
        }
        else if (model is CommandHandlerMethodModel argModel)
        {
            var actionParamModel = argModel.HasCommandModelArg ? "model" : "_";
            var actionParamIsCallback = argModel.HasIsCallbackArg ? "isCallback" : "_";
            yield return $"Essentials.StartupService.TryHandleCommand(" +
                $"{argModel.Command.ToLiteral()}, ({actionParamModel}, {actionParamIsCallback}) => {{";
            var argTexts = new List<string>();
            if (argModel.HasCommandModelArg) argTexts.Add(actionParamModel);
            if (argModel.HasIsCallbackArg) argTexts.Add(actionParamIsCallback);
            foreach (var (name, typeName, hasDefaultValue, defaultValue) in argModel.SplitArgs)
            {
                var existsText = "exists_" + name;
                var isTypeMatchText = "isTypeMatch_" + name;
                var valueText = "value_" + name;
                yield return $"    var ({(hasDefaultValue ? existsText : "_")}, {isTypeMatchText}) = model.TryGetArgumentValue<{typeName}>(\"{name}\", out var {valueText});";
                yield return $"    if (!{isTypeMatchText}) throw new InvalidCastException(\"Argument type mismatch\");";
                argTexts.Add(hasDefaultValue ? $"{existsText} ? {valueText} : {defaultValue.ToPrimitive() ?? "default"}" : valueText);
            }
            yield return MethodInvoke("    ", argTexts);
            yield return "}, true);";
        }
        else if (model is DependencyInjectionMethodModel diModel)
        {
            var awaitable = diModel.Awaitable;
            if (awaitable) yield return "await Task.Run(() => {";
            var indentStr = awaitable ? "    " : "";
            if (awaitable) yield return $"{indentStr}Func<{diModel.ParameterType}, Task>";
            else yield return $"{indentStr}Action<{diModel.ParameterType}>";
            yield return $"{indentStr}    action = {diModel.MethodName};";
            yield return $"{indentStr}var result = DependencyGroups.InvokeInjection(action, " +
                         $"{diModel.Identifier.ToLiteral()}, " +
                         $"(AttributeTargets){diModel.Targets});";
            var logStr = diModel.Identifier + "@" + diModel.Targets;
            yield return $"{indentStr}if (result) Context.Trace(\"Dependency injection success: {logStr}\");";
            yield return $"{indentStr}else Context.Warn(\"Dependency injection failed: {logStr}\");";
            if (awaitable) yield return "});";
        }
        else if (model is NewDependencyInjectionPointModel newDiModel)
        {
            yield return $"{(model.Awaitable ? "await " : "")}" +
                $"{model.MethodName}_InvokeInjection_{newDiModel.Identifier.SnakeIdToPascal()}();";
        }
        yield break;
        string MethodInvoke(string prefix = "", params IEnumerable<string> args)
            => $"{prefix}{(model.Awaitable ? "await " : "")}{model.MethodName}({string.Join(", ", args)});";
    }
}
