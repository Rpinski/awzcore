﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Dieser Code wurde von einem Tool generiert.
//     Laufzeitversion:2.0.50727.5446
//
//     Änderungen an dieser Datei können falsches Verhalten verursachen und gehen verloren, wenn
//     der Code erneut generiert wird.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// Dieser Quellcode wurde automatisch generiert von xsd, Version=2.0.50727.3038.
// 
namespace awzcore.Serialization.XmlStub {
    using System.Xml.Serialization;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.awzhome.de/xmlns/NQComponent/1.0")]
    [System.Xml.Serialization.XmlRootAttribute("NQComponent", Namespace="http://www.awzhome.de/xmlns/NQComponent/1.0", IsNullable=false)]
    public partial class DefinitionRootType {
        
        private DefinitionRootTypeResources resourcesField;
        
        private RequirementType[] requirementsField;
        
        private CompatibilityConditionType[] compatibilityField;
        
        private AssemblyType[] assembliesField;
        
        private string nameField;
        
        private string updaterurlField;
        
        private string displaynameField;
        
        private string displayversionField;
        
        /// <remarks/>
        public DefinitionRootTypeResources resources {
            get {
                return this.resourcesField;
            }
            set {
                this.resourcesField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("requirement", IsNullable=false)]
        public RequirementType[] requirements {
            get {
                return this.requirementsField;
            }
            set {
                this.requirementsField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("condition", IsNullable=false)]
        public CompatibilityConditionType[] compatibility {
            get {
                return this.compatibilityField;
            }
            set {
                this.compatibilityField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("assembly", IsNullable=false)]
        public AssemblyType[] assemblies {
            get {
                return this.assembliesField;
            }
            set {
                this.assembliesField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name {
            get {
                return this.nameField;
            }
            set {
                this.nameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute("updater-url")]
        public string updaterurl {
            get {
                return this.updaterurlField;
            }
            set {
                this.updaterurlField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute("display-name")]
        public string displayname {
            get {
                return this.displaynameField;
            }
            set {
                this.displaynameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute("display-version")]
        public string displayversion {
            get {
                return this.displayversionField;
            }
            set {
                this.displayversionField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.awzhome.de/xmlns/NQComponent/1.0")]
    public partial class DefinitionRootTypeResources {
        
        private ResourceType translatedresourceField;
        
        private ResourceType nontranslatedresourceField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("translated-resource")]
        public ResourceType translatedresource {
            get {
                return this.translatedresourceField;
            }
            set {
                this.translatedresourceField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("non-translated-resource")]
        public ResourceType nontranslatedresource {
            get {
                return this.nontranslatedresourceField;
            }
            set {
                this.nontranslatedresourceField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.awzhome.de/xmlns/NQComponent/1.0")]
    public partial class ResourceType {
        
        private string keyField;
        
        private string namespaceField;
        
        private string pathField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string key {
            get {
                return this.keyField;
            }
            set {
                this.keyField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string @namespace {
            get {
                return this.namespaceField;
            }
            set {
                this.namespaceField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string path {
            get {
                return this.pathField;
            }
            set {
                this.pathField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.awzhome.de/xmlns/NQComponent/1.0")]
    public partial class AutoInjectionType {
        
        private string nameField;
        
        private string typeField;
        
        private bool asdependentField;
        
        private bool asdependentFieldSpecified;
        
        private bool overridableField;
        
        private bool overridableFieldSpecified;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name {
            get {
                return this.nameField;
            }
            set {
                this.nameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string type {
            get {
                return this.typeField;
            }
            set {
                this.typeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute("as-dependent")]
        public bool asdependent {
            get {
                return this.asdependentField;
            }
            set {
                this.asdependentField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool asdependentSpecified {
            get {
                return this.asdependentFieldSpecified;
            }
            set {
                this.asdependentFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool overridable {
            get {
                return this.overridableField;
            }
            set {
                this.overridableField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool overridableSpecified {
            get {
                return this.overridableFieldSpecified;
            }
            set {
                this.overridableFieldSpecified = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.awzhome.de/xmlns/NQComponent/1.0")]
    public partial class ServiceType {
        
        private string[] substitutionsField;
        
        private string[] attachedtoField;
        
        private RequirementType[] requirementsField;
        
        private ServiceTypeAutoinjection autoinjectionField;
        
        private string nameField;
        
        private string classField;
        
        private HostModeType categoryField;
        
        private bool categoryFieldSpecified;
        
        private bool singleinstanceField;
        
        private bool singleinstanceFieldSpecified;
        
        private string invokemethodField;
        
        private string quitmethodField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("service", IsNullable=false)]
        public string[] substitutions {
            get {
                return this.substitutionsField;
            }
            set {
                this.substitutionsField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute("attached-to")]
        [System.Xml.Serialization.XmlArrayItemAttribute("list", IsNullable=false)]
        public string[] attachedto {
            get {
                return this.attachedtoField;
            }
            set {
                this.attachedtoField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("requirement", IsNullable=false)]
        public RequirementType[] requirements {
            get {
                return this.requirementsField;
            }
            set {
                this.requirementsField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("auto-injection")]
        public ServiceTypeAutoinjection autoinjection {
            get {
                return this.autoinjectionField;
            }
            set {
                this.autoinjectionField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name {
            get {
                return this.nameField;
            }
            set {
                this.nameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string @class {
            get {
                return this.classField;
            }
            set {
                this.classField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public HostModeType category {
            get {
                return this.categoryField;
            }
            set {
                this.categoryField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool categorySpecified {
            get {
                return this.categoryFieldSpecified;
            }
            set {
                this.categoryFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute("single-instance")]
        public bool singleinstance {
            get {
                return this.singleinstanceField;
            }
            set {
                this.singleinstanceField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool singleinstanceSpecified {
            get {
                return this.singleinstanceFieldSpecified;
            }
            set {
                this.singleinstanceFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute("invoke-method")]
        public string invokemethod {
            get {
                return this.invokemethodField;
            }
            set {
                this.invokemethodField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute("quit-method")]
        public string quitmethod {
            get {
                return this.quitmethodField;
            }
            set {
                this.quitmethodField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.awzhome.de/xmlns/NQComponent/1.0")]
    public partial class RequirementType {
        
        private string nameField;
        
        private string versionField;
        
        private VersionContraintOperatorType operatorField;
        
        private bool operatorFieldSpecified;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name {
            get {
                return this.nameField;
            }
            set {
                this.nameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string version {
            get {
                return this.versionField;
            }
            set {
                this.versionField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public VersionContraintOperatorType @operator {
            get {
                return this.operatorField;
            }
            set {
                this.operatorField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool operatorSpecified {
            get {
                return this.operatorFieldSpecified;
            }
            set {
                this.operatorFieldSpecified = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.awzhome.de/xmlns/NQComponent/1.0")]
    public enum VersionContraintOperatorType {
        
        /// <remarks/>
        equal,
        
        /// <remarks/>
        greater,
        
        /// <remarks/>
        lower,
        
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("greater-or-equal")]
        greaterorequal,
        
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("lower-or-equal")]
        lowerorequal,
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.awzhome.de/xmlns/NQComponent/1.0")]
    public partial class ServiceTypeAutoinjection {
        
        private AutoInjectionType[] itemsField;
        
        private ItemsChoiceType[] itemsElementNameField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("list", typeof(AutoInjectionType))]
        [System.Xml.Serialization.XmlElementAttribute("service", typeof(AutoInjectionType))]
        [System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemsElementName")]
        public AutoInjectionType[] Items {
            get {
                return this.itemsField;
            }
            set {
                this.itemsField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ItemsElementName")]
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public ItemsChoiceType[] ItemsElementName {
            get {
                return this.itemsElementNameField;
            }
            set {
                this.itemsElementNameField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.awzhome.de/xmlns/NQComponent/1.0", IncludeInSchema=false)]
    public enum ItemsChoiceType {
        
        /// <remarks/>
        list,
        
        /// <remarks/>
        service,
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.awzhome.de/xmlns/NQComponent/1.0")]
    public enum HostModeType {
        
        /// <remarks/>
        general,
        
        /// <remarks/>
        gui,
        
        /// <remarks/>
        console,
        
        /// <remarks/>
        winservice,
        
        /// <remarks/>
        webserver,
        
        /// <remarks/>
        browser,
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.awzhome.de/xmlns/NQComponent/1.0")]
    public partial class AssemblyType {
        
        private ServiceType[] servicesField;
        
        private string fileField;
        
        private bool ismainField;
        
        private bool ismainFieldSpecified;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("service", IsNullable=false)]
        public ServiceType[] services {
            get {
                return this.servicesField;
            }
            set {
                this.servicesField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string file {
            get {
                return this.fileField;
            }
            set {
                this.fileField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool ismain {
            get {
                return this.ismainField;
            }
            set {
                this.ismainField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ismainSpecified {
            get {
                return this.ismainFieldSpecified;
            }
            set {
                this.ismainFieldSpecified = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.awzhome.de/xmlns/NQComponent/1.0")]
    public partial class CompatibilityConditionType {
        
        private string versionField;
        
        private VersionContraintOperatorType operatorField;
        
        private bool operatorFieldSpecified;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string version {
            get {
                return this.versionField;
            }
            set {
                this.versionField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public VersionContraintOperatorType @operator {
            get {
                return this.operatorField;
            }
            set {
                this.operatorField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool operatorSpecified {
            get {
                return this.operatorFieldSpecified;
            }
            set {
                this.operatorFieldSpecified = value;
            }
        }
    }
}
