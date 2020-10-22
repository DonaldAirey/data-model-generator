// <copyright file="VSConstants.cs" company="Theta Rex, Inc.">
//    Copyright © 2016-2017 - Dark Bond, Inc.  All Rights Reserved.
// </copyright>
// <author>Donald Roy Airey</author>
namespace GammaFour.VisualStudio
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class VSConstants
    {
        public const int cmdidToolsOptions = 264;
        public const int RPC_E_SYS_CALL_FAILED = -2147417856;
        public const int RPC_E_SERVER_DIED_DNE = -2147418094;
        public const int RPC_E_CANTCALLOUT_AGAIN = -2147418095;
        public const int RPC_E_INVALID_PARAMETER = -2147418096;
        public const int RPC_E_INVALID_DATA = -2147418097;
        public const int RPC_E_SERVER_CANTUNMARSHAL_DATA = -2147418098;
        public const int RPC_E_OUT_OF_RESOURCES = -2147417855;
        public const int RPC_E_SERVER_CANTMARSHAL_DATA = -2147418099;
        public const int RPC_E_CLIENT_CANTMARSHAL_DATA = -2147418101;
        public const int RPC_E_CANTTRANSMIT_CALL = -2147418102;
        public const int RPC_E_INVALID_DATAPACKET = -2147418103;
        public const int RPC_E_CLIENT_DIED = -2147418104;
        public const int RPC_E_SERVER_DIED = -2147418105;
        public const int RPC_E_CONNECTION_TERMINATED = -2147418106;
        public const int RPC_E_CLIENT_CANTUNMARSHAL_DATA = -2147418100;
        public const int RPC_E_ATTEMPTED_MULTITHREAD = -2147417854;
        public const int RPC_E_NOT_REGISTERED = -2147417853;
        public const int RPC_E_FAULT = -2147417852;
        public const int RPC_E_INVALID_IPID = -2147417837;
        public const int RPC_E_INVALID_EXTENSION = -2147417838;
        public const int RPC_E_INVALID_HEADER = -2147417839;
        public const int RPC_E_VERSION_MISMATCH = -2147417840;
        public const int RPC_E_THREAD_NOT_INIT = -2147417841;
        public const int RPC_E_WRONG_THREAD = -2147417842;
        public const int RPC_E_CANTCALLOUT_ININPUTSYNCCALL = -2147417843;
        public const int RPC_E_INVALID_CALLDATA = -2147417844;
        public const int RPC_E_SERVERCALL_REJECTED = -2147417845;
        public const int RPC_E_SERVERCALL_RETRYLATER = -2147417846;
        public const int RPC_E_RETRY = -2147417847;
        public const int RPC_E_DISCONNECTED = -2147417848;
        public const int RPC_E_INVALIDMETHOD = -2147417849;
        public const int RPC_E_CHANGED_MODE = -2147417850;
        public const int RPC_E_SERVERFAULT = -2147417851;
        public const int RPC_E_CANTCALLOUT_INEXTERNALCALL = -2147418107;
        public const int RPC_E_INVALID_OBJECT = -2147417836;
        public const int RPC_E_CANTCALLOUT_INASYNCCALL = -2147418108;
        public const int RPC_E_CALL_CANCELED = -2147418110;
        public const int OLE_E_CANTCONVERT = -2147221487;
        public const int OLE_E_NOT_INPLACEACTIVE = -2147221488;
        public const int OLE_E_INVALIDHWND = -2147221489;
        public const int OLE_E_WRONGCOMPOBJ = -2147221490;
        public const int OLE_E_INVALIDRECT = -2147221491;
        public const int OLE_E_PROMPTSAVECANCELLED = -2147221492;
        public const int OLE_E_NOSTORAGE = -2147221486;
        public const int OLE_E_STATIC = -2147221493;
        public const int OLE_E_CANT_GETMONIKER = -2147221495;
        public const int OLE_E_CLASSDIFF = -2147221496;
        public const int OLE_E_BLANK = -2147221497;
        public const int OLE_E_NOCACHE = -2147221498;
        public const int OLE_E_NOTRUNNING = -2147221499;
        public const int OLE_E_NOCONNECTION = -2147221500;
        public const int OLE_E_CANT_BINDTOSOURCE = -2147221494;
        public const int DISP_E_UNKNOWNINTERFACE = -2147352575;
        public const int DISP_E_MEMBERNOTFOUND = -2147352573;
        public const int DISP_E_PARAMNOTFOUND = -2147352572;
        public const int RPC_E_CALL_REJECTED = -2147418111;
        public const int DISP_E_BUFFERTOOSMALL = -2147352557;
        public const int DISP_E_DIVBYZERO = -2147352558;
        public const int DISP_E_NOTACOLLECTION = -2147352559;
        public const int DISP_E_BADCALLEE = -2147352560;
        public const int DISP_E_PARAMNOTOPTIONAL = -2147352561;
        public const int DISP_E_BADPARAMCOUNT = -2147352562;
        public const int DISP_E_ARRAYISLOCKED = -2147352563;
        public const int DISP_E_UNKNOWNLCID = -2147352564;
        public const int DISP_E_BADINDEX = -2147352565;
        public const int DISP_E_EXCEPTION = -2147352567;
        public const int DISP_E_BADVARTYPE = -2147352568;
        public const int DISP_E_NONAMEDARGS = -2147352569;
        public const int DISP_E_UNKNOWNNAME = -2147352570;
        public const int DISP_E_TYPEMISMATCH = -2147352571;
        public const int RPC_E_CANTPOST_INSENDCALL = -2147418109;
        public const int OLE_E_ADVISENOTSUPPORTED = -2147221501;
        public const int RPC_S_CALLPENDING = -2147417835;
        public const int RPC_E_CALL_COMPLETE = -2147417833;
        //
        // Summary:
        //     Error HRESULT for the call to a not implemented method.
        public const int E_NOTIMPL = -2147467263;
        //
        // Summary:
        //     Error HRESULT for the request of a not implemented interface.
        public const int E_NOINTERFACE = -2147467262;
        //
        // Summary:
        //     Error HRESULT for a generic failure.
        public const int E_FAIL = -2147467259;
        //
        // Summary:
        //     Error HRESULT for an invalid argument.
        public const int E_INVALIDARG = -2147024809;
        //
        // Summary:
        //     Error HRESULT for out of memory.
        public const int E_OUTOFMEMORY = -2147024882;
        //
        // Summary:
        //     Error HRESULT for a client abort.
        public const int UNDO_E_CLIENTABORT = -2147205119;
        //
        // Summary:
        //     Error HRESULT for an unexpected condition.
        public const int E_UNEXPECTED = -2147418113;
        //
        // Summary:
        //     HRESULT for generic success.
        public const int S_OK = 0;
        //
        // Summary:
        //     Is returned by build interfaces that have parameters for specifying an array
        //     of IVsOutput's but the implementation can only apply the method to all outputs.
        public const int VS_E_SPECIFYING_OUTPUT_UNSUPPORTED = -2147220991;
        //
        // Summary:
        //     VS specific error HRESULT returned by interfaces to asynchronous behavior when
        //     the object in question in already busy.
        public const int VS_E_BUSY = -2147220992;
        public const int RPC_E_UNEXPECTED = -2147352577;
        public const int CO_E_CANCEL_DISABLED = -2147417792;
        public const int CO_E_ACNOTINITIALIZED = -2147417793;
        public const int CO_E_DECODEFAILED = -2147417795;
        //
        // Summary:
        //     HRESULT for FALSE (not an error).
        public const int S_FALSE = 1;
        //
        // Summary:
        //     Error HRESULT for a null or invalid pointer.
        public const int E_POINTER = -2147467261;
        //
        // Summary:
        //     Error HRESULT for an invalid HANDLE.
        public const int E_HANDLE = -2147024890;
        //
        // Summary:
        //     Error HRESULT for an abort.
        public const int E_ABORT = -2147467260;
        //
        // Summary:
        //     Reset and clear selection in list of available components.
        public const int CPPM_CLEARSELECTION = 2314;
        //
        // Summary:
        //     Set multiple-selection mode for picker.
        public const int CPPM_SETMULTISELECT = 2313;
        //
        // Summary:
        //     Initialize tab with VARIANT in VSCOMPONENTSELECTORTABINIT.
        public const int CPPM_INITIALIZETAB = 2312;
        //
        // Summary:
        //     Retrieve information about selection.
        public const int CPPM_GETSELECTION = 2311;
        //
        // Summary:
        //     Determine whether Select button should be enabled.
        public const int CPPM_QUERYCANSELECT = 2310;
        //
        // Summary:
        //     Initialize list of available components.
        public const int CPPM_INITIALIZELIST = 2309;
        //
        // Summary:
        //     Inform of doubld-click on selected item on page.
        public const int CPDN_SELDBLCLICK = 2305;
        //
        // Summary:
        //     Inform of selection change on page.
        public const int CPDN_SELCHANGED = 2304;
        //
        // Summary:
        //     Message broadcast via IVsBroadcastMessageEvents::OnBroadcastMessage to indicate
        //     that the application is running critically low on available virtual memory. The
        //     wParam contains the available virtual memory in bytes. This message will be broadcast
        //     once per minute for as long as the critical memory condition persists.
        public const int VSM_VIRTUALMEMORYCRITICAL = 4182;
        //
        // Summary:
        //     Message broadcast via IVsBroadcastMessageEvents::OnBroadcastMessage to indicate
        //     that the application is running low on available virtual memory. The wParam contains
        //     the available virtual memory in bytes. This message will be broadcast once per
        //     minute for as long as the low memory condition persists.
        public const int VSM_VIRTUALMEMORYLOW = 4181;
        public const int VSM_EXITMODAL = 4180;
        public const int VSM_ENTERMODAL = 4179;
        //
        // Summary:
        //     Toolbar metrics changed.
        public const int VSM_TOOLBARMETRICSCHANGE = 4178;
        //
        // Summary:
        //     Error HRESULT for a pending condition.
        public const int E_PENDING = -2147483638;
        //
        // Summary:
        //     Error HRESULT for an access denied.
        public const int E_ACCESSDENIED = -2147024891;
        public const int CO_E_FAILEDTOOPENPROCESSTOKEN = -2147417796;
        public const int RPC_S_WAITONTIMER = -2147417834;
        public const int CO_E_INCOMPATIBLESTREAMVERSION = -2147417797;
        public const int CO_E_EXCEEDSYSACLLIMIT = -2147417799;
        public const int CO_E_FAILEDTOOPENTHREADTOKEN = -2147417819;
        public const int CO_E_FAILEDTOGETSECCTX = -2147417820;
        public const int CO_E_FAILEDTOIMPERSONATE = -2147417821;
        public const int RPC_E_INVALID_STD_NAME = -2147417822;
        public const int RPC_E_FULLSIC_REQUIRED = -2147417823;
        public const int RPC_E_NO_SYNC = -2147417824;
        public const int CO_E_FAILEDTOGETTOKENINFO = -2147417818;
        public const int RPC_E_TIMEOUT = -2147417825;
        public const int RPC_E_INVALID_OBJREF = -2147417827;
        public const int RPC_E_REMOTE_DISABLED = -2147417828;
        public const int RPC_E_ACCESS_DENIED = -2147417829;
        public const int RPC_E_NO_GOOD_SECURITY_PACKAGES = -2147417830;
        public const int RPC_E_TOO_LATE = -2147417831;
        public const int RPC_E_UNSECURE_CALL = -2147417832;
        public const int RPC_E_NO_CONTEXT = -2147417826;
        public const int CO_E_TRUSTEEDOESNTMATCHCLIENT = -2147417817;
        public const int CO_E_FAILEDTOQUERYCLIENTBLANKET = -2147417816;
        public const int CO_E_FAILEDTOSETDACL = -2147417815;
        public const int CO_E_FAILEDTOCLOSEHANDLE = -2147417800;
        public const int CO_E_FAILEDTOCREATEFILE = -2147417801;
        public const int CO_E_FAILEDTOGENUUID = -2147417802;
        public const int CO_E_PATHTOOLONG = -2147417803;
        public const int CO_E_FAILEDTOGETWINDIR = -2147417804;
        public const int CO_E_SETSERLHNDLFAILED = -2147417805;
        public const int CO_E_LOOKUPACCNAMEFAILED = -2147417806;
        public const int CO_E_NOMATCHINGNAMEFOUND = -2147417807;
        public const int CO_E_LOOKUPACCSIDFAILED = -2147417808;
        public const int CO_E_NOMATCHINGSIDFOUND = -2147417809;
        public const int CO_E_CONVERSIONFAILED = -2147417810;
        public const int CO_E_INVALIDSID = -2147417811;
        public const int CO_E_WRONGTRUSTEENAMESYNTAX = -2147417812;
        public const int CO_E_NETACCESSAPIFAILED = -2147417813;
        public const int CO_E_ACCESSCHECKFAILED = -2147417814;
        public const int CO_E_ACESINWRONGORDER = -2147417798;
        public const int OLE_E_ENUM_NOMORE = -2147221502;
        public const int DISP_E_OVERFLOW = -2147352566;
        public const int OLE_E_OLEVERB = -2147221504;
        //
        // Summary:
        //     Special value for a cookie (e.g. returned from IVsRunningDocumentTable.FindAndLockDocument):
        //     no cookie.
        public const uint VSCOOKIE_NIL = 0;
        //
        // Summary:
        //     Special items inside a VsHierarchy: all the currently selected items.
        [EditorBrowsable(EditorBrowsableState.Never)]
        public const uint VSITEMID_SELECTION = 4294967293;
        //
        // Summary:
        //     Special items inside a VsHierarchy: the hierarchy itself.
        [EditorBrowsable(EditorBrowsableState.Never)]
        public const uint VSITEMID_ROOT = 4294967294;
        //
        // Summary:
        //     Special items inside a VsHierarchy: no node.
        [EditorBrowsable(EditorBrowsableState.Never)]
        public const uint VSITEMID_NIL = uint.MaxValue;
        public const string SharedProjectReferenceProvider_string = "{88B47069-C019-4EEC-B69C-3C8630F83BA5}";
        public const string ConnectedServiceInstanceReferenceProvider_string = "{C18E5D73-E6D1-43AA-AC5E-58D82E44DA9C}";
        [EditorBrowsable(EditorBrowsableState.Never)]
        public const uint CEF_CLONEFILE = 1;
        public const string FileReferenceProvider_string = "{7B069159-FF02-4752-93E8-96B3CADF441A}";
        public const string PlatformReferenceProvider_string = "{97324595-E3F9-4AA8-85B7-DC941E812152}";
        [EditorBrowsable(EditorBrowsableState.Never)]
        public const uint CEF_SILENT = 4;
        public const string ComReferenceProvider_string = "{4560BE15-8871-482A-801D-76AA47F1763A}";
        [EditorBrowsable(EditorBrowsableState.Never)]
        public const uint CEF_OPENASNEW = 8;
        public const string ProjectReferenceProvider_string = "{51ECA6BD-5AE4-43F0-AA76-DD0A7B08F40C}";
        public const string AssemblyReferenceProvider_string = "{9A341D95-5A64-11D3-BFF9-00C04F990235}";
        public const string MiscFilesProjectUniqueName = "<MiscFiles>";
        public const string SolutionItemsProjectUniqueName = "<SolnItems>";
        public const uint VSUTDCF_PRIVATE = 4294901760;
        public const uint VSUTDCF_PACKAGE = 4;
        public const uint VSUTDCF_REBUILD = 2;
        public const uint VSUTDCF_DTEEONLY = 1;
        public const uint VS_BUILDABLEPROJECTCFGOPTS_PRIVATE = 4294901760;
        public const uint VS_BUILDABLEPROJECTCFGOPTS_PACKAGE = 8;
        public const uint VS_BUILDABLEPROJECTCFGOPTS_BUILD_ACTIVE_DOCUMENT_ONLY = 4;
        public const uint VS_BUILDABLEPROJECTCFGOPTS_BUILD_SELECTION_ONLY = 2;
        public const uint VS_BUILDABLEPROJECTCFGOPTS_REBUILD = 1;
        //
        // Summary:
        //     IVsSelectionEvents.OnElementValueChanged flag: The undo manager.
        [EditorBrowsable(EditorBrowsableState.Never)]
        public const uint UndoManager = 0;
        //
        // Summary:
        //     IVsSelectionEvents.OnElementValueChanged flag: A window frame.
        [EditorBrowsable(EditorBrowsableState.Never)]
        public const uint WindowFrame = 1;
        [EditorBrowsable(EditorBrowsableState.Never)]
        public const uint CEF_OPENFILE = 2;
        //
        // Summary:
        //     IVsSelectionEvents.OnElementValueChanged flag: The startup project.
        [EditorBrowsable(EditorBrowsableState.Never)]
        public const uint StartupProject = 3;
        //
        // Summary:
        //     VS specific error HRESULT for "Wizard back button pressed".
        public const int VS_E_WIZARDBACKBUTTONPRESS = -2147213313;
        //
        // Summary:
        //     VS specific error HRESULT for "Editor disabled".
        public const int VS_E_EDITORDISABLED = -2147213296;
        //
        // Summary:
        //     VS specific success HRESULT for "Project forwarded".
        public const int VS_S_PROJECTFORWARDED = 270320;
        //
        // Summary:
        //     VS specific success HRESULT for "Toolbox marker".
        public const int VS_S_TBXMARKER = 270321;
        //
        // Summary:
        //     VS Specific error HRESULT for a project not supported by current edition of the
        //     product
        public const int VS_E_INCOMPATIBLEPROJECT = -2147213309;
        //
        // Summary:
        //     VS Specific error HRESULT for a non-Windows Store app project, which is not supported
        //     by the VS Express SKU
        public const int VS_E_INCOMPATIBLECLASSICPROJECT = -2147213308;
        //
        // Summary:
        //     IVsSelectionEvents.OnElementValueChanged flag: A document frame.
        [EditorBrowsable(EditorBrowsableState.Never)]
        public const uint DocumentFrame = 2;
        //
        // Summary:
        //     VS specific error HRESULT for "Unsupported format".
        public const int VS_E_UNSUPPORTEDFORMAT = -2147213333;
        //
        // Summary:
        //     VS Specific error HRESULT for a project not supported in the current OS version,
        //     for example Windows Store app projects require Windows 8 or higher
        public const int VS_E_INCOMPATIBLEPROJECT_UNSUPPORTED_OS = -2147213307;
        //
        // Summary:
        //     VS specific error HRESULT code returned when an attempt to do a task wait operation
        //     would result in a deadlock due to circular dependency
        public const int VS_E_CIRCULARTASKDEPENDENCY = -2147213305;
        //
        // Summary:
        //     VS specific success HRESULT returned when a safe-repair is required. A safe-repair
        //     is one which is not a real full upgrade but merely a repair to make the project
        //     asset compatible without the risk of issues being encountered by the previous
        //     versions of the product
        public const int VS_S_PROJECT_SAFEREPAIRREQUIRED = 270322;
        //
        // Summary:
        //     VS specific success HRESULT returned when an unsafe-repair is required. An usafe-repair
        //     is one which is not a full upgrade, but with a risk of issues being encountered
        //     by the newer or previous version of the product. For example if a newer dependent
        //     SDK is not currently installed
        public const int VS_S_PROJECT_UNSAFEREPAIRREQUIRED = 270323;
        //
        // Summary:
        //     VS specific success HRESULT returned when a one-way upgrade is required. A one-way
        //     upgrade is a full upgrade which will make the project incompatible with the previous
        //     version of the product
        public const int VS_S_PROJECT_ONEWAYUPGRADEREQUIRED = 270324;
        //
        // Summary:
        //     VS specific success HRESULT returned when a project is not supported by current
        //     edition of the product
        public const int VS_S_INCOMPATIBLEPROJECT = 270325;
        public const uint ALL = 1;
        public const uint SELECTED = 2;
        //
        // Summary:
        //     VS specific error HRESULT code returned when an attempt to do a save using RDTSAVEOPT_SileSave
        //     fails because source control requires a UI prompt in order to make the file to
        //     be saved editable
        public const int VS_E_PROMPTREQUIRED = -2147213306;
        //
        // Summary:
        //     VS specific error HRESULT for "Incompatible document data".
        public const int VS_E_INCOMPATIBLEDOCDATA = -2147213334;
        public const int OLE_E_ADVF = -2147221503;
        //
        // Summary:
        //     VS specific error HRESULT for "Project configuration failed".
        public const int VS_E_PROJECTMIGRATIONFAILED = -2147213339;
        //
        // Summary:
        //     VS specific error HRESULT for "Solution already open".
        public const int VS_E_SOLUTIONALREADYOPEN = -2147213340;
        //
        // Summary:
        //     VS specific error HRESULT for "Solution not open".
        public const int VS_E_SOLUTIONNOTOPEN = -2147213341;
        //
        // Summary:
        //     VS specific error HRESULT for "Project not loaded".
        public const int VS_E_PROJECTNOTLOADED = -2147213342;
        //
        // Summary:
        //     IVsSelectionEvents.OnElementValueChanged flag: The property borowser.
        [EditorBrowsable(EditorBrowsableState.Never)]
        public const uint PropertyBrowserSID = 4;
        //
        // Summary:
        //     IVsSelectionEvents.OnElementValueChanged flag: A user context.
        [EditorBrowsable(EditorBrowsableState.Never)]
        public const uint UserContext = 5;
        //
        // Summary:
        //     VS specific error HRESULT for "Project already exists".
        public const int VS_E_PROJECTALREADYEXISTS = -2147213344;
        //
        // Summary:
        //     VS specific error HRESULT for "Package not loaded".
        public const int VS_E_PACKAGENOTLOADED = -2147213343;
        //
        // Summary:
        //     GUID of the build pane inside the output window.
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static readonly Guid GUID_BuildOutputWindowPane;
        //
        // Summary:
        //     GUID of the debug pane inside the output window.
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static readonly Guid GUID_OutWindowDebugPane;
        //
        // Summary:
        //     GUID of the general output pane inside the output window.
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static readonly Guid GUID_OutWindowGeneralPane;
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static readonly Guid BuildOrder;
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static readonly Guid BuildOutput;
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static readonly Guid DebugOutput;
        //
        // Summary:
        //     Physical file on disk or web (IVsProject::GetMkDocument returns a file path).
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static readonly Guid GUID_ItemType_PhysicalFile;
        //
        // Summary:
        //     GUID for the 2K command set. This is a set of standard editor commands.
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static readonly Guid VSStd2K;
        //
        // Summary:
        //     Non-physical folder (folder is logical and not a physical file system directory).
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static readonly Guid GUID_ItemType_VirtualFolder;
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static readonly Guid VsStd2010;
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static readonly Guid GUID_ExternalEditor;
        //
        // Summary:
        //     This GUID identifies the standard set of commands known by VisualStudio 97 (version
        //     6).
        public static readonly Guid GUID_VSStandardCommandSet97;
        //
        // Summary:
        //     This GUID identifies commands fired as a resoult of a WM_APPCOMMAND message received
        //     by the main window.
        public static readonly Guid GUID_AppCommand;
        public static readonly Guid IID_IUnknown;
        //
        // Summary:
        //     A nested hierarchy project.
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static readonly Guid GUID_ItemType_SubProject;
        //
        // Summary:
        //     Physical folder on disk or web (IVsProject::GetMkDocument returns a directory
        //     path).
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static readonly Guid GUID_ItemType_PhysicalFolder;
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static readonly Guid GUID_DefaultEditor;
        //
        // Summary:
        //     SUIHostCommandDispatcher service returns an object that implements IOleCommandTarget.
        //     This object handles command routing for the Environment. Use this service if
        //     you need to route a command based on the current selection/state of the Environment.
        public static readonly Guid SID_SUIHostCommandDispatcher;
        //
        // Summary:
        //     The BrowseFile page.
        public static readonly Guid GUID_BrowseFilePage;
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static readonly Guid VsStd16;
        //
        // Summary:
        //     Command Group GUID for commands that only apply to the UIHierarchyWindow.
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static readonly Guid GUID_VsUIHierarchyWindowCmds;
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static readonly Guid VsStd15;
        public static readonly IntPtr HIERARCHY_DONTCHANGE;
        public static readonly IntPtr SELCONTAINER_DONTCHANGE;
        public static readonly IntPtr HIERARCHY_DONTPROPAGATE;
        public static readonly IntPtr SELCONTAINER_DONTPROPAGATE;
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static readonly Guid VsStd14;
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static readonly Guid VsStd12;
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static readonly Guid CLSID_VsUIHierarchyWindow;
        //
        // Summary:
        //     The document's data is HTML.
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static readonly Guid CLSID_HtmDocData;
        //
        // Summary:
        //     GUID of the HTML language service.
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static readonly Guid CLSID_HtmlLanguageService;
        //
        // Summary:
        //     GUID of the HTML editor factory.
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static readonly Guid GUID_HtmlEditorFactory;
        //
        // Summary:
        //     GUID of the Text editor factory.
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static readonly Guid GUID_TextEditorFactory;
        //
        // Summary:
        //     GUID used to mark a TextBuffer in order to tell to the HTML editor factory to
        //     accept preexisting doc data.
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static readonly Guid GUID_HTMEDAllowExistingDocData;
        //
        // Summary:
        //     GUID for the environment package.
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static readonly Guid CLSID_VsEnvironmentPackage;
        //
        // Summary:
        //     GUID for the "Visual Studio" pseudo folder in the registry.
        public static readonly Guid GUID_VsNewProjectPseudoFolder;
        //
        // Summary:
        //     GUID for the "Miscellaneous Files" project.
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static readonly Guid CLSID_MiscellaneousFilesProject;
        //
        // Summary:
        //     GUID for Solution Items project.
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static readonly Guid CLSID_SolutionItemsProject;
        //
        // Summary:
        //     Pseudo service that returns a IID_IVsOutputWindowPane interface of the General
        //     output pane in the VS environment. Querying for this service will cause the General
        //     output pane to be created if it hasn't yet been created.
        public static readonly Guid SID_SVsGeneralOutputWindowPane;
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static readonly Guid VsStd11;
        //
        // Summary:
        //     GUID of the HTML package.
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static readonly Guid CLSID_HtmedPackage;
        public static readonly Guid guidCOMPLUSLibrary;
        //
        // Summary:
        //     SharedProjectReference item (normally child of "References" folder).
        //
        // Remarks:
        //     SharedProjectReference items represent imported shared MSBuild project files
        //     (e.g. *.projitems file). Normally these shared MSBuild project files are "owned"
        //     by a particular Shared Project (aka Shared Assets Project) loaded in the Solution.
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static readonly Guid GUID_ItemType_SharedProjectReference;
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static readonly Guid CLSID_VsTaskListPackage;
        public static readonly Guid SID_SVsToolboxActiveXDataProvider;
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static readonly Guid CLSID_VsDocOutlinePackage;
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static readonly Guid CLSID_VsCfgProviderEventsHelper;
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static readonly Guid GUID_COMPlusPage;
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static readonly Guid GUID_COMClassicPage;
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static readonly Guid GUID_SolutionPage;
        public static readonly Guid AssemblyReferenceProvider_Guid;
        public static readonly Guid ProjectReferenceProvider_Guid;
        public static readonly Guid ComReferenceProvider_Guid;
        public static readonly Guid PlatformReferenceProvider_Guid;
        public static readonly Guid FileReferenceProvider_Guid;
        public static readonly Guid ConnectedServiceInstanceReferenceProvider_Guid;
        //
        // Summary:
        //     Kind of view for document or data: Any defined view.
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static readonly Guid LOGVIEWID_Any;
        //
        // Summary:
        //     Kind of view for document or data: Primary (default) view.
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static readonly Guid LOGVIEWID_Primary;
        //
        // Summary:
        //     Kind of view for document or data: Debugger view.
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static readonly Guid LOGVIEWID_Debugging;
        //
        // Summary:
        //     Kind of view for document or data: Code editor view.
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static readonly Guid LOGVIEWID_Code;
        //
        // Summary:
        //     Kind of view for document or data: Designer view.
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static readonly Guid LOGVIEWID_Designer;
        //
        // Summary:
        //     Kind of view for document or data: Text editor view.
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static readonly Guid LOGVIEWID_TextView;
        //
        // Summary:
        //     Kind of view for document or data: A user defined view.
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static readonly Guid LOGVIEWID_UserChooseView;
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static readonly Guid CLSID_ComPlusOnlyDebugEngine;
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static readonly Guid CLSID_VsTaskList;
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static readonly Guid GUID_VsTaskListViewUncheckedTasks;
        public static readonly Guid SharedProjectReferenceProvider_Guid;
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static readonly Guid GUID_VsTaskListViewCurrentFileTasks;
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static readonly Guid GUID_VsTaskListViewCheckedTasks;
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static readonly Guid GUID_VS_DEPTYPE_BUILD_PROJECT;
        //
        // Summary:
        //     The project designer guid.
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static readonly Guid GUID_ProjectDesignerEditor;
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static readonly Guid UICONTEXT_SolutionBuilding;
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static readonly Guid UICONTEXT_Debugging;
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static readonly Guid UICONTEXT_Dragging;
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static readonly Guid UICONTEXT_FullScreenMode;
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static readonly Guid UICONTEXT_DesignMode;
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static readonly Guid UICONTEXT_SolutionExists;
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static readonly Guid UICONTEXT_EmptySolution;
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static readonly Guid UICONTEXT_NoSolution;
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static readonly Guid UICONTEXT_SolutionHasMultipleProjects;
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static readonly Guid GUID_VsTaskListViewCommentTasks;
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static readonly Guid GUID_VsTaskListViewCompilerTasks;
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static readonly Guid UICONTEXT_SolutionHasSingleProject;
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static readonly Guid GUID_VsTaskListViewShortcutTasks;
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static readonly Guid GUID_VsTaskListViewUserTasks;
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static readonly Guid GUID_VsTaskListViewHTMLTasks;
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static readonly Guid UIContext_SolutionClosing;
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static readonly Guid UICONTEXT_SolutionHasAppContainerProject;
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static readonly Guid UICONTEXT_CodeWindow;
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static readonly Guid GUID_VsTaskListViewAll;

        //
        // Summary:
        //     Set of the standard, shared commands in CMDSETID.StandardCommandSet12_guid
        [Guid("2A8866DC-7BDE-4dc8-A360-A60679534384")]
        public enum VSStd12CmdID
        {
            ShowUserNotificationsToolWindow = 1,
            OpenProjectFromScc = 2,
            ShareProject = 3,
            PeekDefinition = 4,
            AccountSettings = 5,
            PeekNavigateForward = 6,
            PeekNavigateBackward = 7,
            RetargetProject = 8,
            RetargetProjectInstallComponent = 9,
            AddReferenceProjectOnly = 10,
            AddWebReferenceProjectOnly = 11,
            AddServiceReferenceProjectOnly = 12,
            AddReferenceNonProjectOnly = 13,
            AddWebReferenceNonProjectOnly = 14,
            AddServiceReferenceNonProjectOnly = 15,
            NavigateTo = 256,
            MoveSelLinesUp = 258,
            MoveSelLinesDown = 259
        }
        //
        // Summary:
        //     These element IDs are the only element IDs that can be used with the selection
        //     service.
        public enum VSSELELEMID
        {
            SEID_UndoManager = 0,
            SEID_WindowFrame = 1,
            SEID_DocumentFrame = 2,
            SEID_StartupProject = 3,
            SEID_PropertyBrowserSID = 4,
            SEID_UserContext = 5,
            SEID_ResultList = 6,
            SEID_LastWindowFrame = 7
        }
        //
        // Summary:
        //     The following commands are special commands that only apply to the UIHierarchyWindow.
        //     They are defined as part of the command group GUID: CMDSETID.UIHierarchyWindowCommandSet_guid.
        [Guid("60481700-078b-11d1-aaf8-00a0c9055a90")]
        public enum VsUIHierarchyWindowCmdIds
        {
            UIHWCMDID_RightClick = 1,
            UIHWCMDID_DoubleClick = 2,
            UIHWCMDID_EnterKey = 3,
            UIHWCMDID_StartLabelEdit = 4,
            UIHWCMDID_CommitLabelEdit = 5,
            UIHWCMDID_CancelLabelEdit = 6
        }
        [Flags]
        public enum CEF : uint
        {
            CloneFile = 1,
            OpenFile = 2,
            Silent = 4,
            OpenAsNew = 8
        }
        public enum VSITEMID : uint
        {
            //
            // Summary:
            //     Special items inside a VsHierarchy: all the currently selected items.
            Selection = 4294967293,
            //
            // Summary:
            //     Special items inside a VsHierarchy: the hierarchy itself.
            Root = 4294967294,
            //
            // Summary:
            //     Special items inside a VsHierarchy: no node.
            Nil = uint.MaxValue
        }
        //
        // Summary:
        //     Set of the standard, shared commands in CMDSETID.StandardCommandSet15_guid
        [Guid("8F380902-6040-4097-9837-D3F40E66F908")]
        public enum VSStd16CmdID
        {
            NewProject2 = 1,
            DocumentTabsLeft = 2,
            DocumentTabsTop = 3,
            DocumentTabsRight = 4
        }
        //
        // Summary:
        //     Set of the standard, shared commands in CMDSETID.StandardCommandSet15_guid
        [Guid("712C6C80-883B-4AAD-B430-BBCA5256FA9D")]
        public enum VSStd15CmdID
        {
            NavigateToFile = 1,
            NavigateToType = 2,
            NavigateToSymbol = 3,
            NavigateToMember = 4,
            NavigateToRecentFile = 5,
            FindAllRefPresetGroupingComboList = 42,
            FindAllRefPresetGroupingComboGetList = 43,
            FindAllRefLockWindow = 44,
            GetToolsAndFeatures = 60,
            ShowLineAnnotations = 76,
            MoveToNextAnnotation = 77,
            MoveToPreviousAnnotation = 78,
            ShowStructure = 79,
            HelpAccessibility = 112,
            ToggleAutoHideChannels = 256,
            EnableRestoreDocumentsOnSolutionLoad = 512,
            DisableRestoreDocumentsOnSolutionLoad = 513,
            CloseAllButToolWindows = 528
        }
        //
        // Summary:
        //     Set of the standard, shared commands in CMDSETID.StandardCommandSet14_guid
        [Guid("4C7763BF-5FAF-4264-A366-B7E1F27BA958")]
        public enum VSStd14CmdID
        {
            ShowQuickFixes = 1,
            ShowRefactorings = 2,
            SmartBreakLine = 3,
            ManageWindowLayouts = 4,
            SaveWindowLayout = 5,
            ShowQuickFixesForPosition = 6,
            ShowQuickFixesForPosition2 = 7,
            DeleteFR1 = 10,
            DeleteFR2 = 20,
            ErrorContextComboList = 30,
            ErrorContextComboGetList = 31,
            ErrorBuildContextComboList = 40,
            ErrorBuildContextComboGetList = 41,
            ErrorListClearFilters = 50,
            WindowLayoutList0 = 4096,
            WindowLayoutListFirst = 4096,
            WindowLayoutList1 = 4097,
            WindowLayoutList2 = 4098,
            WindowLayoutList3 = 4099,
            WindowLayoutList4 = 4100,
            WindowLayoutList5 = 4101,
            WindowLayoutList6 = 4102,
            WindowLayoutList7 = 4103,
            WindowLayoutList8 = 4104,
            WindowLayoutList9 = 4105,
            WindowLayoutListDynamicFirst = 4112,
            WindowLayoutListLast = 8191
        }
        //
        // Summary:
        //     Set of the standard, shared commands in CMDSETID.StandardCommandSet11_guid
        [Guid("D63DB1F0-404E-4B21-9648-CA8D99245EC3")]
        public enum VSStd11CmdID
        {
            FloatAll = 1,
            MoveAllToNext = 2,
            MoveAllToPrevious = 3,
            MultiSelect = 4,
            PaneNextTabAndMultiSelect = 5,
            PanePrevTabAndMultiSelect = 6,
            PinTab = 7,
            BringFloatingWindowsToFront = 8,
            PromoteTab = 9,
            MoveToMainTabWell = 10,
            ToggleFilter = 11,
            FilterToCurrentProject = 12,
            FilterToCurrentDocument = 13,
            FilterToOpenDocuments = 14,
            WindowSearch = 17,
            GlobalSearch = 18,
            GlobalSearchBack = 19,
            SolutionExplorerSearch = 20,
            StartupProjectProperties = 21,
            CloseAllButPinned = 22,
            ResolveFaultedProjects = 23,
            ExecuteSelectionInInteractive = 24,
            ExecuteLineInInteractive = 25,
            InteractiveSessionInterrupt = 26,
            InteractiveSessionRestart = 27,
            SolutionExplorerCollapseAll = 29,
            SolutionExplorerBack = 30,
            SolutionExplorerHome = 31,
            SolutionExplorerForward = 33,
            SolutionExplorerNewScopedWindow = 34,
            SolutionExplorerToggleSingleClickPreview = 35,
            SolutionExplorerSyncWithActiveDocument = 36,
            NewProjectFromTemplate = 37,
            SolutionExplorerScopeToThis = 38,
            SolutionExplorerFilterOpened = 39,
            SolutionExplorerFilterPendingChanges = 40,
            PasteAsLink = 41,
            LocateFindTarget = 42
        }
        public enum SelectionElement : uint
        {
            //
            // Summary:
            //     IVsSelectionEvents.OnElementValueChanged flag: The undo manager.
            UndoManager = 0,
            //
            // Summary:
            //     IVsSelectionEvents.OnElementValueChanged flag: A window frame.
            WindowFrame = 1,
            //
            // Summary:
            //     IVsSelectionEvents.OnElementValueChanged flag: A document frame.
            DocumentFrame = 2,
            //
            // Summary:
            //     IVsSelectionEvents.OnElementValueChanged flag: The startup project.
            StartupProject = 3,
            //
            // Summary:
            //     IVsSelectionEvents.OnElementValueChanged flag: The property borowser.
            PropertyBrowserSID = 4,
            //
            // Summary:
            //     IVsSelectionEvents.OnElementValueChanged flag: A user context.
            UserContext = 5,
            //
            // Summary:
            //     IVsSelectionEvents.OnElementValueChanged flag: The current result list.
            ResultList = 6,
            //
            // Summary:
            //     IVsSelectionEvents.OnElementValueChanged flag: The most recently deactivated
            //     frame.
            LastWindowFrame = 7
        }
        //
        // Summary:
        //     Set of the standard, shared editor commands in StandardCommandSet2k.
        [Guid("1496A755-94DE-11D0-8C3F-00C04FC2AAE2")]
        public enum VSStd2KCmdID
        {
            TYPECHAR = 1,
            BACKSPACE = 2,
            RETURN = 3,
            TAB = 4,
            ECMD_TAB = 4,
            BACKTAB = 5,
            DELETE = 6,
            LEFT = 7,
            LEFT_EXT = 8,
            RIGHT = 9,
            RIGHT_EXT = 10,
            UP = 11,
            UP_EXT = 12,
            DOWN = 13,
            DOWN_EXT = 14,
            HOME = 15,
            HOME_EXT = 16,
            END = 17,
            END_EXT = 18,
            BOL = 19,
            BOL_EXT = 20,
            FIRSTCHAR = 21,
            FIRSTCHAR_EXT = 22,
            EOL = 23,
            EOL_EXT = 24,
            LASTCHAR = 25,
            LASTCHAR_EXT = 26,
            PAGEUP = 27,
            PAGEUP_EXT = 28,
            PAGEDN = 29,
            PAGEDN_EXT = 30,
            TOPLINE = 31,
            TOPLINE_EXT = 32,
            BOTTOMLINE = 33,
            BOTTOMLINE_EXT = 34,
            SCROLLUP = 35,
            SCROLLDN = 36,
            SCROLLPAGEUP = 37,
            SCROLLPAGEDN = 38,
            SCROLLLEFT = 39,
            SCROLLRIGHT = 40,
            SCROLLBOTTOM = 41,
            SCROLLCENTER = 42,
            SCROLLTOP = 43,
            SELECTALL = 44,
            SELTABIFY = 45,
            SELUNTABIFY = 46,
            SELLOWCASE = 47,
            SELUPCASE = 48,
            SELTOGGLECASE = 49,
            SELTITLECASE = 50,
            SELSWAPANCHOR = 51,
            GOTOLINE = 52,
            GOTOBRACE = 53,
            GOTOBRACE_EXT = 54,
            GOBACK = 55,
            SELECTMODE = 56,
            TOGGLE_OVERTYPE_MODE = 57,
            CUT = 58,
            COPY = 59,
            PASTE = 60,
            CUTLINE = 61,
            DELETELINE = 62,
            DELETEBLANKLINES = 63,
            DELETEWHITESPACE = 64,
            DELETETOEOL = 65,
            DELETETOBOL = 66,
            OPENLINEABOVE = 67,
            OPENLINEBELOW = 68,
            INDENT = 69,
            UNINDENT = 70,
            UNDO = 71,
            UNDONOMOVE = 72,
            REDO = 73,
            REDONOMOVE = 74,
            DELETEALLTEMPBOOKMARKS = 75,
            TOGGLETEMPBOOKMARK = 76,
            GOTONEXTBOOKMARK = 77,
            GOTOPREVBOOKMARK = 78,
            FIND = 79,
            REPLACE = 80,
            REPLACE_ALL = 81,
            FINDNEXT = 82,
            FINDNEXTWORD = 83,
            FINDPREV = 84,
            FINDPREVWORD = 85,
            FINDAGAIN = 86,
            TRANSPOSECHAR = 87,
            TRANSPOSEWORD = 88,
            TRANSPOSELINE = 89,
            SELECTCURRENTWORD = 90,
            DELETEWORDRIGHT = 91,
            DELETEWORDLEFT = 92,
            WORDPREV = 93,
            WORDPREV_EXT = 94,
            WORDNEXT = 96,
            WORDNEXT_EXT = 97,
            COMMENTBLOCK = 98,
            UNCOMMENTBLOCK = 99,
            SETREPEATCOUNT = 100,
            WIDGETMARGIN_LBTNDOWN = 101,
            SHOWCONTEXTMENU = 102,
            CANCEL = 103,
            PARAMINFO = 104,
            TOGGLEVISSPACE = 105,
            TOGGLECARETPASTEPOS = 106,
            COMPLETEWORD = 107,
            SHOWMEMBERLIST = 108,
            FIRSTNONWHITEPREV = 109,
            FIRSTNONWHITENEXT = 110,
            HELPKEYWORD = 111,
            FORMATSELECTION = 112,
            OPENURL = 113,
            INSERTFILE = 114,
            TOGGLESHORTCUT = 115,
            QUICKINFO = 116,
            LEFT_EXT_COL = 117,
            RIGHT_EXT_COL = 118,
            UP_EXT_COL = 119,
            DOWN_EXT_COL = 120,
            TOGGLEWORDWRAP = 121,
            ISEARCH = 122,
            ISEARCHBACK = 123,
            BOL_EXT_COL = 124,
            EOL_EXT_COL = 125,
            WORDPREV_EXT_COL = 126,
            WORDNEXT_EXT_COL = 127,
            OUTLN_HIDE_SELECTION = 128,
            OUTLN_TOGGLE_CURRENT = 129,
            OUTLN_TOGGLE_ALL = 130,
            OUTLN_STOP_HIDING_ALL = 131,
            OUTLN_STOP_HIDING_CURRENT = 132,
            OUTLN_COLLAPSE_TO_DEF = 133,
            DOUBLECLICK = 134,
            EXTERNALLY_HANDLED_WIDGET_CLICK = 135,
            COMMENT_BLOCK = 136,
            UNCOMMENT_BLOCK = 137,
            OPENFILE = 138,
            NAVIGATETOURL = 139,
            HANDLEIMEMESSAGE = 140,
            SELTOGOBACK = 141,
            COMPLETION_HIDE_ADVANCED = 142,
            FORMATDOCUMENT = 143,
            OUTLN_START_AUTOHIDING = 144,
            FINAL = 145,
            ECMD_DECREASEFILTER = 146,
            ECMD_COPYTIP = 148,
            ECMD_PASTETIP = 149,
            ECMD_LEFTCLICK = 150,
            ECMD_GOTONEXTBOOKMARKINDOC = 151,
            ECMD_GOTOPREVBOOKMARKINDOC = 152,
            ECMD_INVOKESNIPPETFROMSHORTCUT = 154,
            AUTOCOMPLETE = 155,
            ECMD_INVOKESNIPPETPICKER2 = 156,
            ECMD_DELETEALLBOOKMARKSINDOC = 157,
            ECMD_CONVERTTABSTOSPACES = 158,
            ECMD_CONVERTSPACESTOTABS = 159,
            ECMD_FINAL = 160,
            STOP = 220,
            REVERSECANCEL = 221,
            SLNREFRESH = 222,
            SAVECOPYOFITEMAS = 223,
            NEWELEMENT = 224,
            NEWATTRIBUTE = 225,
            NEWCOMPLEXTYPE = 226,
            NEWSIMPLETYPE = 227,
            NEWGROUP = 228,
            NEWATTRIBUTEGROUP = 229,
            NEWKEY = 230,
            NEWRELATION = 231,
            EDITKEY = 232,
            EDITRELATION = 233,
            MAKETYPEGLOBAL = 234,
            PREVIEWDATASET = 235,
            GENERATEDATASET = 236,
            CREATESCHEMA = 237,
            LAYOUTINDENT = 238,
            LAYOUTUNINDENT = 239,
            REMOVEHANDLER = 240,
            EDITHANDLER = 241,
            ADDHANDLER = 242,
            STYLE = 243,
            STYLEGETLIST = 244,
            FONTSTYLE = 245,
            FONTSTYLEGETLIST = 246,
            PASTEASHTML = 247,
            VIEWBORDERS = 248,
            VIEWDETAILS = 249,
            EXPANDCONTROLS = 250,
            COLLAPSECONTROLS = 251,
            SHOWSCRIPTONLY = 252,
            INSERTTABLE = 253,
            INSERTCOLLEFT = 254,
            INSERTCOLRIGHT = 255,
            INSERTROWABOVE = 256,
            INSERTROWBELOW = 257,
            DELETETABLE = 258,
            DELETECOLS = 259,
            DELETEROWS = 260,
            SELECTTABLE = 261,
            SELECTTABLECOL = 262,
            SELECTTABLEROW = 263,
            SELECTTABLECELL = 264,
            MERGECELLS = 265,
            SPLITCELL = 266,
            INSERTCELL = 267,
            DELETECELLS = 268,
            SEAMLESSFRAME = 269,
            VIEWFRAME = 270,
            DELETEFRAME = 271,
            SETFRAMESOURCE = 272,
            NEWLEFTFRAME = 273,
            NEWRIGHTFRAME = 274,
            NEWTOPFRAME = 275,
            NEWBOTTOMFRAME = 276,
            SHOWGRID = 277,
            SNAPTOGRID = 278,
            BOOKMARK = 279,
            HYPERLINK = 280,
            IMAGE = 281,
            INSERTFORM = 282,
            INSERTSPAN = 283,
            DIV = 284,
            HTMLCLIENTSCRIPTBLOCK = 285,
            HTMLSERVERSCRIPTBLOCK = 286,
            BULLETEDLIST = 287,
            NUMBEREDLIST = 288,
            EDITSCRIPT = 289,
            EDITCODEBEHIND = 290,
            DOCOUTLINEHTML = 291,
            DOCOUTLINESCRIPT = 292,
            RUNATSERVER = 293,
            WEBFORMSVERBS = 294,
            WEBFORMSTEMPLATES = 295,
            ENDTEMPLATE = 296,
            EDITDEFAULTEVENT = 297,
            SUPERSCRIPT = 298,
            SUBSCRIPT = 299,
            EDITSTYLE = 300,
            ADDIMAGEHEIGHTWIDTH = 301,
            REMOVEIMAGEHEIGHTWIDTH = 302,
            LOCKELEMENT = 303,
            VIEWSTYLEORGANIZER = 304,
            ECMD_AUTOCLOSEOVERRIDE = 305,
            NEWANY = 306,
            NEWANYATTRIBUTE = 307,
            DELETEKEY = 308,
            AUTOARRANGE = 309,
            VALIDATESCHEMA = 310,
            NEWFACET = 311,
            VALIDATEXMLDATA = 312,
            DOCOUTLINETOGGLE = 313,
            VALIDATEHTMLDATA = 314,
            VIEWXMLSCHEMAOVERVIEW = 315,
            SHOWDEFAULTVIEW = 316,
            EXPAND_CHILDREN = 317,
            COLLAPSE_CHILDREN = 318,
            TOPDOWNLAYOUT = 319,
            LEFTRIGHTLAYOUT = 320,
            INSERTCELLRIGHT = 321,
            EDITMASTER = 322,
            INSERTSNIPPET = 323,
            FORMATANDVALIDATION = 324,
            COLLAPSETAG = 325,
            SELECT_TAG = 329,
            SELECT_TAG_CONTENT = 330,
            CHECK_ACCESSIBILITY = 331,
            UNCOLLAPSETAG = 332,
            GENERATEPAGERESOURCE = 333,
            SHOWNONVISUALCONTROLS = 334,
            RESIZECOLUMN = 335,
            RESIZEROW = 336,
            MAKEABSOLUTE = 337,
            MAKERELATIVE = 338,
            MAKESTATIC = 339,
            INSERTLAYER = 340,
            UPDATEDESIGNVIEW = 341,
            UPDATESOURCEVIEW = 342,
            INSERTCAPTION = 343,
            DELETECAPTION = 344,
            MAKEPOSITIONNOTSET = 345,
            AUTOPOSITIONOPTIONS = 346,
            EDITIMAGE = 347,
            COMPILE = 350,
            PROJSETTINGS = 352,
            LINKONLY = 353,
            REMOVE = 355,
            PROJSTARTDEBUG = 356,
            PROJSTEPINTO = 357,
            ECMD_UPDATEMGDRES = 358,
            UPDATEWEBREF = 360,
            ADDRESOURCE = 362,
            WEBDEPLOY = 363,
            ECMD_PROJTOOLORDER = 367,
            ECMD_PROJECTTOOLFILES = 368,
            ECMD_OTB_PGO_INSTRUMENT = 369,
            ECMD_OTB_PGO_OPT = 370,
            ECMD_OTB_PGO_UPDATE = 371,
            ECMD_OTB_PGO_RUNSCENARIO = 372,
            ADDHTMLPAGE = 400,
            ADDHTMLPAGECTX = 401,
            ADDMODULE = 402,
            ADDMODULECTX = 403,
            ADDWFCFORM = 406,
            ADDWEBFORM = 410,
            ECMD_ADDMASTERPAGE = 411,
            ADDUSERCONTROL = 412,
            ECMD_ADDCONTENTPAGE = 413,
            ADDDHTMLPAGE = 426,
            ADDIMAGEGENERATOR = 432,
            ADDINHERWFCFORM = 434,
            ADDINHERCONTROL = 436,
            ADDWEBUSERCONTROL = 438,
            BUILDANDBROWSE = 439,
            ADDTBXCOMPONENT = 442,
            ADDWEBSERVICE = 444,
            ECMD_ADDSTYLESHEET = 445,
            ECMD_SETBROWSELOCATION = 446,
            ECMD_REFRESHFOLDER = 447,
            ECMD_SETBROWSELOCATIONCTX = 448,
            ECMD_VIEWMARKUP = 449,
            ECMD_NEXTMETHOD = 450,
            ECMD_PREVMETHOD = 451,
            ECMD_RENAMESYMBOL = 452,
            ECMD_SHOWREFERENCES = 453,
            ECMD_CREATESNIPPET = 454,
            ECMD_CREATEREPLACEMENT = 455,
            ECMD_INSERTCOMMENT = 456,
            VIEWCOMPONENTDESIGNER = 457,
            GOTOTYPEDEF = 458,
            SHOWSNIPPETHIGHLIGHTING = 459,
            HIDESNIPPETHIGHLIGHTING = 460,
            ADDVFPPAGE = 500,
            SETBREAKPOINT = 501,
            SHOWALLFILES = 600,
            ADDTOPROJECT = 601,
            ADDBLANKNODE = 602,
            ADDNODEFROMFILE = 603,
            CHANGEURLFROMFILE = 604,
            EDITTOPIC = 605,
            EDITTITLE = 606,
            MOVENODEUP = 607,
            MOVENODEDOWN = 608,
            MOVENODELEFT = 609,
            MOVENODERIGHT = 610,
            ADDOUTPUT = 700,
            ADDFILE = 701,
            MERGEMODULE = 702,
            ADDCOMPONENTS = 703,
            LAUNCHINSTALLER = 704,
            LAUNCHUNINSTALL = 705,
            LAUNCHORCA = 706,
            FILESYSTEMEDITOR = 707,
            REGISTRYEDITOR = 708,
            FILETYPESEDITOR = 709,
            USERINTERFACEEDITOR = 710,
            CUSTOMACTIONSEDITOR = 711,
            LAUNCHCONDITIONSEDITOR = 712,
            EDITOR = 713,
            EXCLUDE = 714,
            REFRESHDEPENDENCIES = 715,
            VIEWOUTPUTS = 716,
            VIEWDEPENDENCIES = 717,
            VIEWFILTER = 718,
            KEY = 750,
            STRING = 751,
            BINARY = 752,
            DWORD = 753,
            KEYSOLO = 754,
            IMPORT = 755,
            FOLDER = 756,
            PROJECTOUTPUT = 757,
            FILE = 758,
            ADDMERGEMODULES = 759,
            CREATESHORTCUT = 760,
            LARGEICONS = 761,
            SMALLICONS = 762,
            LIST = 763,
            DETAILS = 764,
            ADDFILETYPE = 765,
            ADDACTION = 766,
            SETASDEFAULT = 767,
            MOVEUP = 768,
            MOVEDOWN = 769,
            ADDDIALOG = 770,
            IMPORTDIALOG = 771,
            ADDFILESEARCH = 772,
            ADDREGISTRYSEARCH = 773,
            ADDCOMPONENTSEARCH = 774,
            ADDLAUNCHCONDITION = 775,
            ADDCUSTOMACTION = 776,
            OUTPUTS = 777,
            DEPENDENCIES = 778,
            FILTER = 779,
            COMPONENTS = 780,
            ENVSTRING = 781,
            CREATEEMPTYSHORTCUT = 782,
            ADDFILECONDITION = 783,
            ADDREGISTRYCONDITION = 784,
            ADDCOMPONENTCONDITION = 785,
            ADDURTCONDITION = 786,
            ADDIISCONDITION = 787,
            SPECIALFOLDERBASE = 800,
            USERSAPPLICATIONDATAFOLDER = 800,
            COMMONFILES64FOLDER = 801,
            COMMONFILESFOLDER = 802,
            CUSTOMFOLDER = 803,
            USERSDESKTOP = 804,
            USERSFAVORITESFOLDER = 805,
            FONTSFOLDER = 806,
            GLOBALASSEMBLYCACHEFOLDER = 807,
            MODULERETARGETABLEFOLDER = 808,
            USERSPERSONALDATAFOLDER = 809,
            PROGRAMFILES64FOLDER = 810,
            PROGRAMFILESFOLDER = 811,
            USERSPROGRAMSMENU = 812,
            USERSSENDTOMENU = 813,
            SHAREDCOMPONENTSFOLDER = 814,
            USERSSTARTMENU = 815,
            USERSSTARTUPFOLDER = 816,
            SYSTEM64FOLDER = 817,
            SYSTEMFOLDER = 818,
            APPLICATIONFOLDER = 819,
            USERSTEMPLATEFOLDER = 820,
            WEBCUSTOMFOLDER = 821,
            WINDOWSFOLDER = 822,
            SPECIALFOLDERLAST = 823,
            EXPORTEVENTS = 900,
            IMPORTEVENTS = 901,
            VIEWEVENT = 902,
            VIEWEVENTLIST = 903,
            VIEWCHART = 904,
            VIEWMACHINEDIAGRAM = 905,
            VIEWPROCESSDIAGRAM = 906,
            VIEWSOURCEDIAGRAM = 907,
            VIEWSTRUCTUREDIAGRAM = 908,
            VIEWTIMELINE = 909,
            VIEWSUMMARY = 910,
            APPLYFILTER = 911,
            CLEARFILTER = 912,
            STARTRECORDING = 913,
            STOPRECORDING = 914,
            PAUSERECORDING = 915,
            ACTIVATEFILTER = 916,
            SHOWFIRSTEVENT = 917,
            SHOWPREVIOUSEVENT = 918,
            SHOWNEXTEVENT = 919,
            SHOWLASTEVENT = 920,
            REPLAYEVENTS = 921,
            STOPREPLAY = 922,
            INCREASEPLAYBACKSPEED = 923,
            DECREASEPLAYBACKSPEED = 924,
            ADDMACHINE = 925,
            ADDREMOVECOLUMNS = 926,
            SORTCOLUMNS = 927,
            SAVECOLUMNSETTINGS = 928,
            RESETCOLUMNSETTINGS = 929,
            SIZECOLUMNSTOFIT = 930,
            AUTOSELECT = 931,
            AUTOFILTER = 932,
            AUTOPLAYTRACK = 933,
            GOTOEVENT = 934,
            ZOOMTOFIT = 935,
            ADDGRAPH = 936,
            REMOVEGRAPH = 937,
            CONNECTMACHINE = 938,
            DISCONNECTMACHINE = 939,
            EXPANDSELECTION = 940,
            COLLAPSESELECTION = 941,
            ADDFILTER = 942,
            ADDPREDEFINED0 = 943,
            ADDPREDEFINED1 = 944,
            ADDPREDEFINED2 = 945,
            ADDPREDEFINED3 = 946,
            ADDPREDEFINED4 = 947,
            ADDPREDEFINED5 = 948,
            ADDPREDEFINED6 = 949,
            ADDPREDEFINED7 = 950,
            ADDPREDEFINED8 = 951,
            TIMELINESIZETOFIT = 952,
            FIELDVIEW = 1000,
            SELECTEXPERT = 1001,
            TOPNEXPERT = 1002,
            SORTORDER = 1003,
            PROPPAGE = 1004,
            HELP = 1005,
            SAVEREPORT = 1006,
            INSERTSUMMARY = 1007,
            INSERTGROUP = 1008,
            INSERTSUBREPORT = 1009,
            INSERTCHART = 1010,
            INSERTPICTURE = 1011,
            SETASSTARTPAGE = 1100,
            RECALCULATELINKS = 1101,
            WEBPERMISSIONS = 1102,
            COMPARETOMASTER = 1103,
            WORKOFFLINE = 1104,
            SYNCHRONIZEFOLDER = 1105,
            SYNCHRONIZEALLFOLDERS = 1106,
            COPYPROJECT = 1107,
            IMPORTFILEFROMWEB = 1108,
            INCLUDEINPROJECT = 1109,
            EXCLUDEFROMPROJECT = 1110,
            BROKENLINKSREPORT = 1111,
            ADDPROJECTOUTPUTS = 1112,
            ADDREFERENCE = 1113,
            ADDWEBREFERENCE = 1114,
            ADDWEBREFERENCECTX = 1115,
            UPDATEWEBREFERENCE = 1116,
            RUNCUSTOMTOOL = 1117,
            SETRUNTIMEVERSION = 1118,
            VIEWREFINOBJECTBROWSER = 1119,
            QUICKOBJECTSEARCH = 1119,
            PUBLISH = 1120,
            PUBLISHCTX = 1121,
            STARTOPTIONS = 1124,
            ADDREFERENCECTX = 1125,
            STARTOPTIONSCTX = 1127,
            DETACHLOCALDATAFILECTX = 1128,
            ADDSERVICEREFERENCE = 1129,
            ADDSERVICEREFERENCECTX = 1130,
            UPDATESERVICEREFERENCE = 1131,
            CONFIGURESERVICEREFERENCE = 1132,
            DRAG_MOVE = 1140,
            DRAG_COPY = 1141,
            DRAG_CANCEL = 1142,
            TESTDIALOG = 1200,
            SPACEACROSS = 1201,
            SPACEDOWN = 1202,
            TOGGLEGRID = 1203,
            TOGGLEGUIDES = 1204,
            SIZETOTEXT = 1205,
            CENTERVERT = 1206,
            CENTERHORZ = 1207,
            FLIPDIALOG = 1208,
            SETTABORDER = 1209,
            BUTTONRIGHT = 1210,
            BUTTONBOTTOM = 1211,
            AUTOLAYOUTGROW = 1212,
            AUTOLAYOUTNORESIZE = 1213,
            AUTOLAYOUTOPTIMIZE = 1214,
            GUIDESETTINGS = 1215,
            RESOURCEINCLUDES = 1216,
            RESOURCESYMBOLS = 1217,
            OPENBINARY = 1218,
            RESOURCEOPEN = 1219,
            RESOURCENEW = 1220,
            RESOURCENEWCOPY = 1221,
            INSERT = 1222,
            EXPORT = 1223,
            CTLMOVELEFT = 1224,
            CTLMOVEDOWN = 1225,
            CTLMOVERIGHT = 1226,
            CTLMOVEUP = 1227,
            CTLSIZEDOWN = 1228,
            CTLSIZEUP = 1229,
            CTLSIZELEFT = 1230,
            CTLSIZERIGHT = 1231,
            NEWACCELERATOR = 1232,
            CAPTUREKEYSTROKE = 1233,
            INSERTACTIVEXCTL = 1234,
            INVERTCOLORS = 1235,
            FLIPHORIZONTAL = 1236,
            FLIPVERTICAL = 1237,
            ROTATE90 = 1238,
            SHOWCOLORSWINDOW = 1239,
            NEWSTRING = 1240,
            NEWINFOBLOCK = 1241,
            DELETEINFOBLOCK = 1242,
            ADJUSTCOLORS = 1243,
            LOADPALETTE = 1244,
            SAVEPALETTE = 1245,
            CHECKMNEMONICS = 1246,
            DRAWOPAQUE = 1247,
            TOOLBAREDITOR = 1248,
            GRIDSETTINGS = 1249,
            NEWDEVICEIMAGE = 1250,
            OPENDEVICEIMAGE = 1251,
            DELETEDEVICEIMAGE = 1252,
            VIEWASPOPUP = 1253,
            CHECKMENUMNEMONICS = 1254,
            SHOWIMAGEGRID = 1255,
            SHOWTILEGRID = 1256,
            MAGNIFY = 1257,
            ResProps = 1258,
            IMPORTICONIMAGE = 1259,
            EXPORTICONIMAGE = 1260,
            OPENEXTERNALEDITOR = 1261,
            PICKRECTANGLE = 1300,
            PICKREGION = 1301,
            PICKCOLOR = 1302,
            ERASERTOOL = 1303,
            FILLTOOL = 1304,
            PENCILTOOL = 1305,
            BRUSHTOOL = 1306,
            AIRBRUSHTOOL = 1307,
            LINETOOL = 1308,
            CURVETOOL = 1309,
            TEXTTOOL = 1310,
            RECTTOOL = 1311,
            OUTLINERECTTOOL = 1312,
            FILLEDRECTTOOL = 1313,
            ROUNDRECTTOOL = 1314,
            OUTLINEROUNDRECTTOOL = 1315,
            FILLEDROUNDRECTTOOL = 1316,
            ELLIPSETOOL = 1317,
            OUTLINEELLIPSETOOL = 1318,
            FILLEDELLIPSETOOL = 1319,
            SETHOTSPOT = 1320,
            ZOOMTOOL = 1321,
            ZOOM1X = 1322,
            ZOOM2X = 1323,
            ZOOM6X = 1324,
            ZOOM8X = 1325,
            TRANSPARENTBCKGRND = 1326,
            OPAQUEBCKGRND = 1327,
            ERASERSMALL = 1328,
            ERASERMEDIUM = 1329,
            ERASERLARGE = 1330,
            ERASERLARGER = 1331,
            CIRCLELARGE = 1332,
            CIRCLEMEDIUM = 1333,
            CIRCLESMALL = 1334,
            SQUARELARGE = 1335,
            SQUAREMEDIUM = 1336,
            SQUARESMALL = 1337,
            LEFTDIAGLARGE = 1338,
            LEFTDIAGMEDIUM = 1339,
            LEFTDIAGSMALL = 1340,
            RIGHTDIAGLARGE = 1341,
            RIGHTDIAGMEDIUM = 1342,
            RIGHTDIAGSMALL = 1343,
            SPLASHSMALL = 1344,
            SPLASHMEDIUM = 1345,
            SPLASHLARGE = 1346,
            LINESMALLER = 1347,
            LINESMALL = 1348,
            LINEMEDIUM = 1349,
            LINELARGE = 1350,
            LINELARGER = 1351,
            LARGERBRUSH = 1352,
            LARGEBRUSH = 1353,
            STDBRUSH = 1354,
            SMALLBRUSH = 1355,
            SMALLERBRUSH = 1356,
            ZOOMIN = 1357,
            ZOOMOUT = 1358,
            PREVCOLOR = 1359,
            PREVECOLOR = 1360,
            NEXTCOLOR = 1361,
            NEXTECOLOR = 1362,
            IMG_OPTIONS = 1363,
            STARTWEBADMINTOOL = 1400,
            NESTRELATEDFILES = 1401,
            CANCELDRAG = 1500,
            DEFAULTACTION = 1501,
            CTLMOVEUPGRID = 1502,
            CTLMOVEDOWNGRID = 1503,
            CTLMOVELEFTGRID = 1504,
            CTLMOVERIGHTGRID = 1505,
            CTLSIZERIGHTGRID = 1506,
            CTLSIZEUPGRID = 1507,
            CTLSIZELEFTGRID = 1508,
            CTLSIZEDOWNGRID = 1509,
            NEXTCTL = 1510,
            PREVCTL = 1511,
            RENAME = 1550,
            EXTRACTMETHOD = 1551,
            ENCAPSULATEFIELD = 1552,
            EXTRACTINTERFACE = 1553,
            PROMOTELOCAL = 1554,
            REMOVEPARAMETERS = 1555,
            REORDERPARAMETERS = 1556,
            GENERATEMETHODSTUB = 1557,
            IMPLEMENTINTERFACEIMPLICIT = 1558,
            IMPLEMENTINTERFACEEXPLICIT = 1559,
            IMPLEMENTABSTRACTCLASS = 1560,
            SURROUNDWITH = 1561,
            ToggleWordWrapOW = 1600,
            GotoNextLocationOW = 1601,
            GotoPrevLocationOW = 1602,
            BuildOnlyProject = 1603,
            RebuildOnlyProject = 1604,
            CleanOnlyProject = 1605,
            SetBuildStartupsOnlyOnRun = 1606,
            UnhideAll = 1607,
            HideFolder = 1608,
            UnhideFolders = 1609,
            CopyFullPathName = 1610,
            SaveFolderAsSolution = 1611,
            ManageUserSettings = 1612,
            NewSolutionFolder = 1613,
            ClearPaneOW = 1615,
            GotoErrorTagOW = 1616,
            GotoNextErrorTagOW = 1617,
            GotoPrevErrorTagOW = 1618,
            ClearPaneFR1 = 1619,
            GotoErrorTagFR1 = 1620,
            GotoNextErrorTagFR1 = 1621,
            GotoPrevErrorTagFR1 = 1622,
            ClearPaneFR2 = 1623,
            GotoErrorTagFR2 = 1624,
            GotoNextErrorTagFR2 = 1625,
            GotoPrevErrorTagFR2 = 1626,
            OutputPaneCombo = 1627,
            OutputPaneComboList = 1628,
            DisableDockingChanges = 1629,
            ToggleFloat = 1630,
            ResetLayout = 1631,
            NewSolutionFolderBar = 1638,
            DataShortcut = 1639,
            NextToolWindow = 1640,
            PrevToolWindow = 1641,
            BrowseToFileInExplorer = 1642,
            ShowEzMDIFileMenu = 1643,
            PrevToolWindowNav = 1645,
            StaticAnalysisOnlyProject = 1646,
            ECMD_RUNFXCOPSEL = 1647,
            CloseAllButThis = 1650,
            CVShowInheritedMembers = 1651,
            CVShowBaseTypes = 1652,
            CVShowDerivedTypes = 1653,
            CVShowHidden = 1654,
            CVBack = 1655,
            CVForward = 1656,
            CVSearchCombo = 1657,
            CVSearch = 1658,
            CVSortObjectsAlpha = 1659,
            CVSortObjectsType = 1660,
            CVSortObjectsAccess = 1661,
            CVGroupObjectsType = 1662,
            CVSortMembersAlpha = 1663,
            CVSortMembersType = 1664,
            CVSortMembersAccess = 1665,
            CVTypeBrowserSettings = 1666,
            CVViewMembersAsImplementor = 1667,
            CVViewMembersAsSubclass = 1668,
            CVViewMembersAsUser = 1669,
            CVReserved1 = 1670,
            CVReserved2 = 1671,
            CVShowProjectReferences = 1672,
            CVGroupMembersType = 1673,
            CVClearSearch = 1674,
            CVFilterToType = 1675,
            CVSortByBestMatch = 1676,
            CVSearchMRUList = 1677,
            CVViewOtherMembers = 1678,
            CVSearchCmd = 1679,
            CVGoToSearchCmd = 1680,
            ControlGallery = 1700,
            OBShowInheritedMembers = 1711,
            OBShowBaseTypes = 1712,
            OBShowDerivedTypes = 1713,
            OBShowHidden = 1714,
            OBBack = 1715,
            OBForward = 1716,
            OBSearchCombo = 1717,
            OBSearch = 1718,
            OBSortObjectsAlpha = 1719,
            OBSortObjectsType = 1720,
            OBSortObjectsAccess = 1721,
            OBGroupObjectsType = 1722,
            OBSortMembersAlpha = 1723,
            OBSortMembersType = 1724,
            OBSortMembersAccess = 1725,
            OBTypeBrowserSettings = 1726,
            OBViewMembersAsImplementor = 1727,
            OBViewMembersAsSubclass = 1728,
            OBViewMembersAsUser = 1729,
            OBNamespacesView = 1730,
            OBContainersView = 1731,
            OBReserved1 = 1732,
            OBGroupMembersType = 1733,
            OBClearSearch = 1734,
            OBFilterToType = 1735,
            OBSortByBestMatch = 1736,
            OBSearchMRUList = 1737,
            OBViewOtherMembers = 1738,
            OBSearchCmd = 1739,
            OBGoToSearchCmd = 1740,
            OBShowExtensionMembers = 1741,
            FullScreen2 = 1775,
            FSRSortObjectsAlpha = 1776,
            FSRSortByBestMatch = 1777,
            NavigateBack = 1800,
            NavigateForward = 1801,
            ECMD_CORRECTION_1 = 1900,
            ECMD_CORRECTION_2 = 1901,
            ECMD_CORRECTION_3 = 1902,
            ECMD_CORRECTION_4 = 1903,
            ECMD_CORRECTION_5 = 1904,
            ECMD_CORRECTION_6 = 1905,
            ECMD_CORRECTION_7 = 1906,
            ECMD_CORRECTION_8 = 1907,
            ECMD_CORRECTION_9 = 1908,
            ECMD_CORRECTION_10 = 1909,
            OBAddReference = 1914,
            FindReferences = 1915,
            CodeDefView = 1926,
            CodeDefViewGoToPrev = 1927,
            CodeDefViewGoToNext = 1928,
            CodeDefViewEditDefinition = 1929,
            CodeDefViewChooseEncoding = 1930,
            ViewInClassDiagram = 1931,
            ECMD_ADDDBTABLE = 1950,
            ECMD_ADDDATATABLE = 1951,
            ECMD_ADDFUNCTION = 1952,
            ECMD_ADDRELATION = 1953,
            ECMD_ADDKEY = 1954,
            ECMD_ADDCOLUMN = 1955,
            ECMD_CONVERT_DBTABLE = 1956,
            ECMD_CONVERT_DATATABLE = 1957,
            ECMD_GENERATE_DATABASE = 1958,
            ECMD_CONFIGURE_CONNECTIONS = 1959,
            ECMD_IMPORT_XMLSCHEMA = 1960,
            ECMD_SYNC_WITH_DATABASE = 1961,
            ECMD_CONFIGURE = 1962,
            ECMD_CREATE_DATAFORM = 1963,
            ECMD_CREATE_ENUM = 1964,
            ECMD_INSERT_FUNCTION = 1965,
            ECMD_EDIT_FUNCTION = 1966,
            ECMD_SET_PRIMARY_KEY = 1967,
            ECMD_INSERT_COLUMN = 1968,
            ECMD_AUTO_SIZE = 1969,
            ECMD_SHOW_RELATION_LABELS = 1970,
            VSDGenerateDataSet = 1971,
            VSDPreview = 1972,
            VSDConfigureAdapter = 1973,
            VSDViewDatasetSchema = 1974,
            VSDDatasetProperties = 1975,
            VSDParameterizeForm = 1976,
            VSDAddChildForm = 1977,
            ECMD_EDITCONSTRAINT = 1978,
            ECMD_DELETECONSTRAINT = 1979,
            ECMD_EDITDATARELATION = 1980,
            CloseProject = 1982,
            ReloadCommandBars = 1983,
            SolutionPlatform = 1990,
            SolutionPlatformGetList = 1991,
            ECMD_DATAACCESSOR = 2000,
            ECMD_ADD_DATAACCESSOR = 2001,
            ECMD_QUERY = 2002,
            ECMD_ADD_QUERY = 2003,
            ECMD_PUBLISHSELECTION = 2005,
            ECMD_PUBLISHSLNCTX = 2006,
            CallBrowserShowCallsTo = 2010,
            CallBrowserShowCallsFrom = 2011,
            CallBrowserShowNewCallsTo = 2012,
            CallBrowserShowNewCallsFrom = 2013,
            CallBrowser1ShowCallsTo = 2014,
            CallBrowser2ShowCallsTo = 2015,
            CallBrowser3ShowCallsTo = 2016,
            CallBrowser4ShowCallsTo = 2017,
            CallBrowser5ShowCallsTo = 2018,
            CallBrowser6ShowCallsTo = 2019,
            CallBrowser7ShowCallsTo = 2020,
            CallBrowser8ShowCallsTo = 2021,
            CallBrowser9ShowCallsTo = 2022,
            CallBrowser10ShowCallsTo = 2023,
            CallBrowser11ShowCallsTo = 2024,
            CallBrowser12ShowCallsTo = 2025,
            CallBrowser13ShowCallsTo = 2026,
            CallBrowser14ShowCallsTo = 2027,
            CallBrowser15ShowCallsTo = 2028,
            CallBrowser16ShowCallsTo = 2029,
            CallBrowser1ShowCallsFrom = 2030,
            CallBrowser2ShowCallsFrom = 2031,
            CallBrowser3ShowCallsFrom = 2032,
            CallBrowser4ShowCallsFrom = 2033,
            CallBrowser5ShowCallsFrom = 2034,
            CallBrowser6ShowCallsFrom = 2035,
            CallBrowser7ShowCallsFrom = 2036,
            CallBrowser8ShowCallsFrom = 2037,
            CallBrowser9ShowCallsFrom = 2038,
            CallBrowser10ShowCallsFrom = 2039,
            CallBrowser11ShowCallsFrom = 2040,
            CallBrowser12ShowCallsFrom = 2041,
            CallBrowser13ShowCallsFrom = 2042,
            CallBrowser14ShowCallsFrom = 2043,
            CallBrowser15ShowCallsFrom = 2044,
            CallBrowser16ShowCallsFrom = 2045,
            CallBrowser1ShowFullNames = 2046,
            CallBrowser2ShowFullNames = 2047,
            CallBrowser3ShowFullNames = 2048,
            CallBrowser4ShowFullNames = 2049,
            CallBrowser5ShowFullNames = 2050,
            CallBrowser6ShowFullNames = 2051,
            CallBrowser7ShowFullNames = 2052,
            CallBrowser8ShowFullNames = 2053,
            CallBrowser9ShowFullNames = 2054,
            CallBrowser10ShowFullNames = 2055,
            CallBrowser11ShowFullNames = 2056,
            CallBrowser12ShowFullNames = 2057,
            CallBrowser13ShowFullNames = 2058,
            CallBrowser14ShowFullNames = 2059,
            CallBrowser15ShowFullNames = 2060,
            CallBrowser16ShowFullNames = 2061,
            CallBrowser1Settings = 2062,
            CallBrowser2Settings = 2063,
            CallBrowser3Settings = 2064,
            CallBrowser4Settings = 2065,
            CallBrowser5Settings = 2066,
            CallBrowser6Settings = 2067,
            CallBrowser7Settings = 2068,
            CallBrowser8Settings = 2069,
            CallBrowser9Settings = 2070,
            CallBrowser10Settings = 2071,
            CallBrowser11Settings = 2072,
            CallBrowser12Settings = 2073,
            CallBrowser13Settings = 2074,
            CallBrowser14Settings = 2075,
            CallBrowser15Settings = 2076,
            CallBrowser16Settings = 2077,
            CallBrowser1SortAlpha = 2078,
            CallBrowser2SortAlpha = 2079,
            CallBrowser3SortAlpha = 2080,
            CallBrowser4SortAlpha = 2081,
            CallBrowser5SortAlpha = 2082,
            CallBrowser6SortAlpha = 2083,
            CallBrowser7SortAlpha = 2084,
            CallBrowser8SortAlpha = 2085,
            CallBrowser9SortAlpha = 2086,
            CallBrowser10SortAlpha = 2087,
            CallBrowser11SortAlpha = 2088,
            CallBrowser12SortAlpha = 2089,
            CallBrowser13SortAlpha = 2090,
            CallBrowser14SortAlpha = 2091,
            CallBrowser15SortAlpha = 2092,
            CallBrowser16SortAlpha = 2093,
            CallBrowser1SortAccess = 2094,
            CallBrowser2SortAccess = 2095,
            CallBrowser3SortAccess = 2096,
            CallBrowser4SortAccess = 2097,
            CallBrowser5SortAccess = 2098,
            CallBrowser6SortAccess = 2099,
            CallBrowser7SortAccess = 2100,
            CallBrowser8SortAccess = 2101,
            CallBrowser9SortAccess = 2102,
            CallBrowser10SortAccess = 2103,
            CallBrowser11SortAccess = 2104,
            CallBrowser12SortAccess = 2105,
            CallBrowser13SortAccess = 2106,
            CallBrowser14SortAccess = 2107,
            CallBrowser15SortAccess = 2108,
            CallBrowser16SortAccess = 2109,
            ShowCallBrowser = 2120,
            CallBrowser1 = 2121,
            CallBrowser2 = 2122,
            CallBrowser3 = 2123,
            CallBrowser4 = 2124,
            CallBrowser5 = 2125,
            CallBrowser6 = 2126,
            CallBrowser7 = 2127,
            CallBrowser8 = 2128,
            CallBrowser9 = 2129,
            CallBrowser10 = 2130,
            CallBrowser11 = 2131,
            CallBrowser12 = 2132,
            CallBrowser13 = 2133,
            CallBrowser14 = 2134,
            CallBrowser15 = 2135,
            CallBrowser16 = 2136,
            CallBrowser17 = 2137,
            GlobalUndo = 2138,
            GlobalRedo = 2139,
            CallBrowserShowCallsToCmd = 2140,
            CallBrowserShowCallsFromCmd = 2141,
            CallBrowserShowNewCallsToCmd = 2142,
            CallBrowserShowNewCallsFromCmd = 2143,
            CallBrowser1Search = 2145,
            CallBrowser2Search = 2146,
            CallBrowser3Search = 2147,
            CallBrowser4Search = 2148,
            CallBrowser5Search = 2149,
            CallBrowser6Search = 2150,
            CallBrowser7Search = 2151,
            CallBrowser8Search = 2152,
            CallBrowser9Search = 2153,
            CallBrowser10Search = 2154,
            CallBrowser11Search = 2155,
            CallBrowser12Search = 2156,
            CallBrowser13Search = 2157,
            CallBrowser14Search = 2158,
            CallBrowser15Search = 2159,
            CallBrowser16Search = 2160,
            CallBrowser1Refresh = 2161,
            CallBrowser2Refresh = 2162,
            CallBrowser3Refresh = 2163,
            CallBrowser4Refresh = 2164,
            CallBrowser5Refresh = 2165,
            CallBrowser6Refresh = 2166,
            CallBrowser7Refresh = 2167,
            CallBrowser8Refresh = 2168,
            CallBrowser9Refresh = 2169,
            CallBrowser10Refresh = 2170,
            CallBrowser11Refresh = 2171,
            CallBrowser12Refresh = 2172,
            CallBrowser13Refresh = 2173,
            CallBrowser14Refresh = 2174,
            CallBrowser15Refresh = 2175,
            CallBrowser16Refresh = 2176,
            CallBrowser1SearchCombo = 2180,
            CallBrowser2SearchCombo = 2181,
            CallBrowser3SearchCombo = 2182,
            CallBrowser4SearchCombo = 2183,
            CallBrowser5SearchCombo = 2184,
            CallBrowser6SearchCombo = 2185,
            CallBrowser7SearchCombo = 2186,
            CallBrowser8SearchCombo = 2187,
            CallBrowser9SearchCombo = 2188,
            CallBrowser10SearchCombo = 2189,
            CallBrowser11SearchCombo = 2190,
            CallBrowser12SearchCombo = 2191,
            CallBrowser13SearchCombo = 2192,
            CallBrowser14SearchCombo = 2193,
            CallBrowser15SearchCombo = 2194,
            CallBrowser16SearchCombo = 2195,
            TaskListProviderCombo = 2200,
            TaskListProviderComboList = 2201,
            CreateUserTask = 2202,
            ErrorListShowErrors = 2210,
            ErrorListShowWarnings = 2211,
            ErrorListShowMessages = 2212,
            Registration = 2214,
            CallBrowser1SearchComboList = 2215,
            CallBrowser2SearchComboList = 2216,
            CallBrowser3SearchComboList = 2217,
            CallBrowser4SearchComboList = 2218,
            CallBrowser5SearchComboList = 2219,
            CallBrowser6SearchComboList = 2220,
            CallBrowser7SearchComboList = 2221,
            CallBrowser8SearchComboList = 2222,
            CallBrowser9SearchComboList = 2223,
            CallBrowser10SearchComboList = 2224,
            CallBrowser11SearchComboList = 2225,
            CallBrowser12SearchComboList = 2226,
            CallBrowser13SearchComboList = 2227,
            CallBrowser14SearchComboList = 2228,
            CallBrowser15SearchComboList = 2229,
            CallBrowser16SearchComboList = 2230,
            SnippetProp = 2240,
            SnippetRef = 2241,
            SnippetRepl = 2242,
            StartPage = 2245,
            EditorLineFirstColumn = 2250,
            EditorLineFirstColumnExtend = 2251,
            SEServerExplorer = 2260,
            SEDataExplorer = 2261,
            ViewCallHierarchy = 2301,
            ToggleConsumeFirstCompletionMode = 2303,
            ECMD_VALIDATION_TARGET = 11281,
            ECMD_VALIDATION_TARGET_GET_LIST = 11282,
            ECMD_CSS_TARGET = 11283,
            ECMD_CSS_TARGET_GET_LIST = 11284,
            Design = 12288,
            DesignOn = 12289,
            SEDesign = 12291,
            NewDiagram = 12292,
            NewTable = 12294,
            NewDBItem = 12302,
            NewTrigger = 12304,
            Debug = 12306,
            NewProcedure = 12307,
            NewQuery = 12308,
            RefreshLocal = 12309,
            DbAddDataConnection = 12311,
            DBDefDBRef = 12312,
            RunCmd = 12313,
            RunOn = 12314,
            NewDBRef = 12315,
            SetAsDef = 12316,
            CreateCmdFile = 12317,
            Cancel = 12318,
            NewDatabase = 12320,
            NewUser = 12321,
            NewRole = 12322,
            ChangeLogin = 12323,
            NewView = 12324,
            ModifyConnection = 12325,
            Disconnect = 12326,
            CopyScript = 12327,
            AddSCC = 12328,
            RemoveSCC = 12329,
            GetLatest = 12336,
            CheckOut = 12337,
            CheckIn = 12338,
            UndoCheckOut = 12339,
            AddItemSCC = 12340,
            NewPackageSpec = 12341,
            NewPackageBody = 12342,
            InsertSQL = 12343,
            RunSelection = 12344,
            UpdateScript = 12345,
            NewScript = 12348,
            NewFunction = 12349,
            NewTableFunction = 12350,
            NewInlineFunction = 12351,
            AddDiagram = 12352,
            AddTable = 12353,
            AddSynonym = 12354,
            AddView = 12355,
            AddProcedure = 12356,
            AddFunction = 12357,
            AddTableFunction = 12358,
            AddInlineFunction = 12359,
            AddPkgSpec = 12360,
            AddPkgBody = 12361,
            AddTrigger = 12362,
            ExportData = 12363,
            DbnsVcsAdd = 12364,
            DbnsVcsRemove = 12365,
            DbnsVcsCheckout = 12366,
            DbnsVcsUndoCheckout = 12367,
            DbnsVcsCheckin = 12368,
            SERetrieveData = 12384,
            SEEditTextObject = 12385,
            DesignSQLBlock = 12388,
            RegisterSQLInstance = 12389,
            UnregisterSQLInstance = 12390,
            CommandWindowSaveScript = 12550,
            CommandWindowRunScript = 12551,
            CommandWindowCursorUp = 12552,
            CommandWindowCursorDown = 12553,
            CommandWindowCursorLeft = 12554,
            CommandWindowCursorRight = 12555,
            CommandWindowHistoryUp = 12556,
            CommandWindowHistoryDown = 12557
        }
        [Guid("5EFC7975-14BC-11CF-9B2B-00AA00573819")]
        public enum VSStd97CmdID
        {
            AlignBottom = 1,
            AlignHorizontalCenters = 2,
            AlignLeft = 3,
            AlignRight = 4,
            AlignToGrid = 5,
            AlignTop = 6,
            AlignVerticalCenters = 7,
            ArrangeBottom = 8,
            ArrangeRight = 9,
            BringForward = 10,
            BringToFront = 11,
            CenterHorizontally = 12,
            CenterVertically = 13,
            Code = 14,
            Copy = 15,
            Cut = 16,
            Delete = 17,
            FontName = 18,
            FontSize = 19,
            Group = 20,
            HorizSpaceConcatenate = 21,
            HorizSpaceDecrease = 22,
            HorizSpaceIncrease = 23,
            HorizSpaceMakeEqual = 24,
            InsertObject = 25,
            Paste = 26,
            Print = 27,
            Properties = 28,
            Redo = 29,
            MultiLevelRedo = 30,
            SelectAll = 31,
            SendBackward = 32,
            SendToBack = 33,
            ShowTable = 34,
            SizeToControl = 35,
            SizeToControlHeight = 36,
            SizeToControlWidth = 37,
            SizeToFit = 38,
            SizeToGrid = 39,
            SnapToGrid = 40,
            TabOrder = 41,
            Toolbox = 42,
            Undo = 43,
            MultiLevelUndo = 44,
            Ungroup = 45,
            VertSpaceConcatenate = 46,
            VertSpaceDecrease = 47,
            VertSpaceIncrease = 48,
            VertSpaceMakeEqual = 49,
            ZoomPercent = 50,
            BackColor = 51,
            Bold = 52,
            BorderColor = 53,
            BorderDashDot = 54,
            BorderDashDotDot = 55,
            BorderDashes = 56,
            BorderDots = 57,
            BorderShortDashes = 58,
            BorderSolid = 59,
            BorderSparseDots = 60,
            BorderWidth1 = 61,
            BorderWidth2 = 62,
            BorderWidth3 = 63,
            BorderWidth4 = 64,
            BorderWidth5 = 65,
            BorderWidth6 = 66,
            BorderWidthHairline = 67,
            Flat = 68,
            ForeColor = 69,
            Italic = 70,
            JustifyCenter = 71,
            JustifyGeneral = 72,
            JustifyLeft = 73,
            JustifyRight = 74,
            Raised = 75,
            Sunken = 76,
            Underline = 77,
            Chiseled = 78,
            Etched = 79,
            Shadowed = 80,
            CompDebug1 = 81,
            CompDebug2 = 82,
            CompDebug3 = 83,
            CompDebug4 = 84,
            CompDebug5 = 85,
            CompDebug6 = 86,
            CompDebug7 = 87,
            CompDebug8 = 88,
            CompDebug9 = 89,
            CompDebug10 = 90,
            CompDebug11 = 91,
            CompDebug12 = 92,
            CompDebug13 = 93,
            CompDebug14 = 94,
            CompDebug15 = 95,
            ExistingSchemaEdit = 96,
            Find = 97,
            GetZoom = 98,
            QueryOpenDesign = 99,
            QueryOpenNew = 100,
            SingleTableDesign = 101,
            SingleTableNew = 102,
            ShowGrid = 103,
            NewTable = 104,
            CollapsedView = 105,
            FieldView = 106,
            VerifySQL = 107,
            HideTable = 108,
            PrimaryKey = 109,
            Save = 110,
            SaveAs = 111,
            SortAscending = 112,
            SortDescending = 113,
            AppendQuery = 114,
            CrosstabQuery = 115,
            DeleteQuery = 116,
            MakeTableQuery = 117,
            SelectQuery = 118,
            UpdateQuery = 119,
            Parameters = 120,
            Totals = 121,
            ViewCollapsed = 122,
            ViewFieldList = 123,
            ViewKeys = 124,
            ViewGrid = 125,
            InnerJoin = 126,
            RightOuterJoin = 127,
            LeftOuterJoin = 128,
            FullOuterJoin = 129,
            UnionJoin = 130,
            ShowSQLPane = 131,
            ShowGraphicalPane = 132,
            ShowDataPane = 133,
            ShowQBEPane = 134,
            SelectAllFields = 135,
            OLEObjectMenuButton = 136,
            ObjectVerbList0 = 137,
            ObjectVerbList1 = 138,
            ObjectVerbList2 = 139,
            ObjectVerbList3 = 140,
            ObjectVerbList4 = 141,
            ObjectVerbList5 = 142,
            ObjectVerbList6 = 143,
            ObjectVerbList7 = 144,
            ObjectVerbList8 = 145,
            ObjectVerbList9 = 146,
            ConvertObject = 147,
            CustomControl = 148,
            CustomizeItem = 149,
            Rename = 150,
            Import = 151,
            NewPage = 152,
            Move = 153,
            Cancel = 154,
            Font = 155,
            ExpandLinks = 156,
            ExpandImages = 157,
            ExpandPages = 158,
            RefocusDiagram = 159,
            TransitiveClosure = 160,
            CenterDiagram = 161,
            ZoomIn = 162,
            ZoomOut = 163,
            RemoveFilter = 164,
            HidePane = 165,
            DeleteTable = 166,
            DeleteRelationship = 167,
            Remove = 168,
            JoinLeftAll = 169,
            JoinRightAll = 170,
            AddToOutput = 171,
            OtherQuery = 172,
            GenerateChangeScript = 173,
            SaveSelection = 174,
            AutojoinCurrent = 175,
            AutojoinAlways = 176,
            EditPage = 177,
            ViewLinks = 178,
            Stop = 179,
            Pause = 180,
            Resume = 181,
            FilterDiagram = 182,
            ShowAllObjects = 183,
            ShowApplications = 184,
            ShowOtherObjects = 185,
            ShowPrimRelationships = 186,
            Expand = 187,
            Collapse = 188,
            Refresh = 189,
            Layout = 190,
            ShowResources = 191,
            InsertHTMLWizard = 192,
            ShowDownloads = 193,
            ShowExternals = 194,
            ShowInBoundLinks = 195,
            ShowOutBoundLinks = 196,
            ShowInAndOutBoundLinks = 197,
            Preview = 198,
            OpenWith = 199,
            ShowPages = 200,
            RunQuery = 201,
            ClearQuery = 202,
            RecordFirst = 203,
            RecordLast = 204,
            RecordNext = 205,
            RecordPrevious = 206,
            RecordGoto = 207,
            RecordNew = 208,
            InsertNewMenu = 209,
            InsertSeparator = 210,
            EditMenuNames = 211,
            DebugExplorer = 212,
            DebugProcesses = 213,
            ViewThreadsWindow = 214,
            WindowUIList = 215,
            NewProject = 216,
            OpenProject = 217,
            OpenSolution = 218,
            CloseSolution = 219,
            AddNewItem = 220,
            FileNew = 221,
            FileOpen = 222,
            FileClose = 223,
            SaveSolution = 224,
            SaveSolutionAs = 225,
            SaveProjectItemAs = 226,
            PageSetup = 227,
            PrintPreview = 228,
            Exit = 229,
            Replace = 230,
            Goto = 231,
            PropertyPages = 232,
            FullScreen = 233,
            ProjectExplorer = 234,
            PropertiesWindow = 235,
            TaskListWindow = 236,
            OutputWindow = 237,
            ObjectBrowser = 238,
            DocOutlineWindow = 239,
            ImmediateWindow = 240,
            WatchWindow = 241,
            LocalsWindow = 242,
            CallStack = 243,
            AddExistingItem = 244,
            NewFolder = 245,
            SetStartupProject = 246,
            ProjectSettings = 247,
            StepInto = 248,
            StepOver = 249,
            StepOut = 250,
            RunToCursor = 251,
            AddWatch = 252,
            EditWatch = 253,
            QuickWatch = 254,
            ToggleBreakpoint = 255,
            ClearBreakpoints = 256,
            ShowBreakpoints = 257,
            SetNextStatement = 258,
            ShowNextStatement = 259,
            EditBreakpoint = 260,
            Open = 261,
            DetachDebugger = 262,
            CustomizeKeyboard = 263,
            ToolsOptions = 264,
            NewWindow = 265,
            Split = 266,
            Cascade = 267,
            TileHorz = 268,
            TileVert = 269,
            TechSupport = 270,
            About = 271,
            DebugOptions = 272,
            DeleteWatch = 274,
            CollapseWatch = 275,
            FindSimplePattern = 276,
            FindInFiles = 277,
            ReplaceInFiles = 278,
            NextLocation = 279,
            PreviousLocation = 280,
            GotoQuick = 281,
            PbrsToggleStatus = 282,
            PropbrsHide = 283,
            DockingView = 284,
            HideActivePane = 285,
            PaneNextTab = 286,
            PanePrevTab = 287,
            PaneCloseToolWindow = 288,
            PaneActivateDocWindow = 289,
            DockingViewMDI = 290,
            DockingViewFloater = 291,
            AutoHideWindow = 292,
            MoveToDropdownBar = 293,
            FindCmd = 294,
            Start = 295,
            Restart = 296,
            AddinManager = 297,
            MultiLevelUndoList = 298,
            MultiLevelRedoList = 299,
            ToolboxAddTab = 300,
            ToolboxDeleteTab = 301,
            ToolboxRenameTab = 302,
            ToolboxTabMoveUp = 303,
            ToolboxTabMoveDown = 304,
            ToolboxRenameItem = 305,
            ToolboxListView = 306,
            SearchSetCombo = 307,
            WindowUIGetList = 308,
            InsertValuesQuery = 309,
            ShowProperties = 310,
            ThreadSuspend = 311,
            ThreadResume = 312,
            ThreadSetFocus = 313,
            DisplayRadix = 314,
            OpenProjectItem = 315,
            PaneNextPane = 316,
            PanePrevPane = 317,
            ClearPane = 318,
            GotoErrorTag = 319,
            TaskListSortByCategory = 320,
            TaskListSortByFileLine = 321,
            TaskListSortByPriority = 322,
            TaskListSortByDefaultSort = 323,
            TaskListShowTooltip = 324,
            TaskListFilterByNothing = 325,
            CancelEZDrag = 326,
            TaskListFilterByCategoryCompiler = 327,
            TaskListFilterByCategoryComment = 328,
            ToolboxAddItem = 329,
            ToolboxReset = 330,
            SaveProjectItem = 331,
            ViewForm = 332,
            ViewCode = 333,
            PreviewInBrowser = 334,
            BrowseWith = 336,
            SearchCombo = 337,
            EditLabel = 338,
            Exceptions = 339,
            DefineViews = 340,
            ToggleSelMode = 341,
            ToggleInsMode = 342,
            LoadUnloadedProject = 343,
            UnloadLoadedProject = 344,
            ElasticColumn = 345,
            HideColumn = 346,
            TaskListPreviousView = 347,
            ZoomDialog = 348,
            FindHiddenText = 349,
            FindMatchCase = 350,
            FindWholeWord = 351,
            FindRegularExpression = 352,
            FindBackwards = 353,
            FindInSelection = 354,
            FindStop = 355,
            TaskListNextError = 357,
            TaskListPrevError = 358,
            TaskListFilterByCategoryUser = 359,
            TaskListFilterByCategoryShortcut = 360,
            TaskListFilterByCategoryHTML = 361,
            TaskListFilterByCurrentFile = 362,
            TaskListFilterByChecked = 363,
            TaskListFilterByUnchecked = 364,
            TaskListSortByDescription = 365,
            TaskListSortByChecked = 366,
            ProjectReferences = 367,
            StartNoDebug = 368,
            LockControls = 369,
            FindNext = 370,
            FindPrev = 371,
            FindSelectedNext = 372,
            FindSelectedPrev = 373,
            SearchGetList = 374,
            InsertBreakpoint = 375,
            EnableBreakpoint = 376,
            F1Help = 377,
            MoveToNextEZCntr = 384,
            NewProjectFromExisting = 385,
            UpdateMarkerSpans = 386,
            MoveToPreviousEZCntr = 393,
            ProjectProperties = 396,
            PropSheetOrProperties = 397,
            TshellStep = 398,
            TshellRun = 399,
            MarkerCmd0 = 400,
            MarkerCmd1 = 401,
            MarkerCmd2 = 402,
            MarkerCmd3 = 403,
            MarkerCmd4 = 404,
            MarkerCmd5 = 405,
            MarkerCmd6 = 406,
            MarkerCmd7 = 407,
            MarkerCmd8 = 408,
            MarkerCmd9 = 409,
            MarkerLast = 409,
            MarkerEnd = 410,
            ReloadProject = 412,
            UnloadProject = 413,
            NewBlankSolution = 414,
            SelectProjectTemplate = 415,
            DetachAttachOutline = 420,
            ShowHideOutline = 421,
            SyncOutline = 422,
            RunToCallstCursor = 423,
            NoCmdsAvailable = 424,
            ContextWindow = 427,
            Alias = 428,
            GotoCommandLine = 429,
            EvaluateExpression = 430,
            ImmediateMode = 431,
            EvaluateStatement = 432,
            FindResultWindow1 = 433,
            FindResultWindow2 = 434,
            OpenProjectFromWeb = 450,
            FileOpenFromWeb = 451,
            FontNameGetList = 500,
            FontSizeGetList = 501,
            RenameBookmark = 559,
            ToggleBookmark = 560,
            DeleteBookmark = 561,
            BookmarkWindowGoToBookmark = 562,
            EnableBookmark = 564,
            NewBookmarkFolder = 565,
            NextBookmarkFolder = 568,
            PrevBookmarkFolder = 569,
            Window1 = 570,
            Window2 = 571,
            Window3 = 572,
            Window4 = 573,
            Window5 = 574,
            Window6 = 575,
            Window7 = 576,
            Window8 = 577,
            Window9 = 578,
            Window10 = 579,
            Window11 = 580,
            Window12 = 581,
            Window13 = 582,
            Window14 = 583,
            Window15 = 584,
            Window16 = 585,
            Window17 = 586,
            Window18 = 587,
            Window19 = 588,
            Window20 = 589,
            Window21 = 590,
            Window22 = 591,
            Window23 = 592,
            Window24 = 593,
            Window25 = 594,
            MoreWindows = 595,
            AutoHideAllWindows = 597,
            TaskListTaskHelp = 598,
            ClassView = 599,
            MRUProj1 = 600,
            MRUProj2 = 601,
            MRUProj3 = 602,
            MRUProj4 = 603,
            MRUProj5 = 604,
            MRUProj6 = 605,
            MRUProj7 = 606,
            MRUProj8 = 607,
            MRUProj9 = 608,
            MRUProj10 = 609,
            MRUProj11 = 610,
            MRUProj12 = 611,
            MRUProj13 = 612,
            MRUProj14 = 613,
            MRUProj15 = 614,
            MRUProj16 = 615,
            MRUProj17 = 616,
            MRUProj18 = 617,
            MRUProj19 = 618,
            MRUProj20 = 619,
            MRUProj21 = 620,
            MRUProj22 = 621,
            MRUProj23 = 622,
            MRUProj24 = 623,
            MRUProj25 = 624,
            SplitNext = 625,
            SplitPrev = 626,
            CloseAllDocuments = 627,
            NextDocument = 628,
            PrevDocument = 629,
            Tool1 = 630,
            Tool2 = 631,
            Tool3 = 632,
            Tool4 = 633,
            Tool5 = 634,
            Tool6 = 635,
            Tool7 = 636,
            Tool8 = 637,
            Tool9 = 638,
            Tool10 = 639,
            Tool11 = 640,
            Tool12 = 641,
            Tool13 = 642,
            Tool14 = 643,
            Tool15 = 644,
            Tool16 = 645,
            Tool17 = 646,
            Tool18 = 647,
            Tool19 = 648,
            Tool20 = 649,
            Tool21 = 650,
            Tool22 = 651,
            Tool23 = 652,
            Tool24 = 653,
            ExternalCommands = 654,
            PasteNextTBXCBItem = 655,
            ToolboxShowAllTabs = 656,
            ProjectDependencies = 657,
            CloseDocument = 658,
            ToolboxSortItems = 659,
            ViewBarView1 = 660,
            ViewBarView2 = 661,
            ViewBarView3 = 662,
            ViewBarView4 = 663,
            ViewBarView5 = 664,
            ViewBarView6 = 665,
            ViewBarView7 = 666,
            ViewBarView8 = 667,
            ViewBarView9 = 668,
            ViewBarView10 = 669,
            ViewBarView11 = 670,
            ViewBarView12 = 671,
            ViewBarView13 = 672,
            ViewBarView14 = 673,
            ViewBarView15 = 674,
            ViewBarView16 = 675,
            ManageIndexes = 675,
            ViewBarView17 = 676,
            ManageRelationships = 676,
            ViewBarView18 = 677,
            ManageConstraints = 677,
            ViewBarView19 = 678,
            TaskListCustomView1 = 678,
            ViewBarView20 = 679,
            TaskListCustomView2 = 679,
            ViewBarView21 = 680,
            TaskListCustomView3 = 680,
            ViewBarView22 = 681,
            TaskListCustomView4 = 681,
            ViewBarView23 = 682,
            TaskListCustomView5 = 682,
            ViewBarView24 = 683,
            TaskListCustomView6 = 683,
            SolutionCfg = 684,
            TaskListCustomView7 = 684,
            SolutionCfgGetList = 685,
            TaskListCustomView8 = 685,
            TaskListCustomView9 = 686,
            TaskListCustomView10 = 687,
            TaskListCustomView11 = 688,
            TaskListCustomView12 = 689,
            TaskListCustomView13 = 690,
            TaskListCustomView14 = 691,
            TaskListCustomView15 = 692,
            TaskListCustomView16 = 693,
            TaskListCustomView17 = 694,
            TaskListCustomView18 = 695,
            TaskListCustomView19 = 696,
            TaskListCustomView20 = 697,
            TaskListCustomView21 = 698,
            TaskListCustomView22 = 699,
            TaskListCustomView23 = 700,
            TaskListCustomView24 = 701,
            TaskListCustomView25 = 702,
            TaskListCustomView26 = 703,
            TaskListCustomView27 = 704,
            TaskListCustomView28 = 705,
            TaskListCustomView29 = 706,
            TaskListCustomView30 = 707,
            TaskListCustomView31 = 708,
            TaskListCustomView32 = 709,
            TaskListCustomView33 = 710,
            TaskListCustomView34 = 711,
            TaskListCustomView35 = 712,
            TaskListCustomView36 = 713,
            TaskListCustomView37 = 714,
            TaskListCustomView38 = 715,
            TaskListCustomView39 = 716,
            TaskListCustomView40 = 717,
            TaskListCustomView41 = 718,
            TaskListCustomView42 = 719,
            TaskListCustomView43 = 720,
            TaskListCustomView44 = 721,
            TaskListCustomView45 = 722,
            TaskListCustomView46 = 723,
            TaskListCustomView47 = 724,
            TaskListCustomView48 = 725,
            TaskListCustomView49 = 726,
            TaskListCustomView50 = 727,
            WhiteSpace = 728,
            CommandWindow = 729,
            CommandWindowMarkMode = 730,
            LogCommandWindow = 731,
            Shell = 732,
            SingleChar = 733,
            ZeroOrMore = 734,
            OneOrMore = 735,
            BeginLine = 736,
            EndLine = 737,
            BeginWord = 738,
            EndWord = 739,
            CharInSet = 740,
            CharNotInSet = 741,
            Or = 742,
            Escape = 743,
            TagExp = 744,
            PatternMatchHelp = 745,
            RegExList = 746,
            AutosWindow = 747,
            DebugReserved1 = 747,
            ThisWindow = 748,
            DebugReserved2 = 748,
            DebugReserved3 = 749,
            WildZeroOrMore = 754,
            WildSingleChar = 755,
            WildSingleDigit = 756,
            WildCharInSet = 757,
            WildCharNotInSet = 758,
            FindWhatText = 759,
            TaggedExp1 = 760,
            TaggedExp2 = 761,
            TaggedExp3 = 762,
            TaggedExp4 = 763,
            TaggedExp5 = 764,
            TaggedExp6 = 765,
            TaggedExp7 = 766,
            TaggedExp8 = 767,
            TaggedExp9 = 768,
            EditorWidgetClick = 769,
            CmdWinUpdateAC = 770,
            SlnCfgMgr = 771,
            AddNewProject = 772,
            AddExistingProject = 773,
            AddExistingProjFromWeb = 774,
            AutoHideContext1 = 776,
            AutoHideContext2 = 777,
            AutoHideContext3 = 778,
            AutoHideContext4 = 779,
            AutoHideContext5 = 780,
            AutoHideContext6 = 781,
            AutoHideContext7 = 782,
            AutoHideContext8 = 783,
            AutoHideContext9 = 784,
            AutoHideContext10 = 785,
            AutoHideContext11 = 786,
            AutoHideContext12 = 787,
            AutoHideContext13 = 788,
            AutoHideContext14 = 789,
            AutoHideContext15 = 790,
            AutoHideContext16 = 791,
            AutoHideContext17 = 792,
            AutoHideContext18 = 793,
            AutoHideContext19 = 794,
            AutoHideContext20 = 795,
            AutoHideContext21 = 796,
            AutoHideContext22 = 797,
            AutoHideContext23 = 798,
            AutoHideContext24 = 799,
            AutoHideContext25 = 800,
            AutoHideContext26 = 801,
            AutoHideContext27 = 802,
            AutoHideContext28 = 803,
            AutoHideContext29 = 804,
            AutoHideContext30 = 805,
            AutoHideContext31 = 806,
            AutoHideContext32 = 807,
            AutoHideContext33 = 808,
            ShellNavBackward = 809,
            ShellNavForward = 810,
            ShellNavigate1 = 811,
            ShellNavigate2 = 812,
            ShellNavigate3 = 813,
            ShellNavigate4 = 814,
            ShellNavigate5 = 815,
            ShellNavigate6 = 816,
            ShellNavigate7 = 817,
            ShellNavigate8 = 818,
            ShellNavigate9 = 819,
            ShellNavigate10 = 820,
            ShellNavigate11 = 821,
            ShellNavigate12 = 822,
            ShellNavigate13 = 823,
            ShellNavigate14 = 824,
            ShellNavigate15 = 825,
            ShellNavigate16 = 826,
            ShellNavigate17 = 827,
            ShellNavigate18 = 828,
            ShellNavigate19 = 829,
            ShellNavigate20 = 830,
            ShellNavigate21 = 831,
            ShellNavigate22 = 832,
            ShellNavigate23 = 833,
            ShellNavigate24 = 834,
            ShellNavigate25 = 835,
            ShellNavigate26 = 836,
            ShellNavigate27 = 837,
            ShellNavigate28 = 838,
            ShellNavigate29 = 839,
            ShellNavigate30 = 840,
            ShellNavigate31 = 841,
            ShellNavigate32 = 842,
            ShellNavigate33 = 843,
            ShellWindowNavigate1 = 844,
            ShellWindowNavigate2 = 845,
            ShellWindowNavigate3 = 846,
            ShellWindowNavigate4 = 847,
            ShellWindowNavigate5 = 848,
            ShellWindowNavigate6 = 849,
            ShellWindowNavigate7 = 850,
            ShellWindowNavigate8 = 851,
            ShellWindowNavigate9 = 852,
            ShellWindowNavigate10 = 853,
            ShellWindowNavigate11 = 854,
            ShellWindowNavigate12 = 855,
            ShellWindowNavigate13 = 856,
            ShellWindowNavigate14 = 857,
            ShellWindowNavigate15 = 858,
            ShellWindowNavigate16 = 859,
            ShellWindowNavigate17 = 860,
            ShellWindowNavigate18 = 861,
            ShellWindowNavigate19 = 862,
            ShellWindowNavigate20 = 863,
            ShellWindowNavigate21 = 864,
            ShellWindowNavigate22 = 865,
            ShellWindowNavigate23 = 866,
            ShellWindowNavigate24 = 867,
            ShellWindowNavigate25 = 868,
            ShellWindowNavigate26 = 869,
            ShellWindowNavigate27 = 870,
            ShellWindowNavigate28 = 871,
            ShellWindowNavigate29 = 872,
            ShellWindowNavigate30 = 873,
            ShellWindowNavigate31 = 874,
            ShellWindowNavigate32 = 875,
            ShellWindowNavigate33 = 876,
            OBSDoFind = 877,
            OBSMatchCase = 878,
            OBSMatchSubString = 879,
            OBSMatchWholeWord = 880,
            OBSMatchPrefix = 881,
            BuildSln = 882,
            RebuildSln = 883,
            DeploySln = 884,
            CleanSln = 885,
            BuildSel = 886,
            RebuildSel = 887,
            DeploySel = 888,
            CleanSel = 889,
            CancelBuild = 890,
            BatchBuildDlg = 891,
            BuildCtx = 892,
            RebuildCtx = 893,
            DeployCtx = 894,
            CleanCtx = 895,
            QryManageIndexes = 896,
            PrintDefault = 897,
            BrowseDoc = 898,
            ShowStartPage = 899,
            MRUFile1 = 900,
            MRUFile2 = 901,
            MRUFile3 = 902,
            MRUFile4 = 903,
            MRUFile5 = 904,
            MRUFile6 = 905,
            MRUFile7 = 906,
            MRUFile8 = 907,
            MRUFile9 = 908,
            MRUFile10 = 909,
            MRUFile11 = 910,
            MRUFile12 = 911,
            MRUFile13 = 912,
            MRUFile14 = 913,
            MRUFile15 = 914,
            MRUFile16 = 915,
            MRUFile17 = 916,
            MRUFile18 = 917,
            MRUFile19 = 918,
            MRUFile20 = 919,
            MRUFile21 = 920,
            MRUFile22 = 921,
            MRUFile23 = 922,
            MRUFile24 = 923,
            MRUFile25 = 924,
            ExtToolsCurPath = 925,
            ExtToolsCurDir = 926,
            ExtToolsCurFileName = 927,
            ExtToolsCurExtension = 928,
            ExtToolsProjDir = 929,
            ExtToolsProjFileName = 930,
            ExtToolsSlnDir = 931,
            ExtToolsSlnFileName = 932,
            GotoDefn = 935,
            GotoDecl = 936,
            BrowseDefn = 937,
            SyncClassView = 938,
            ShowMembers = 939,
            ShowBases = 940,
            ShowDerived = 941,
            ShowDefns = 942,
            ShowRefs = 943,
            ShowCallers = 944,
            ShowCallees = 945,
            AddClass = 946,
            AddNestedClass = 947,
            AddInterface = 948,
            AddMethod = 949,
            AddProperty = 950,
            AddEvent = 951,
            AddVariable = 952,
            ImplementInterface = 953,
            Override = 954,
            AddFunction = 955,
            AddConnectionPoint = 956,
            AddIndexer = 957,
            BuildOrder = 958,
            SaveOptions = 959,
            OBShowHidden = 960,
            OBEnableGrouping = 961,
            OBSetGroupingCriteria = 962,
            OBBack = 963,
            OBForward = 964,
            OBShowPackages = 965,
            OBSearchCombo = 966,
            OBSearchOptWholeWord = 967,
            OBSearchOptSubstring = 968,
            OBSearchOptPrefix = 969,
            OBSearchOptCaseSensitive = 970,
            CVGroupingNone = 971,
            CVGroupingSortOnly = 972,
            CVGroupingGrouped = 973,
            CVShowPackages = 974,
            CVNewFolder = 975,
            CVGroupingSortAccess = 976,
            ObjectSearch = 977,
            ObjectSearchResults = 978,
            Build1 = 979,
            Build2 = 980,
            Build3 = 981,
            Build4 = 982,
            Build5 = 983,
            Build6 = 984,
            Build7 = 985,
            Build8 = 986,
            Build9 = 987,
            BuildLast = 988,
            Rebuild1 = 989,
            Rebuild2 = 990,
            Rebuild3 = 991,
            Rebuild4 = 992,
            Rebuild5 = 993,
            Rebuild6 = 994,
            Rebuild7 = 995,
            Rebuild8 = 996,
            Rebuild9 = 997,
            RebuildLast = 998,
            Clean1 = 999,
            Clean2 = 1000,
            Clean3 = 1001,
            Clean4 = 1002,
            Clean5 = 1003,
            Clean6 = 1004,
            Clean7 = 1005,
            Clean8 = 1006,
            Clean9 = 1007,
            CleanLast = 1008,
            Deploy1 = 1009,
            Deploy2 = 1010,
            Deploy3 = 1011,
            Deploy4 = 1012,
            Deploy5 = 1013,
            Deploy6 = 1014,
            Deploy7 = 1015,
            Deploy8 = 1016,
            Deploy9 = 1017,
            DeployLast = 1018,
            BuildProjPicker = 1019,
            RebuildProjPicker = 1020,
            CleanProjPicker = 1021,
            DeployProjPicker = 1022,
            ResourceView = 1023,
            ShowHomePage = 1024,
            EditMenuIDs = 1025,
            LineBreak = 1026,
            CPPIdentifier = 1027,
            QuotedString = 1028,
            SpaceOrTab = 1029,
            Integer = 1030,
            CustomizeToolbars = 1036,
            MoveToTop = 1037,
            WindowHelp = 1038,
            ViewPopup = 1039,
            CheckMnemonics = 1040,
            PRSortAlphabeticaly = 1041,
            PRSortByCategory = 1042,
            ViewNextTab = 1043,
            CheckForUpdates = 1044,
            Browser1 = 1045,
            Browser2 = 1046,
            Browser3 = 1047,
            Browser4 = 1048,
            Browser5 = 1049,
            Browser6 = 1050,
            Browser7 = 1051,
            Browser8 = 1052,
            Browser9 = 1053,
            Browser10 = 1054,
            Browser11 = 1055,
            OpenDropDownOpen = 1058,
            OpenDropDownOpenWith = 1059,
            ToolsDebugProcesses = 1060,
            PaneNextSubPane = 1062,
            PanePrevSubPane = 1063,
            MoveFileToProject1 = 1070,
            MoveFileToProject2 = 1071,
            MoveFileToProject3 = 1072,
            MoveFileToProject4 = 1073,
            MoveFileToProject5 = 1074,
            MoveFileToProject6 = 1075,
            MoveFileToProject7 = 1076,
            MoveFileToProject8 = 1077,
            MoveFileToProject9 = 1078,
            MoveFileToProjectLast = 1079,
            MoveFileToProjectPick = 1081,
            DefineSubset = 1095,
            SubsetCombo = 1096,
            SubsetGetList = 1097,
            OBSortObjectsAlpha = 1098,
            OBSortObjectsType = 1099,
            OBSortObjectsAccess = 1100,
            OBGroupObjectsType = 1101,
            OBGroupObjectsAccess = 1102,
            OBSortMembersAlpha = 1103,
            OBSortMembersType = 1104,
            OBSortMembersAccess = 1105,
            PopBrowseContext = 1106,
            GotoRef = 1107,
            OBSLookInReferences = 1108,
            ExtToolsTargetPath = 1109,
            ExtToolsTargetDir = 1110,
            ExtToolsTargetFileName = 1111,
            ExtToolsTargetExtension = 1112,
            ExtToolsCurLine = 1113,
            ExtToolsCurCol = 1114,
            ExtToolsCurText = 1115,
            BrowseNext = 1116,
            BrowsePrev = 1117,
            BrowseUnload = 1118,
            QuickObjectSearch = 1119,
            ExpandAll = 1120,
            ExtToolsBinDir = 1121,
            BookmarkWindow = 1122,
            CodeExpansionWindow = 1123,
            NextDocumentNav = 1124,
            PrevDocumentNav = 1125,
            ForwardBrowseContext = 1126,
            StandardMax = 1500,
            FindReferences = 1915,
            FormsFirst = 24576,
            FormsLast = 28671,
            VBEFirst = 32768,
            Zoom200 = 32770,
            Zoom150 = 32771,
            Zoom100 = 32772,
            Zoom75 = 32773,
            Zoom50 = 32774,
            Zoom25 = 32775,
            Zoom10 = 32784,
            VBELast = 40959,
            SterlingFirst = 40960,
            SterlingLast = 49151,
            uieventidFirst = 49152,
            uieventidSelectRegion = 49153,
            uieventidDrop = 49154,
            uieventidLast = 57343
        }
        [Guid("12F1A339-02B9-46e6-BDAF-1071F76056BF")]
        public enum AppCommandCmdID
        {
            BrowserBackward = 1,
            BrowserForward = 2,
            BrowserRefresh = 3,
            BrowserStop = 4,
            BrowserSearch = 5,
            BrowserFavorites = 6,
            BrowserHome = 7,
            VolumeMute = 8,
            VolumeDown = 9,
            VolumeUp = 10,
            MediaNextTrack = 11,
            MediaPreviousTrack = 12,
            MediaStop = 13,
            MediaPlayPause = 14,
            LaunchMail = 15,
            LaunchMediaSelect = 16,
            LaunchApp1 = 17,
            LaunchApp2 = 18,
            BassDown = 19,
            BassBoost = 20,
            BassUp = 21,
            TrebleDown = 22,
            TrebleUp = 23,
            MicrophoneVolumeMute = 24,
            MicrophoneVolumeDown = 25,
            MicrophoneVolumeUp = 26
        }
        [Flags]
        public enum VsUIAccelModifiers : uint
        {
            VSAM_None = 0,
            VSAM_Shift = 1,
            VSAM_Control = 2,
            VSAM_Alt = 4,
            VSAM_Windows = 8
        }
        public enum VsSearchNavigationKeys : uint
        {
            SNK_Enter = 0,
            SNK_Down = 1,
            SNK_Up = 2,
            SNK_PageDown = 3,
            SNK_PageUp = 4,
            SNK_Home = 5,
            SNK_End = 6,
            SNK_Escape = 7
        }
        public enum VsSearchTaskStatus : uint
        {
            Created = 0,
            Started = 1,
            Completed = 2,
            Stopped = 3,
            Error = 4
        }
        //
        // Summary:
        //     Result codes from IVsUIShell.ShowMessageBox
        public enum MessageBoxResult
        {
            IDOK = 1,
            IDCANCEL = 2,
            IDABORT = 3,
            IDRETRY = 4,
            IDIGNORE = 5,
            IDYES = 6,
            IDNO = 7,
            IDCLOSE = 8,
            IDHELP = 9,
            IDTRYAGAIN = 10,
            IDCONTINUE = 11
        }
        //
        // Summary:
        //     Set of the standard, shared commands in CMDSETID.StandardCommandSet2010_guid
        [Guid("5DD0BB59-7076-4C59-88D3-DE36931F63F0")]
        public enum VSStd2010CmdID
        {
            DynamicToolBarListFirst = 1,
            DynamicToolBarListLast = 300,
            WindowFrameDockMenu = 500,
            NextDocumentTab = 600,
            PreviousDocumentTab = 601,
            ShellNavigate1First = 1000,
            ShellNavigate2First = 1033,
            ShellNavigate3First = 1066,
            ShellNavigate4First = 1099,
            ShellNavigate5First = 1132,
            ShellNavigate6First = 1165,
            ShellNavigate7First = 1198,
            ShellNavigate8First = 1231,
            ShellNavigate9First = 1264,
            ShellNavigate10First = 1297,
            ShellNavigate11First = 1330,
            ShellNavigate12First = 1363,
            ShellNavigate13First = 1396,
            ShellNavigate14First = 1429,
            ShellNavigate15First = 1462,
            ShellNavigate16First = 1495,
            ShellNavigate17First = 1528,
            ShellNavigate18First = 1561,
            ShellNavigate19First = 1594,
            ShellNavigate20First = 1627,
            ShellNavigate21First = 1660,
            ShellNavigate22First = 1693,
            ShellNavigate23First = 1726,
            ShellNavigate24First = 1759,
            ShellNavigate25First = 1792,
            ShellNavigate26First = 1825,
            ShellNavigate27First = 1858,
            ShellNavigate28First = 1891,
            ShellNavigate29First = 1924,
            ShellNavigate30First = 1957,
            ShellNavigate31First = 1990,
            ShellNavigate32First = 2023,
            ShellNavigateLast = 2055,
            ZoomIn = 2100,
            ZoomOut = 2101,
            OUTLN_EXPAND_ALL = 2500,
            OUTLN_COLLAPSE_ALL = 2501,
            OUTLN_EXPAND_CURRENT = 2502,
            OUTLN_COLLAPSE_CURRENT = 2503,
            ExtensionManager = 3000
        }

        //
        // Summary:
        //     Known project retargeting setup drivers For installing missing components
        public static class SetupDrivers
        {
            public static readonly Guid SetupDriver_VS;
            public static readonly Guid SetupDriver_WebPI;
            public static readonly Guid SetupDriver_OOBFeed;
        }
        public static class UICONTEXT
        {
            public const string RESXEditor_string = "{FEA4DCC9-3645-44CD-92E7-84B55A16465C}";
            public const string FSharpProject_string = "{F2A71F9B-5D33-465A-A702-920D77279786}";
            public const string VCProject_string = "{8BC9CEB8-8B4A-11D0-8D11-00A0C91BC942}";
            public const string CSharpProject_string = "{FAE04EC1-301F-11D3-BF4B-00C04F79EFBC}";
            public const string VBProject_string = "{164B10B9-B200-11D0-8C61-00A0C91E29D5}";
            public const string ToolboxChooseItemsDataSourceInitialized_string = "{99E78B3D-93FF-4EDB-927F-79FF19F598D6}";
            public const string ToolboxInitialized_string = "{DC5DB425-F0FD-4403-96A1-F475CDBA9EE0}";
            [Obsolete("Do not use this field.  The UIContext to which it refers will never be activated.")]
            public const string ProjectCreating_string = "{03BDEAC4-7186-458B-A2B0-941605D9917F}";
            public const string SolutionOrProjectUpgrading_string = "{EF4F870B-7B85-4F29-9D15-CE1ABFBE733B}";
            public const string BulkFileOperation_string = "{1F45BEB3-297F-49D9-9C18-069695B9031F}";
            public const string FullSolutionLoading_string = "{164FD4DC-B2A4-448E-BB60-0583CD343D3B}";
            public const string OutputWindowCreated_string = "{34E76E81-EE4A-11D0-AE2E-00A0C90FFFC3}";
            public const string SolutionHasAppContainerProject_string = "{7CAC4AE1-2E6B-4B02-A91C-71611E86F273}";
            public const string SolutionHasSingleProject_string = "{ADFC4E66-0397-11D1-9F4E-00A0C911004F}";
            public const string SolutionHasMultipleProjects_string = "{93694FA0-0397-11D1-9F4E-00A0C911004F}";
            public const string SolutionExistsAndNotBuildingAndNotDebugging_string = "{D0E4DEEC-1B53-4CDA-8559-D454583AD23B}";
            public const string SolutionExistsAndFullyLoaded_string = "{10534154-102D-46E2-ABA8-A6BFA25BA0BE}";
            public const string SolutionExists_string = "{F1536EF8-92EC-443C-9ED7-FDADF150DA82}";
            public const string SolutionBuilding_string = "{ADFC4E60-0397-11D1-9F4E-00A0C911004F}";
            public const string RepositoryOpen_string = "{D8CDD15A-D1F0-4AD5-B0F4-2DE654546D5B}";
            public const string VBCodeAttribute_string = "{C28E28CA-E6DC-446F-BE1A-D496BEF8340A}";
            public const string VBCodeClass_string = "{C28E28CA-E6DC-446F-BE1A-D496BEF83401}";
            public const string VBCodeDelegate_string = "{C28E28CA-E6DC-446F-BE1A-D496BEF83402}";
            public const string VBCodeEnum_string = "{C28E28CA-E6DC-446F-BE1A-D496BEF83408}";
            public const string CloudEnvironmentConnected_string = "{CE73BF3D-D614-438A-9B93-24E9E9D7453A}";
            public const string ToolWindowActive_string = "{30840431-1832-47A3-94A4-64E5EC71B0CD}";
            public const string DocumentWindowActive_string = "{8F0F3ED3-2241-4638-95CE-D8D5C5222C1D}";
            public const string FolderOpened_string = "{4646B819-1AE0-4E79-97F4-8A8176FDD664}";
            public const string ShellInitialized_string = "{E80EF1CB-6D64-4609-8FAA-FEACFD3BC89F}";
            public const string SharedMSBuildFilesManagerHierarchyLoaded_string = "{22912BB2-3FF9-4D55-B4DB-D210B6035D4C}";
            public const string SynchronousSolutionOperation_string = "{30315F71-BB05-436B-8CC1-6A62B368C842}";
            public const string SolutionHasWindowsPhone80NativeProject_string = "de9f6b31-c1e5-b965-95f3-1885af956fc9";
            public const string WizardOpen_string = "{C3DA54E0-794F-440C-8655-DA03CD0DD05E}";
            public const string ProjectRetargeting_string = "{DE039A0E-C18F-490C-944A-888B8E86DA4B}";
            public const string SolutionHasSilverlightWindowsPhoneProject_string = "781D1330-8DE9-429D-BF73-C74F19E4FCB1";
            public const string StandardPreviewerConfigurationChanging_string = "{6D3CAD8E-9129-4ec0-929E-69B6F5D4400D}";
            [Obsolete("Obsolete as of Visual Studio 2019.")]
            public const string BackgroundProjectLoad_string = "{dc769521-31a2-41a5-9bbb-210b5d63568d}";
            public const string VBCodeVariable_string = "{C28E28CA-E6DC-446F-BE1A-D496BEF83403}";
            public const string VBCodeStruct_string = "{C28E28CA-E6DC-446F-BE1A-D496BEF83407}";
            public const string VBCodeProperty_string = "{C28E28CA-E6DC-446F-BE1A-D496BEF83404}";
            public const string VBCodeParameter_string = "{C28E28CA-E6DC-446F-BE1A-D496BEF83405}";
            public const string VBCodeNamespace_string = "{C28E28CA-E6DC-446F-BE1A-D496BEF83409}";
            public const string VBCodeInterface_string = "{C28E28CA-E6DC-446F-BE1A-D496BEF83406}";
            public const string VBCodeFunction_string = "{C28E28CA-E6DC-446F-BE1A-D496BEF83400}";
            public const string IdeUserSignedIn_string = "{6FB82950-B2F8-4F94-9417-506703704DB2}";
            public const string ToolboxVisible_string = "{643905EE-DAE9-4F52-A343-6A5A7349D52C}";
            public const string SolutionOpening_string = "{D2567162-F94F-4091-8798-A096E61B8B50}";
            public const string DesignMode_string = "{ADFC4E63-0397-11D1-9F4E-00A0C911004F}";
            public const string DataSourceWindowAutoVisible_string = "{2E78870D-AC7C-4460-A4A1-3FE37D00EF81}";
            public const string DataSourceWizardSuppressed_string = "{5705AD15-40EE-4426-AD3E-BA750610D599}";
            public const string MainToolBarVisible_string = "{206F83B1-2911-4CDF-95DF-EAB51E21F938}";
            public const string FullScreenMode_string = "{ADFC4E62-0397-11D1-9F4E-00A0C911004F}";
            public const string CodeWindow_string = "{8FE2DF1D-E0DA-4EBE-9D5C-415D40E487B5}";
            public const string MainToolBarInvisible_string = "{C70BC0E0-343C-486E-963C-5E08EFA0FC8D}";
            public const string VBProjOpened_string = "{9DA22B82-6211-11d2-9561-00600818403B}";
            public const string HistoricalDebugging_string = "{D1B1E38F-1A7E-4236-AF55-6FA8F5FA76E6}";
            //
            // Summary:
            //     Indicates that the first launch after setup is in progress. Packages may use
            //     ProvideAutoLoad with this UIContext in order to perform expensive one-time per-user
            //     initialization. Note that during FirstLaunchSetup many services are unavailable
            //     and a package may NOT display any UI. FirstLaunchSetup is not guaranteed to be
            //     called; it is just an opportunity for packages to build caches.
            public const string FirstLaunchSetup_string = "{E7B2B2DB-973B-4CE9-A8D7-8498895DEA73}";
            public const string DataSourceWindowSupported_string = "{95C314C4-660B-4627-9F82-1BAF1C764BBF}";
            public const string ApplicationDesigner_string = "{D06CD5E3-D961-44DC-9D80-C89A1A8D9D56}";
            public const string CloudDebugging_string = "{C22BCF10-E1EB-42C6-95A5-E01418C08A29}";
            public const string MinimalMode_string = "{8AED84FC-BA6C-4233-8D76-5BA42B0EE91D}";
            public const string NoSolution_string = "{ADFC4E64-0397-11D1-9F4E-00A0C911004F}";
            public const string PropertyPageDesigner_string = "{86670EFA-3C28-4115-8776-A4D5BB1F27CC}";
            public const string NotBuildingAndNotDebugging_string = "{48EA4A80-F14E-4107-88FA-8D0016F30B9C}";
            public const string OsWindows8OrHigher_string = "{67CFF80C-0863-4202-A4E4-CE80FDF8506E}";
            public const string SolutionClosing_string = "{DA9F8018-6EA4-48DF-BDB6-B85ABD8FC51E}";
            public const string Dragging_string = "{B706F393-2E5B-49E7-9E2E-B1825F639B63}";
            public const string EmptySolution_string = "{ADFC4E65-0397-11D1-9F4E-00A0C911004F}";
            public const string Debugging_string = "{ADFC4E61-0397-11D1-9F4E-00A0C911004F}";
            public const string SettingsDesigner_string = "{515231AD-C9DC-4AA3-808F-E1B65E72081C}";
            public static readonly Guid Debugging_guid;
            public static readonly Guid VBCodeInterface_guid;
            public static readonly Guid DataSourceWizardSuppressed_guid;
            public static readonly Guid VBCodeProperty_guid;
            public static readonly Guid VBCodeNamespace_guid;
            public static readonly Guid VBCodeVariable_guid;
            public static readonly Guid VBCodeParameter_guid;
            public static readonly Guid VBCodeStruct_guid;
            public static readonly Guid DataSourceWindowSupported_guid;
            public static readonly Guid IdeUserSignedIn_guid;
            [Obsolete("Obsolete as of Visual Studio 2019.")]
            public static readonly Guid BackgroundProjectLoad_guid;
            public static readonly Guid CloudEnvironmentConnected_guid;
            public static readonly Guid RESXEditor_guid;
            public static readonly Guid ToolWindowActive_guid;
            public static readonly Guid DocumentWindowActive_guid;
            public static readonly Guid SettingsDesigner_guid;
            public static readonly Guid FolderOpened_guid;
            public static readonly Guid ShellInitialized_guid;
            public static readonly Guid DataSourceWindowAutoVisible_guid;
            public static readonly Guid PropertyPageDesigner_guid;
            public static readonly Guid SynchronousSolutionOperation_guid;
            public static readonly Guid ApplicationDesigner_guid;
            public static readonly Guid WizardOpen_guid;
            public static readonly Guid VBProjOpened_guid;
            public static readonly Guid VBCodeFunction_guid;
            public static readonly Guid CodeWindow_guid;
            public static readonly Guid StandardPreviewerConfigurationChanging_guid;
            public static readonly Guid SharedMSBuildFilesManagerHierarchyLoaded_guid;
            public static readonly Guid OsWindows8OrHigher_guid;
            public static readonly Guid Dragging_guid;
            public static readonly Guid DesignMode_guid;
            public static readonly Guid SolutionHasAppContainerProject_guid;
            public static readonly Guid SolutionHasSingleProject_guid;
            public static readonly Guid HistoricalDebugging_guid;
            public static readonly Guid SolutionHasMultipleProjects_guid;
            public static readonly Guid SolutionExistsAndNotBuildingAndNotDebugging_guid;
            public static readonly Guid CloudDebugging_guid;
            public static readonly Guid SolutionExistsAndFullyLoaded_guid;
            public static readonly Guid SolutionExists_guid;
            public static readonly Guid NoSolution_guid;
            public static readonly Guid SolutionBuilding_guid;
            public static readonly Guid RepositoryOpen_guid;
            public static readonly Guid SolutionClosing_guid;
            public static readonly Guid ProjectRetargeting_guid;
            public static readonly Guid ToolboxVisible_guid;
            public static readonly Guid NotBuildingAndNotDebugging_guid;
            public static readonly Guid MainToolBarInvisible_guid;
            public static readonly Guid SolutionOpening_guid;
            public static readonly Guid FullSolutionLoading_guid;
            public static readonly Guid MainToolBarVisible_guid;
            public static readonly Guid VBCodeDelegate_guid;
            public static readonly Guid VBCodeClass_guid;
            public static readonly Guid VBCodeAttribute_guid;
            public static readonly Guid FSharpProject_guid;
            public static readonly Guid EmptySolution_guid;
            public static readonly Guid VCProject_guid;
            public static readonly Guid CSharpProject_guid;
            public static readonly Guid VBCodeEnum_guid;
            public static readonly Guid FirstLaunchSetup_guid;
            public static readonly Guid ToolboxChooseItemsDataSourceInitialized_guid;
            public static readonly Guid FullScreenMode_guid;
            public static readonly Guid ToolboxInitialized_guid;
            [Obsolete("Do not use this field.  The UIContext to which it refers will never be activated.")]
            public static readonly Guid ProjectCreating_guid;
            public static readonly Guid MinimalMode_guid;
            public static readonly Guid SolutionOrProjectUpgrading_guid;
            public static readonly Guid BulkFileOperation_guid;
            public static readonly Guid VBProject_guid;
            public static readonly Guid OutputWindowCreated_guid;
        }
        public static class VsTaskListView
        {
            public static readonly Guid All;
            public static readonly Guid UserTasks;
            public static readonly Guid ShortcutTasks;
            public static readonly Guid HTMLTasks;
            public static readonly Guid CompilerTasks;
            public static readonly Guid CommentTasks;
            public static readonly Guid CurrentFileTasks;
            public static readonly Guid CheckedTasks;
            public static readonly Guid UncheckedTasks;
        }
        public static class StandardToolWindows
        {
            public const string Output_string = "{34E76E81-EE4A-11D0-AE2E-00A0C90FFFC3}";
            public static readonly Guid ApplicationVerifier;
            public static readonly Guid SQLSchemaUpdateScript;
            public static readonly Guid RunningDocuments;
            public static readonly Guid ResourceView;
            public static readonly Guid Registers;
            public static readonly Guid PropertyManager;
            public static readonly Guid Properties;
            public static readonly Guid Processes;
            public static readonly Guid PerformanceExplorer;
            public static readonly Guid PendingCheckIn;
            public static readonly Guid ParallelTasks;
            public static readonly Guid ParallelStacks;
            public static readonly Guid Output;
            public static readonly Guid ObjectTestBench;
            public static readonly Guid ObjectBrowser;
            public static readonly Guid Modules;
            public static readonly Guid ManualTestExecution;
            public static readonly Guid MacroExplorer;
            public static readonly Guid Locals;
            public static readonly Guid ServerExplorer;
            public static readonly Guid LocalChanges;
            public static readonly Guid SolutionExplorer;
            public static readonly Guid SourceHistory;
            public static readonly Guid WebBrowserPreview;
            public static readonly Guid WebBrowser;
            public static readonly Guid Watch;
            public static readonly Guid VSTOAddBookmark;
            public static readonly Guid VSMDPropertyBrowser;
            public static readonly Guid VCPPPropertyManager;
            public static readonly Guid UserNotifications;
            public static readonly Guid UAMSynchronizations;
            public static readonly Guid Toolbox;
            public static readonly Guid Threads;
            public static readonly Guid TestView;
            public static readonly Guid TestRunQueue;
            public static readonly Guid TestResults;
            public static readonly Guid TestManager;
            public static readonly Guid TestImpactView;
            public static readonly Guid TeamExplorer;
            public static readonly Guid TaskList;
            public static readonly Guid StyleOrganizer;
            public static readonly Guid StartPage;
            public static readonly Guid SourceControlExplorer;
            public static readonly Guid WebPartGallery;
            public static readonly Guid LoadTestPostRun;
            public static readonly Guid Layers;
            public static readonly Guid Command;
            public static readonly Guid ColorPalette;
            public static readonly Guid CodeMetrics;
            public static readonly Guid CodeDefinition;
            public static readonly Guid CodeCoverageResults;
            public static readonly Guid ClassView;
            public static readonly Guid ClassDetails;
            public static readonly Guid CallStack;
            public static readonly Guid CallHierarchy;
            public static readonly Guid CallBrowserSecondary;
            public static readonly Guid CallBrowser;
            public static readonly Guid CSSPropertyGrid;
            public static readonly Guid CSSProperties;
            public static readonly Guid CSSManageStyles;
            public static readonly Guid CSSApplyStyles;
            public static readonly Guid Breakpoints;
            public static readonly Guid Bookmarks;
            public static readonly Guid Behaviors;
            public static readonly Guid Autos;
            public static readonly Guid ConditionalFormatting;
            public static readonly Guid LoadTest;
            public static readonly Guid ConsoleIO;
            public static readonly Guid DataCollectionControl;
            public static readonly Guid Immediate;
            public static readonly Guid HTMLPropertyGrid;
            public static readonly Guid FindSymbolResults;
            public static readonly Guid FindSymbol;
            public static readonly Guid FindReplace;
            public static readonly Guid FindInFiles;
            public static readonly Guid Find2;
            public static readonly Guid Find1;
            public static readonly Guid ErrorList;
            public static readonly Guid EntityModelBrowser;
            public static readonly Guid EntityMappingDetails;
            public static readonly Guid DocumentOutline;
            public static readonly Guid Disassembly;
            public static readonly Guid DeviceSecurityManager;
            public static readonly Guid DebugHistory;
            public static readonly Guid DatabaseSchemaView;
            public static readonly Guid DataSource;
            public static readonly Guid DataGenerationPreview;
            public static readonly Guid DataGenerationDetails;
            public static readonly Guid DBProEventMonitor;
            public static readonly Guid XMLSchemaExplorer;
        }
        public static class ReferenceManagerHandler
        {
            public const string guidRecentMenuCmdSetString = "8206e3a8-09d6-4f97-985f-7b980b672a97";
            public const uint cmdidClearRecentReferences = 256;
            public const uint cmdidRemoveFromRecentReferences = 512;
            public static readonly Guid guidRecentMenuCmdSet;
        }
        public static class ComponentSelectorPageGuid
        {
            //
            // Summary:
            //     .Net managed assembly page (used with SVsComponentSelectorDlg -- Add Reference
            //     dialog)
            public const string ManagedAssemblyPage_string = "{9A341D95-5A64-11D3-BFF9-00C04F990235}";
            //
            // Summary:
            //     COM object page (used with SVsComponentSelectorDlg -- Add Reference dialog)
            public const string COMPage_string = "{9A341D96-5A64-11D3-BFF9-00C04F990235}";
            //
            // Summary:
            //     Projects page (used with SVsComponentSelectorDlg -- Add Reference dialog)
            public const string ProjectsPage_string = "{9A341D97-5A64-11D3-BFF9-00C04F990235}";
            public static readonly Guid ManagedAssemblyPage_guid;
            public static readonly Guid COMPage_guid;
            public static readonly Guid ProjectsPage_guid;
        }
        //
        // Summary:
        //     Standard filter names for document scopes for the Navigate To feature.
        public static class StandardNavigateToDocumentScopeFilters
        {
            public const string CurrentDocument = "Navigate To Current Document";
            public const string CurrentProject = "Navigate To Current Project";
            public const string OpenDocuments = "Navigate To Open Documents";
        }
        //
        // Summary:
        //     Standard filter shortcut names for the Navigate To feature.
        public static class StandardNavigateToFilterShortcuts
        {
            public const string Help = "Navigate To Help";
            public const string Line = "Navigate To Lines";
            public const string Files = "Navigate To Files";
            public const string CurrentProjectFiles = "Navigate To Current Project Files";
            public const string Symbols = "Navigate To Symbols";
            public const string CurrentProjectSymbols = "Navigate To Current Project Symbols";
            public const string CurrentDocumentSymbols = "Navigate To Current Document Symbols";
            public const string TypeSymbols = "Navigate To Type Symbols";
            public const string Members = "Navigate To Members";
            public const string RecentFiles = "Navigate To Recent Files";
        }
        //
        // Summary:
        //     Standard filter names for item kinds for the Navigate To feature.
        public static class StandardNavigateToKindFilters
        {
            public const string Line = "Navigate To Line";
            public const string File = "Navigate To File";
            public const string Class = "Navigate To Class";
            public const string Structure = "Navigate To Structure";
            public const string Interface = "Navigate To Interface";
            public const string Delegate = "Navigate To Delegate";
            public const string Enum = "Navigate To Enum";
            public const string Module = "Navigate To Module";
            public const string Constant = "Navigate To Constant";
            public const string EnumItem = "Navigate To EnumItem";
            public const string Field = "Navigate To Field";
            public const string Method = "Navigate To Method";
            public const string Property = "Navigate To Property";
            public const string Event = "Navigate To Event";
            public const string OtherSymbol = "Navigate To Other Symbol";
            public const string RecentFile = "Navigate To Recent File";
        }
        public static class DebugTargetHandler
        {
            public const string guidDebugTargetHandlerCmdSetString = "6E87CFAD-6C05-4adf-9CD7-3B7943875B7C";
            public const uint cmdidDebugTargetAnchorItem = 257;
            public const uint cmdidDebugTargetAnchorItemNoAttachToProcess = 258;
            public const uint cmdidGenericDebugTarget = 512;
            public const uint cmdidDebugTypeCombo = 16;
            public const uint cmdidDebugTypeItemHandler = 17;
            public static readonly Guid guidDebugTargetHandlerCmdSet;
        }
        public static class AppPackageDebugTargets
        {
            public const string guidAppPackageDebugTargetCmdSetString = "FEEA6E9D-77D8-423F-9EDE-3970CBB76125";
            public const uint cmdidAppPackage_Simulator = 256;
            public const uint cmdidAppPackage_LocalMachine = 512;
            public const uint cmdidAppPackage_TetheredDevice = 768;
            public const uint cmdidAppPackage_RemoteMachine = 1024;
            public const uint cmdidAppPackage_Emulator = 1280;
            public static readonly Guid guidAppPackageDebugTargetCmdSet;
        }
        //
        // Summary:
        //     Known project target platfoms
        public static class ProjectTargets
        {
            public static readonly Guid AppContainer_Win8;
            public static readonly Guid AppContainer_Win8_1;
            public static readonly Guid WindowsPhone_80SL;
            public static readonly Guid WindowsPhone_81SL;
            public static readonly Guid OneCore_1;
        }
        public static class LOGVIEWID
        {
            //
            // Summary:
            //     Kind of view for document or data: Any defined view.
            public const string Any_string = "{FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF}";
            //
            // Summary:
            //     Kind of view for document or data: Code editor view.
            public const string Code_string = "{7651A701-06E5-11D1-8EBD-00A0C90F26EA}";
            //
            // Summary:
            //     Kind of view for document or data: Debugger view.
            public const string Debugging_string = "{7651A700-06E5-11D1-8EBD-00A0C90F26EA}";
            //
            // Summary:
            //     Kind of view for document or data: Designer view.
            public const string Designer_string = "{7651A702-06E5-11D1-8EBD-00A0C90F26EA}";
            public const string ProjectSpecificEditor_string = "{80A3471A-6B87-433E-A75A-9D461DE0645F}";
            //
            // Summary:
            //     Kind of view for document or data: Text editor view.
            public const string TextView_string = "{7651A703-06E5-11D1-8EBD-00A0C90F26EA}";
            //
            // Summary:
            //     Kind of view for document or data: A user defined view.
            public const string UserChooseView_string = "{7651A704-06E5-11D1-8EBD-00A0C90F26EA}";
            //
            // Summary:
            //     Kind of view for document or data: Any defined view.
            public static readonly Guid Any_guid;
            //
            // Summary:
            //     Kind of view for document or data: Code editor view.
            public static readonly Guid Code_guid;
            //
            // Summary:
            //     Kind of view for document or data: Debugger view.
            public static readonly Guid Debugging_guid;
            //
            // Summary:
            //     Kind of view for document or data: Designer view.
            public static readonly Guid Designer_guid;
            public static readonly Guid ProjectSpecificEditor_guid;
            //
            // Summary:
            //     Kind of view for document or data: Primary (default) view.
            public static readonly Guid Primary_guid;
            //
            // Summary:
            //     Kind of view for document or data: Text editor view.
            public static readonly Guid TextView_guid;
            //
            // Summary:
            //     Kind of view for document or data: A user defined view.
            public static readonly Guid UserChooseView_guid;
        }
        //
        // Summary:
        //     Debugger Port Supplier Guids for the VsDebugTargetInfo* structures and IVsDebugRemoteDiscoveryUI
        //     interface.
        public static class DebugPortSupplierGuids
        {
            public const string NoAuth_string = "{3b476d38-a401-11d2-aad4-00c04f990171}";
            public static readonly Guid NoAuth_guid;
        }
        public static class ToolboxMultitargetingFields
        {
            //
            // Summary:
            //     The full type name, e.g. System.Windows.Forms.Button
            public const string TypeName = "TypeName";
            //
            // Summary:
            //     The full assembly name (strong name), including version
            public const string AssemblyName = "AssemblyName";
            //
            // Summary:
            //     A semicolon-delimited list of TFMs this item supports (without profiles)
            public const string Frameworks = "Frameworks";
            //
            // Summary:
            //     The GUID of the package that implements IVsProvideTargetedToolboxItems and knows
            //     about this item type
            public const string ItemProvider = "ItemProvider";
            //
            // Summary:
            //     A boolean value indicating whether to use the project target framework's version
            //     in toolbox item tooltips
            public const string UseProjectTargetFrameworkVersionInTooltip = "UseProjectTargetFrameworkVersionInTooltip";
        }
        public static class CLSID
        {
            public const string MiscellaneousFilesProject_string = "{A2FE74E1-B743-11D0-AE1A-00A0C90FFFC3}";
            //
            // Summary:
            //     CLSID of the UIHierarchy window tree control object
            public const string VsUIHierarchyWindow_string = "{7D960B07-7AF8-11D0-8E5E-00A0C911005A}";
            //
            // Summary:
            //     DocData object of the HTML Editor
            public const string HtmDocData_string = "{62C81794-A9EC-11D0-8198-00A0C91BBEE3}";
            //
            // Summary:
            //     The guid of the CLSID_VsSearchQueryParser object implementing IVsSearchQueryParser
            //     interface
            public const string VsSearchQueryParser_string = "{B71B3DF9-7A4A-4D70-8293-3874DB098FDD}";
            public const string VsUIWpfLoader_string = "{0B127700-143C-4AB5-9D39-BFF47151B563}";
            public const string VsTaskListPackage_string = "{4A9B7E50-AA16-11D0-A8C5-00A0C921A4D2}";
            public const string VsTaskList_string = "{BC5955D5-AA0D-11D0-A8C5-00A0C921A4D2}";
            public const string VsCfgProviderEventsHelper_string = "{99913F1F-1EE3-11D1-8A6E-00C04F682E21}";
            public const string VsEnvironmentPackage_string = "{DA9FB551-C724-11D0-AE1F-00A0C90FFFC3}";
            public const string VsTextBuffer_string = "{8E7B96A8-E33D-11d0-A6D5-00C04FB67F6A}";
            public const string SolutionFolderProject_string = "{2150E333-8FDC-42A3-9474-1A3956D46DE8}";
            public const string SolutionItemsProject_string = "{D1DCDB85-C5E8-11D2-BFCA-00C04F990235}";
            public const string UnloadedProject_string = "{76E22BD3-C2EC-47F1-802B-53197756DAE8}";
            public static readonly Guid UnloadedProject_guid;
            public static readonly Guid VsUIHierarchyWindow_guid;
            public static readonly Guid MiscellaneousFilesProject_guid;
            public static readonly Guid HtmDocData_guid;
            //
            // Summary:
            //     The guid of the CLSID_VsSearchQueryParser object implementing IVsSearchQueryParser
            //     interface
            public static readonly Guid VsSearchQueryParser_guid;
            public static readonly Guid VsUIWpfLoader_guid;
            public static readonly Guid VsTaskListPackage_guid;
            public static readonly Guid SolutionItemsProject_guid;
            public static readonly Guid VsEnvironmentPackage_guid;
            public static readonly Guid VsCfgProviderEventsHelper_guid;
            public static readonly Guid VsTextBuffer_guid;
            public static readonly Guid SolutionFolderProject_guid;
            public static readonly Guid VsTaskList_guid;
        }
        //
        // Summary:
        //     Known values that can be used to specify OldVersion (LowerBound/UpperBound) values
        //     for ProvideBindingRedirectionAttribute.
        public static class WellKnownOldVersionValues
        {
            //
            // Summary:
            //     Value is n.0.0.0, where n is the major version number of the target assembly.
            public const string LowestMajor = "LowestMajor";
            //
            // Summary:
            //     Value is n.n.0.0, where n.n is the major and minor version numbers of the target
            //     assembly.
            public const string LowestMajorMinor = "LowestMajorMinor";
            //
            // Summary:
            //     Value is the current version number of the target assembly.
            public const string Current = "Current";
        }
        public static class WellKnownToolboxStringMaps
        {
            public const string MultiTargeting = "MultiTargeting:{FBB22D27-7B21-42ac-88C8-595F94BDBCA5}";
        }
        public static class CMDSETID
        {
            public const string StandardCommandSet97_string = "{5EFC7975-14BC-11CF-9B2B-00AA00573819}";
            //
            // Summary:
            //     Command Group GUID for commands implemented by the Solution Explorer package
            public const string SolutionExplorerPivotList_string = "{afe48dbb-c199-46ce-ba09-adbd5e933ea3}";
            public const string VsDocOutlinePackageCommandSet_string = "{21AF45B0-FFA5-11D0-B63F-00A0C922E851}";
            //
            // Summary:
            //     Command Group GUID for commands that only apply to the UIHierarchyWindow.
            public const string UIHierarchyWindowCommandSet_string = "{60481700-078B-11D1-AAF8-00A0C9055A90}";
            //
            // Summary:
            //     GUID for the command group representing the shell's main menu.
            public const string ShellMainMenu_string = "{D309F791-903F-11D0-9EFC-00A0C911004F}";
            //
            // Summary:
            //     GUID for the Visual Studio 16 command set. This is a set of new commands added
            //     to Visual Studio 16.
            public const string StandardCommandSet16_string = "{8F380902-6040-4097-9837-D3F40E66F908}";
            //
            // Summary:
            //     GUID for various C# command groups, menus and commands as well as some shared
            //     commands (like the refactoring commands) that originally were C# only commands.
            public const string CSharpGroup_string = "{5D7E7F65-A63F-46ee-84F1-990B2CAB23F9}";
            //
            // Summary:
            //     GUID for the Visual Studio 14 command set. This is a set of new commands added
            //     to Visual Studio 14.
            public const string StandardCommandSet14_string = "{4C7763BF-5FAF-4264-A366-B7E1F27BA958}";
            //
            // Summary:
            //     GUID for the Visual Studio 15 command set. This is a set of new commands added
            //     to Visual Studio 15.
            public const string StandardCommandSet15_string = "{712C6C80-883B-4AAD-B430-BBCA5256FA9D}";
            //
            // Summary:
            //     GUID for the Visual Studio 11 command set. This is a set of new commands added
            //     to Visual Studio 11.
            public const string StandardCommandSet11_string = "{D63DB1F0-404E-4B21-9648-CA8D99245EC3}";
            public const string StandardCommandSet2K_string = "{1496A755-94DE-11D0-8C3F-00C04FC2AAE2}";
            //
            // Summary:
            //     GUID for the Visual Studio 2010 command set. This is a set of new commands added
            //     to Visual Studio 2010.
            public const string StandardCommandSet2010_string = "{5DD0BB59-7076-4C59-88D3-DE36931F63F0}";
            //
            // Summary:
            //     GUID for the Visual Studio 12 command set. This is a set of new commands added
            //     to Visual Studio 12.
            public const string StandardCommandSet12_string = "{2A8866DC-7BDE-4dc8-A360-A60679534384}";
            //
            // Summary:
            //     GUID for the Visual Studio 12 command set. This is a set of new commands added
            //     to Visual Studio 12.
            public static readonly Guid StandardCommandSet12_guid;
            //
            // Summary:
            //     Command Group GUID for commands implemented by the Solution Explorer package
            public static readonly Guid SolutionExplorerPivotList_guid;
            public static readonly Guid StandardCommandSet97_guid;
            public static readonly Guid VsDocOutlinePackageCommandSet_guid;
            //
            // Summary:
            //     Command Group GUID for commands that only apply to the UIHierarchyWindow.
            public static readonly Guid UIHierarchyWindowCommandSet_guid;
            //
            // Summary:
            //     GUID for the command group representing the shell's main menu.
            public static readonly Guid ShellMainMenu_guid;
            //
            // Summary:
            //     GUID for the Visual Studio 16 command set. This is a set of new commands added
            //     to Visual Studio 16.
            public static readonly Guid StandardCommandSet16_guid;
            //
            // Summary:
            //     GUID for the Visual Studio 2010 command set. This is a set of new commands added
            //     to Visual Studio 2010.
            public static readonly Guid StandardCommandSet2010_guid;
            //
            // Summary:
            //     GUID for the Visual Studio 15 command set. This is a set of new commands added
            //     to Visual Studio 15.
            public static readonly Guid StandardCommandSet15_guid;
            //
            // Summary:
            //     GUID for the Visual Studio 14 command set. This is a set of new commands added
            //     to Visual Studio 14.
            public static readonly Guid StandardCommandSet14_guid;
            //
            // Summary:
            //     GUID for the Visual Studio 11 command set. This is a set of new commands added
            //     to Visual Studio 11.
            public static readonly Guid StandardCommandSet11_guid;
            public static readonly Guid StandardCommandSet2K_guid;
            //
            // Summary:
            //     GUID for various C# command groups, menus and commands as well as some shared
            //     commands (like the refactoring commands) that originally were C# only commands.
            public static readonly Guid CSharpGroup_guid;
        }
        //
        // Summary:
        //     Well-known reasons used for IVsUIShellOpenDocument3.SetNewDocumentState
        public static class NewDocumentStateReason
        {
            public static readonly Guid FindSymbolResults;
            public static readonly Guid FindResults;
            public static readonly Guid Navigation;
            public static readonly Guid SolutionExplorer;
            public static readonly Guid TeamExplorer;
        }
        public static class VsPackageGuid
        {
            //
            // Summary:
            //     GUID of the HTML Editor package.
            public const string VsEnvironmentPackage_string = "{DA9FB551-C724-11D0-AE1F-00A0C90FFFC3}";
            //
            // Summary:
            //     GUID of the HTML Editor package.
            public const string HtmlEditorPackage_string = "{1B437D20-F8FE-11D2-A6AE-00104BCC7269}";
            public const string VsTaskListPackage_string = "{4A9B7E50-AA16-11D0-A8C5-00A0C921A4D2}";
            //
            // Summary:
            //     GUID of the Document Outline tool window package.
            public const string VsDocOutlinePackage_string = "{21AF45B0-FFA5-11D0-B63F-00A0C922E851}";
            //
            // Summary:
            //     GUID of the HTML Editor package.
            public static readonly Guid VsEnvironmentPackage_guid;
            //
            // Summary:
            //     GUID of the HTML Editor package.
            public static readonly Guid HtmlEditorPackage_guid;
            public static readonly Guid VsTaskListPackage_guid;
            //
            // Summary:
            //     GUID of the Document Outline tool window package.
            public static readonly Guid VsDocOutlinePackage_guid;
        }
        public static class VsEditorFactoryGuid
        {
            //
            // Summary:
            //     GUID of HTML Editor editor factory
            public const string HtmlEditor_string = "{C76D83F8-A489-11D0-8195-00A0C91BBEE3}";
            //
            // Summary:
            //     GUID of the Source Code (Text) Editor editor factory
            public const string TextEditor_string = "{8B382828-6202-11d1-8870-0000F87579D2}";
            //
            // Summary:
            //     Guid for editor factory to launch external (EXE based) editors
            public const string ExternalEditor_string = "{8B382828-6202-11D1-8870-0000F87579D2}";
            //
            // Summary:
            //     Guid for Project Properties Designer editor factory
            public const string ProjectDesignerEditor_string = "{04B8AB82-A572-4FEF-95CE-5222444B6B64}";
            //
            // Summary:
            //     GUID of HTML Editor editor factory
            public static readonly Guid HtmlEditor_guid;
            //
            // Summary:
            //     GUID of the Source Code (Text) Editor editor factory
            public static readonly Guid TextEditor_guid;
            //
            // Summary:
            //     Guid for EditorFactory to launch external (EXE based) editors
            public static readonly Guid ExternalEditor_guid;
            //
            // Summary:
            //     Guid for Project Properties Designer editor factory
            public static readonly Guid ProjectDesignerEditor_guid;
        }
        public static class VsLanguageServiceGuid
        {
            public const string HtmlLanguageService_string = "{58E975A0-F8FE-11D2-A6AE-00104BCC7269}";
            public static readonly Guid HtmlLanguageService_guid;
        }
        public static class OutputWindowPaneGuid
        {
            //
            // Summary:
            //     GUID of the build output pane inside the output window.
            public const string BuildOutputPane_string = "{1BD8A850-02D1-11D1-BEE7-00A0C913D1F8}";
            //
            // Summary:
            //     GUID of the sorted build output pane inside the output window.
            public const string SortedBuildOutputPane_string = "{2032B126-7C8D-48AD-8026-0E0348004FC0}";
            //
            // Summary:
            //     GUID of the debug pane inside the output window.
            public const string DebugPane_string = "{FC076020-078A-11D1-A7DF-00A0C9110051}";
            //
            // Summary:
            //     GUID of the general output pane inside the output window.
            public const string GeneralPane_string = "{3C24D581-5591-4884-A571-9FE89915CD64}";
            //
            // Summary:
            //     GUID of the Store Validation output pane inside the output window.
            public const string StoreValidationPane_string = "{54065C74-1B11-4249-9EA7-5540D1A6D528}";
            //
            // Summary:
            //     GUID of the build output pane inside the output window.
            public static readonly Guid BuildOutputPane_guid;
            //
            // Summary:
            //     GUID of the sorted build output pane inside the output window.
            public static readonly Guid SortedBuildOutputPane_guid;
            //
            // Summary:
            //     GUID of the debug pane inside the output window.
            public static readonly Guid DebugPane_guid;
            //
            // Summary:
            //     GUID of the general output pane inside the output window.
            public static readonly Guid GeneralPane_guid;
            //
            // Summary:
            //     GUID of the Store Validation output pane inside the output window.
            public static readonly Guid StoreValidationPane_guid;
        }
        //
        // Summary:
        //     These values are used with the VSHPROPID_ItemType property.
        public static class ItemTypeGuid
        {
            //
            // Summary:
            //     Physical file on disk or web (IVsProject::GetMkDocument returns a file path).
            public const string PhysicalFile_string = "{6BB5F8EE-4483-11D3-8BCF-00C04F8EC28C}";
            //
            // Summary:
            //     Physical folder on disk or web (IVsProject::GetMkDocument returns a directory
            //     path).
            public const string PhysicalFolder_string = "{6BB5F8EF-4483-11D3-8BCF-00C04F8EC28C}";
            //
            // Summary:
            //     Non-physical folder (folder is logical and not a physical file system directory).
            public const string VirtualFolder_string = "{6BB5F8F0-4483-11D3-8BCF-00C04F8EC28C}";
            //
            // Summary:
            //     Nested or Sub Project.
            public const string SubProject_string = "{EA6618E8-6E24-4528-94BE-6889FE16485C}";
            //
            // Summary:
            //     SharedProjectReference item (normally child of "References" folder).
            //
            // Remarks:
            //     SharedProjectReference items represent imported shared MSBuild project files
            //     (e.g. *.projitems file). Normally these shared MSBuild project files are "owned"
            //     by a particular Shared Project (aka Shared Assets Project) loaded in the Solution.
            public const string SharedProjectReference_string = "{FBA6BD9A-47F3-4C04-BDC0-7F76A9E2E582}";
            //
            // Summary:
            //     Physical file on disk or web (IVsProject::GetMkDocument returns a file path).
            public static readonly Guid PhysicalFile_guid;
            //
            // Summary:
            //     Physical folder on disk or web (IVsProject::GetMkDocument returns a directory
            //     path).
            public static readonly Guid PhysicalFolder_guid;
            //
            // Summary:
            //     Non-physical folder (folder is logical and not a physical file system directory).
            public static readonly Guid VirtualFolder_guid;
            //
            // Summary:
            //     Nested or Sub Project.
            public static readonly Guid SubProject_guid;
            //
            // Summary:
            //     SharedProjectReference item (normally child of "References" folder).
            //
            // Remarks:
            //     SharedProjectReference items represent imported shared MSBuild project files
            //     (e.g. *.projitems file). Normally these shared MSBuild project files are "owned"
            //     by a particular Shared Project (aka Shared Assets Project) loaded in the Solution.
            public static readonly Guid SharedProjectReference_guid;
        }
        public static class CodeModelLanguage
        {
            public const string VC = "{B5E9BD32-6D3E-4B5D-925E-8A43B79820B4}";
            public const string VB = "{B5E9BD33-6D3E-4B5D-925E-8A43B79820B4}";
            public const string CSharp = "{B5E9BD34-6D3E-4B5D-925E-8A43B79820B4}";
            public const string IDL = "{B5E9BD35-6D3E-4B5D-925E-8A43B79820B4}";
            public const string MC = "{B5E9BD36-6D3E-4B5D-925E-8A43B79820B4}";
        }
        public static class WizardType
        {
            public const string AddSubProject = "{0F90E1D2-4999-11D1-B6D1-00A0C90F2744}";
            public const string AddItem = "{0F90E1D1-4999-11D1-B6D1-00A0C90F2744}";
            public const string NewProject = "{0F90E1D0-4999-11D1-B6D1-00A0C90F2744}";
        }
        public static class VsDependencyTypeGuid
        {
            //
            // Summary:
            //     Build project dependency (used with IVsDependency::get_Type)
            public const string BuildProject_string = "{707D11B6-91CA-11D0-8A3E-00A0C91E2ACD}";
            //
            // Summary:
            //     Build project dependency (used with IVsDependency::get_Type)
            public static readonly Guid BuildProject_guid;
        }
        //
        // Summary:
        //     These are optional general editor settings that can be used to customize editor
        //     behaviors
        public static class VsEditorUserDataGuid
        {
            //
            // Summary:
            //     uint: Sets the DPI context in which the editor instance should be created. Valid
            //     values for this setting come from Microsoft.VisualStudio.Shell.Interop.__VSDPIMODE.
            public const string EditorDpiContext_string = "{E80CBA74-D298-4DB5-A28D-6511675031F4}";
            //
            // Summary:
            //     uint: Sets the DPI context in which the editor instance should be created. Valid
            //     values for this setting come from Microsoft.VisualStudio.Shell.Interop.__VSDPIMODE.
            public static readonly Guid EditorDpiContext_guid;
        }
        //
        // Summary:
        //     These are optional IVsUserData properties that a Language Service may provide
        //     in order to influence the behavior of the Source Code (Text) Editor. The IVsUserData
        //     interface is retrieved by QueryInterface on the IVsLanguageInfo object of the
        //     Language Service implementation.
        public static class VsLanguageUserDataGuid
        {
            public const string SupportCF_HTML_string = "{27E97702-589E-11D2-8233-0080C747D9A0}";
            public static readonly Guid SupportCF_HTML_guid;
        }
        //
        // Summary:
        //     These are IVsUserData properties that are supported by the TextBuffer (DocData)
        //     object of the Source Code (Text) Editor. The IVsUserData interface is retrieved
        //     by QueryInterface on the IVsTextLines object of the Text Editor.
        public static class VsTextBufferUserDataGuid
        {
            //
            // Summary:
            //     string: Moniker of document loaded in the buffer. It will be the full path of
            //     file if the document is a file.
            public const string VsBufferMoniker_string = "{978A8E17-4DF8-432A-9623-D530A26452BC}";
            //
            // Summary:
            //     string: This property provides a specific error message for when the buffer originates
            //     the BUFFER_E_READONLY error. Set this string to be the (localized) text you want
            //     displayed to the user. Note that the buffer itself does not put up UI, but only
            //     calls IVsUIShell::SetErrorInfo. The caller can decide whether to show the message
            //     to the user.
            public const string UserReadOnlyErrorString_string = "{A3BCFE56-CF1B-11D1-88B1-0000F87579D2}";
            //
            // Summary:
            //     string: The comma-separated list of text view roles for the text view.
            public const string VsTextViewRoles_string = "{297078FF-81A2-43D8-9CA3-4489C53C99BA}";
            //
            // Summary:
            //     string: This property will be used to set the SEID_PropertyBrowserSID element
            //     of the selection for text views. This is only used if you have a custom property
            //     browser. If this property is not set, the standard property browser will be associated
            //     with the view.
            public const string PropertyBrowserSID_string = "{CE6DDBBA-8D13-11D1-8889-0000F87579D2}";
            //
            // Summary:
            //     bool:
            public const string VsBufferFileReload_string = "{80D2B881-81A3-4F0B-BCF0-70A0054E672F}";
            //
            // Summary:
            //     bool: (default = true) If true then a change to the buffer's moniker will cause
            //     the buffer to change the language service based on the file extension of the
            //     moniker.
            public const string VsBufferDetectLangSID_string = "{17F375AC-C814-11D1-88AD-0000F87579D2}";
            //
            // Summary:
            //     object: This property is used to get access to the buffer's storage object. The
            //     returned pointer can be QI'd for IVsTextStorage and IVsPersistentTextImage. This
            //     is a get-only property. To set the storage, use the buffer's InitializeContentEx
            //     method.
            public const string BufferStorage_string = "{D97F167A-638E-11D2-88F6-0000F87579D2}";
            //
            // Summary:
            //     bool: If true and the current BufferEncoding is CHARFMT_MBCS, the buffer will
            //     runs it's HTML charset tag detection code to determine a codepage to load and
            //     save the file. The detected codepage overrides any codepage set in CHARFMT_MBCS.
            //     This is forced on in the buffer's IPersistFileFormat::LoadDocData when it sees
            //     an HTML type of file, according to the extension mapping in "$RootKey$\Languages\File
            //     Extensions".
            public const string VsBufferDetectCharSet_string = "{36358D1F-BF7E-11D1-B03A-00C04FB68006}";
            //
            // Summary:
            //     uint: This should only be used by editor factories that want to specify a codepage
            //     on loading from the openwith dialog. This data is only for a set purpose. You
            //     cannot get the value of this back.
            public const string VsBufferEncodingPromptOnLoad_string = "{99EC03F0-C843-4C09-BE74-CDCA5158D36C}";
            public const string VsBufferEncodingVSTFF_string = "{16417F39-A6B7-4C90-89FA-770D2C60440B}";
            //
            // Summary:
            //     string: The ContentType for the text buffer.
            public const string VsBufferContentType_string = "{1BEB4195-98F4-4589-80E0-480CE32FF059}";
            //
            // Summary:
            //     bool: true if buffer is a file on disk
            public const string VsBufferIsDiskFile_string = "{D9126592-1473-11D3-BEC6-0080C747D9A0}";
            //
            // Summary:
            //     bool:
            public const string VsInitEncodingDialogFromUserData_string = "{C2382D84-6650-4386-860F-248ECB222FC1}";
            //
            // Summary:
            //     object: Use this property if the file opened in the buffer is associated with
            //     list of extra files under source code control (SCC). Set this property with an
            //     implementation of IVsBufferExtraFiles in order to control how the buffer handles
            //     SCC operations. The IVsBufferExtraFiles object set will determine what files
            //     are checked out from Source Code Control (SCC) when edits are made to the buffer.
            //     This property controls the behavior of IVsTextManager2::AttemptToCheckOutBufferFromScc3
            //     and GetBufferSccStatus3 as well as which files are passed by the buffer when
            //     it calls IVsQueryEditQuerySave2 methods.
            public const string VsBufferExtraFiles_string = "{FD494BF6-1167-4635-A20C-5C24B2D7B33D}";
            //
            // Summary:
            //     bool:
            public static readonly Guid VsBufferFileReload_guid;
            //
            // Summary:
            //     bool:
            public static readonly Guid VsInitEncodingDialogFromUserData_guid;
            //
            // Summary:
            //     string: The ContentType for the text buffer.
            public static readonly Guid VsBufferContentType_guid;
            //
            // Summary:
            //     object: Use this property if the file opened in the buffer is associated with
            //     list of extra files under source code control (SCC). Set this property with an
            //     implementation of IVsBufferExtraFiles in order to control how the buffer handles
            //     SCC operations. The IVsBufferExtraFiles object set will determine what files
            //     are checked out from Source Code Control (SCC) when edits are made to the buffer.
            //     This property controls the behavior of IVsTextManager2::AttemptToCheckOutBufferFromScc3
            //     and GetBufferSccStatus3 as well as which files are passed by the buffer when
            //     it calls IVsQueryEditQuerySave2 methods.
            public static readonly Guid VsBufferExtraFiles_guid;
            //
            // Summary:
            //     string: This property will be used to set the SEID_PropertyBrowserSID element
            //     of the selection for text views. This is only used if you have a custom property
            //     browser. If this property is not set, the standard property browser will be associated
            //     with the view.
            public static readonly Guid PropertyBrowserSID_guid;
            //
            // Summary:
            //     string: This property provides a specific error message for when the buffer originates
            //     the BUFFER_E_READONLY error. Set this string to be the (localized) text you want
            //     displayed to the user. Note that the buffer itself does not put up UI, but only
            //     calls IVsUIShell::SetErrorInfo. The caller can decide whether to show the message
            //     to the user.
            public static readonly Guid UserReadOnlyErrorString_guid;
            //
            // Summary:
            //     bool: (default = true) If true then a change to the buffer's moniker will cause
            //     the buffer to change the language service based on the file extension of the
            //     moniker.
            public static readonly Guid VsBufferDetectLangSID_guid;
            //
            // Summary:
            //     bool: If true and the current BufferEncoding is CHARFMT_MBCS, the buffer will
            //     runs it's HTML charset tag detection code to determine a codepage to load and
            //     save the file. The detected codepage overrides any codepage set in CHARFMT_MBCS.
            //     This is forced on in the buffer's IPersistFileFormat::LoadDocData when it sees
            //     an HTML type of file, according to the extension mapping in "$RootKey$\Languages\File
            //     Extensions".
            public static readonly Guid VsBufferDetectCharSet_guid;
            //
            // Summary:
            //     uint: This should only be used by editor factories that want to specify a codepage
            //     on loading from the openwith dialog. This data is only for a set purpose. You
            //     cannot get the value of this back.
            public static readonly Guid VsBufferEncodingPromptOnLoad_guid;
            public static readonly Guid VsBufferEncodingVSTFF_guid;
            //
            // Summary:
            //     bool: true if buffer is a file on disk
            public static readonly Guid VsBufferIsDiskFile_guid;
            //
            // Summary:
            //     string: Moniker of document loaded in the TextBuffer. It will be the full path
            //     of file if the document is a file.
            public static readonly Guid VsBufferMoniker_guid;
            //
            // Summary:
            //     object: This property is used to get access to the buffer's storage object. The
            //     returned pointer can be QI'd for IVsTextStorage and IVsPersistentTextImage. This
            //     is a get-only property. To set the storage, use the buffer's InitializeContentEx
            //     method.
            public static readonly Guid BufferStorage_guid;
            //
            // Summary:
            //     string: The comma-separated list of text view roles for the text view.
            public static readonly Guid VsTextViewRoles_guid;
        }
        //
        // Summary:
        //     Known editor property categories use with IVsTextEditorPropertyCategoryContainer
        //     interface.
        public static class EditPropyCategoryGuid
        {
            //
            // Summary:
            //     GUID for text manager global properties
            public const string TextManagerGlobal_string = "{6BFB60A2-48D8-424E-81A2-040ACA0B1F68}";
            //
            // Summary:
            //     GUID for view properties that override everything -- Tools.Options *and* user
            //     commands
            public const string ViewMasterSettings_string = "{D1756E7C-B7FD-49A8-B48E-87B14A55655A}";
            //
            // Summary:
            //     GUID for text manager global properties
            public static readonly Guid TextManagerGlobal_guid;
            //
            // Summary:
            //     GUID for view properties that override everything -- Tools.Options *and* user
            //     commands
            public static readonly Guid ViewMasterSettings_guid;
        }
        //
        // Summary:
        //     These CATID Guids are used to extend objects passed to the property browser and
        //     automation objects that support Automation Extenders.
        public static class CATID
        {
            public const string CSharpFileProperties_string = "{8D58E6AF-ED4E-48B0-8C7B-C74EF0735451}";
            public const string VCActiveXReferenceNode_string = "{9E8182D3-C60A-44F4-A74B-14C90EF9CACE}";
            public const string VCProjectReferenceNode_string = "{593DCFCE-20A7-48E4-ACA1-49ADE9049887}";
            public const string VCAssemblyReferenceNode_string = "{FE8299C9-19B6-4F20-ABEA-E1FD9A33B683}";
            public const string VCFileNode_string = "{EE8299C9-19B6-4F20-ABEA-E1FD9A33B683}";
            public const string VCFileGroup_string = "{EE8299CA-19B6-4F20-ABEA-E1FD9A33B683}";
            public const string VCProjectNode_string = "{EE8299CB-19B6-4F20-ABEA-E1FD9A33B683}";
            public const string VBReferenceProperties_string = "{2289B812-8191-4E81-B7B3-174045AB0CB5}";
            public const string VCReferences_string = "{FE8299CA-19B6-4F20-ABEA-E1FD9A33B683}";
            public const string VBFolderProperties_string = "{932DC619-2EAA-4192-B7E6-3D15AD31DF49}";
            public const string VBFileProperties_string = "{EA5BD05D-3C72-40A5-95A0-28A2773311CA}";
            public const string VBProjectProperties_string = "{E0FDC879-C32A-4751-A3D3-0B3824BD575F}";
            public const string VBAFolderProperties_string = "{79231B36-6213-481D-AA7D-0F931E8F2CF9}";
            public const string VBAFileProperties_string = "{AC2912B2-50ED-4E62-8DFF-429B4B88FC9E}";
            public const string CSharpFolderProperties_string = "{914FE278-054A-45DB-BF9E-5F22484CC84C}";
            //
            // Summary:
            //     This CATID is used to extend EnvDTE.ProjectItem automation objects for project
            //     types that support it (including VB and C# projects).
            public const string ProjectItemAutomationObject_string = "{610D4615-D0D5-11D2-8599-006097C68E81}";
            //
            // Summary:
            //     This CATID is used to extend EnvDTE.Project automation objects for project types
            //     that support it (including VB and C# projects).
            public const string ProjectAutomationObject_string = "{610D4614-D0D5-11D2-8599-006097C68E81}";
            public static readonly Guid VCActiveXReferenceNode_guid;
            public static readonly Guid CSharpFileProperties_guid;
            public static readonly Guid VCProjectReferenceNode_guid;
            public static readonly Guid VCAssemblyReferenceNode_guid;
            public static readonly Guid CSharpFolderProperties_guid;
            public static readonly Guid VCFileNode_guid;
            //
            // Summary:
            //     This CATID is used to extend EnvDTE.Project automation objects for project types
            //     that support it (including VB and C# projects).
            public static readonly Guid ProjectAutomationObject_guid;
            public static readonly Guid VBAFolderProperties_guid;
            public static readonly Guid VCProjectNode_guid;
            public static readonly Guid VBReferenceProperties_guid;
            //
            // Summary:
            //     This CATID is used to extend EnvDTE.ProjectItem automation objects for project
            //     types that support it (including VB and C# projects).
            public static readonly Guid ProjectItemAutomationObject_guid;
            public static readonly Guid VBProjectProperties_guid;
            public static readonly Guid VBFolderProperties_guid;
            public static readonly Guid VBAFileProperties_guid;
            public static readonly Guid VBFileProperties_guid;
            public static readonly Guid VCFileGroup_guid;
            public static readonly Guid VCReferences_guid;
        }
        public static class DebugEnginesGuids
        {
            public const string COMPlusLegacyEngine_string = "{351668CC-8477-4fbf-BFE3-5F1006E4DB1F}";
            public const string SqlDebugEngine3_string = "{3B476D3A-A401-11D2-AAD4-00C04F990171}";
            public const string CoreSystemClr_string = "{2E36F1D4-B23C-435D-AB41-18E608940038}";
            public const string Script_string = "{F200A7E7-DEA5-11D0-B854-00A0244A1DE2}";
            public const string ManagedOnlyEngineV4_string = "{FB0D4648-F776-4980-95F8-BB7F36EBC1EE}";
            public const string NativeOnly_string = "{3B476D35-A401-11D2-AAD4-00C04F990171}";
            public const string ManagedAndNative_string = "{92EF0900-2251-11D2-B72E-0000F87572EF}";
            public const string ManagedOnlyEngineV2_string = "{5FFF7536-0C87-462D-8FD2-7971D948E6DC}";
            public const string SQLLocalEngine_string = "{E04BDE58-45EC-48DB-9807-513F78865212}";
            public const string SqlDebugEngine2_string = "{3B476D30-A401-11D2-AAD4-00C04F990171}";
            public const string ManagedOnly_string = "{449EC4CC-30D2-4032-9256-EE18EB41B62B}";
            public const string COMPlusNewArchEngine_string = "{97552AEF-4F41-447a-BCC3-802EAA377343}";
            public static readonly Guid ManagedAndNative_guid;
            public static readonly Guid Script_guid;
            public static readonly Guid SQLLocalEngine_guid;
            public static readonly Guid NativeOnly_guid;
            public static readonly Guid SqlDebugEngine2_guid;
            [EditorBrowsable(EditorBrowsableState.Never)]
            public static readonly Guid NativeOnly;
            //
            // Summary:
            //     The guid of the Legacy Debugger Engine (CPDE)
            public static readonly Guid COMPlusLegacyEngine_guid;
            //
            // Summary:
            //     The guid of the Debugger engine for managed (Core)
            public static readonly Guid CoreSystemClr_guid;
            //
            // Summary:
            //     The guid of the Debugger Engine for managed (v4.5, v4.0)
            public static readonly Guid ManagedOnlyEngineV4_guid;
            //
            // Summary:
            //     The guid of the Debugger Engine for managed (v3.5, v3.0, v2.0)
            public static readonly Guid ManagedOnlyEngineV2_guid;
            //
            // Summary:
            //     The guid of the Debugger Engine for Managed code
            public static readonly Guid ManagedOnly_guid;
            [EditorBrowsable(EditorBrowsableState.Never)]
            public static readonly Guid SqlDebugEngine3;
            [EditorBrowsable(EditorBrowsableState.Never)]
            public static readonly Guid SqlDebugEngine2;
            [EditorBrowsable(EditorBrowsableState.Never)]
            public static readonly Guid SQLLocalEngine;
            [EditorBrowsable(EditorBrowsableState.Never)]
            public static readonly Guid ManagedAndNative;
            [EditorBrowsable(EditorBrowsableState.Never)]
            public static readonly Guid Script;
            //
            // Summary:
            //     The guid of the New Debugger Engine (Concord)
            public static readonly Guid COMPlusNewArchEngine_guid;
            public static readonly Guid SqlDebugEngine3_guid;
        }
        public static class MruList
        {
            public const string Projects_string = "A9C4A31F-F9CB-47A9-ABC0-49CE82D0B3AC";
            public const string Files_string = "01235AAD-8F1B-429F-9D02-61A0101EA275";
            public const string SolutionFiles_string = "335041A8-B61A-4E9F-B0FE-D42DFA193855";
            //
            // Summary:
            //     Guid used for accessing the project MRU list through the IVsMRUItemsStore interface.
            public static readonly Guid Projects;
            //
            // Summary:
            //     Guid used for accessing the file MRU list through the IVsMRUItemsStore interface.
            public static readonly Guid Files;
            //
            // Summary:
            //     Guid used for accessing the solution file MRU list through the IVsMRUItemsStore
            //     interface.
            public static readonly Guid SolutionFiles;
        }
    }
