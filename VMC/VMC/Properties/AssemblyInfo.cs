using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using log4net;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("VMC")]
[assembly: AssemblyDescription("VLC based Media Center")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Andrew Brincat")]
[assembly: AssemblyProduct("VMC")]
[assembly: AssemblyCopyright("Copyright ©  2014")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("75daa905-69ab-457f-8bfd-82755828be9e")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("0.0.1")]
[assembly: AssemblyFileVersion("1.0.0.0")]

//Log4net
[assembly: log4net.Config.XmlConfigurator(ConfigFile = "app.config", Watch = true)]