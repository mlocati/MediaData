﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Il codice è stato generato da uno strumento.
//     Versione runtime:4.0.30319.42000
//
//     Le modifiche apportate a questo file possono provocare un comportamento non corretto e andranno perse se
//     il codice viene rigenerato.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MLocati.MediaData.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "14.0.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Never")]
        public global::MLocati.MediaData.VideoProcessor.NormalizeCase VideoNormalization {
            get {
                return ((global::MLocati.MediaData.VideoProcessor.NormalizeCase)(this["VideoNormalization"]));
            }
            set {
                this["VideoNormalization"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string LastMediaDirectory {
            get {
                return ((string)(this["LastMediaDirectory"]));
            }
            set {
                this["LastMediaDirectory"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool ToolWordWrap {
            get {
                return ((bool)(this["ToolWordWrap"]));
            }
            set {
                this["ToolWordWrap"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("a")]
        public string ShowProgessingOutput_Images {
            get {
                return ((string)(this["ShowProgessingOutput_Images"]));
            }
            set {
                this["ShowProgessingOutput_Images"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("5MBa")]
        public string ShowProgessingOutput_Videos {
            get {
                return ((string)(this["ShowProgessingOutput_Videos"]));
            }
            set {
                this["ShowProgessingOutput_Videos"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("-1a")]
        public string ShowProgessingOutput_VideosTranscoding {
            get {
                return ((string)(this["ShowProgessingOutput_VideosTranscoding"]));
            }
            set {
                this["ShowProgessingOutput_VideosTranscoding"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool DeleteToTrash {
            get {
                return ((bool)(this["DeleteToTrash"]));
            }
            set {
                this["DeleteToTrash"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool Maps_EnableCache {
            get {
                return ((bool)(this["Maps_EnableCache"]));
            }
            set {
                this["Maps_EnableCache"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string Maps_LastPosition {
            get {
                return ((string)(this["Maps_LastPosition"]));
            }
            set {
                this["Maps_LastPosition"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string Maps_LastProvider {
            get {
                return ((string)(this["Maps_LastProvider"]));
            }
            set {
                this["Maps_LastProvider"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string GoogleAPI_Elevation {
            get {
                return ((string)(this["GoogleAPI_Elevation"]));
            }
            set {
                this["GoogleAPI_Elevation"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute(@"ArcGIS_DarbAE_Q2_2011_NAVTQ_Eng_V5_MapProvider|ArcGIS_ShadedRelief_World_2D_Map|ArcGIS_World_Physical_Map|CloudMade, Demo|GoogleChinaSatelliteMap|GoogleKoreaHybridMap|GoogleKoreaMap|GoogleKoreaSatelliteMap|GoogleSatelliteMap|LatviaMap|Lithuania 2.5d Map|LithuaniaHybridMap|LithuaniaHybridMapOld|LithuaniaOrtoFotoMap|LithuaniaTOP50|MapBender, WMS demo|NearHybridMap|NearMap|NearSatelliteMap|None|SpainMap|YahooHybridMap|YahooMap|YahooSatelliteMap")]
        public string Maps_DisabledProviders {
            get {
                return ((string)(this["Maps_DisabledProviders"]));
            }
            set {
                this["Maps_DisabledProviders"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Never")]
        public global::MLocati.MediaData.Processor.SetFileDates SetFileDates {
            get {
                return ((global::MLocati.MediaData.Processor.SetFileDates)(this["SetFileDates"]));
            }
            set {
                this["SetFileDates"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string AppCulture {
            get {
                return ((string)(this["AppCulture"]));
            }
            set {
                this["AppCulture"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string BatchRenameFormats {
            get {
                return ((string)(this["BatchRenameFormats"]));
            }
            set {
                this["BatchRenameFormats"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string BatchRenameLastFormat {
            get {
                return ((string)(this["BatchRenameLastFormat"]));
            }
            set {
                this["BatchRenameLastFormat"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool DoUpgradeSettings {
            get {
                return ((bool)(this["DoUpgradeSettings"]));
            }
            set {
                this["DoUpgradeSettings"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("7")]
        public double MaxAllowedDistanceError_Horizontal {
            get {
                return ((double)(this["MaxAllowedDistanceError_Horizontal"]));
            }
            set {
                this["MaxAllowedDistanceError_Horizontal"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("2")]
        public double MaxAllowedDistanceError_Vertical {
            get {
                return ((double)(this["MaxAllowedDistanceError_Vertical"]));
            }
            set {
                this["MaxAllowedDistanceError_Vertical"] = value;
            }
        }
    }
}
