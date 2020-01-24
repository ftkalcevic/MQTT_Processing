//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MqttConfig
{
    
    
    /// <summary>
    /// The MqttConfig Configuration Section.
    /// </summary>
    public partial class MqttConfig : global::System.Configuration.ConfigurationSection
    {
        
        #region Singleton Instance
        /// <summary>
        /// The XML name of the MqttConfig Configuration Section.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.3.5")]
        internal const string MqttConfigSectionName = "mqttConfig";
        
        /// <summary>
        /// The XML path of the MqttConfig Configuration Section.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.3.5")]
        internal const string MqttConfigSectionPath = "mqttConfig";
        
        /// <summary>
        /// Gets the MqttConfig instance.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.3.5")]
        public static global::MqttConfig.MqttConfig Instance
        {
            get
            {
                return ((global::MqttConfig.MqttConfig)(global::System.Configuration.ConfigurationManager.GetSection(global::MqttConfig.MqttConfig.MqttConfigSectionPath)));
            }
        }
        #endregion
        
        #region Xmlns Property
        /// <summary>
        /// The XML name of the <see cref="Xmlns"/> property.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.3.5")]
        internal const string XmlnsPropertyName = "xmlns";
        
        /// <summary>
        /// Gets the XML namespace of this Configuration Section.
        /// </summary>
        /// <remarks>
        /// This property makes sure that if the configuration file contains the XML namespace,
        /// the parser doesn't throw an exception because it encounters the unknown "xmlns" attribute.
        /// </remarks>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.3.5")]
        [global::System.Configuration.ConfigurationPropertyAttribute(global::MqttConfig.MqttConfig.XmlnsPropertyName, IsRequired=false, IsKey=false, IsDefaultCollection=false)]
        public string Xmlns
        {
            get
            {
                return ((string)(base[global::MqttConfig.MqttConfig.XmlnsPropertyName]));
            }
        }
        #endregion
        
        #region IsReadOnly override
        /// <summary>
        /// Gets a value indicating whether the element is read-only.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.3.5")]
        public override bool IsReadOnly()
        {
            return false;
        }
        #endregion
        
        #region BrokerServer Property
        /// <summary>
        /// The XML name of the <see cref="BrokerServer"/> property.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.3.5")]
        internal const string BrokerServerPropertyName = "brokerServer";
        
        /// <summary>
        /// Gets or sets the BrokerServer.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.3.5")]
        [global::System.ComponentModel.DescriptionAttribute("The BrokerServer.")]
        [global::System.Configuration.ConfigurationPropertyAttribute(global::MqttConfig.MqttConfig.BrokerServerPropertyName, IsRequired=true, IsKey=false, IsDefaultCollection=false)]
        public virtual string BrokerServer
        {
            get
            {
                return ((string)(base[global::MqttConfig.MqttConfig.BrokerServerPropertyName]));
            }
            set
            {
                base[global::MqttConfig.MqttConfig.BrokerServerPropertyName] = value;
            }
        }
        #endregion
        
        #region BrokerPort Property
        /// <summary>
        /// The XML name of the <see cref="BrokerPort"/> property.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.3.5")]
        internal const string BrokerPortPropertyName = "brokerPort";
        
        /// <summary>
        /// Gets or sets the BrokerPort.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.3.5")]
        [global::System.ComponentModel.DescriptionAttribute("The BrokerPort.")]
        [global::System.Configuration.ConfigurationPropertyAttribute(global::MqttConfig.MqttConfig.BrokerPortPropertyName, IsRequired=true, IsKey=false, IsDefaultCollection=false)]
        public virtual int BrokerPort
        {
            get
            {
                return ((int)(base[global::MqttConfig.MqttConfig.BrokerPortPropertyName]));
            }
            set
            {
                base[global::MqttConfig.MqttConfig.BrokerPortPropertyName] = value;
            }
        }
        #endregion
        
        #region MqttEventHandlers Property
        /// <summary>
        /// The XML name of the <see cref="MqttEventHandlers"/> property.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.3.5")]
        internal const string MqttEventHandlersPropertyName = "mqttEventHandlers";
        
        /// <summary>
        /// Gets or sets the MqttEventHandlers.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.3.5")]
        [global::System.ComponentModel.DescriptionAttribute("The MqttEventHandlers.")]
        [global::System.Configuration.ConfigurationPropertyAttribute(global::MqttConfig.MqttConfig.MqttEventHandlersPropertyName, IsRequired=false, IsKey=false, IsDefaultCollection=false)]
        public virtual global::MqttConfig.MqttEventHandlers MqttEventHandlers
        {
            get
            {
                return ((global::MqttConfig.MqttEventHandlers)(base[global::MqttConfig.MqttConfig.MqttEventHandlersPropertyName]));
            }
            set
            {
                base[global::MqttConfig.MqttConfig.MqttEventHandlersPropertyName] = value;
            }
        }
        #endregion
    }
}
namespace MqttConfig
{
    
    
    /// <summary>
    /// A collection of MqttEventHandler instances.
    /// </summary>
    [global::System.Configuration.ConfigurationCollectionAttribute(typeof(global::MqttConfig.MqttEventHandler), CollectionType=global::System.Configuration.ConfigurationElementCollectionType.BasicMapAlternate, AddItemName=global::MqttConfig.MqttEventHandlers.MqttEventHandlerPropertyName)]
    public partial class MqttEventHandlers : global::System.Configuration.ConfigurationElementCollection
    {
        
        #region Constants
        /// <summary>
        /// The XML name of the individual <see cref="global::MqttConfig.MqttEventHandler"/> instances in this collection.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.3.5")]
        internal const string MqttEventHandlerPropertyName = "mqttEventHandler";
        #endregion
        
        #region Overrides
        /// <summary>
        /// Gets the type of the <see cref="global::System.Configuration.ConfigurationElementCollection"/>.
        /// </summary>
        /// <returns>The <see cref="global::System.Configuration.ConfigurationElementCollectionType"/> of this collection.</returns>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.3.5")]
        public override global::System.Configuration.ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return global::System.Configuration.ConfigurationElementCollectionType.BasicMapAlternate;
            }
        }
        
        /// <summary>
        /// Gets the name used to identify this collection of elements
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.3.5")]
        protected override string ElementName
        {
            get
            {
                return global::MqttConfig.MqttEventHandlers.MqttEventHandlerPropertyName;
            }
        }
        
        /// <summary>
        /// Indicates whether the specified <see cref="global::System.Configuration.ConfigurationElement"/> exists in the <see cref="global::System.Configuration.ConfigurationElementCollection"/>.
        /// </summary>
        /// <param name="elementName">The name of the element to verify.</param>
        /// <returns>
        /// <see langword="true"/> if the element exists in the collection; otherwise, <see langword="false"/>.
        /// </returns>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.3.5")]
        protected override bool IsElementName(string elementName)
        {
            return (elementName == global::MqttConfig.MqttEventHandlers.MqttEventHandlerPropertyName);
        }
        
        /// <summary>
        /// Gets the element key for the specified configuration element.
        /// </summary>
        /// <param name="element">The <see cref="global::System.Configuration.ConfigurationElement"/> to return the key for.</param>
        /// <returns>
        /// An <see cref="object"/> that acts as the key for the specified <see cref="global::System.Configuration.ConfigurationElement"/>.
        /// </returns>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.3.5")]
        protected override object GetElementKey(global::System.Configuration.ConfigurationElement element)
        {
            return ((global::MqttConfig.MqttEventHandler)(element)).Key;
        }
        
        /// <summary>
        /// Creates a new <see cref="global::MqttConfig.MqttEventHandler"/>.
        /// </summary>
        /// <returns>
        /// A new <see cref="global::MqttConfig.MqttEventHandler"/>.
        /// </returns>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.3.5")]
        protected override global::System.Configuration.ConfigurationElement CreateNewElement()
        {
            return new global::MqttConfig.MqttEventHandler();
        }
        #endregion
        
        #region Indexer
        /// <summary>
        /// Gets the <see cref="global::MqttConfig.MqttEventHandler"/> at the specified index.
        /// </summary>
        /// <param name="index">The index of the <see cref="global::MqttConfig.MqttEventHandler"/> to retrieve.</param>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.3.5")]
        public global::MqttConfig.MqttEventHandler this[int index]
        {
            get
            {
                return ((global::MqttConfig.MqttEventHandler)(base.BaseGet(index)));
            }
        }
        
        /// <summary>
        /// Gets the <see cref="global::MqttConfig.MqttEventHandler"/> with the specified key.
        /// </summary>
        /// <param name="Name">The key of the <see cref="global::MqttConfig.MqttEventHandler"/> to retrieve.</param>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.3.5")]
        public global::MqttConfig.MqttEventHandler this[object Name]
        {
            get
            {
                return ((global::MqttConfig.MqttEventHandler)(base.BaseGet(Name)));
            }
        }
        #endregion
        
        #region Add
        /// <summary>
        /// Adds the specified <see cref="global::MqttConfig.MqttEventHandler"/> to the <see cref="global::System.Configuration.ConfigurationElementCollection"/>.
        /// </summary>
        /// <param name="mqttEventHandler">The <see cref="global::MqttConfig.MqttEventHandler"/> to add.</param>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.3.5")]
        public void Add(global::MqttConfig.MqttEventHandler mqttEventHandler)
        {
            base.BaseAdd(mqttEventHandler);
        }
        #endregion
        
        #region Remove
        /// <summary>
        /// Removes the specified <see cref="global::MqttConfig.MqttEventHandler"/> from the <see cref="global::System.Configuration.ConfigurationElementCollection"/>.
        /// </summary>
        /// <param name="mqttEventHandler">The <see cref="global::MqttConfig.MqttEventHandler"/> to remove.</param>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.3.5")]
        public void Remove(global::MqttConfig.MqttEventHandler mqttEventHandler)
        {
            base.BaseRemove(this.GetElementKey(mqttEventHandler));
        }
        #endregion
        
        #region GetItem
        /// <summary>
        /// Gets the <see cref="global::MqttConfig.MqttEventHandler"/> at the specified index.
        /// </summary>
        /// <param name="index">The index of the <see cref="global::MqttConfig.MqttEventHandler"/> to retrieve.</param>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.3.5")]
        public global::MqttConfig.MqttEventHandler GetItemAt(int index)
        {
            return ((global::MqttConfig.MqttEventHandler)(base.BaseGet(index)));
        }
        
        /// <summary>
        /// Gets the <see cref="global::MqttConfig.MqttEventHandler"/> with the specified key.
        /// </summary>
        /// <param name="Name">The key of the <see cref="global::MqttConfig.MqttEventHandler"/> to retrieve.</param>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.3.5")]
        public global::MqttConfig.MqttEventHandler GetItemByKey(string Name)
        {
            return ((global::MqttConfig.MqttEventHandler)(base.BaseGet(((object)(Name)))));
        }
        #endregion
        
        #region IsReadOnly override
        /// <summary>
        /// Gets a value indicating whether the element is read-only.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.3.5")]
        public override bool IsReadOnly()
        {
            return false;
        }
        #endregion
    }
}
namespace MqttConfig
{
    
    
    /// <summary>
    /// The MqttEventHandler Configuration Element.
    /// </summary>
    public partial class MqttEventHandler : global::System.Configuration.ConfigurationElement
    {
        
        #region IsReadOnly override
        /// <summary>
        /// Gets a value indicating whether the element is read-only.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.3.5")]
        public override bool IsReadOnly()
        {
            return false;
        }
        #endregion
        
        #region Key Property
        /// <summary>
        /// The XML name of the <see cref="Key"/> property.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.3.5")]
        internal const string KeyPropertyName = "Name";
        
        /// <summary>
        /// Gets or sets the Key.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.3.5")]
        [global::System.ComponentModel.DescriptionAttribute("The Key.")]
        [global::System.Configuration.ConfigurationPropertyAttribute(global::MqttConfig.MqttEventHandler.KeyPropertyName, IsRequired=true, IsKey=true, IsDefaultCollection=false)]
        public virtual string Key
        {
            get
            {
                return ((string)(base[global::MqttConfig.MqttEventHandler.KeyPropertyName]));
            }
            set
            {
                base[global::MqttConfig.MqttEventHandler.KeyPropertyName] = value;
            }
        }
        #endregion
        
        #region File Property
        /// <summary>
        /// The XML name of the <see cref="File"/> property.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.3.5")]
        internal const string FilePropertyName = "File";
        
        /// <summary>
        /// Gets or sets the File.
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("ConfigurationSectionDesigner.CsdFileGenerator", "2.0.3.5")]
        [global::System.ComponentModel.DescriptionAttribute("The File.")]
        [global::System.Configuration.ConfigurationPropertyAttribute(global::MqttConfig.MqttEventHandler.FilePropertyName, IsRequired=true, IsKey=false, IsDefaultCollection=false)]
        public virtual string File
        {
            get
            {
                return ((string)(base[global::MqttConfig.MqttEventHandler.FilePropertyName]));
            }
            set
            {
                base[global::MqttConfig.MqttEventHandler.FilePropertyName] = value;
            }
        }
        #endregion
    }
}