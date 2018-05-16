﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading.Tasks;
using Autodesk.DesignScript.Runtime;
using Dynamo.Controls;
using Dynamo.Graph.Nodes;
using Dynamo.UI.Commands;
using Dynamo.Wpf;
using ProtoCore.AST.AssociativeAST;
using Newtonsoft.Json;
using Dynamo.Graph;
using System.Xml;

namespace JsonDataUI.Nodes
{
    public abstract class JsonOptionsBase : NodeModel
    {
        #region Internal Properties
        internal bool NeedsOptions;
        internal bool nesting;
        internal string option;
        internal string[] options = new string[3] { "None", "Update", "Combine" };
        #endregion

        #region Properties

        /// <summary>
        /// Property controlling Nesting behaviour
        /// </summary>
        public bool Nesting
        {
            get { return nesting; }
            set
            {
                nesting = value;
                RaisePropertyChanged("Nested");
                OnNodeModified();
            }
        }

        public string Option
        {
            get { return option; }
            set
            {
                option = value;
                RaisePropertyChanged("Option");
                OnNodeModified();
            }
        }


        /// <summary>
        /// AssociativeNode for JsonOption property
        /// </summary>
        /// <returns>Function call returning selected JsonOption</returns>
        public static AssociativeNode JsonOptionASTNode(string option)
        {
            return AstFactory.BuildFunctionCall(
            new Func<string, JsonData.JsonOption>(JsonData.JsonOptions.ReturnOptionByName),
            new List<AssociativeNode> { AstFactory.BuildStringNode(option) }
            );
           
        }

        /// <summary>
        /// AssociativeNode for Nested property
        /// </summary>
        public static AssociativeNode NestedASTNode (bool nesting)
        {
            return AstFactory.BuildBooleanNode(nesting);
        }

        public List<AssociativeNode> InputNodes(List<AssociativeNode> inputAstNodes)
        {
            var inputs = new List<AssociativeNode>(inputAstNodes);
            inputs.Add(NestedASTNode(this.nesting));
            if (NeedsOptions)
            {
                inputs.Add(JsonOptionASTNode(this.option));
            }
            return inputs;
        }

        #endregion

        #region Constructors

        protected JsonOptionsBase(bool needsOptions = true)
        {
            NeedsOptions = needsOptions;
            RegisterAllPorts();
            ArgumentLacing = LacingStrategy.Auto;
            PopulateView();
        }

        [JsonConstructor]
        protected JsonOptionsBase(
            IEnumerable<PortModel> inPorts,
            IEnumerable<PortModel> outPorts) : base(inPorts, outPorts)
        {
            //PopulateView();
        }

        #endregion

        #region Methods

        public void PopulateView()
        {
            this.option = "None";
            this.nesting = true;
        }

        #endregion
    }
}
