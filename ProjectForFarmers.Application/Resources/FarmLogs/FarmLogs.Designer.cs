﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ProjectForFarmers.Application.Resources.FarmLogs {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class FarmLogs {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal FarmLogs() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("ProjectForFarmers.Application.Resources.FarmLogs.FarmLogs", typeof(FarmLogs).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to schedule has been updated:
        ///Opened: {0}
        ///Start time: {1}:{2}
        ///End time: {3}:{4}.
        /// </summary>
        internal static string DayScheduleChanged {
            get {
                return ResourceManager.GetString("DayScheduleChanged", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Farm created successfully..
        /// </summary>
        internal static string FarmCreated {
            get {
                return ResourceManager.GetString("FarmCreated", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Farm deleted successfully..
        /// </summary>
        internal static string FarmDeleted {
            get {
                return ResourceManager.GetString("FarmDeleted", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Images {0} were added..
        /// </summary>
        internal static string ImagesCreated {
            get {
                return ResourceManager.GetString("ImagesCreated", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Images {0} were deleted..
        /// </summary>
        internal static string ImagesDeleted {
            get {
                return ResourceManager.GetString("ImagesDeleted", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to was changed from {0} to {1}..
        /// </summary>
        internal static string PropertyChanged {
            get {
                return ResourceManager.GetString("PropertyChanged", resourceCulture);
            }
        }
    }
}
