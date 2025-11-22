// This is a workaround when compiling .NET Standard 2.1
// https://stackoverflow.com/questions/64749385/predefined-type-system-runtime-compilerservices-isexternalinit-is-not-defined
namespace System.Runtime.CompilerServices;

internal class IsExternalInit
{
}